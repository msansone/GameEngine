/* -------------------------------------------------------------------------
** ThreadManager.hpp
**
** The ThreadManager class provides a common location to create and access
** threads.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _THREADMANAGER_HPP_
#define _THREADMANAGER_HPP_

#include <boost/thread.hpp>
#include <boost/thread/mutex.hpp>

namespace firemelon
{
	typedef	boost::thread_group ThreadGroup;

	class ThreadManager
	{
	public:

		ThreadManager();
		virtual ~ThreadManager();
		
		ThreadGroup	workerThreads_;

	private:
		
	};
}

#endif // _THREADMANAGER_HPP_