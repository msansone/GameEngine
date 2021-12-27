/* -------------------------------------------------------------------------
** LoadingScreenContainer.hpp
** 
** The LoadingScreenContainer class stores the loading screens.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _LOADINGSCREENCONTAINER_HPP_
#define _LOADINGSCREENCONTAINER_HPP_

#include <vector>
#include <map>

#include "LoadingScreen.hpp"
#include "Assets.hpp"

namespace firemelon
{
	class LoadingScreenContainer
	{
	public:

		LoadingScreenContainer();
		virtual ~LoadingScreenContainer();
		
		void								cleanup();

		int									getLoadingScreenCount();
		boost::shared_ptr<LoadingScreen>	getLoadingScreen(LoadingScreenId loadingScreenId);
		boost::shared_ptr<LoadingScreen>	getLoadingScreenByIndex(int loadingScreenIndex);
		int									getLoadingScreenIndex(LoadingScreenId loadingScreenId);

		void								addLoadingScreen(boost::shared_ptr<LoadingScreen> loadingScreen);

		void								setAssets(boost::shared_ptr<Assets> assets);

	private:

		std::vector<boost::shared_ptr<LoadingScreen>>	loadingScreens_;

		std::map<LoadingScreenId, int>					loadingScreenIdToIndexMap_;

		boost::shared_ptr<Assets>						assets_;
	};
}

#endif // _LOADINGSCREENCONTAINER_HPP_