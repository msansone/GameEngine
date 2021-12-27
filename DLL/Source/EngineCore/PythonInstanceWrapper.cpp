#include "..\..\Headers\EngineCore\PythonInstanceWrapper.hpp"

using namespace firemelon;
using namespace boost::python;

PythonInstanceWrapper::PythonInstanceWrapper()
{
	instanceTypeName_ = "";
	instanceVariableName_ = "";
	isInitialized_ = false;
	scriptName_ = "";
}

PythonInstanceWrapper::~PythonInstanceWrapper()
{
}

void PythonInstanceWrapper::initialize()
{
	if (isInitialized_ == false)
	{
		PythonAcquireGil lock;

		std::string sCode = "from ";

		std::replace(instanceVariableName_.begin(), instanceVariableName_.end(), ' ', '_');

		// boost uuids will have a - character in them. Remove it.
		std::replace(instanceVariableName_.begin(), instanceVariableName_.end(), '-', '_');

		sCode += scriptName_ + " import " + instanceTypeName_ + "\n";
		sCode += instanceVariableName_ + " = " + instanceTypeName_ + "()";

		try
		{
			pyMainModule_ = import("__main__");
			pyMainNamespace_ = pyMainModule_.attr("__dict__");

			str pyCode(sCode);
			boost::python::object obj = boost::python::exec(pyCode, pyMainNamespace_);

			// Get the instance for this object.
			pyInstance_ = extract<object>(pyMainNamespace_[instanceVariableName_]);
			pyInstanceNamespace_ = pyInstance_.attr("__dict__");

			// Import firemelon module to the instance.
			object pyFiremelonModule((handle<>(PyImport_ImportModule("firemelon"))));
			pyInstanceNamespace_["firemelon"] = pyFiremelonModule;
		}
		catch (error_already_set &)
		{
			std::cout << "Error loading entity " << instanceTypeName_ << ": " << instanceVariableName_ << std::endl;
			debugger_->handlePythonError();
		}

		isInitialized_ = true;
	}
}

void PythonInstanceWrapper::cleanup()
{
	PythonAcquireGil lock;

	try
	{
		if (isInitialized_ == true)
		{
			pyInstance_ = boost::python::object();

			pyInstanceNamespace_ = boost::python::object();

			std::string sCode;

			sCode += instanceVariableName_ + " = None\n";

			str pyCode(sCode);

			boost::python::object obj = boost::python::exec(pyCode, pyMainNamespace_);

			pyMainModule_ = boost::python::object();

			pyMainNamespace_ = boost::python::object();
		}
	}
	catch (error_already_set &)
	{
		std::cout << "Error deleting entity " + instanceVariableName_ + " Type: " + instanceTypeName_ << std::endl;

		debugger_->handlePythonError();
	}
}


void PythonInstanceWrapper::renameInstanceVariableName(std::string value)
{
	PythonAcquireGil lock;

	try
	{
		std::string sCode;

		std::replace(value.begin(), value.end(), '-', '_');

		// Copy the instance to a new variable.
		sCode += value + " = " + instanceVariableName_ +"\n";

		// Set the old variable to none.
		sCode += instanceVariableName_ + " = None\n";

		str pyCode(sCode);

		// Set the instance variable name to the new name.
		instanceVariableName_ = value;

		boost::python::object obj = boost::python::exec(pyCode, pyMainNamespace_);

		// Extract the instance from the new variable.
		pyInstance_ = extract<object>(pyMainNamespace_[instanceVariableName_]);
	}
	catch (error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

std::string	PythonInstanceWrapper::getInstanceVariableName()
{
	return instanceVariableName_;
}

std::string	PythonInstanceWrapper::getScriptName()
{
	return scriptName_;
}

std::string	PythonInstanceWrapper::getInstanceTypeName()
{
	return instanceTypeName_;
}

void PythonInstanceWrapper::setInstanceVariableName(std::string value)
{
	instanceVariableName_ = value;
}

void PythonInstanceWrapper::setScriptName(std::string value)
{
	scriptName_ = value;
}

void PythonInstanceWrapper::setInstanceTypeName(std::string value)
{
	instanceTypeName_ = value;
}

object PythonInstanceWrapper::getPyInstance()
{
	return pyInstance_;
}

object PythonInstanceWrapper::getPyInstanceNamespace()
{
	return pyInstanceNamespace_;
}

object PythonInstanceWrapper::getMainModule()
{
	return pyMainModule_;
}

object PythonInstanceWrapper::getMainNamespace()
{
	return pyMainNamespace_;
}

bool PythonInstanceWrapper::getIsInitialized()
{
	return isInitialized_;
}