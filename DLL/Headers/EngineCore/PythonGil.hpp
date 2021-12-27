#ifndef _PYTHONGIL_HPP_
#define _PYTHONGIL_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <boost/python.hpp>
#include <boost/thread.hpp>

namespace firemelon
{
	class FIREMELONAPI PythonAcquireGil 
	{
	public:

		PythonAcquireGil();
		virtual ~PythonAcquireGil();

	private:		
		PyGILState_STATE state_;
	};
	
	class FIREMELONAPI PythonReleaseGil 
	{
	public:

	  PythonReleaseGil();
	  virtual ~PythonReleaseGil();

	private:
		PyThreadState *state_;
	};
}

#endif //_PYTHONGIL_HPP_