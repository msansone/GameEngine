/* -------------------------------------------------------------------------
** OpenGlRenderer.hpp
** 
** The OpenGlRenderer class is derived from the Renderer class. It implements the
** functions for initializing the screen and displaying surfaces on it using
** the OpenGL library.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

/* -------------------------------------------------------------------------
**
** Texture Packing Algoritm Description
**
** The texture packing algorithm is designed to add consecutive rectangles
** (whose locations and dimensions represent images) to an expanding area 
** (the boundary of which represents a texture).
**
** This is accomplished in two steps:
**
** The first step is the addImageRect(PackingRect r) function, which takes a 
** rectangle whose height and width are pre-set as input, and chooses an (x, y)
** location for it, based on a list of candidate locations. Each candidate location
** is looped through, and the one that results in the least amount of unused space
** is selected. For the first call to this function, the list of candidate locations
** is empty, and the location (0, 0) is chosen by default.
**
** The second step is the generateLocationCandidates() function. After addImageRect()
** finishes, it calls generateLocationCandidates(), which builds a list of
** potential locations where the next rectangle could be located at. It does 
** this by looping through each of the rectangles that have already been added
** and testing if the area directly to the right or directly below is unoccupied
** by another rectangle. If none are available it will either append the
** rectangle to the right of the texture, or below, depending on which produces
** less unused area.
**
** ------------------------------------------------------------------------- */


#ifndef _OPENGLRENDERER_HPP_
#define _OPENGLRENDERER_HPP_

#include <fstream>
#include <math.h>
#include <map>
#include <vector>


#include "SDL.h"

#include <glm/glm.hpp>
#include <glm/gtx/transform.hpp>

#include <GL/glew.h>
#include <GL/freeglut.h>
#include <GL/gl.h>
#include <GL/glu.h>
#include <stdio.h>
#include <IL/il.h>
#include <IL/ilu.h>

#include <AlphaMask.hpp>
#include <Position.hpp>
#include <PythonGil.hpp>
#include <Renderer.hpp>

#include "OpenGlSpriteSheet.hpp"
#include "OpenGlTextureAtlas.hpp"

// A rect that represents an image to be packed into the texture.
struct PackingRect 
{
	unsigned int x;
	unsigned int y;
	unsigned int w;
	unsigned int h;
};

struct Point
{
	int x;
	int y;
};

struct Line
{
	Point pt1;
	Point pt2;
};

// LocationCandidates represent locations within the texture bounds where
// image rects might be packed in at. Every time a new image rect is added,
// the list of location candidates is updated.
struct LocationCandidate
{
	PackingRect	rect;
	unsigned int area;
};

// PotentialLocations are populated with data which is used to decide 
// which location candidate will result in the least amount of unused space.
struct PotentialLocation
{
	// Where the rect will be placed, should this option be selected.
	PackingRect			rectLocation;

	unsigned int		rectArea;

	// The expanded texture bounds that will be set if this candidate is selcted.
	PackingRect			newBoundsRect;

	// The area of the new texture bounds rect.
	unsigned int		boundsArea;
};

class OpenGlRenderer : public firemelon::Renderer
{
public:

	OpenGlRenderer(int screenWidth, int screenHeight);
	virtual ~OpenGlRenderer();

	void drawPolygon(std::vector<firemelon::Vertex2> vertices, firemelon::ColorRgbaPtr color);
	
	virtual void						drawRect(float x,   float y,     int   width, int   height, 
											     float red, float green, float blue,  float alpha);

	virtual void						fillRect(float x, float y, int width, int height, 
											     float red, float green, float blue, float alpha);

	// Set up the screen using SDL.
	virtual bool						initializeScreen();

	virtual bool						initializeShader();

	// Load an image from a file into a surface. Returns the surface ID.
	virtual int							loadSpriteSheet(std::string sheetName,   std::string fileName, int   rows,        int  cols,        
		                                                int         cellHeight,  int cellWidth,        float scaleFactor, bool useTransparencyKey,
														int			padding);

