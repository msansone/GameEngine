///* -------------------------------------------------------------------------
//** EntitySerializer.hpp
//** 
//** The EntitySerializer class is the generic base class that should be 
//** subclassed to serialize custom data like health, damage, etc. Virtual 
//** serialize/deserialize functions must be implemented, so it can be replicated 
//** on the client.
//**
//** Author: Mike Sansone
//** ------------------------------------------------------------------------- */
//
//#ifndef _ENTITYSERIALIZER_HPP_
//#define _ENTITYSERIALIZER_HPP_
//
//#if defined(FIREMELON_EXPORTS)
//#   define FIREMELONAPI   __declspec(dllexport)
//#else
//#   define FIREMELONAPI   __declspec(dllimport)
//#endif  // FIREMELON_EXPORTS
//
//#include <boost/signals2.hpp>
//#include <boost/uuid/uuid.hpp>
//#include <boost/uuid/uuid_io.hpp>
//#include <boost/uuid/uuid_generators.hpp>
//
//#include "SerializerCore.hpp"
//#include "DebugHelper.hpp"
//
//namespace firemelon
//{
//	typedef boost::shared_ptr<boost::signals2::signal<void ()>> SerializeEntitySignal;
//	typedef boost::shared_ptr<boost::signals2::signal<void ()>> DeserializeEntitySignal;
//	typedef boost::shared_ptr<SerializerCore> SerializerCorePointer;
//
//	class FIREMELONAPI EntitySerializer
//	{
//	public:
//		friend class EntityController;
//		friend class Room;
//		friend class JoinData;
//		friend class NetworkLayer;
//		friend class EngineController;
//		friend class GameEngine;
//		
//		EntitySerializer();
//		EntitySerializer(const EntitySerializer &obj);
//		virtual ~EntitySerializer();
//		
//		virtual void			serialize(SerializerCorePointer serializer);
//		virtual void			deserialize(SerializerCorePointer serializer);
//
//		SerializerCorePointer	getSerializerCore();
//
//		EntitySerializer*		getThisPy();
//		EntitySerializer*		getThis();
//
//	protected:
//		
//	private:
//		
//		boost::uuids::uuid		uuid_;
//		
//		void					preSerialize();
//		void					preDeserialize();
//		
//		SerializerCorePointer	serializer_;
//
//		SerializeEntitySignal	serializeEntitySignal_;
//		DeserializeEntitySignal	deserializeEntitySignal_;
//	};
//}
//
//#endif // _ENTITYSERIALIZER_HPP_