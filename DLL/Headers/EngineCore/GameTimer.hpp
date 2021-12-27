/* -------------------------------------------------------------------------
** GameTimer.hpp
**
** The GameTimer class is the generic base class for the timer the engine
** will use. The user is responsible for implementing the virual functions
** using the timer library of their choosing.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _GAMETIMER_HPP_
#define _GAMETIMER_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include "PythonGil.hpp"

namespace firemelon
{
	class FIREMELONAPI GameTimer
	{
	public:
		
		GameTimer();
		virtual ~GameTimer();
		
		int				addTimerPy();
		virtual int		addTimer();
		
		void			logTimeStartPy(int timerId);
		virtual void	logTimeStart(int timerId);
		
		double			logTimeEndPy(int timerId);
		virtual double	logTimeEnd(int timerId);
		
		double			getLoggedTimePy(int timerId);
		virtual double	getLoggedTime(int timerId);

		double			getTimerElapsedPy(int timerId);
		virtual double	getTimerElapsed(int timerId);

		virtual double	getTimeSinceTick();
		virtual void	start() = 0;
		virtual double	tick() = 0;
		virtual void	frameComplete() = 0;

		double			getTimeElapsedPy();
		virtual double	getTimeElapsed() = 0;
		
		unsigned int	getPingTimePy();
		unsigned int	getPingTime();

	private:

		unsigned pingTime_;
	};
}

#endif // _GAMETIMER_HPP_