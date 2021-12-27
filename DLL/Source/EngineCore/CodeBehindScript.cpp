#include "..\..\Headers\EngineCore\CodeBehindScript.hpp"

using namespace firemelon;
using namespace boost::python;

CodeBehindScript::CodeBehindScript(DebuggerPtr debugger)
{
	debugger_ = debugger;

	isInitialized_ = false;
}

CodeBehindScript::~CodeBehindScript()
{
}

boost::shared_ptr<PythonInstanceWrapper> CodeBehindScript::getPythonInstanceWrapper()
{
	return pythonInstanceWrapper_;
}

void CodeBehindScript::cleanup()
{

}

void CodeBehindScript::initialize()
{
}

void CodeBehindScript::preInitialize()
{
	if (isInitialized_ == false)
	{
		initialize();

		isInitialized_ = true;
	}
}

void CodeBehindScript::preCleanup()
{
	if (isInitialized_ == true)
	{
		cleanup();
	}
}

void CodeBehindScript::setPythonInstanceWrapper(boost::shared_ptr<PythonInstanceWrapper> pythonInstanceWrapper)
{
	pythonInstanceWrapper_ = pythonInstanceWrapper;
}

