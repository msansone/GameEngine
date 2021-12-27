///* -------------------------------------------------------------------------
//** ReplicationStateContainer.hpp
//**
//** The ReplicationStateContainer class stores Replication states. It is needed
//** because allocating and deallocating the replication states every frame is
//** way too slow. Instead it will allocate them into an std::vector, and 
//** recycle them, only allocating more when needed.
//** 
//** Author: Mike Sansone
//** ------------------------------------------------------------------------- */
//
//#ifndef _REPLICATIONSTATECONTAINER_HPP_
//#define _REPLICATIONSTATECONTAINER_HPP_
//
//#include <boost/shared_ptr.hpp>
//
//#include <vector>
//
//#include "EntityReplicationState.hpp"
//
//namespace firemelon
//{
//	class ReplicationStateContainer
//	{
//	public:
//		
//		ReplicationStateContainer();
//		virtual ~ReplicationStateContainer();
//		
//		unsigned int			size();
//
//		void					clear();
//
//		EntityReplicationState* addNewReplicationState();
//		
//		EntityReplicationState*	operator[](std::size_t index);
//
//	private:
//
//		unsigned int							size_;
//
//		std::vector<EntityReplicationState*>	replicationStates_;
//
//	};
//}
//
//#endif // _REPLICATIONSTATECONTAINER_HPP_