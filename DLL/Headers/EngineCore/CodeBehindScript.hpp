/* -------------------------------------------------------------------------
** CodeBehindScript.hpp
**
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _CODEBEHINDCOMPONENTSCRIPT_HPP_
#define _CODEBEHINDCOMPONENTSCRIPT_HPP_

#include "Debugger.hpp"
#include "PythonInstanceWrapper.hpp"
#include "Types.hpp"

namespace firemelon
{
	class CodeBehindScript
	{
	public:

		CodeBehindScript(DebuggerPtr debugger);
		virtual ~CodeBehindScript();

		boost::shared_ptr<PythonInstanceWrapper>	getPythonInstanceWrapper();
		void										setPythonInstanceWrapper(boost::shared_ptr<PythonInstanceWrapper> pythonInstanceWrapper);
	
		void										preCleanup();
		void										preInitialize();

	protected:

		DebuggerPtr									debugger_;

	private:

		virtual void	cleanup();
		virtual void	initialize();


		bool										isInitialized_;

		boost::shared_ptr<PythonInstanceWrapper>	pythonInstanceWrapper_;
	};
}

#endif // _CODEBEHINDCOMPONENTSCRIPT_HPP_
