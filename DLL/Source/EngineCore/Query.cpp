#include "..\..\Headers\EngineCore\Query.hpp"

using namespace firemelon;
using namespace boost::python;

int Query::idCounter_ = 0;

Query::Query()
{
	roomManager_ = nullptr;
	
	scriptName_ = "";
	scriptTypeName_ = "";
	
	// Increment the ID to be used in the python instance name.
	id_ = idCounter_;
	idCounter_++;

	Debugger::queryCount++;
}

Query::~Query()
{
	bool debug = true;

	Debugger::queryCount--;
}

void Query::initialize()
{
	PythonAcquireGil lock;

	try
	{
		// The scripts should be loaded just once and error if they are missing.
		std::string sCode = "from ";

		std::string scriptVar = scriptTypeName_ + boost::lexical_cast<std::string>(id_);
		std::replace(scriptVar.begin(), scriptVar.end(), ' ', '_');

		sCode += scriptName_ + " import " + scriptTypeName_ + "\n";
		sCode += scriptVar + " = " + scriptTypeName_ + "()";
	
		pyMainModule_ = import("__main__");
		pyMainNamespace_ = pyMainModule_.attr("__dict__");
	
		str pyCode(sCode);	
		boost::python::object obj = boost::python::exec(pyCode, pyMainNamespace_);

		// Get the instance for this object.
		pyQueryInstance_ = extract<object>(pyMainNamespace_[scriptVar]);
		pyQueryInstanceNamespace_ = pyQueryInstance_.attr("__dict__");
	
		// Import firemelon module to the instance.
		object pyFiremelonModule((handle<>(PyImport_ImportModule("firemelon"))));
		pyQueryInstanceNamespace_["firemelon"] = pyFiremelonModule;

		// Store the functions as python objects.
		pyRun_ = pyQueryInstance_.attr("run");	
		
		pyParameters_ = pyQueryInstance_.attr("createParameters")();
		pyResult_ = pyQueryInstance_.attr("createResult")();

		// Set the query type value	
		pyQueryInstance_.attr("_queryType") = queryType_;
		
		pyQueryInstanceNamespace_["roomManager"] = ptr(roomManager_.get());		
		pyQueryInstanceNamespace_["roomContainer"] = ptr(roomContainer_.get());		
	}
	catch(error_already_set &)
	{
		std::cout<<"Error loading query " + scriptTypeName_<<std::endl;
		debugger_->handlePythonError();
	}
}

void Query::runQuery(QueryContainer queryContainer)
{
	try
	{
		PythonAcquireGil lock;

		pyRun_(queryContainer);
	}
	catch(error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

boost::shared_ptr<RoomManager> Query::getRoomManager()
{
	return roomManager_;
}