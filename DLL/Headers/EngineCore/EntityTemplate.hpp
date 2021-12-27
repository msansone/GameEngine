/* -------------------------------------------------------------------------
** EntityTemplate.hpp
**
** The EntityTemplate class stores a entityTemplate that describes the basic components 
** an entity should have. This includes States, Animations, and Hitboxes.
**
** The StateMachineController is the components that entities instantiates to point to the current
** state, animation frame, and hitboxes for the  instance. These can store a reference to
** a entityTemplate. Doing it this way should save space, storing the data in a sort-of global
** location, rather than storing copies of the exact same data in nearly every entity.
**
** If an individual entity is going to have hitboxes, states, and/or animations added
** or removed on the fly, it does not need to use a entityTemplate. Loading of the map
** is much faster when using a entityTemplate for entities that there are a lot of and 
** are not going to change dynamically, for example, tiles.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _ENTITYTEMPLATE_HPP_
#define _ENTITYTEMPLATE_HPP_

#include <vector>
#include <string>
#include <map>

#include "Animation.hpp"
#include "HitboxManager.hpp"
#include "StageElements.hpp"
#include "StageMetadata.hpp"
#include "Renderer.hpp"
#include "Types.hpp"
#include "Vec2.hpp"

namespace firemelon
{
	class EntityTemplate
	{
	public:
		friend class Assets;

		EntityTemplate();
		virtual ~EntityTemplate();
	
		int													addStageElements(boost::shared_ptr<StageElements> newState);
//		int													addAnimation(int id);
		int													addStateHitbox(boost::shared_ptr<Hitbox> hitbox);

		boost::shared_ptr<StageElements>					getStageElements(int index);
		int													getAnimation(int index);

		int													getStateHitboxCount();
		boost::shared_ptr<Hitbox>							getStateHitbox(int index);

		void												setInitialStateIndex(int initialStateIndex);
		int													getInitialStateIndex();

		int													getStateIndexFromName(std::string name);
		std::string											getStateNameFromIndex(int index);

		int													getStageElementsCount();
		
		void												setScriptName(std::string scriptName);
		std::string											getScriptName();

		void												setTypeName(std::string typeName);
		std::string											getScriptTypeName();
		
		void												setClassification(EntityClassification classification);
		EntityClassification								getClassification();
		
		void												setKeepRoomActive(bool keepRoomActive);
		bool												getKeepRoomActive();

		StageMetadataPtr									getStageMetadata();

	private:

		std::vector<boost::shared_ptr<Position>>			anchorPoints_;

		std::vector<std::string>							anchorPointNames_;
	
		EntityClassification								classification_;

		int													initialStateIndex_;

		// When an entity is added to a room, if it is set to keep active, it will increment a reference
		// count. When this count is greater than 0, the room will be considered active.
		bool												keepRoomActive_;
		
		boost::shared_ptr<Renderer>							renderer_;

		std::string											scriptName_;
		
		StageMetadataPtr									stageMetadata_;
				
		std::vector<boost::shared_ptr<Hitbox>>				stateHitboxList_;

		std::map<int, std::string>							stateIdNameMap_;

		std::map<std::string, int>							stateNameIdMap_;

		std::vector<boost::shared_ptr<StageElements>>		stageElements_;

		std::string											scriptTypeName_;


	};
}

#endif // _ENTITYTEMPLATE_HPP_