#include "..\..\Headers\EngineCore\ScriptingData.hpp"

using namespace firemelon;

ScriptingData::ScriptingData()
{
}

ScriptingData::ScriptingData(std::string fileName, std::string typeName)
{
	fileName_ = fileName;
	scriptTypeName_ = typeName;
}

ScriptingData::~ScriptingData()
{
}

std::string ScriptingData::getFileName()
{
	return fileName_;
}

std::string	ScriptingData::getScriptTypeName()
{
	return scriptTypeName_;
}