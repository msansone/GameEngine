/* -------------------------------------------------------------------------
** Animation.hpp
** 
** The Animation class contains a list of frames that make up an animation, as 
** well as data that describes its behavior, such as style and update speed.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _ANIMATION_HPP_
#define _ANIMATION_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <vector>

#include "AnimationFrame.hpp"
#include "PythonGil.hpp"

namespace firemelon
{
	enum AnimationStyle
	{
		// Starts the animation over again from frame 0 after the last frame.
		ANIMATION_STYLE_REPEAT = 0,

		// Nothing is displayed after the last frame.
		ANIMATION_STYLE_SINGLE = 1,

		// The last frame remains displayed after the animation is complete.
		ANIMATION_STYLE_SINGLE_PERSIST = 2,

		// The sprite state is reset to state 0 after the animation is complete,
		// and the virtual stateChanged function is called.
		ANIMATION_STYLE_SINGLE_END_STATE = 3
	};

	class FIREMELONAPI Animation
	{
	public:
		friend class Assets;
		friend class StageRenderable;

		Animation();
		virtual ~Animation();
			
		void								addFramePy(boost::shared_ptr<AnimationFrame> newFrame);
		void								addFrame(boost::shared_ptr<AnimationFrame> newFrame);

		int									getAlphaMaskSheetIdPy();
		int									getAlphaMaskSheetId();

		boost::shared_ptr<AnimationFrame>	getFramePy(int index);
		boost::shared_ptr<AnimationFrame>	getFrame(int index);

		int									getFrameCountPy();
		int									getFrameCount();

		std::string							getNamePy();
		std::string							getName();

		int									getSpriteSheetIdPy();
		int									getSpriteSheetId();

		void								setAlphaMaskSheetIdPy(int id);
		void								setAlphaMaskSheetId(int id);
		
		void								setSpriteSheetIdPy(int id);
		void								setSpriteSheetId(int id);

	private:

		void	setName(std::string name);

		std::string										name_;

		int												spriteSheetId_;		// The sprite sheet ID that this animation points to.

		int												alphaMaskSheetId_;	// The alpha mask sheet ID that this animation points to.
		
		std::vector<boost::shared_ptr<AnimationFrame>>	frames_;

	};

	typedef boost::shared_ptr<Animation>	AnimationPtr;
	typedef std::vector<AnimationPtr>		AnimationPtrList;
}

#endif // _ANIMATION_HPP_