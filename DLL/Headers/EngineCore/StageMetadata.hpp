/* -------------------------------------------------------------------------
** StageMetadata.hpp
**
** The StageMetadata class contains the metadata for a stage, so that it can 
** be accessed by all of the pieces that use it, such as the stage renderable
** and hitboxes. It is necessary because those stage elements cannot hold a
** reference to the stage itself, as this would create a cyclical dependecy.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _STAGEMETADATA_HPP_
#define _STAGEMETADATA_HPP_

#include <iostream>

#include <boost/shared_ptr.hpp>

#include "RenderEffects.hpp"
#include "Types.hpp"

namespace firemelon
{
	enum AnimationSlotOrigin
	{
		ANIMATION_SLOT_ORIGIN_TOP_LEFT = 0,
		ANIMATION_SLOT_ORIGIN_CENTER = 1,
		ANIMATION_SLOT_ORIGIN_TOP_MIDDLE = 2,
		ANIMATION_SLOT_ORIGIN_TOP_RIGHT = 3,
		ANIMATION_SLOT_ORIGIN_MIDDLE_LEFT = 4,
		ANIMATION_SLOT_ORIGIN_MIDDLE_RIGHT = 5,
		ANIMATION_SLOT_ORIGIN_BOTTOM_LEFT = 6,
		ANIMATION_SLOT_ORIGIN_BOTTOM_MIDDLE = 7,
		ANIMATION_SLOT_ORIGIN_BOTTOM_RIGHT = 8,
		ANIMATION_SLOT_ORIGIN_CUSTOM = 9
	};

	class StageMetadata
	{
	public:
		friend class StageController;
		friend class StageElements;
		friend class StageRenderable;

		StageMetadata();
		virtual ~StageMetadata();

		int						getBackgroundDepth();
		void					setBackgroundDepth(int backgroundDepth);

		int						getHeight();
		void					setHeight(int stageHeight);

		StageOrigin				getOrigin();
		void					setOrigin(StageOrigin origin);
		
		PositionPtr				getPosition();

		RotationOperationPtr	getRotationOperation();

		int						getWidth();
		void					setWidth(int stageWidth);

	private:

		float					angularVelocity_;

		int						backgroundDepth_;

		int						height_;

		StageOrigin				origin_;
		
		PositionPtr				position_;

		RenderEffectsPtr		renderEffects_;

		int						width_;

	};

	typedef boost::shared_ptr<StageMetadata> StageMetadataPtr;
}

#endif // _STAGEMETADATA_HPP_