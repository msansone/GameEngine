/* -------------------------------------------------------------------------
** AnimationPlayer.hpp
**
** The AnimationPlayer class contains all of the variables necessary to 
** play an instance of an animation, such as the current frame and the current
** update time.
** 
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _ANIMATIONPLAYER_HPP_
#define _ANIMATIONPLAYER_HPP_

#include <iostream>

#include "Animation.hpp"

namespace firemelon
{
	class AnimationPlayer
	{
	public:
		friend class Stage;
		friend class StageController;
		friend class StageElements;
		friend class StageRenderable;

		AnimationPlayer(int framesPerSecond, AnimationStyle animationStyle);
		virtual ~AnimationPlayer();

		AnimationStyle	getAnimationStylePy();
		AnimationStyle	getAnimationStyle();

		int				getCurrentFramePy();
		int				getCurrentFrame();

		int				getFramesPerSecondPy();
		int				getFramesPerSecond();

		bool			getIsVisible();

		void			resetPy();
		void			reset();

		void			setAnimationStylePy(AnimationStyle animationStyle);
		void			setAnimationStyle(AnimationStyle animationStyle);

		void			setCurrentFramePy(int frameIndex);
		void			setCurrentFrame(int frameIndex);

		void			setFramesPerSecondPy(int framesPerSecond);
		void			setFramesPerSecond(int framesPerSecond);

		void			setIsVisible(bool value);

		void			toggleIsVisible();

		bool			updateAnimation(boost::shared_ptr<Animation> animation, double time);
		bool			updateAnimationPy(boost::shared_ptr<Animation> animation, double time);
		
	private:

		// Synch this animation up with another animation. It assumes that the animations being synched have
		// the same number of frames, speed, style, etc. An example use case is if each animation slot stores
		// the graphics for a player's avatar, and you equip a new item in the middle of an animation, you 
		// will want the item's animation to be on the same frame as the others.
		void	synch(boost::shared_ptr<AnimationPlayer> animationPlayer);

		AnimationStyle	animationStyle_;

		// The current frame the animation is to display.
		int				currentFrame_;

		// A timer used to determine if the animation should advance to the next frame.
		double			currentUpdateTime_;

		int				framesPerSecond_;

		// The frame the animation displayed in the previous update.
		int				previousFrame_;
		
		// If the animation should be rendered.
		bool			isVisible_;

		// The time interval that must pass before going to the next frame.
		double			updateInterval_;
		
	};
	
	typedef boost::shared_ptr<AnimationPlayer>	AnimationPlayerPtr;
	typedef std::vector<AnimationPlayerPtr>		AnimationPlayerPtrList;
}

#endif // _ANIMATIONPLAYER_HPP_