/* -------------------------------------------------------------------------
** HitboxController.hpp
**
** The HitboxController class stores instances of the hitboxes and is used
** to activate or deactivate them in entities.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _HITBOXCONTROLLER_HPP_
#define _HITBOXCONTROLLER_HPP_

#include <vector>

#include "Position.hpp"
#include "RotationOperation.hpp"

namespace firemelon
{
	class HitboxController
	{
	public:
		friend class PhysicsManager;

		HitboxController();
		virtual ~HitboxController();

		void			setOwnerPosition(PositionPtr position);
		PositionPtr		getOwnerPosition();

		void			setStagePosition(PositionPtr position);
		PositionPtr		getStagePosition();

		void			activateHitbox(int hitboxId, unsigned char edgeFlags = 0xFF);

		void			deactivateHitbox(int hitboxId);

		void			deactivateAllHitboxes();

		int				getActiveHitboxCount();
		int				getActiveHitboxId(int index);

		unsigned char	getActiveHitboxEdgeFlags(int index);

		bool			getActiveHitboxCollisionStatus(int hitboxId);
		void			setActiveHitboxCollisionStatus(int hitboxId, bool status);

		//void			setHitboxStatus(int stateIndex, int hitboxIndex, HitboxStatus status);

		void			setStageHeight(int stageHeight);
		int				getStageHeight();

		void			setStageWidth(int stageWidth);
		int				getStageWidth();

		int				getCollisionGridCellStartRow();
		void			setCollisionGridCellStartRow(int value);

		int				getCollisionGridCellEndRow();
		void			setCollisionGridCellEndRow(int value);
		
		int				getCollisionGridCellStartCol();
		void			setCollisionGridCellStartCol(int value);

		int				getCollisionGridCellEndCol();
		void			setCollisionGridCellEndCol(int value);

	private:

		// The collidable grid cell bounds this collidable occupies. Used so that the grid doesn't need to be searched.
		int							gridStartCol_;

		int							gridStartRow_;

		int							gridEndRow_;

		int							gridEndCol_;

		std::vector<int>			activeHitboxIds_;

		std::vector<unsigned char>	activeHitboxEdgeFlags_;

		std::vector<bool>			activeHitboxCollisionStatuses_;

		bool						hasActiveColliders_;

		PositionPtr					ownerPosition_;

		RotationOperationPtr		rotationOperation_;

		int							stageHeight_;

		PositionPtr					stagePosition_;

		int							stageWidth_;

	};

	typedef boost::shared_ptr<HitboxController> HitboxControllerPtr;
}

#endif // _HITBOXCONTROLLER_HPP_