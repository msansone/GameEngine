#include "..\..\Headers\EngineCore\PythonGil.hpp"

using namespace firemelon;
using namespace boost::python;

PythonAcquireGil::PythonAcquireGil()
{
	state_ = PyGILState_Ensure();
}

PythonAcquireGil::~PythonAcquireGil()
{
	PyGILState_Release(state_);
}

PythonReleaseGil::PythonReleaseGil()
{
	state_ = PyEval_SaveThread();
}

PythonReleaseGil::~PythonReleaseGil()
{
	PyEval_RestoreThread(state_);
}