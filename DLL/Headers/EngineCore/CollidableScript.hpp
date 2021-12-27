/* -------------------------------------------------------------------------
** CollidableScript.hpp
**
** The CollidableScript is the interface by which the collision functions of
** a python script are called.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _COLLIDABLESCRIPT_HPP_
#define _COLLIDABLESCRIPT_HPP_

#include "CodeBehindScript.hpp"
#include "CollisionData.hpp"
#include "Types.hpp"

namespace firemelon
{
	class CollidableScript : public CodeBehindScript
	{
	public:
		friend class CollidableCodeBehind;

		CollidableScript(DebuggerPtr debugger);

		virtual ~CollidableScript();

		void				collision(boost::shared_ptr<CollisionData> collision);

		void				collisionEnter(boost::shared_ptr<CollisionData> collision);

		void				collisionExit(boost::shared_ptr<CollisionData> collision);

		CollisionDataPtr	getCollisionData(int hitboxIndex);

		CollisionResolution	resolveCollision(boost::shared_ptr<CollisionData> collision);
				
	protected:

	private:

		virtual void	cleanup();

		virtual void	initialize();

		boost::shared_ptr<HitboxManager>	hitboxManager_;

		PyObj								pyCollision_;
		PyObj								pyCollisionEnter_;
		PyObj								pyCollisionExit_;
		PyObj								pyGetCollisionData_;
		PyObj								pyResolveCollision_;
	};
}

#endif // _COLLIDABLESCRIPT_HPP_
