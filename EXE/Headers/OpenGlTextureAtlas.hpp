/* -------------------------------------------------------------------------
** OpenGlTextureAtlas.hpp
** 
** The OpenGlTexture class is used to load and store a texture made up of
** all the images. It will also keep track of where each image begins and ends.
**
** Author: Mike Sansone
** ------------------------------------------------------------------------- */

#ifndef _OPENGLTEXTUREATLAS_HPP_
#define _OPENGLTEXTUREATLAS_HPP_

#include <GL/glew.h>
#include <GL/freeglut.h>
#include <GL/gl.h>
#include <GL/glu.h>
#include <stdio.h>
#include <IL/il.h>
#include <IL/ilu.h>

#include <vector>
#include <string>
#include <iostream>

struct FRect
{
    GLfloat x;
    GLfloat y;
    GLfloat w;
    GLfloat h;
};

struct ColorRGBA
{
    GLfloat r;
    GLfloat g;
    GLfloat b;
    GLfloat a;
};

struct TexCoord
{
    GLfloat s;
    GLfloat t;
};

struct VertexPos3D
{
    GLfloat x;
	GLfloat y;
	GLfloat z;
};

struct VertexData3D
{
    VertexPos3D	pos;
	ColorRGBA	rgba;
	TexCoord	texCoord;
	TexCoord	alphaMaskTexCoord;
	GLint		useAlphaMask;
	ColorRGBA	rgbaOutline;
	GLfloat		fadeBlendPercent;
	ColorRGBA	rgbaFadeBlend;
	GLfloat		blendPercent;
	ColorRGBA	rgbaBlend;
	VertexPos3D radialFadeOriginCoord;
	GLfloat		radialFadeDistance;
};

struct ColorVertex3D
{
    VertexPos3D pos;
    ColorRGBA	rgba;
};

class OpenGlTextureAtlas
{
public:
    OpenGlTextureAtlas();
    virtual ~OpenGlTextureAtlas();
    
	// Takes a PNG image as a byte array, determines where it will
	// fit on the texture atlas, or expands it if there isn't enough room,
	// and copies it to the atlas.
    bool	addTextureFromPixels(GLuint* pixels, GLuint imageWidth, GLuint imageHeight);
   
	void	freeTexture();

    GLuint	getTextureId();
    GLuint	getTextureWidth();
    GLuint	getTextureHeight();

private:
	
	GLuint	powerOfTwo(GLuint num);


    // Texture name
    GLuint			textureId_;

	// Texture dimensions
    GLuint			textureWidth_;
    GLuint			textureHeight_;

    // Unpadded image dimensions
    GLuint			imageWidth_;
    GLuint			imageHeight_;

	// Use a non-power of two texture
	bool			useNpotTexture_;
};

#endif // _OPENGLTEXTUREATLAS_HPP_