/* -------------------------------------------------------------------------
** OpenGlSpriteSheet.hpp
** 
** The OpenGlSpriteSheet class is derived from the SpriteSheet class. It 
** stores the metadata used to find the texture clipping areas inside of a
** texture atlas.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _OPENGLSPRITESHEET_HPP_
#define _OPENGLSPRITESHEET_HPP_

#include <SpriteSheet.hpp>

#include <iostream>
#include <string>
#include <vector>

class OpenGlSpriteSheet : public firemelon::SpriteSheet
{
public:
	OpenGlSpriteSheet(std::string name, int rows, int cols, int cellHeight, int cellWidth, float scaleFactor);
	virtual ~OpenGlSpriteSheet();

	virtual void		freeSheet();

	void				setSheetOffsetX(int value);
	int					getSheetOffsetX();

	void				setSheetOffsetY(int value);
	int					getSheetOffsetY();
	
	void				setSheetWidth(int value);
	int					getSheetWidth();

	void				setSheetHeight(int value);
	int					getSheetHeight();

private:
	
	float				scaleFactor_;

	int					sheetOffsetX_;
	int					sheetOffsetY_;

	int					sheetHeight_;
	int					sheetWidth_;
};

#endif // _OPENGLSPRITESHEET_HPP_