/* -------------------------------------------------------------------------
** EntityScript.hpp
**
** The EntityScript contains the python script functions for the base entity
** methods.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _ENTITYSCRIPT_HPP_
#define _ENTITYSCRIPT_HPP_

#include "BaseIds.hpp"
#include "CodeBehindScript.hpp"
#include "Debugger.hpp"
#include "EntityController.hpp"
#include "EntityMetadata.hpp"
#include "Position.hpp"
#include "PythonInstanceWrapper.hpp"
#include "Types.hpp"

namespace firemelon
{
	class EntityScript : public CodeBehindScript
	{
	public:
		friend class EntityCodeBehind;
		friend class Room;

		EntityScript(DebuggerPtr debugger);
		virtual ~EntityScript();

		void	destroyed();
		void	initializeBegin();
		void	created();
		void	roomEntered(RoomId roomId);
		void	roomExited(RoomId roomId);
		void	update(double time);

	protected:

	private:

		void	cleanup();
		void	initialize();

		EntityControllerPtr	controller_;

		stringmap							entityProperties_;

		boost::shared_ptr<EntityMetadata>	metadata_;
		boost::shared_ptr<Position>			position_;

		PyObj								pyCreated_;
		PyObj								pyDestroyed_;
		PyObj								pyRoomEntered_;
		PyObj								pyRoomExited_;
		PyObj								pyUpdate_;
	};
}

#endif // _ENTITYSCRIPT_HPP_
