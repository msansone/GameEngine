/* -------------------------------------------------------------------------
** RenderableScript.hpp
**
** The RenderableScript is the interface by which the rendering functions
** of a python script are called.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _RENDERABLESCRIPT_HPP_
#define _RENDERABLESCRIPT_HPP_

#include "BaseIds.hpp"
#include "CodeBehindScript.hpp"
#include "Debugger.hpp"
#include "StageController.hpp"
#include "Types.hpp"


namespace firemelon
{
	class RenderableScript : public CodeBehindScript
	{
	public:
		friend class RenderableCodeBehind;

		RenderableScript(DebuggerPtr debugger);
		virtual ~RenderableScript();

	protected:

	private:

		virtual void	cleanup();

		// TODO Subclass AnimatedRenderable?
		void			frameTriggered(TriggerSignalId frameTriggerSignal);

		virtual void	initialize();

		void			rendered(int x, int y);

		StageControllerPtr	stageController_;

		PyObj				pyFrameTriggered_;

		PyObj				pyRendered_;
	};
}

#endif // _RENDERABLESCRIPT_HPP_
