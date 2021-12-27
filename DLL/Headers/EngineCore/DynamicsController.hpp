/* -------------------------------------------------------------------------
** DynamicsController.hpp
** 
** The DynamicsController class contains functions and data to control movement
** of entities. Entities will attach a dynamics controller in their initializebegin
** function, and it will be registered with the physicsmanager for updates. The 
** update method handles the physics integration step. It can be subclassed to expand 
** functionality if desired.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _DYNAMICSCONTROLLER_HPP_
#define _DYNAMICSCONTROLLER_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <vector>

#include <boost/lexical_cast.hpp>

#include "PhysicsConfig.hpp"
#include "PhysicsSnapshot.hpp"
#include "Renderer.hpp"
#include "Position.hpp"
#include "Debugger.hpp"

namespace firemelon
{
	enum Axis
	{
		AXIS_X = 1,
		AXIS_Y = 2,
		AXIS_XY = 3
	};

	class FIREMELONAPI DynamicsController
	{
	public:
		friend class CameraController;
		friend class CodeBehindContainer;
		friend class CollidableCodeBehind;
		friend class DynamicsControllerHolder;
		friend class Entity;
		friend class ParticleEntityCodeBehind;
		friend class ParticleEmitterEntityCodeBehind;
		friend class PhysicsManager;
		friend class Room;
		friend class StateMachineController; // Need for faster execution in performance critical code.

		DynamicsController();
		virtual ~DynamicsController();
		
		int									getOwnerIdPy();
		int									getOwnerId();
		
		PhysicsSnapshot*					getPhysicsSnapshotPy();
		boost::shared_ptr<PhysicsSnapshot>	getPhysicsSnapshot();
		PhysicsSnapshot*					getPhysicsSnapshotPreviousPy();
		boost::shared_ptr<PhysicsSnapshot>	getPhysicsSnapshotPrevious();
		
		boost::shared_ptr<Vec2<float>>		getLookPy();
		boost::shared_ptr<Vec2<float>>		getLook();
		
		// setPosition sets the position but preserves the previous position value, so it can
		// still be interpolated correctly, and other uses.
		void								setPositionXPy(float value);
		void								setPositionX(float value);

		// relocatePositionY sets a new position and sets the previous position value to the
		// current position, so no interpolation will occur
		void								relocatePositionXPy(float value);
		void								relocatePositionX(float value);

		float								getPositionXPy();
		float								getPositionX();
		
		void								setPositionYPy(float value);
		void								setPositionY(float value);

		void								relocatePositionYPy(float value);
		void								relocatePositionY(float value);

		float								getPositionYPy();
		float								getPositionY();

		float								getVelocityXPy();
		float								getVelocityX();
		float								getVelocityYPy();
		float								getVelocityY();
		void								setVelocityXPy(float value);
		void								setVelocityX(float value);
		void								setVelocityYPy(float value);
		void								setVelocityY(float value);
		
		void								setAccelerationXPy(float value);
		void								setAccelerationX(float value);
		void								setAccelerationYPy(float value);
		void								setAccelerationY(float value);
		
		void								setLookXPy(float value);
		void								setLookX(float value);
		void								setLookYPy(float value);
		void								setLookY(float value);
		
		void								setMovementXPy(float value);
		void								setMovementX(float value);
		float								getMovementXPy();
		float								getMovementX();
		
		void								setMovementYPy(float value);
		void								setMovementY(float value);
		float								getMovementYPy();
		float								getMovementY();
		
		void								setOwnerStageHeightPy(int value);
		void								setOwnerStageHeight(int value);
		int									getOwnerStageHeightPy();
		int									getOwnerStageHeight();
		
		void								setOwnerStageWidthPy(int value);
		void								setOwnerStageWidth(int value);
		int									getOwnerStageWidthPy();
		int									getOwnerStageWidth();
		
		boost::shared_ptr<PhysicsConfig>	getLocalPhysicsConfig();
		PhysicsConfig*						getLocalPhysicsConfigPy();
	
		void								setIsAffectedByGravity(bool value);
		void								setIsAffectedByGravityPy(bool value);

		bool								getIsAffectedByGravity();
		bool								getIsAffectedByGravityPy();

		void								applyForcePy(float forceX, float forceY);
		void								applyForce(float forceX, float forceY);
		void								applyForce(boost::shared_ptr<Vec2<float>> force);

		void								applyImpulsePy(float impulseX, float impulseY);
		void								applyImpulse(float impulseX, float impulseY);

		void								resetPy();
		void								reset();
		
		void								clearForcesXPy();
		void								clearForcesX();
		void								clearForcesYPy();
		void								clearForcesY();
		
		void								setAttachedTo(DynamicsController* attachedTo);

		DynamicsController*					getAttachedTo();

		void								setAttachmentAxisPy(Axis axis);
		void								setAttachmentAxis(Axis axis);

		int									getAttachedEntityCount();
		
		void								addAttachedEntity(DynamicsController* attachedEntity);
		void								removeAttachedEntity(DynamicsController* attachedEntity);

		DynamicsController*					getAttachedEntity(int index);
		
		DynamicsController*					getThisPy();
		DynamicsController*					getThis();
		
		void								initializePy();
		void								initialize();
		
		void								renderPy(int renderX, int renderY);
		void								render(int renderX, int renderY);
		
		Renderer*							getRendererPy();
		boost::shared_ptr<Renderer>			getRenderer();

		bool								getUseLocalPhysicsSettings();
		bool								getUseLocalPhysicsSettingsPy();

		void								setUseLocalPhysicsSettings(bool value);
		void								setUseLocalPhysicsSettingsPy(bool value);

	protected:
		
		boost::shared_ptr<Position>			getPosition();

	private:
		
		virtual void						userPreUpdate();

		virtual void						userPostUpdate();

		void								preRender();

		void								integrate(double time);

		void								updateAttachments();

		void								setOwnerId(int ID);
		
		void								setPosition(boost::shared_ptr<Position> position);

		void								setPositionXInternal(float value, bool setPrevious, bool solidCollisionResolution);

		void								setPositionYInternal(float value, bool setPrevious, bool solidCollisionResolution);

		void								setGlobalPhysicsConfig(PhysicsConfigPtr physicsConfig);

		// The dynamics controller object that this dynamics controller is attached to.
		// Attaching keeps the position in synch with the attached dynamics controller.
		// You can only be attached to one at a time.
		DynamicsController*					attachedTo_;

		Axis								attachmentAxis_;

		// List of dynamics controllers that are currently attached to this one.
		std::vector<DynamicsController*>	entitiesAttachedToThis_;

		bool								isAffectedByGravity_;

		// The location of the layer that the owner entity exists in, relative to the camera.
		// Needed for rendering.
		boost::shared_ptr<Position>			layerPosition_;
		
		boost::shared_ptr<Vec2<float>>		look_;

		// Stage height and width are needed for rendering.
		int									ownerStageHeight_;
		int									ownerStageWidth_;	

		float								mass_;

		float								minVelocity_;

		// Location stores the integer position. 
		// It is shared among other subsystems that also need to know where it is located.
		boost::shared_ptr<Position>			position_;

		boost::shared_ptr<PhysicsConfig>	globalPhysicsConfig_;
		boost::shared_ptr<PhysicsConfig>	localPhysicsConfig_;
		boost::shared_ptr<PhysicsConfig>	activePhysicsConfig_;
		boost::shared_ptr<PhysicsSnapshot>	physicsSnapshot_;
		boost::shared_ptr<PhysicsSnapshot>	physicsSnapshotPreviousStep_;

		int									ownerId_;

		bool								useLocalPhysicsSettings_;

		// A renderer is declared in case any visual debugging is necessary.
		boost::shared_ptr<Renderer>			renderer_;
	};
}

#endif // _DYNAMICSCONTROLLER_HPP_