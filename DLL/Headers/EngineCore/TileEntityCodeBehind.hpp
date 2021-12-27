/* -------------------------------------------------------------------------
** TileEntityCodeBehind.hpp
**
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _TILEENTITYCODEBEHIND_HPP_
#define _TILEENTITYCODEBEHIND_HPP_

#include "EntityCodeBehind.hpp"

namespace firemelon
{
	class TileEntityCodeBehind : public EntityCodeBehind
	{
	public:

		TileEntityCodeBehind();
		virtual ~TileEntityCodeBehind();

		virtual void				update(double time) {};

	protected:

	private:

		virtual void				initializeBegin() {};
		virtual void				created() {};
		virtual void				preDestroy() {};
		virtual void				destroy() {};
		virtual void				roomEntered(RoomId roomId) {};
		virtual void				roomExited(RoomId roomId) {};
	};
}

#endif // _TILEENTITYCODEBEHIND_HPP_
