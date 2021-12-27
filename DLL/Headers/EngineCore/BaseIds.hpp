/* -------------------------------------------------------------------------
** BaseIds.hpp
** 
** The BaseIds class is the parent class of the generated Ids.hpp class. It
** defines type ID types as well as all of the predefined ID values that are 
** used internally by the engine.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _BASEIDS_HPP_
#define _BASEIDS_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <boost/uuid/uuid.hpp>
#include <boost/uuid/uuid_io.hpp>
#include <boost/uuid/nil_generator.hpp>
#include <boost/uuid/string_generator.hpp>

#include <map>
#include <string>
#include <vector>

#include "NameIdPair.hpp"
#include "ScriptingData.hpp"
#include "PythonGil.hpp"

namespace firemelon
{
	typedef int					FiremelonId;
	typedef boost::uuids::uuid	Guid;
	typedef int					InputChannel;

	// Asset ID types
	typedef FiremelonId			AssetId;
	typedef FiremelonId			EntityClassificationId;
	typedef FiremelonId			EntityTypeId;
	typedef unsigned short		HitboxIdentity;
	typedef FiremelonId			GameButtonId;
	typedef FiremelonId			GameButtonGroupId;
	typedef Guid				GameButtonUuid;
	typedef Guid				GameButtonGroupUuid;
	typedef FiremelonId			LoadingScreenId;
	typedef FiremelonId			MenuItemId;
	typedef FiremelonId			ParticleEmitterId;
	typedef FiremelonId			ParticleId;
	typedef FiremelonId			QueryId;
	typedef FiremelonId			RoomId;
	typedef FiremelonId			SpawnPointId;
	typedef FiremelonId			TransitionId;
	typedef FiremelonId			TriggerSignalId;
	typedef FiremelonId			UiWidgetId;


	typedef std::map<boost::uuids::uuid, int> UuidIntegerMap;

	class FIREMELONAPI BaseIds
	{
	public:

		AssetId				ASSET_NULL;

		RoomId				ROOM_NULL;
		RoomId				ROOM_UNLOAD;
	
		EntityTypeId		ENTITY_NULL;
		EntityTypeId		ENTITY_TILE;
		EntityTypeId		ENTITY_CAMERA;
			
		HitboxIdentity		HITBOX_WORLDGEOMETRY;
		HitboxIdentity		HITBOX_EVENT;

		SpawnPointId		SPAWNPOINT_NULL;
		ParticleEmitterId	PARTICLEEMITTER_NULL;
		ParticleId			PARTICLE_NULL;
		TransitionId		TRANSITION_NULL;
		GameButtonId		GAMEBUTTON_NULL;
		GameButtonGroupId	GAMEBUTTONGROUP_NULL;
		
		static int					getIntegerFromUuidStringPy(std::string uuidString);
		static int					getIntegerFromUuidString(std::string uuidString);
		static int					getIntegerFromUuid(boost::uuids::uuid uuid);
		static boost::uuids::uuid	getUuidFromInteger(int id);
		
		static bool					nameExists(std::string name);

		static UuidIntegerMap		getUuidIntegerMap();

		// Vector containing all the game IDs, and their names.
		static std::vector<NameIdPair>		idNames;

		// Map an integer ID to a script data struct
		static std::map<int, ScriptingData>	idScriptDataMap;
		
		// Map a string name to an integer ID.
		static std::map<std::string, FiremelonId>	nameIdMap;
		static std::map<FiremelonId, std::string>	idNameMap;

		BaseIds();
		virtual ~BaseIds();

		FiremelonId	getIdFromName(std::string name);

		FiremelonId	getIdFromNamePy(std::string name);

		std::string	getNameFromId(FiremelonId id);

		std::string	getNameFromIdPy(FiremelonId id);

	private:
		
		static UuidIntegerMap integerGenerator_;
		static bool isInitialized_;
	};
}

#endif // _BASEIDS_HPP_