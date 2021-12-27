#ifndef _ENTITYCREATIONPARAMETERS_HPP_
#define _ENTITYCREATIONPARAMETERS_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <string>
#include "Types.hpp"
#include "BaseIds.hpp"
#include "PythonGil.hpp"

namespace firemelon
{
	class FIREMELONAPI EntityCreationParameters
	{
	public:
		friend class Room;
		friend class GameEngine;

		EntityCreationParameters();
		virtual ~EntityCreationParameters();
		
		int					getXPy();
		int					getX();
		void				setXPy(int value);
		void				setX(int value);
		
		int					getYPy();
		int					getY();
		void				setYPy(int value);
		void				setY(int value);
		
		int					getWPy();
		int					getW();
		void				setWPy(int value);
		void				setW(int value);
		
		int					getHPy();
		int					getH();
		void				setHPy(int value);
		void				setH(int value);
		
		int					getLayerPy();
		int					getLayer();
		void				setLayerPy(int value);
		void				setLayer(int value);
		
		RoomId				getRoomIdPy();
		RoomId				getRoomId();
		void				setRoomIdPy(RoomId value);
		void				setRoomId(RoomId value);
		
		SpawnPointId		getSpawnPointIdPy();
		SpawnPointId		getSpawnPointId();
		void				setSpawnPointIdPy(SpawnPointId value);
		void				setSpawnPointId(SpawnPointId value);
		
		EntityTypeId		getEntityTypeIdPy();
		EntityTypeId		getEntityTypeId();
		void				setEntityTypeIdPy(EntityTypeId value);
		void				setEntityTypeId(EntityTypeId value);
		
		InputChannel		getInputChannelPy();
		InputChannel		getInputChannel();
		void				setInputChannelPy(InputChannel value);
		void				setInputChannel(InputChannel value);
		
		int					getRenderOrderPy();
		int					getRenderOrder();
		void				setRenderOrderPy(int value);
		void				setRenderOrder(int value);
		
		int					getParticipantIndex();
		void				setParticipantIndex(int value);
		
		bool				getAcceptInputPy();
		bool				getAcceptInput();
		void				setAcceptInputPy(bool value);
		void				setAcceptInput(bool value);
		
		bool				getAttachCameraPy();
		bool				getAttachCamera();
		void				setAttachCameraPy(bool value);
		void				setAttachCamera(bool value);
		
		std::string			getInitialStateNamePy();
		std::string			getInitialStateName();
		void				setInitialStateNamePy(std::string value);
		void				setInitialStateName(std::string value);
		
		std::string			getEntityNamePy();
		std::string			getEntityName();
		void				setEntityNamePy(std::string value);
		void				setEntityName(std::string value);
		
		std::string			getIpAddress();
		void				setIpAddress(std::string value);
		
		void				addPropertyPy(std::string key, std::string value);
		void				addProperty(std::string key, std::string value);
		
		//BufferWrapperPtr	getEntityDataBuffer();
		//void				setEntityDataBuffer(BufferWrapperPtr bufferWrapper);

	private:
	
		int					x_;
		int					y_;
		int					w_;
		int					h_;
		int					layer_;
		RoomId				roomId_;
		SpawnPointId		spawnPointId_;
		EntityTypeId		entityTypeId_;
		InputChannel		inputChannel_;
		int					renderOrder_;
		int					participantIndex_;
		bool				acceptInput_;
		bool				attachCamera_;
		stringmap			properties_;
		std::string			initialStateName_;
		std::string			entityName_;
		std::string			ipAddress_;
		//BufferWrapperPtr	bufferWrapper_;
	};
}

#endif // _ENTITYCREATIONPARAMETERS_HPP_