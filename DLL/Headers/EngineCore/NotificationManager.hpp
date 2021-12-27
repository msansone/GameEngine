/* -------------------------------------------------------------------------
** NotificationManager.hpp
**
** This class is used to display notifications. It is a base class which must
** be implemented by the user, so that they can use whatever library they want
** to display the message (i.e. Windows message box, et al).
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _NOTIFICATION_MANAGER_HPP_
#define _NOTIFICATION_MANAGER_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <boost/make_shared.hpp>
#include <boost/shared_ptr.hpp>

namespace firemelon
{
	class FIREMELONAPI NotificationManager
	{
	public:

		NotificationManager();
		virtual ~NotificationManager();

		virtual void	displayNotification(std::string message);

	protected:

	private:

	};

	typedef boost::shared_ptr<NotificationManager> NotificationManagerPtr;	
}

#endif // _NOTIFICATION_MANAGER_HPP_