	// Load an image from a memory stream into a surface. Returns the surface ID.
	virtual int							loadSpriteSheet(std::string sheetName, char* buffer,      int  filesize,    
		                                                int         rows,      int   cols,        int  cellHeight, 
		                                                int         cellWidth, float scaleFactor, bool useTransparencyKey,
														int			padding);

	// Renders the sheet with the given ID.
	virtual void					renderSheetById(float x, float y, int sheetId);

	virtual void					renderSheetById(float x, float y, int sheetId, boost::shared_ptr<firemelon::RenderEffects> renderEffects);

	// Renders a sheet cell from the surface at the given x, y location.
	virtual void						renderSheetCell(float x, float y, int sheetId, int sourceX, int sourceY);

	virtual void						renderSheetCell(float x, float y, int sheetId, int sourceX, int sourceY, boost::shared_ptr<firemelon::RenderEffects> renderEffects);

	virtual void						renderSheetCell(firemelon::Quad quad, int sheetId, int sourceX, int sourceY, boost::shared_ptr<firemelon::RenderEffects> renderEffects);

	// Renders a subsection of the sheet from the given source rect.
	virtual void						renderSheetSection(float x, float y, int sheetId, firemelon::Rect source);

	virtual void						renderSheetSection(float x, float y, int sheetId, firemelon::Rect source, boost::shared_ptr<firemelon::RenderEffects> renderEffects);

	// Fill the screen with a color.
	virtual void						fillScreen(unsigned int color);

	virtual void						sceneBegin();

	virtual void						sceneComplete();
	
	virtual firemelon::SpriteSheetPtr	getSheet(int sheetID);

	virtual firemelon::SpriteSheetPtr	getSheetByName(std::string sheetname);

	virtual int							getSheetIDByName(std::string sheetname);
	
	void								setIsFullscreenPy(bool value);
	void								setIsFullscreen(bool value);
	
	bool								getIsFullscreenPy();
	bool								getIsFullscreen();
	
	virtual void						setScreenSize(int width, int height);

	virtual int							getScreenHeight();
	virtual int							getScreenWidth();
	
	virtual void						setFadeColor(float red, float green, float blue);
	virtual void						setFadeOpacity(float opacity);
	virtual float						getFadeOpacity();

	virtual void						saveScreenshot(std::string fileName);

protected:

	GLuint			loadShaders(std::string vertexShaderName, std::string fragmentShaderName, std::string geometryShaderName);
	
	virtual void	sheetsLoaded();

private:

	bool		initializeOpenGl();
	bool		initializeDevIl();

	void		addQuad(firemelon::Quad quad, GLfloat texLeft, GLfloat texTop, GLfloat texRight, GLfloat texBottom, firemelon::RenderEffectsPtr renderEffects);

	void		buildTextureAtlas();

	void		initVao();
	void		freeVao();
	void		freeLineVao();

	bool		initVbo();
	void		freeVbo();
	void		freeLineVbo();

	void		updateVbo();
	void		updateLineVbo();

	// Add an image rect to determine where it should be packed
	// in the texture.
	void		addImageRect(PackingRect &rect);

	// Using the list of located rects, generate a list of possible
	// location candidates that a new rect might get placed at.
	void		generateLocationCandidates();

	// If the given point p exists on the given line l.
	bool		isPointOnLine(Point p, Line l);

	// Determines if the area at r.x, r.y is occupied by a located rect.
	bool		isPointFree(PackingRect r);

	// Determines if the given rect intersects with any located rects.
	bool		isAreaFree(PackingRect r);
	
	// Find the point of the collision closest to the starting point of the line.
	Point		raycast(Line ray);

	// Size a rectangle inside the bounds to the maximum size it can occupy 
	// without colliding with any located rects.
	void		inflateRect(PackingRect& rect);

	// Test whether two rects collide.
	bool		rectCollision(PackingRect r1, PackingRect r2);

