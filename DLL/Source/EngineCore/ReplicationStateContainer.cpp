//#include "..\..\Headers\EngineCore\ReplicationStateContainer.hpp"
//
//using namespace firemelon;
//
//ReplicationStateContainer::ReplicationStateContainer()
//{
//	size_ = 0;
//}
//
//ReplicationStateContainer::~ReplicationStateContainer()
//{
//	int size = replicationStates_.size();
//
//	for (int i = 0; i < size; i++)
//	{
//		delete replicationStates_[i];
//	}
//}
//
//EntityReplicationState* ReplicationStateContainer::operator[](std::size_t index)
//{
//	if (index >= 0 && index < size_)
//	{
//		return replicationStates_[index];
//	}
//
//	return nullptr;
//}
//
//unsigned int ReplicationStateContainer::size()
//{
//	return size_;
//}
//
//void ReplicationStateContainer::clear()
//{
//	size_ = 0;
//}
//
//EntityReplicationState* ReplicationStateContainer::addNewReplicationState()
//{
//	EntityReplicationState* replicationState;
//
//	int vectorSize = replicationStates_.size();
//
//	if (size_ >= vectorSize)
//	{
//		// If the size is greater than or equal to the size of the underlying vector,
//		// push the replication state onto the vector and increment the size. If the
//		// size is less than the underlying vector size, then increment the size so
//		// the next replicationState in the vector will be recycled.
//		replicationState = new EntityReplicationState();
//
//		replicationStates_.push_back(replicationState);
//	}
//	else
//	{
//		// Recycle an existing replication state that is not currently in use.
//		replicationState = replicationStates_[size_];
//
//		replicationState->reset();
//	}
//	
//	size_++;
//
//	return replicationState;
//}