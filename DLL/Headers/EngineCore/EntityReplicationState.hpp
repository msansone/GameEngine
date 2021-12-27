///* -------------------------------------------------------------------------
//** EntityReplicationState.hpp
//**
//** The EntityReplicationState class is used to stored all of the data needed
//** to replicate an entity from the server onto the client. It will get 
//** populated and then serialized into a byte array.
//**
//** Author: Mike Sansone
//** ------------------------------------------------------------------------- */
//
//#ifndef _ENTITYREPLICATIONSTATE_HPP_
//#define _ENTITYREPLICATIONSTATE_HPP_
//
//#include "EntitySerializer.hpp"
//#include "BaseIds.hpp"
//
//namespace firemelon
//{
//	class EntityReplicationState
//	{
//	public:
//		
//		EntityReplicationState();
//		EntityReplicationState(const EntityReplicationState &obj);
//		virtual ~EntityReplicationState();
//		
//		void				reset();
//
//		EntityTypeId		getEntityTypeId();
//		void				setEntityTypeId(EntityTypeId value);
//
//		int					getServerId();
//		void				setServerId(int value);
//
//		int					getMapLayer();
//		void				setMapLayer(int value);
//		
//		int					getStateIndex();
//		void				setStateIndex(int value);
//		
//		EntitySerializer*	getEntitySerializer();
//		void				setEntitySerializer(EntitySerializer* value);
//		
//		BufferWrapperPtr	getEntityDataBuffer();
//		void				setEntityDataBuffer(BufferWrapperPtr value);
//
//		float				getPositionX();
//		void				setPositionX(float value);
//		
//		float				getPositionY();
//		void				setPositionY(float value);
//
//		float				getVelocityX();
//		void				setVelocityX(float value);
//		
//		float				getVelocityY();
//		void				setVelocityY(float value);
//
//		float				getMovementX();
//		void				setMovementX(float value);
//		
//		float				getMovementY();
//		void				setMovementY(float value);
//
//		float				getAccelerationX();
//		void				setAccelerationX(float value);
//		
//		float				getAccelerationY();
//		void				setAccelerationY(float value);
//		
//		float				getLookX();
//		void				setLookX(float value);
//
//		float				getLookY();
//		void				setLookY(float value);
//
//		std::string			getIpAddress();
//		void				setIpAddress(std::string value);
//		
//		bool				getAttachCamera();
//		void				setAttachCamera(bool value);
//
//		bool				getAcceptInput();
//		void				setAcceptInput(bool value);
//		
//	private:
//		
//		EntityTypeId				entityTypeId_;
//
//		int							serverId_;
//		int							mapLayer_;
//		int							stateIndex_;
//		
//		EntitySerializer*			entitySerializer_;
//
//		// This is needed when receiving an entity from the server. It stores the data in a buffer,
//		// which gets set in the entity's state data serializer when the entity is replicated.
//		BufferWrapperPtr			stateDataBuffer_;
//		
//		float						positionX_;
//		float						positionY_;
//		float						movementX_;
//		float						movementY_;
//		float						velocityX_;
//		float						velocityY_;
//		float						accelerationX_;
//		float						accelerationY_;
//		float						lookX_;
//		float						lookY_;
//
//		bool						attachCamera_;
//		bool						acceptInput_;
//
//		std::string					ipAddress_;
//	};
//}
//
//#endif // _ENTITYREPLICATIONSTATE_HPP_