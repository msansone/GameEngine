/* -------------------------------------------------------------------------
** EngineManager.hpp
**
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _ENGINEMANAGER_HPP_
#define _ENGINEMANAGER_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include "Debugger.hpp"
#include "GameEngine.hpp"
#include <boost/python.hpp>

namespace firemelon
{	
	class FIREMELONAPI EngineManager
	{
	public:
		EngineManager();
		virtual ~EngineManager();
		
		GameEngine*	getEngine();

	private:
		
		GameEngine*	engine_;

	};
	
	FIREMELONAPI EngineManager*	CreateEngineManager();	
	FIREMELONAPI void			DeleteEngineManager(EngineManager* engineManager);

	EngineManager*	CreateEngineManager()
	{	
		return new EngineManager(); 
	};

	void DeleteEngineManager(EngineManager* engineManager)
	{ 
		if (engineManager != nullptr)
		{
			delete engineManager;
		}
	};

}

#endif // _ENGINEMANAGER_HPP_