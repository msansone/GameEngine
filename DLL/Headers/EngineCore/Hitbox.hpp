/* -------------------------------------------------------------------------
** Hitbox.hpp
**
** The Hitbox class stores rectangle collision geometry, as well as metadata
** required to correctly process the collision, such as identity.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _HITBOX_HPP_
#define _HITBOX_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <iostream>
#include <vector>

#include "Types.hpp"
#include "BaseIds.hpp"
#include "PythonGil.hpp"
#include "RotationOperation.hpp"
#include "StageMetadata.hpp"
#include "Vec2.hpp"

namespace firemelon
{	
	enum CollisionStyle
	{
		COLLISION_STYLE_SOLID = 0,
		COLLISION_STYLE_INCLINE = 1,
		COLLISION_STYLE_DECLINE = 2,
		COLLISION_STYLE_INVERTED_INCLINE = 3,
		COLLISION_STYLE_INVERTED_DECLINE = 4,
		COLLISION_STYLE_ONE_WAY_TOP = 5,
		COLLISION_STYLE_ONE_WAY_BOTTOM = 6,
		COLLISION_STYLE_ONE_WAY_LEFT = 7,
		COLLISION_STYLE_ONE_WAY_RIGHT = 8,
		COLLISION_STYLE_SNAP_TO_BOTTOM = 9,
		COLLISION_STYLE_SNAP_TO_TOP = 10,
		COLLISION_STYLE_SNAP_TO_LEFT = 11,
		COLLISION_STYLE_SNAP_TO_RIGHT = 12
	};

	class FIREMELONAPI Hitbox
	{
	public:

		friend class CollidableCodeBehind;
		friend class CollisionDispatcher;
		friend class HitboxManager;
		friend class PhysicsManager;
		friend class Room;
		friend class Stage;
		friend class StageElements;

		Hitbox(int top, int left, int height, int width);
		virtual ~Hitbox();

		float			getBottommostCorner();

		float			getLeftmostCorner();

		float			getRightmostCorner();

		float			getTopmostCorner();
		
		void			setCollisionRectPy(Rect rect);
		void			setCollisionRect(Rect rect);

		Rect			getCollisionRectPy();
		Rect			getCollisionRect();
		
		int				getLeftPy();
		int				getLeft();

		int				getTopPy();
		int				getTop();
	
		int				getHeightPy();
		int				getHeight();
		
		int				getWidthPy();
		int				getWidth();
		
		void			setIdentityPy(HitboxIdentity identity);
		void			setIdentity(HitboxIdentity identity);

		HitboxIdentity	getIdentityPy();
		HitboxIdentity	getIdentity();
	
		void			setEdgeFlags(unsigned char value);
		unsigned char	getEdgeFlags();

		void			setIsSolid(bool isSolid);
		bool			getIsSolid();

		void			setBaseRotationDegrees(float baseRotationDegrees);
		float			getBaseRotationDegrees();

		void			setCollisionStatus(bool status);
		bool			getCollisionStatus();
	
		float			getSlope();
		void			setSlope(float slope);

		CollisionStyle	getCollisionStyle();
		CollisionStyle	getCollisionStylePy();
		void			setCollisionStyle(CollisionStyle collisionStyle);

		bool			getUseTopEdge();
		void			setUseTopEdge(bool value);

		bool			getUseRightEdge();
		void			setUseRightEdge(bool value);

		bool			getUseBottomEdge();
		void			setUseBottomEdge(bool value);

		bool			getUseLeftEdge();
		void			setUseLeftEdge(bool value);

		ColorRgbaPtr	getDebugColor();

		PositionPtr		getOwnerPositionPy();
		PositionPtr		getOwnerPosition();

	private:
		
		void			resize(int top, int left, int height, int width);

		// This is the base rotation. The stage or animation rotation will get added into it to result
		// in the final rotation amount.
		float					baseRotationDegrees_;

		Rect					collisionRect_;

		// Center point and its untranslated value.
		float					centerX_;
		float					centerY_;

		ColorRgbaPtr			debugColor_;

		float					untranslatedCenterX_;
		float					untranslatedCenterY_;

		// Four corners, in clockwise order, and their untranslated values.

		PositionPtr				nativePosition_;

		std::vector<Vertex2>	nativeCorners_;

		AnimationSlotOrigin		origin_;
		
		std::vector<Vertex2>	transformedCorners_;


		float untranslatedLeft_;
		float untranslatedTop_;
		float bottom_;
		float untranslatedBottom_;
		float right_;
		float untranslatedRight_;
		
		CollisionStyle					collisionStyle_;

		// Edge flags are a single byte, whose first four bits are used to determine
		// whether the hitbox edge they represent should be considered tangible.

		// This is used for tiles with contiguous edges. The two edges that are touching will
		// be flagged as invalid, so as to create the illusion that the make up a single,
		// larger piece of collision geometry, rather than a series of single tiles, which causes
		// undesirable collision results.
		unsigned char					edgeFlags_;

		bool							useTopEdge_;
		bool							useRightEdge_;
		bool							useBottomEdge_;
		bool							useLeftEdge_;

		int								id_;

		HitboxIdentity					identity_;

		bool							isSolid_;
		
		// This is used for slope tiles, to indicate which type of slope it is.
		float							slope_;
		
		bool							isCollisionActive_;
		
		StageMetadataPtr				stageMetadata_;

		PositionPtr						ownerPosition_;
	};

	typedef boost::shared_ptr<Hitbox> HitboxPtr;
}

#endif // _HITBOX_HPP_