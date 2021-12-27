#include "..\Headers\OpenGlTextureAtlas.hpp"

OpenGlTextureAtlas::OpenGlTextureAtlas()
{
	//Initialize texture ID
	textureId_ = 0;

	//Initialize image dimensions
	imageWidth_ = 0;
	imageHeight_ = 0;

	//Initialize texture dimensions
	textureWidth_ = 0;
	textureHeight_ = 0;

	useNpotTexture_ = true;
}

OpenGlTextureAtlas::~OpenGlTextureAtlas()
{		
	//Free texture data if needed
	freeTexture();
}
    
bool OpenGlTextureAtlas::addTextureFromPixels(GLuint* pixels, GLuint imageWidth, GLuint imageHeight)
{		
	if (textureId_ == 0)
	{
		//Get image dimensions
		imageWidth_ = imageWidth;
		imageHeight_ = imageHeight;

		//Calculate required texture dimensions
		if (useNpotTexture_ == true)
		{
			textureWidth_ = imageWidth;
			textureHeight_ = imageHeight;
		}
		else
		{
			textureWidth_ = powerOfTwo(imageWidth);
			textureHeight_ = powerOfTwo(imageHeight);
		}
		
		// Generate texture ID
		glGenTextures(1, &textureId_);

		// Bind texture ID
		glBindTexture(GL_TEXTURE_2D, textureId_);

		// Generate texture
		glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, 
					 textureWidth_, textureHeight_, 
					 0, GL_RGBA, GL_UNSIGNED_BYTE, pixels);

		//Set texture parameters
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_BORDER);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_BORDER);

		//Unbind texture
		glBindTexture(GL_TEXTURE_2D, NULL);

		//Check for error
		GLenum error = glGetError();

		if (error != GL_NO_ERROR)
		{
			std::cout << "Error loading texture from byte array: " << gluErrorString(error) << std::endl;
			return false;
		}
	}

	return true;
}

void OpenGlTextureAtlas::freeTexture()
{
	//Delete texture
	if( textureId_ != 0 )
	{
		glDeleteTextures( 1, &textureId_ );
		textureId_ = 0;
	}
		
	imageWidth_ = 0;
	imageHeight_ = 0;
	textureWidth_ = 0;
	textureHeight_ = 0;
}

GLuint OpenGlTextureAtlas::getTextureId()
{
	return textureId_;
}

GLuint OpenGlTextureAtlas::getTextureWidth()
{
	return textureWidth_;
}

GLuint OpenGlTextureAtlas::getTextureHeight()
{
	return textureHeight_;
}

GLuint OpenGlTextureAtlas::powerOfTwo(GLuint num)
{
	if( num != 0 )
	{
		num--;
		num |= (num >> 1); //Or first 2 bits
		num |= (num >> 2); //Or next 2 bits
		num |= (num >> 4); //Or next 4 bits
		num |= (num >> 8); //Or next 8 bits
		num |= (num >> 16); //Or next 16 bits
		num++;
	}

	return num;
}
