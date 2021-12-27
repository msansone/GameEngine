/* -------------------------------------------------------------------------
** SpriteSheet.hpp
**
** The SpriteSheet class is the generic base class that the user should inherit
** from. It is used to store surfaces created by the renderer, as well as free them,
** using whichever graphics library the user chooses. Sprite sheets contains the 
** column, row, cell height, and cell width data used to split the surface into 
** the subsections that are used as animation frames.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _SPRITESHEET_HPP_
#define _SPRITESHEET_HPP_

#if defined(FIREMELON_EXPORTS)
#   define FIREMELONAPI   __declspec(dllexport)
#else
#   define FIREMELONAPI   __declspec(dllimport)
#endif  // FIREMELON_EXPORTS

#include <iostream>
#include <string>
#include <vector>

#include "PythonGil.hpp"

namespace firemelon
{
	class FIREMELONAPI SpriteSheet
	{
	public:
		SpriteSheet(std::string name, int rows, int cols, int cellHeight, int cellWidth, float scaleFactor);
		virtual ~SpriteSheet();

		virtual void	freeSheet(); // = 0; // This should be abstract, but VS2015 breaks boost python for some reason, and this is now needed as a workaround.

		int				getIdPy();
		int				getId();

		void			setColumnsPy(int columns);
		void			setColumns(int columns);
		int				getColumnsPy();
		int				getColumns();
		
		void			setRowsPy(int rows);
		void			setRows(int rows);
		int				getRowsPy();
		int				getRows();
		
		void			setCellHeightPy(int cellHeight);
		void			setCellHeight(int cellHeight);
		int				getCellHeightPy();
		int				getCellHeight();
		
		void			setCellWidthPy(int cellWidth);
		void			setCellWidth(int cellWidth);
		int				getCellWidthPy();
		int				getCellWidth();

		void			setPadding(int value);
		int				getPadding();

		float			getScaleFactorPy();
		float			getScaleFactor();

		std::string		getSheetName();

	protected:
	
		int				rows_;
		int				cols_;
		int				cellHeight_;
		int				cellWidth_;
		int				cellCount_;

		int				padding_;

		float			scaleFactor_;

		std::string		name_;
		int				id_;

	private:
	
		static int		idCounter_;
	};

	typedef boost::shared_ptr<SpriteSheet> SpriteSheetPtr;
	typedef std::vector<SpriteSheetPtr> SpriteSheetPtrList;
}

#endif // _SPRITESHEET_HPP_