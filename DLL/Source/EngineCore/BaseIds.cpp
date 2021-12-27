#include "..\..\Headers\EngineCore\BaseIds.hpp"

using namespace firemelon;

std::map<boost::uuids::uuid, int> BaseIds::integerGenerator_;
std::vector<NameIdPair> BaseIds::idNames;
std::map<std::string, int> BaseIds::nameIdMap;
std::map<FiremelonId, std::string> BaseIds::idNameMap;
std::map<int, ScriptingData> BaseIds::idScriptDataMap;
bool BaseIds::isInitialized_ = false;

BaseIds::BaseIds()
{
	boost::uuids::string_generator gen;

	ROOM_NULL = getIntegerFromUuid(gen("{A416A752-2256-49D3-A222-15147FA1CAAF}"));	
	ROOM_UNLOAD = getIntegerFromUuid(gen("{0A8B654E-57C0-4BFE-9A98-508CB09419AF}"));
	
	HITBOX_WORLDGEOMETRY = getIntegerFromUuid(gen("{85F225E5-F4D0-4D81-8505-9E4E66C2F30A}"));
	HITBOX_EVENT = getIntegerFromUuid(gen("{F70C547B-BB9A-4009-87C6-D0DC258B8A29}"));
	
	ENTITY_NULL = getIntegerFromUuid(gen("{BB57E9CD-E960-460D-8B41-6E95556FE432}"));
	ENTITY_TILE = getIntegerFromUuid(gen("{37090DFF-479C-452E-9694-E6D9C7EA6747}"));
	ENTITY_CAMERA = getIntegerFromUuid(gen("{4CFEDF79-EFA3-4D20-8CCE-153EEF4F0085}"));
	
	SPAWNPOINT_NULL = getIntegerFromUuid(gen("{1DF1F8EA-6B18-4453-98CA-AE9A59F7FA0A}"));
	PARTICLEEMITTER_NULL = getIntegerFromUuid(gen("{CCA35E93-D0F2-4F85-B500-AB2557AD6E33}"));
	PARTICLE_NULL = getIntegerFromUuid(gen("{30A45114-AC65-4FCE-A8C4-03B22524EB1B}"));
	TRANSITION_NULL = getIntegerFromUuid(gen("{3827CFE8-3A5D-4981-B4C8-1A0F53506F07}"));

	if (isInitialized_ == false)
	{
		NameIdPair roomNullNameId(ROOM_NULL, "ROOM_NULL");
		BaseIds::idNames.push_back(roomNullNameId);
			
		NameIdPair roomUnloadNameId(ROOM_UNLOAD, "ROOM_UNLOAD");
		BaseIds::idNames.push_back(roomUnloadNameId);

		NameIdPair hitboxWorldGeometryNameId(HITBOX_WORLDGEOMETRY, "HITBOX_WORLDGEOMETRY");
		BaseIds::idNames.push_back(hitboxWorldGeometryNameId);
			
		NameIdPair hitboxEventNameId(HITBOX_EVENT, "HITBOX_EVENT");
		BaseIds::idNames.push_back(hitboxEventNameId);
			
		NameIdPair entityNullNameId(ENTITY_NULL, "ENTITY_NULL");
		BaseIds::idNames.push_back(entityNullNameId);

		NameIdPair entityTileNameId(ENTITY_TILE, "ENTITY_TILE");
		BaseIds::idNames.push_back(entityTileNameId);

		NameIdPair entityCameraNameId(ENTITY_CAMERA, "ENTITY_CAMERA");
		BaseIds::idNames.push_back(entityCameraNameId);
		
		NameIdPair spawnPointNullNameId(SPAWNPOINT_NULL, "SPAWNPOINT_NULL");
		BaseIds::idNames.push_back(spawnPointNullNameId);
		
		NameIdPair particleEmitterNullNameId(PARTICLEEMITTER_NULL, "PARTICLEEMITTER_NULL");
		BaseIds::idNames.push_back(particleEmitterNullNameId);

		NameIdPair transitionNullNameId(TRANSITION_NULL, "TRANSITION_NULL");
		BaseIds::idNames.push_back(transitionNullNameId);

		isInitialized_ = true;	
	}
}

BaseIds::~BaseIds()
{
}

int BaseIds::getIntegerFromUuidStringPy(std::string uuidString)
{
	PythonReleaseGil unlocker;

	return getIntegerFromUuidString(uuidString);
}

int BaseIds::getIntegerFromUuidString(std::string uuidString)
{
	boost::uuids::string_generator gen;

	return getIntegerFromUuid(gen(uuidString));
}

int BaseIds::getIntegerFromUuid(boost::uuids::uuid uuid)
{
	int id = integerGenerator_[uuid];

	if (id == 0)
	{
		// Default value is zero, meaning it is uninitialized.
		id = integerGenerator_.size();
		integerGenerator_[uuid] = id;
	}

	return id;
}

boost::uuids::uuid BaseIds::getUuidFromInteger(int id)
{
	std::map<boost::uuids::uuid, int>::const_iterator it;
	
	for (it = integerGenerator_.begin(); it != integerGenerator_.end(); ++it)
	{
		if (it->second == id)
		{
			return it->first;
		}
	}

	return boost::uuids::nil_uuid();
}

bool BaseIds::nameExists(std::string name)
{
	int size = idNames.size();

	for (int i = 0; i < size; i++)
	{
		if (idNames[i].getName() == name)
		{
			return true;
		}
	}

	return false;
}

UuidIntegerMap BaseIds::getUuidIntegerMap()
{
	return integerGenerator_;
}

int firemelon::BaseIds::getIdFromNamePy(std::string name)
{
	PythonReleaseGil unlocker;

	return getIdFromName(name);
}

int firemelon::BaseIds::getIdFromName(std::string name)
{
	int size = idNames.size();

	for (int i = 0; i < size; i++)
	{
		if (idNames[i].getName() == name)
		{
			return idNames[i].getId();
		}
	}

	return -1;
}
