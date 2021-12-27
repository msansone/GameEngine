/* -------------------------------------------------------------------------
** ScriptingData.hpp
**
** The ScriptingData class is used to store python script meta data.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */
#include <string>

#ifndef _SCRIPTINGDATA_HPP_
#define _SCRIPTINGDATA_HPP_

namespace firemelon
{
	class ScriptingData
	{
	public:
		ScriptingData();
		ScriptingData(std::string fileName, std::string typeName);
		virtual ~ScriptingData();
		
		std::string	getFileName();
		std::string	getScriptTypeName();

	private:
		
		std::string	fileName_;
		std::string	scriptTypeName_;
	};
}

#endif // _SCRIPTINGDATA_HPP_