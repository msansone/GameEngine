/* -------------------------------------------------------------------------
** RotationOperation.hpp
**
** The RotationOperation class represents a rotation operation to be applied
** to a sprite. It contains an angle and a pivot point. The RenderEffects
** class stores a list of RotationOperation objects, so multiple different
** rotations can be applied.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _ROTATIONOPERATION_HPP_
#define _ROTATIONOPERATION_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include "Position.hpp"

namespace firemelon
{
	class FIREMELONAPI RotationOperation
	{
	public:
		friend class StageElements;

		RotationOperation(float angle, int pivotPointX, int pivotPointY);
		virtual ~RotationOperation();

		PositionPtr	getPivotPointPy();
		PositionPtr	getPivotPoint();

		float		getAnglePy();
		float		getAngle();

		void		setAnglePy(float angle);
		void		setAngle(float angle);

	private:

		float		clampAngle(float angle);

		float		angle_;

		PositionPtr	pivotPoint_;

		float		previousAngle_;

		float		previousPreviousAngle_;
	};

	typedef boost::shared_ptr<RotationOperation>	RotationOperationPtr;
	typedef std::vector<RotationOperationPtr>		RotationOperationPtrList;
}

#endif // _ROTATIONOPERATION_HPP_
