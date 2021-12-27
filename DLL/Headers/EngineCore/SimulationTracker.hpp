/* -------------------------------------------------------------------------
** SimulationTracker.hpp
**
** The Simulation Tracker class is used to store static global variables
** to keep track of the state of the simulation. Specifically, whether the 
** simulation is currently on the first update set of potentially more than 
** one for the current frame, that that it can store the position of entities
** before all of the udate steps take place.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _SIMULATIONTRACKER_HPP_
#define _SIMULATIONTRACKER_HPP_

namespace firemelon
{
	class SimulationTracker
	{
	public:
		
		SimulationTracker();
		virtual ~SimulationTracker();

		static bool	isFirstUpdate;

	private:


	};
}

#endif // _SIMULATIONTRACKER_HPP_