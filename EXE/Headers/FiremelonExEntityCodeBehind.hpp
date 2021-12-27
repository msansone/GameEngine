/* -------------------------------------------------------------------------
** FiremelonExEntityCodeBehind.hpp
**
** The FiremelonExEntityCodeBehind class is derived from the EntityCodeBehind
** class. It is used with the FiremelonExCodeBehindComponentFactory class to 
** create the derived firemelon entity codebehind components.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _FIREMELONEXENTITYCODEBEHIND_HPP_
#define _FIREMELONEXENTITYCODEBEHIND_HPP_

#include <EntityCodeBehind.hpp>

#include "BoostGameTimer.hpp"

class FiremelonExEntityCodeBehind : public firemelon::EntityCodeBehind
{
public:

	FiremelonExEntityCodeBehind();
	virtual ~FiremelonExEntityCodeBehind();

private:

	virtual void	initialize();
};

#endif // _FIREMELONEXENTITYCODEBEHIND_HPP_