	// Given a packing rect, return what the new bounds would be if
	// it were added.
	PackingRect	newBounds(PackingRect rect);

	// Determines if two line segments intersect and returns the intersection point if they do.
	bool		lineIntersection(Line first, Line second, Point& intersectionPoint);
	
	// Dot Product of two vectors.
	float		dot(Point p1, Point p2);
	
	// Perpendicular Dot Product of two vectors.
	float		perpDot(Point p1, Point p2);

	// The distance between two points/vectors.
	float		distance(Point p1, Point p2);

	std::string	readString(std::string filepath);

	std::vector<boost::shared_ptr<firemelon::SpriteSheet>>	sheets_;
	
	std::map<std::string, int>				sheetNameIDMap_;
	
	SDL_Window*								window_;
	SDL_Surface*							screen_;
	SDL_Renderer*							sdlRenderer_;
	SDL_GLContext							openGlContext_;
	
	OpenGlTextureAtlas*						textureAtlas_;
	
	GLuint									texturedQuadVao_;
	GLuint									linesVao_;

	GLuint									programId_;	
	GLuint									vertexBufferId_;
	GLuint									indexBufferId_;
	
	int										vertexBufferSize_;
	int										lineVertexBufferSize_;
	int										lineIndexBufferSize_;

	std::vector<VertexData3D>				vertexData_;
	std::vector<GLuint>						indexData_;

	GLint									vertexPos2dLocation_;
    GLint									colorLocation_;
	GLint									blendPercentLocation_;
	GLint									blendColorLocation_;
	GLint									fadeBlendPercentLocation_;
	GLint									fadeBlendColorLocation_;
	GLint									texCoordLocation_;
	GLint									alphaMaskTexCoordLocation_;
	GLint									useAlphaMaskLocation_;
	GLint									outlineColorLocation_;
    GLint									texColorLocation_;
    GLint									texUnitLocation_;
	GLint									radialFadeOriginCoordLocation_;
	GLint									radialFadeDistanceLocation_;
	
    glm::mat4								projectionMatrix_;
    GLint									projectionMatrixLocation_;

    glm::mat4								modelViewMatrix_;
    GLint									modelViewMatrixLocation_;

	glm::ivec2								windowSize_;
	GLint									windowSizeLocation_;

	glm::ivec2								textureSize_;
	GLint									textureSizeLocation_;

	GLuint									lineProgramId_;	
	GLuint									lineVertexBufferId_;
	GLuint									lineIndexBufferId_;
	
	GLuint									vertexPos2dLinesLocation_;
	GLuint									colorLinesLocation_;

    glm::mat4								projectionMatrixLines_;
	GLuint									projectionMatrixLinesLocation_;
    glm::mat4								modelViewMatrixLines_;
	GLuint									modelViewMatrixLinesLocation_;

	std::vector<ColorVertex3D>				lineVertexData_;
	std::vector<GLuint>						lineIndexData_;

	// Data for rendering to offscreen texture.
	GLenum									drawBuffers_[1];
	GLuint									frameBufferId_;
	GLuint									renderedTexture_;

	std::vector<ILuint>						imageIds_;
		
	ColorRGBA								fadeColor_;
	GLfloat									fadeColorPercent_;

	int										screenWidth_;
	int										screenHeight_;
	bool									isFullscreen_;
	bool									debug_;

	// Variables for the texture packing algorithm.

	// The current bounds of the final texture. Start at 0, 0, 0, 0 and expand as needed.
	PackingRect								bounds_;

	// A list of image packing rects which have been placed inside the bounds.
	std::vector<PackingRect>				locatedRects_;
	
	// The total area occupied by all of the locatedRects.
	unsigned int							locatedRectArea_;

	// A list of potential locations where image packing rects can be placed.
	// Gets regenerated after each new fixed rect is added.
	std::vector<LocationCandidate>			locationCandidates_;

	int										quadsAddedPerUpdate_;

	int										sheetBorderSize_;
};

#endif // _OPENGLRENDERER_HPP_