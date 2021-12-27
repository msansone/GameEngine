/* -------------------------------------------------------------------------
** TileCollidableCodeBehind.hpp
**
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _TILECOLLIDABLECODEBEHIND_HPP_
#define _TILECOLLIDABLECODEBEHIND_HPP_

#include "CollidableCodeBehind.hpp"
#include "TileCollisionData.hpp"

namespace firemelon
{
	class TileCollidableCodeBehind : public CollidableCodeBehind
	{
	public:

		TileCollidableCodeBehind();
		virtual ~TileCollidableCodeBehind();

		void										setTileGroupId(unsigned int tileGroupId);

	private:

		virtual void								collision(boost::shared_ptr<CollisionData> collisionData);

		virtual void								collisionEnter(boost::shared_ptr<CollisionData> collisionData);

		virtual void								collisionExit(boost::shared_ptr<CollisionData> collisionData);
		
		virtual boost::shared_ptr<CollisionData>	getCollisionData(int hitboxIndex);

		virtual boost::shared_ptr<CollisionData>	createCollisionData();

		virtual void								cleanup();

		virtual	void								initialize();

		virtual CollisionResolution					resolveCollision(boost::shared_ptr<CollisionData> collisionData);

		PyObj										pyCollisionData_;

		boost::shared_ptr<TileCollisionData>		tileCollisionData_;

		unsigned int								tileGroupId_;
	};
}

#endif // _TILECOLLIDABLECODEBEHIND_HPP_