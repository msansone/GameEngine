/* -------------------------------------------------------------------------
** EntityCodeBehind.hpp
**
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _ENTITYCODEBEHIND_HPP_
#define _ENTITYCODEBEHIND_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include "BaseIds.hpp"
#include "CodeBehind.hpp"
#include "EngineController.hpp"
#include "EntityScript.hpp"
//#include "EntitySerializerHolder.hpp"
#include "Messenger.hpp"
#include "Position.hpp"
#include "Types.hpp"

namespace firemelon
{
	class FIREMELONAPI EntityCodeBehind : public CodeBehind
	{
	public:
		friend class CodeBehindContainer;
		friend class GameStateManager;
		friend class Room;

		EntityCodeBehind();
		virtual ~EntityCodeBehind();

		void						setIsRemoved(bool value);
		bool						getIsRemoved();

	protected:

	private:
		
		//virtual EntitySerializer*	createEntitySerializer();

		// Virtual update function that should not be exposed to someone implementing a codebehind. This is used
		// for internal engine purposes, when defining new kinds of entity code behinds (Camera, Particle Emitter, etc.) 
		// rather than specific code behinds (FreeCamera, FireParticleEmitter, etc.)
		void						preUpdate(double time);
		virtual bool				baseUpdate(double time);
		virtual void				update(double time);


		void						preCleanup();
		virtual void				baseCleanup();
		virtual void				cleanup();

		void						preInitialize();
		virtual void				baseInitialize();
		virtual void				initialize();

		virtual void				initializeBegin();

		virtual void				created();

		virtual void				preDestroyed();
		virtual void				destroyed();

		void						preRoomEntered(RoomId roomId);
		virtual void				roomEntered(RoomId roomId);

		virtual void				roomExited(RoomId roomId);

		boost::shared_ptr<EntityScript>	entityScript_;
				
		bool							isInitialized_;

		// Flags the entity for removal.
		bool							isRemoved_;

		boost::shared_ptr<Position>		position_;
	};
}

#endif // _ENTITYCODEBEHIND_HPP_
