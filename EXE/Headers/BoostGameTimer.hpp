/* -------------------------------------------------------------------------
** BoostGameTimer.hpp
** 
** The BoostGameTimer class is derived from the base GameTimer class. It uses
** the boost chrono library to implement the timer tick and getTimeElapsed
** functions that the engine uses to count frame time.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _BOOSTGAMETIMER_HPP_
#define _BOOSTGAMETIMER_HPP_

#include <GameTimer.hpp>
#include <PythonGil.hpp>

#include<boost/thread.hpp>
#include <boost/chrono.hpp>

typedef boost::chrono::high_resolution_clock hiResClock;
typedef boost::chrono::duration<double> durationDouble;
typedef boost::chrono::time_point<hiResClock, durationDouble> hiResDoubleTimePoint;

class BoostGameTimer : public firemelon::GameTimer
{ 
public: 
	BoostGameTimer(); 
	virtual ~BoostGameTimer();
	
		
	virtual int		addTimer();
	
	virtual void	logTimeStart(int timerId);
	
	virtual double	logTimeEnd(int timerId);
	
	double			getLoggedTimePy(int timerId);
	virtual double	getLoggedTime(int timerId);

	double			getTimerElapsedPy(int timerId);
	virtual double	getTimerElapsed(int timerId);

	virtual void	start();
	
	virtual double	tick();
	
	virtual double	getTimeSinceTick();

	virtual void	frameComplete();

	double			getTimeElapsedPy();
	virtual double	getTimeElapsed();
	
	double			getMinTimeElapsedPy();
	virtual double	getMinTimeElapsed();
	
	double			getMaxTimeElapsedPy();
	virtual double	getMaxTimeElapsed();
	
	double			getAvgTimeElapsedPy();
	virtual double	getAvgTimeElapsed();

	void			setLockFps(float lockFps);
	void			setTimeScale(float timeScale);
	
	int				getFpsPy();
	int				getFps();
	
	double			getTotalTimePy();
	double			getTotalTime();
	
private: 
	

	float					lockFps_;	
	float					minFrameTime_;
	float					fpsTimeCount_;
	float					timeScale_;

	double					deltaTime_;
	double					totalTime_;
	double					minimumDeltaTime_;
	double					maximumDeltaTime_;
	double					averageDeltaTime_;
	
	double					minimumTimer_;
	double					maximumTimer_;
	double					averageTimer_;

	int						fps_;
	int						frameCount_;
	int						fpsFrameCount_;

	bool					writeFps_;

	hiResClock				clock;
	hiResDoubleTimePoint	currentTime_;
	hiResDoubleTimePoint	previousTime_;
	
	std::vector<hiResDoubleTimePoint>	logStartTime_;
	std::vector<double>					loggedTimes_;
};

#endif _BOOSTGAMETIMER_HPP_