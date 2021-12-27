/* -------------------------------------------------------------------------
** JoinData.hpp
**
** The JoinData class stores the metadata used when joining a game. It needs 
** to be filled out before the call to join is made.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _JOINDATA_HPP_
#define _JOINDATA_HPP_

#include <boost/python.hpp>

#include "BaseIds.hpp"
//#include "EntitySerializer.hpp"
#include "PythonGil.hpp"

namespace firemelon
{
	class JoinData
	{
	public:
		
		JoinData();
		JoinData(const JoinData &obj);
		virtual ~JoinData();
		
		void				setEntityTypeIdPy(EntityTypeId entityTypeId);
		void				setEntityTypeId(EntityTypeId entityTypeId);
		
		EntityTypeId		getEntityTypeIdPy();
		EntityTypeId		getEntityTypeId();
		
		void				setSpawnPointIdPy(SpawnPointId spawnPointId);
		void				setSpawnPointId(SpawnPointId spawnPointId);

		SpawnPointId		getSpawnPointIdPy();
		SpawnPointId		getSpawnPointId();
		
		void				setRoomIdPy(RoomId roomId);
		void				setRoomId(RoomId roomId);
		
		RoomId				getRoomIdPy();
		RoomId				getRoomId();
		
		//void				setEntitySerializer(EntitySerializer* entitySerializer);
		//void				setEntitySerializerPy(boost::python::object entitySerializer);

		//EntitySerializer*	getEntitySerializer();

		void				clear();

	private:

		//boost::python::object	pyEntitySerializer_;
		
		BaseIds					ids_;
		EntityTypeId			entityTypeId_;
		SpawnPointId			spawnPointId_;
		RoomId					roomId_;
		//EntitySerializer*		entitySerializer_;
	};
}

#endif // _JOINDATA_HPP_