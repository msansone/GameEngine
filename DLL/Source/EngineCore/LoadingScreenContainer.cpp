#include "..\..\Headers\EngineCore\LoadingScreenContainer.hpp"

using namespace firemelon;

LoadingScreenContainer::LoadingScreenContainer()
{

}

LoadingScreenContainer::~LoadingScreenContainer()
{
}


void LoadingScreenContainer::cleanup()
{
	int size = loadingScreens_.size();

	for (int i = 0; i < size; i++)
	{
		loadingScreens_[i]->cleanup();
	}

	loadingScreens_.clear();
}

int	LoadingScreenContainer::getLoadingScreenCount()
{
	return loadingScreens_.size();
}

boost::shared_ptr<LoadingScreen> LoadingScreenContainer::getLoadingScreen(LoadingScreenId loadingScreenId)
{
	std::map<LoadingScreenId, int>::iterator itr = loadingScreenIdToIndexMap_.find(loadingScreenId);
	
	if (itr == loadingScreenIdToIndexMap_.end())
	{
		return nullptr;
	}
	else
	{
		int index = loadingScreenIdToIndexMap_[loadingScreenId];

		return loadingScreens_[index];
	}
}

boost::shared_ptr<LoadingScreen> LoadingScreenContainer::getLoadingScreenByIndex(int loadingScreenIndex)
{
	int size = loadingScreens_.size();

	if (loadingScreenIndex >= 0 && loadingScreenIndex < size)
	{
		return loadingScreens_[loadingScreenIndex];
	}

	return nullptr;
}

int LoadingScreenContainer::getLoadingScreenIndex(LoadingScreenId loadingScreenId)
{
	std::map<RoomId, int>::iterator itr = loadingScreenIdToIndexMap_.find(loadingScreenId);
	
	if (itr == loadingScreenIdToIndexMap_.end())
	{
		return -1;
	}
	else
	{
		return loadingScreenIdToIndexMap_[loadingScreenId];
	}
}

void LoadingScreenContainer::addLoadingScreen(boost::shared_ptr<LoadingScreen> loadingScreen)
{
	loadingScreen->initializePythonData();

	LoadingScreenId id = loadingScreen->getId();

	loadingScreenIdToIndexMap_[id] = loadingScreens_.size();
	
	loadingScreen->initialize();

	loadingScreens_.push_back(loadingScreen);

}

void LoadingScreenContainer::setAssets(boost::shared_ptr<Assets> assets)
{
	assets_ = assets;
}