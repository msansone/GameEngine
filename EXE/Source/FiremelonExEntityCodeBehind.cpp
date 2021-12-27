#include "..\Headers\FiremelonExEntityCodeBehind.hpp"

using namespace boost::python;
using namespace firemelon;

FiremelonExEntityCodeBehind::FiremelonExEntityCodeBehind()
{
}

FiremelonExEntityCodeBehind::~FiremelonExEntityCodeBehind()
{
}

void FiremelonExEntityCodeBehind::initialize()
{
	std::string pyTypeName = this->getPythonInstanceWrapper()->getInstanceTypeName();

	try
	{
		PythonAcquireGil lock;

		// Get the instance for this object.
		object pyEntityInstance = this->getPythonInstanceWrapper()->getPyInstance();
		object pEntityNamespace = pyEntityInstance.attr("__dict__");

		// Import firemelon_ex module to the instance.
		object pyFiremelonExModule((handle<>(PyImport_ImportModule("firemelon_ex"))));
		pEntityNamespace["firemelon_ex"] = pyFiremelonExModule;

		boost::shared_ptr<BoostGameTimer> timer = boost::static_pointer_cast<BoostGameTimer>(getTimer());

		pEntityNamespace["timer"] = ptr(timer.get());
	}
	catch (error_already_set &)
	{
		std::cout << "Error loading Entity " << pyTypeName << std::endl;
		//handlePythonError();
	}
}