#include "..\..\Headers\EngineCore\TransitionManager.hpp"

using namespace firemelon;

TransitionManager::TransitionManager() :
	transitionCompleteSignal_(new TransitionCompleteSignalRaw())
{
	BaseIds ids;

	activeTransition_ = nullptr;
	
	transitionTime_ = 1.0;
	roomIdToShow_ = ids.ROOM_NULL;	
}

TransitionManager::~TransitionManager()
{
}

void TransitionManager::update(double time)
{
	if (activeTransition_ != nullptr)
	{
		bool simulationIsStopped = engineController_->getIsSimulationStopped();

		if (simulationIsStopped == false)
		{
			activeTransition_->run(time, transitionTime_, true);

			if (activeTransition_->getIsComplete() == true)
			{
				activeTransition_ = nullptr;

				if (roomIdToShow_ != ids_->ROOM_NULL)
				{
					(*transitionCompleteSignal_)(roomIdToShow_);
				}
			}
		}
	}
}

void firemelon::TransitionManager::updateAsync(double time)
{
	if (activeTransitionAsync_ != nullptr)
	{
		bool simulationIsStopped = engineController_->getIsSimulationStopped();

		if (simulationIsStopped == false)
		{
			activeTransitionAsync_->run(time, transitionTimeAsync_, true);

			if (activeTransitionAsync_->getIsComplete() == true)
			{
				activeTransitionAsync_ = nullptr;
			}
		}
	}
}

bool TransitionManager::getIsTransitioningPy()
{
	PythonReleaseGil unlocker;

	return getIsTransitioning();
}

bool TransitionManager::getIsTransitioning()
{
	return (activeTransition_ != nullptr);
}

bool TransitionManager::getIsTransitioningAsyncPy()
{
	PythonReleaseGil unlocker;

	return getIsTransitioningAsync();
}

bool TransitionManager::getIsTransitioningAsync()
{
	return (activeTransitionAsync_ != nullptr);
}

void TransitionManager::activateRoomChangeTransition(TransitionId transitionId, double transitionTime, RoomId roomIdToShow)
{
	transitionTime_ = transitionTime;
	roomIdToShow_ = roomIdToShow;
	
	activeTransition_ = transitionContainer_->getTransition(transitionId);
}

void TransitionManager::activateTransitionPy(TransitionId transitionId, double transitionTime)
{
	PythonReleaseGil unlocker;

	activateTransition(transitionId, transitionTime);
}

void TransitionManager::activateTransition(TransitionId transitionId, double transitionTime)
{
	roomIdToShow_ = ids_->ROOM_NULL;

	transitionTime_ = transitionTime;

	activeTransition_ = transitionContainer_->getTransition(transitionId);
}

void TransitionManager::activateTransitionAsyncPy(TransitionId transitionId, double transitionTime)
{
	PythonReleaseGil unlocker;

	activateTransitionAsync(transitionId, transitionTime);
}

void TransitionManager::activateTransitionAsync(TransitionId transitionId, double transitionTime)
{
	if (transitionId != ids_->TRANSITION_NULL)
	{
		transitionTimeAsync_ = transitionTime;

		activeTransitionAsync_ = transitionContainer_->getTransition(transitionId);
	}
}

void TransitionManager::endTransitionAsyncPy()
{
	PythonReleaseGil unlocker;

	endTransitionAsync();
}

void TransitionManager::endTransitionAsync()
{
	if (activeTransitionAsync_ != nullptr)
	{
		activeTransitionAsync_->end();

		activeTransitionAsync_ = nullptr;
	}
}