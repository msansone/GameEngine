///* -------------------------------------------------------------------------
//** EntitySerializerHolder.hpp
//** 
//** The EntitySerializerHolder class is used to store an entity serializer.
//** It is necessary because the creation of an entity serializer, which is stored
//** at the highest level in the EntityComponents class, needs to be created at
//** a lower level, in the EntityController class. So rather than storing a
//** pointer to a dynamics controller directly in the EntityComponents class
//** it stores a pointer to an entity serializer holder, which contains the
//** entity serializer. This can then be given to the entity controller
//** which will use it to create the entity serializer.
//**
//** Author: Mike Sansone
//** ------------------------------------------------------------------------- */
//
//#ifndef _ENTITYSERIALIZERBUILDER_HPP_
//#define _ENTITYSERIALIZERBUILDER_HPP_
//
//#include "EntitySerializer.hpp"
//
//namespace firemelon
//{
//	class EntitySerializerHolder
//	{
//	public:
//		friend class EntityController;
//
//		EntitySerializerHolder();
//		virtual ~EntitySerializerHolder();
//		
//		EntitySerializer*	getEntitySerializer();
//		void				setEntitySerializer(EntitySerializer* entitySerializer);
//
//	private:
//	
//		EntitySerializer*	entitySerializer_;
//	};
//}
//
//#endif // _ENTITYSERIALIZERBUILDER_HPP_