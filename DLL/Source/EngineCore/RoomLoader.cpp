#include "..\..\Headers\EngineCore\RoomLoader.hpp"

using namespace firemelon;
using namespace boost::python;

RoomLoader::RoomLoader()
{
}

RoomLoader::~RoomLoader()
{
}

void RoomLoader::initialize()
{
	PythonAcquireGil lock;

	try
	{
		// The scripts should be loaded just once and error if they are missing.
		std::string sCode = "from ";

		std::string scriptVar = "roomLoader";

		sCode += scriptName_ + " import " + scriptTypeName_ + "\n";
		sCode += scriptVar + " = " + scriptTypeName_ + "()";
	
		pyMainModule_ = import("__main__");
		pyMainNamespace_ = pyMainModule_.attr("__dict__");
	
		str pyCode(sCode);	
		boost::python::object obj = boost::python::exec(pyCode, pyMainNamespace_);

		// Get the instance for this object.
		pyRoomLoaderInstance_ = extract<object>(pyMainNamespace_[scriptVar]);
		pyRoomLoaderNamespace_ = pyRoomLoaderInstance_.attr("__dict__");
	
		// Import firemelon module to the instance.
		object pyFiremelonModule((handle<>(PyImport_ImportModule("firemelon"))));
		pyRoomLoaderNamespace_["firemelon"] = pyFiremelonModule;

		// Store the functions as python objects.
		pyRoomLoaded_ = pyRoomLoaderInstance_.attr("roomLoaded");
		pyRoomUnloading_ = pyRoomLoaderInstance_.attr("roomUnloading");

		pyRoomLoaderNamespace_["audioPlayer"] = ptr(audioPlayer_.get());
		pyRoomLoaderNamespace_["textManager"] = ptr(textManager_.get());
		pyRoomLoaderNamespace_["messenger"] = ptr(messenger_.get());
		//pyRoomLoaderNamespace_["inputDeviceManager"] = ptr(inputDeviceManager_.get());
	}
	catch(error_already_set &)
	{
		std::cout<<"Error loading room loader " + scriptTypeName_<<std::endl;
		debugger_->handlePythonError();
	}

	userInitialize();
}

void RoomLoader::roomLoaded(RoomId roomId, stringmap roomParameters)
{
	PythonAcquireGil lock;

	try
	{
		pyRoomLoaded_(roomId, roomParameters);
	}
	catch(error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void RoomLoader::roomUnloading(RoomId roomId, stringmap roomParameters)
{
	PythonAcquireGil lock;

	try
	{
		pyRoomUnloading_(roomId, roomParameters);
	}
	catch(error_already_set &)
	{
		debugger_->handlePythonError();
	}
}

void RoomLoader::postMessage(Message message)
{
	postMessageSignal(message);
}

boost::shared_ptr<QueryResultFactory> RoomLoader::getQueryResultFactory()
{
	return queryResultFactory_;
}

boost::shared_ptr<QueryParametersFactory> RoomLoader::getQueryParametersFactory()
{
	return queryParametersFactory_;
}

boost::shared_ptr<TextManager> RoomLoader::getTextManager()
{
	return textManager_;
}

boost::shared_ptr<QueryManager> RoomLoader::getQueryManager()
{
	return queryManager_;
}

boost::shared_ptr<Messenger> RoomLoader::getMessenger()
{
	return messenger_;
}

boost::shared_ptr<AudioPlayer> RoomLoader::getAudioPlayer()
{
	return audioPlayer_;
}