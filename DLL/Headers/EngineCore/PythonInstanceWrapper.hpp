/* -------------------------------------------------------------------------
** PythonInstanceWrapper.hpp
**
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _PYTHONINSTANCEWRAPPER_HPP_
#define _PYTHONINSTANCEWRAPPER_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <boost/python.hpp>

#include "Debugger.hpp"
#include "Types.hpp"
#include "PythonGil.hpp"

namespace firemelon
{
	class FIREMELONAPI PythonInstanceWrapper
	{
	public:
		friend class CodeBehindContainer;
		friend class Room;

		PythonInstanceWrapper();
		virtual ~PythonInstanceWrapper();

		std::string	getInstanceTypeName();
		std::string	getInstanceVariableName();
		std::string	getScriptName();

		PyObj		getPyInstance();
		PyObj		getPyInstanceNamespace();

		void		setScriptName(std::string value);

		void		setInstanceTypeName(std::string value);

		void		setInstanceVariableName(std::string value);

		void		renameInstanceVariableName(std::string value);

		//void	createInstance(std::string typeName, std::string variableName, std::string scriptName);

		void			initialize();

	protected:

		bool	getIsInitialized();

		PyObj	getMainModule();
		PyObj	getMainNamespace();

	private:

		void					cleanup();
				
		DebuggerPtr				debugger_;

		bool					isInitialized_;

		// Script metadata
		std::string				instanceTypeName_;
		std::string				instanceVariableName_;
		std::string				scriptName_;

		// Python objects
		PyObj					pyMainModule_;
		PyObj					pyMainNamespace_;

		PyObj					pyInstance_;
		PyObj					pyInstanceNamespace_;
	};
}

#endif // _PYTHONINSTANCEWRAPPER_HPP_
