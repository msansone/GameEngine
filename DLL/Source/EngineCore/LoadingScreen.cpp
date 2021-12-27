#include "..\..\Headers\EngineCore\LoadingScreen.hpp"

using namespace firemelon;
using namespace boost::python;

LoadingScreen::LoadingScreen()
{
	id_ = -1;
	scriptTypeName_ = "";
	scriptName_ = "";
	scriptVar_ = "";
}

LoadingScreen::~LoadingScreen()
{

}

void LoadingScreen::cleanup()
{
	PythonAcquireGil lock;

	pyInstance_ = boost::python::object();
	pyInstanceNamespace_ = boost::python::object();

	pyInitialize_ = boost::python::object();
	pyPercentChanged_ = boost::python::object();

	try
	{
		std::string sCode;

		sCode += scriptVar_ + " = None\n";

		str pyCode(sCode);

		boost::python::object obj = boost::python::exec(pyCode, pyMainNamespace_);
	}
	catch (error_already_set &)
	{
		std::cout << "Error deleting loading screen " + scriptVar_ + " Type: " + scriptTypeName_ << std::endl;
		debugger_->handlePythonError();
	}
}

LoadingScreenId	LoadingScreen::getId()
{
	return id_;
}

void LoadingScreen::initializePythonData()
{
	PythonAcquireGil lock;

	try
	{
		// The scripts should be loaded just once and error if they are missing.
		std::string sCode = "from ";

		// Only one instance of each loading screen should be created.
		scriptVar_ = scriptTypeName_ + boost::lexical_cast<std::string>(id_);
		std::replace(scriptVar_.begin(), scriptVar_.end(), ' ', '_');
		

		sCode += scriptName_ + " import " + scriptTypeName_ + "\n";
		sCode += scriptVar_ + " = " + scriptTypeName_ + "()";

		pyMainModule_ = import("__main__");
		pyMainNamespace_ = pyMainModule_.attr("__dict__");
	
		str pyCode(sCode);	
		boost::python::object obj = boost::python::exec(pyCode, pyMainNamespace_);

		// Get the instance for this object.
		pyInstance_ = extract<object>(pyMainNamespace_[scriptVar_]);
		pyInstanceNamespace_ = pyInstance_.attr("__dict__");
	
		// Import firemelon module to the instance.
		object pyFiremelonModule((handle<>(PyImport_ImportModule("firemelon"))));
		pyInstanceNamespace_["firemelon"] = pyFiremelonModule;

		// Store the functions as python objects.
		pyInitialize_ = pyInstance_.attr("initialize");	
		pyPercentChanged_ = pyInstance_.attr("percentChanged");	
		
		//pyPercentChanged_ = boost::python::object();

		pyInstanceNamespace_["renderer"] = ptr(renderer_.get());
		pyInstanceNamespace_["textManager"] = ptr(textManager_.get());	
		pyInstanceNamespace_["fontManager"] = ptr(fontManager_.get());		
	}
	catch(error_already_set &)
	{
		std::cout<<"Error loading load screen " + scriptTypeName_<<std::endl;
		debugger_->handlePythonError();
	}

	return;
}

void LoadingScreen::initialize()
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

void LoadingScreen::percentChanged(int percentDone)
{
	PythonAcquireGil lock;

	try
	{
		pyPercentChanged_(percentDone);
	}
	catch(error_already_set &)
	{
		debugger_->handlePythonError();
	}

	return;
}