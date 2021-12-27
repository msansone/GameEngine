#include "..\..\Headers\EngineCore\AnimationPlayer.hpp"

using namespace firemelon;

AnimationPlayer::AnimationPlayer(int framesPerSecond, AnimationStyle animationStyle)
{
	animationStyle_ = animationStyle;

	currentFrame_ = -1;

	framesPerSecond_ = 60;

	previousFrame_ = -1;

	currentUpdateTime_ = 0.0;

	isVisible_ = true;

	updateInterval_ = 1.0 / (double)framesPerSecond;

	// If this is too small, make it larger, otherwise animations could get stuck in an infinite loop.
	if (updateInterval_ < 0.0001)
	{
		std::cout << "Warning: Animation seconds per frame was too small. Changing to 60 FPS." << std::endl;
		updateInterval_ = 1 / 60;
	}
}

AnimationPlayer::~AnimationPlayer()
{
}

AnimationStyle AnimationPlayer::getAnimationStylePy()
{
	PythonReleaseGil unlocker;

	return getAnimationStyle();
}

AnimationStyle AnimationPlayer::getAnimationStyle()
{
	return animationStyle_;
}

void AnimationPlayer::setAnimationStylePy(AnimationStyle animationStyle)
{
	PythonReleaseGil unlocker;

	setAnimationStyle(animationStyle);
}

void AnimationPlayer::setAnimationStyle(AnimationStyle animationStyle)
{
	animationStyle_ = animationStyle;
}

void AnimationPlayer::setFramesPerSecondPy(int framesPerSecond)
{
	PythonReleaseGil unlocker;

	setFramesPerSecond(framesPerSecond);
}

void AnimationPlayer::setFramesPerSecond(int framesPerSecond)
{
	framesPerSecond_ = framesPerSecond;

	updateInterval_ = 1.0 / (double)framesPerSecond_;

	// If this is too small, make it larger, otherwise animations could get stuck in an infinite loop.
	if (updateInterval_ < 0.0001)
	{
		std::cout << "Warning: Animation seconds per frame was too small. Changing to 60 FPS." << std::endl;
		updateInterval_ = 1 / 60;
	}
}

int AnimationPlayer::getFramesPerSecondPy()
{
	PythonReleaseGil unlocker;

	return getFramesPerSecond();
}

int AnimationPlayer::getFramesPerSecond()
{
	return framesPerSecond_;
}

int AnimationPlayer::getCurrentFramePy()
{
	PythonReleaseGil unlocker;

	return getCurrentFrame();
}

int AnimationPlayer::getCurrentFrame()
{
	return currentFrame_;
}

void AnimationPlayer::setCurrentFramePy(int frameIndex)
{
	PythonReleaseGil unlocker;

	return setCurrentFrame(frameIndex);
}

void AnimationPlayer::setCurrentFrame(int frameIndex)
{
	if (frameIndex >= 0)
	{
		currentFrame_ = frameIndex;
	}
}

bool AnimationPlayer::getIsVisible()
{
	return isVisible_;
}

void AnimationPlayer::setIsVisible(bool value)
{
	isVisible_ = value;
}

void AnimationPlayer::toggleIsVisible()
{
	isVisible_ = !isVisible_;
}

bool AnimationPlayer::updateAnimationPy(boost::shared_ptr<Animation> animation, double time)
{
	PythonReleaseGil unlocker;

	return updateAnimation(animation, time);
}

bool AnimationPlayer::updateAnimation(boost::shared_ptr<Animation> animation, double time)
{
	bool frameChanged = false;

	previousFrame_ = currentFrame_;
	
	currentUpdateTime_ += time;
	
	while (currentUpdateTime_ >= updateInterval_)
	{
		// This is causing infinite loops sometimes if update interval is very small. Rather than investigate, just reset to zero rather than
		// handling mutiple time chunks. findmelater investigate when i have time
		currentUpdateTime_ -= updateInterval_;

		//currentUpdateTime_ = 0;

		int frameId = currentFrame_;

		if (frameId >= -1)
		{
			currentFrame_ += 1;
			
			int frameCount = animation->getFrameCount();

			if (currentFrame_ >= frameCount)
			{
				switch (animationStyle_)
				{
				case ANIMATION_STYLE_REPEAT:
				
					currentFrame_ = 0;
				
					break;

				case ANIMATION_STYLE_SINGLE_PERSIST:

					currentFrame_ = frameCount - 1;
					
					break;

				case ANIMATION_STYLE_SINGLE_END_STATE:
				
					// Don't change the frame, the end of state change needs to be detected and handled by the state machine controller.
					
					break;

				case ANIMATION_STYLE_SINGLE:
				
					// -2 ends the animation. It's not -1 because that's the "uninitiaized" value.
					// This is because you need to be able to detect the very first "frame change" when the animation goes
					// from uninitialized to the first frame.
					currentFrame_ = -2;

					break;

				default:
					break;
				}

			}

		}

	}

	if (previousFrame_ != currentFrame_)
	{
		frameChanged = true;
	}

	return frameChanged;
}

void AnimationPlayer::synch(boost::shared_ptr<AnimationPlayer> animationPlayer)
{
	currentFrame_ = animationPlayer->currentFrame_;

	previousFrame_ = animationPlayer->previousFrame_;

	currentUpdateTime_ = animationPlayer->currentUpdateTime_;

	updateInterval_ = animationPlayer->updateInterval_;

	isVisible_ = animationPlayer->isVisible_;
}

void AnimationPlayer::resetPy()
{
	PythonReleaseGil unlocker;

	reset();
}

void AnimationPlayer::reset()
{
	currentFrame_ = 0;

	currentUpdateTime_ = 0.0; 
}