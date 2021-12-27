/* -------------------------------------------------------------------------
** AlphaMask.hpp
**
** The AlphaMask contains the metadata used to apply an alpha mask to a sprite,
** including the sprite sheet ID of the alpha mask data, and the source cell
** position.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _ALPHAMASK_HPP_
#define _ALPHAMASK_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include "PythonGil.hpp"

namespace firemelon
{
	class FIREMELONAPI AlphaMask
	{
	public:

		AlphaMask();
		virtual ~AlphaMask();

		int		getSheetIdPy();
		int		getSheetId();

		void	setSheetIdPy(int value);
		void	setSheetId(int value);

		int		getSheetCellColumn();
		int		getSheetCellColumnPy();
		void	setSheetCellColumn(int value);
		void	setSheetCellColumnPy(int value);

		int		getSheetCellRow();
		int		getSheetCellRowPy();
		void	setSheetCellRow(int value);
		void	setSheetCellRowPy(int value);

	private:

		int sheetCellColumn_;
		int sheetCellRow_;
		int	sheetId_;

	};

	typedef boost::shared_ptr<AlphaMask>	AlphaMaskPtr;
	typedef std::vector<AlphaMaskPtr>		AlphaMaskPtrList;
}

#endif // _ALPHAMASK_HPP_