/* -------------------------------------------------------------------------
** RenderableCodeBehind.hpp
**
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _RENDERABLECODEBEHIND_HPP_
#define _RENDERABLECODEBEHIND_HPP_

#include "CodeBehind.hpp"
#include "RenderableScript.hpp"
#include "Types.hpp"

namespace firemelon
{
	class RenderableCodeBehind : public CodeBehind
	{
	public:
		friend class CodeBehindContainer;
		friend class Entity;
		friend class GameStateManager;

		RenderableCodeBehind();
		virtual ~RenderableCodeBehind();

		void	preCleanup();
		void	preInitialize();

	protected:

		virtual void	cleanup();

		virtual void	frameTriggered(TriggerSignalId frameTriggerSignal);

		virtual void	rendered(int x, int y);

		virtual void	initialize();

	private:

		boost::shared_ptr<Renderable>				renderable_;

		boost::shared_ptr<RenderableScript>			renderableScript_;

		StageControllerPtr							stageController_;
	};
}

#endif // _RENDERABLECODEBEHIND_HPP_