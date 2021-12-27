#include "..\..\Headers\EngineCore\Transition.hpp"

using namespace firemelon;
using namespace boost::python;

Transition::Transition()
{
	id_ = -1;
	scriptTypeName_ = "";
	scriptName_ = "";
	timeElapsed_ = 0.0;
	isComplete_ = false;
}

Transition::~Transition()
{
}

void Transition::cleanup()
{
	PythonAcquireGil lock;

	try
	{
		// Only one instance of each loading screen should be created.
		std::string scriptVar = scriptTypeName_ + boost::lexical_cast<std::string>(id_);
		std::replace(scriptVar.begin(), scriptVar.end(), ' ', '_');

		// The scripts should be loaded just once and error if they are missing.
		std::string sCode = scriptVar + " = None ";

		str pyCode(sCode);
		boost::python::object obj = boost::python::exec(pyCode, pyMainNamespace_);
		
		// Store the functions as python objects.
		pyMainModule_ = boost::python::object();
		pyMainNamespace_ = boost::python::object();
		pyInstance_ = boost::python::object();
		pyInstanceNamespace_ = boost::python::object();
		pyInitialize_ = boost::python::object();
		pyBegin_ = boost::python::object();
		pyUpdate_ = boost::python::object();
		pyComplete_ = boost::python::object();
	}
	catch (error_already_set &)
	{
		std::cout << "Error loading transition " + scriptTypeName_ << std::endl;
		debugger_->handlePythonError();
	}
}

TransitionId Transition::getId()
{
	return id_;
}

bool Transition::getIsComplete()
{
	return isComplete_;
}

void Transition::initializePythonData()
{
	PythonAcquireGil lock;

	try
	{
		// The scripts should be loaded just once and error if they are missing.
		std::string sCode = "from ";

		// Only one instance of each loading screen should be created.
		std::string scriptVar = scriptTypeName_ + boost::lexical_cast<std::string>(id_);
		std::replace(scriptVar.begin(), scriptVar.end(), ' ', '_');
		
		sCode += scriptName_ + " import " + scriptTypeName_ + "\n";
		sCode += scriptVar + " = " + scriptTypeName_ + "()";

		pyMainModule_ = import("__main__");
		pyMainNamespace_ = pyMainModule_.attr("__dict__");
	
		str pyCode(sCode);	
		boost::python::object obj = boost::python::exec(pyCode, pyMainNamespace_);

		// Get the instance for this object.
		pyInstance_ = extract<object>(pyMainNamespace_[scriptVar]);
		pyInstanceNamespace_ = pyInstance_.attr("__dict__");
	
		// Import firemelon module to the instance.
		object pyFiremelonModule((handle<>(PyImport_ImportModule("firemelon"))));
		pyInstanceNamespace_["firemelon"] = pyFiremelonModule;

		// Store the functions as python objects.
		pyInitialize_ = pyInstance_.attr("initialize");
		pyBegin_ = pyInstance_.attr("begin");
		pyUpdate_ = pyInstance_.attr("update");
		pyComplete_ = pyInstance_.attr("complete");
		
		pyInstanceNamespace_["renderer"] = ptr(renderer_.get());	
		pyInstanceNamespace_["textManager"] = ptr(textManager_.get());		
	}
	catch(error_already_set &)
	{
		std::cout<<"Error loading transition " + scriptTypeName_<<std::endl;
		debugger_->handlePythonError();
	}

	return;
}

void Transition::run(double time, double totalTime, bool allowCompletion)
{
	if (timeElapsed_ == 0.0)
	{
		isComplete_ = false;

		begin();
	}

	{
	PythonAcquireGil lock;

	pyInstanceNamespace_["timeElapsed"] = timeElapsed_;	
	pyInstanceNamespace_["totalTime"] = totalTime;	
	}

	update(timeElapsed_);
	
	timeElapsed_ += time;

	if (allowCompletion == true)
	{
		if (timeElapsed_ >= totalTime)
		{
			timeElapsed_ = 0.0;

			complete();

			isComplete_ = true;
		}
	}
	else
	{
		// Keep the time elapsed at the max value if it is theoretically complete,
		// but not officially complete.
		if (timeElapsed_ >= totalTime)
		{
			timeElapsed_ = totalTime;
		}
	}
}

void Transition::end()
{
	timeElapsed_ = 0.0;

	complete();

	isComplete_ = true;
}

void Transition::initialize()
{
	PythonAcquireGil lock;

	try
	{
		pyInitialize_();
	}
	catch(error_already_set &)
	{
		debugger_->handlePythonError();
	}

	return;
}

void Transition::begin()
{
	PythonAcquireGil lock;

	try
	{
		pyBegin_();
	}
	catch(error_already_set &)
	{
		debugger_->handlePythonError();
	}

	return;
}

void Transition::update(double time)
{
	PythonAcquireGil lock;

	try
	{
		pyUpdate_(time);
	}
	catch(error_already_set &)
	{
		debugger_->handlePythonError();
	}

	return;
}

void Transition::complete()
{
	PythonAcquireGil lock;

	try
	{
		pyComplete_();
	}
	catch(error_already_set &)
	{
		debugger_->handlePythonError();
	}

	return;
}