/* -------------------------------------------------------------------------
** AnimationManager.hpp
** 
** The AnimationManager class contains all of the animations defined by the 
** editor, and provides an interface to retrieve them.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _ANIMATIONMANAGER_HPP_
#define _ANIMATIONMANAGER_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <string>
#include <vector>
#include <map>

#include "BaseIds.hpp"
#include "Animation.hpp"
#include "PythonGil.hpp"

namespace firemelon
{
	class FIREMELONAPI AnimationManager
	{
	public:
		friend class Assets;

		AnimationManager();
		virtual ~AnimationManager();
	
		int								addAnimation(boost::shared_ptr<Animation> animation);
		
		int								getAnimationId(std::string name);

		boost::shared_ptr<Animation>	getAnimationByIndexPy(int index);
		boost::shared_ptr<Animation>	getAnimationByIndex(int index);

		boost::shared_ptr<Animation>	getAnimationByNamePy(std::string name);
		boost::shared_ptr<Animation>	getAnimationByName(std::string name);

		int								translateAnimationId(AssetId animationId);

	private:
		
		std::vector<boost::shared_ptr<Animation>>	animations_;

		std::map<std::string, int>					animationNameIdMap_;

		// Map the animation ID from the asset file to the animation ID generated by the engine.
		// This is because the ID stored in the room file is not guaranteed to be the 
		// same as the the ID generated by the engine.
		std::map<AssetId, int>						animationIdMap_;
	};

	typedef boost::shared_ptr<AnimationManager> AnimationManagerPtr;
}

#endif // _ANIMATIONMANAGER_HPP_