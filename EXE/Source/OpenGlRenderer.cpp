#include "..\Headers\OpenGlRenderer.hpp"

#include <glm/gtc/type_ptr.hpp>

using namespace firemelon;

OpenGlRenderer::OpenGlRenderer(int screenWidth, int screenHeight) : Renderer()
{
	screenWidth_ = screenWidth;
	screenHeight_ = screenHeight;
	isFullscreen_ = false;
	colorLocation_ = 0;
	projectionMatrixLocation_ = 0;
    modelViewMatrixLocation_ = 0;
	windowSizeLocation_ = 0;
	textureSizeLocation_ = 0;
	texCoordLocation_ = 0;
	alphaMaskTexCoordLocation_ = 0;
	useAlphaMaskLocation_ = 0;
	outlineColorLocation_ = 0;
	texUnitLocation_ = 0;
	texColorLocation_ = 0;
	radialFadeOriginCoordLocation_ = 0;
	radialFadeDistanceLocation_ = 0;
	
	texturedQuadVao_ = 0;
	linesVao_ = 0;

	textureAtlas_ = new OpenGlTextureAtlas();

	vertexBufferId_ = 0;
	indexBufferId_ = 0;
	
	lineVertexBufferId_ = 0;
	lineIndexBufferId_ = 0;

	vertexPos2dLinesLocation_ = 0;
	colorLinesLocation_ = 0;
	projectionMatrixLinesLocation_ = 0;
	modelViewMatrixLinesLocation_ = 0;

	// The initial buffer size will be set to 500, so won't need
	// to re-allocate unless the new size is > 500.
	vertexBufferSize_ = 500;
	lineVertexBufferSize_ = 500;
	lineIndexBufferSize_ = lineVertexBufferSize_ * 2;
	
	frameBufferId_ = 0;

	bounds_.x = 0;
	bounds_.y = 0;
	bounds_.w = 0;
	bounds_.h = 0;

	fadeColor_.a = 1.0f;
	fadeColor_.r = 1.0f;
	fadeColor_.g = 1.0f;
	fadeColor_.b = 1.0f;

	fadeColorPercent_ = 0.0f;

	locatedRectArea_ = 0;
	quadsAddedPerUpdate_ = 0;

	sheetBorderSize_ = 1;


	debug_ = false;
}

OpenGlRenderer::~OpenGlRenderer()
{
	freeVbo();
	freeLineVbo();

	freeVao();
	freeLineVao();

	delete textureAtlas_;
	
	glDeleteProgram(programId_);

	glDeleteProgram(lineProgramId_);

	SDL_GL_DeleteContext(openGlContext_);
    SDL_DestroyWindow(window_);
}

void OpenGlRenderer::drawPolygon(std::vector<Vertex2> vertices, ColorRgbaPtr color)
{
	// Polygons require at least 2 verticies
	if (vertices.size() > 1)
	{
		int firstPolygonIndex = lineVertexData_.size();

		// Render a polygon to the screen. Create the lines from the vertices in order.
		for (size_t i = 0; i < vertices.size(); i++)
		{
			ColorVertex3D currentVertex;

			currentVertex.pos.x = vertices[i].x;
			currentVertex.pos.y = vertices[i].y;
			currentVertex.pos.z = 0.0f;

			currentVertex.rgba.r = color->getR();
			currentVertex.rgba.g = color->getG();
			currentVertex.rgba.b = color->getB();
			currentVertex.rgba.a = color->getA();

			lineVertexData_.push_back(currentVertex);

			if (i > 0)
			{
				lineIndexData_.push_back(lineVertexData_.size() - 2);
				lineIndexData_.push_back(lineVertexData_.size() - 1);
			}
		}

		// No more vertices to add. Connect the last vertex added to the first in the line index data.
		lineIndexData_.push_back(lineVertexData_.size() - 1);
		lineIndexData_.push_back(firstPolygonIndex);
	}

}

int OpenGlRenderer::loadSpriteSheet(std::string sheetname, std::string filename, int rows, int cols, int cellHeight, int cellWidth, float scaleFactor, bool useTransparencyKey, int padding)
{		
	// Currently unused. Implement if I need it later.
	return -1;
}

// Creates a sprite sheet from a byte array.
int OpenGlRenderer::loadSpriteSheet(std::string sheetname, char* buffer, int filesize, int rows, int cols, int cellHeight, int cellWidth, float scaleFactor, bool useTransparencyKey, int padding)
{	
	std::cout << "Loading sprite sheet " << sheetname << std::endl;

#if defined(_DEBUG)	

	if (sheetname == "Stonemason")
	{
		bool debug = true;
	}

#endif

	// Generate and set current image ID
	ILuint imgID = 0;
	ilGenImages(1, &imgID);
	ilBindImage(imgID);

	ILboolean success = ilLoadL(IL_PNG, buffer, filesize);
	 
	ILinfo imageInfo;

	//Image loaded successfully
	if (success == IL_TRUE)
	{
		//Convert image to RGBA
        success = ilConvertImage(IL_RGBA, IL_UNSIGNED_BYTE);
		
		if (success == IL_TRUE)
        {
			iluGetImageInfo(&imageInfo);
			
			GLuint textureWidth = 0;
			GLuint textureHeight = 0;

			if (useTransparencyKey == true)
			{			
				GLuint size = (imageInfo.Width * imageInfo.Height) * 4;

				for (int i = 0; i < size; i += 4 )
				{
					//Get pixel color and test if it is the transparency color key.

					ILubyte r = imageInfo.Data[i];
					ILubyte g = imageInfo.Data[i + 1];
					ILubyte b = imageInfo.Data[i + 2];

					// Color key is 0xFF00FF for magenta.
					if (r == 0xFF && g == 0 && b == 0xFF)
					{
						//Make transparent
						imageInfo.Data[i] = 0xFF;
						imageInfo.Data[i + 1] = 0xFF;
						imageInfo.Data[i + 2] = 0xFF;
						imageInfo.Data[i + 3] = 0;
					}					
				}
			}

			//Texture is the wrong size
			if( imageInfo.Width != textureWidth || imageInfo.Height != textureHeight )
			{
				//Place image at upper left
				iluImageParameter(ILU_PLACEMENT, ILU_UPPER_LEFT);

				//Resize image
				iluEnlargeCanvas((int)textureWidth, (int)textureHeight, 1);
			}

			PackingRect rect;
			rect.h = imageInfo.Height;
			rect.w = imageInfo.Width;

			addImageRect(rect);

			imageIds_.push_back(imgID);

			// Add the padding to the cell height and cell width. This is a requirement of the renderer, because there needs to be space between cells when the shader calculates
			// the sprite outlines. If this wasn't there, it could potential detect the edges in an adjacent cell and draw an outline that it shouldn't.

			// Note: The images needs to be rendered *with the padding* as a part of the rendering image, otherwise, pixels in the image that touch the very bottom row won't have an outline applied,
			// because the shader will not operate on any pixels outside of the image rect. Including the padding provides an extra row for the shader to operate on, in which
			// it can draw the outline if necessary.

			// I think it only needs to do this if a sheet cell padding value is set, though, because some images (such as tilesheet environment graphics) don't get outlined.
			// and they won't interfere with other outlines, because the implicit outline around them is always automatically added.

			boost::shared_ptr<OpenGlSpriteSheet> newSheet = boost::shared_ptr<OpenGlSpriteSheet>(new OpenGlSpriteSheet(sheetname, rows, cols, cellHeight, cellWidth, scaleFactor));
	
			// Set the padding between cells.
			newSheet->setPadding(padding);

			// Add 1 to account for the single pixel alpha border.
			newSheet->setSheetOffsetX(rect.x + 1);
			newSheet->setSheetOffsetY(rect.y + 1);

			// Subtract 2 to account for the single pixel alpha border.
			newSheet->setSheetWidth(imageInfo.Width - 2);
			newSheet->setSheetHeight(imageInfo.Height - 2);

			sheets_.push_back(newSheet);

			sheetNameIDMap_[sheetname] = sheets_.size() - 1;

			return sheets_.size() - 1;
		}
		else
		{
			ILenum error = ilGetError();

			std::cout<<"Failed to convert image pixels to RGBA format: "<<iluErrorString(error)<<std::endl;

			return -1;
		}
	}
	else
	{
		ILenum error = ilGetError();

		std::cout<<"Failed to load sprite sheet image: "<<iluErrorString(error)<<std::endl;

		return -1;
	}
}


void OpenGlRenderer::buildTextureAtlas()
{
	// Loop through the images loaded into DevIL and append them together
	// veritcally. Use this image to create a texture.
	ILuint atlasImageId_;
	ilGenImages(1, &atlasImageId_);
	ilBindImage(atlasImageId_);
	
	int bufferSize = bounds_.w * bounds_.h * 4;

	char* buffer = new char[bufferSize];

	if (debug_ == true)
	{
		memset(buffer, 0xFF, bufferSize);
	}
	else
	{
		memset(buffer, 0, bufferSize);
	}

	ILboolean success = ilTexImage((ILuint)bounds_.w, (ILuint)bounds_.h,
								   1,				   4,
								   IL_RGBA,			   IL_UNSIGNED_BYTE,
								   buffer);

	// Now loop through the images and copy them into the large image.
	int size = imageIds_.size();

	for (int i = 0; i < size; i++)
	{
		boost::shared_ptr<OpenGlSpriteSheet> sheet = boost::static_pointer_cast<OpenGlSpriteSheet>(sheets_[i]);

		// Bind to the current image to get its data.
		ILuint imgID = imageIds_[i];
		
		ilBindImage(imgID);

		ILinfo imageInfo;

		iluGetImageInfo(&imageInfo);

		// Bind the atlas again to copy the data over.
		ilBindImage(atlasImageId_);

		// Subtract 1 to account for the single pixel alpha border.
		int xOff = sheet->getSheetOffsetX() - 1;

		int yOff = sheet->getSheetOffsetY() - 1;

		ilSetPixels(xOff, yOff, 0, 
					imageInfo.Width,
					imageInfo.Height, 
					1, 
					IL_RGBA,
					IL_UNSIGNED_BYTE, 
					imageInfo.Data);

		if (debug_ == true)
		{
			std::cout << sheets_[i]->getSheetName() << " " << xOff << ", " << yOff << std::endl;
			
			ilEnable(IL_FILE_OVERWRITE);

			ilSave(IL_PNG, "TextureAtlas.png");
		}

		// No longer need this image.
		ilDeleteImage(imgID);
	}

	// This image now contains every sheet. Use it to create the texture in the texture atlas.
	ILinfo imageInfo;
	
	iluGetImageInfo(&imageInfo);

	textureAtlas_->addTextureFromPixels((GLuint*)imageInfo.Data, imageInfo.Width, imageInfo.Height);


	// Now that the texture has been created, we are done with the
	// buffer and image. Free them.
	delete buffer;

	ilDeleteImage(atlasImageId_);

	imageIds_.clear();
}

void OpenGlRenderer::sheetsLoaded()
{
	// All sheets have now been loaded. Build the texture
	// atlas from the image data.
	buildTextureAtlas();
}

void OpenGlRenderer::renderSheetById(float x, float y, int sheetId)
{
	boost::shared_ptr<RenderEffects> renderEffects = boost::shared_ptr<firemelon::RenderEffects>(new RenderEffects);

	renderSheetById(x, y, sheetId, renderEffects);
}

void OpenGlRenderer::renderSheetById(float x, float y, int sheetId, boost::shared_ptr<firemelon::RenderEffects> renderEffects)
{
	int size = sheets_.size();

	// Render an entire sprite sheet at location x, y.
	if (sheetId >= 0 && sheetId < size)
	{
		boost::shared_ptr<OpenGlSpriteSheet> openGlSheet = boost::static_pointer_cast<OpenGlSpriteSheet>(sheets_[sheetId]);

		Rect source;

		source.x = 0;

		source.y = 0;

		source.h = openGlSheet->getSheetHeight();

		source.w = openGlSheet->getSheetWidth();

		renderSheetSection(x, y, sheetId, source, renderEffects);
	}
	else
	{
		std::cout << "Unable to render. Sprite sheet ID " << sheetId << " not found." << std::endl;
	}
}

void OpenGlRenderer::renderSheetSection(float x, float y, int sheetId, Rect source)
{
	boost::shared_ptr<RenderEffects> renderEffects = boost::shared_ptr<firemelon::RenderEffects>(new RenderEffects);

	renderSheetSection(x, y, sheetId, source, renderEffects);
}

void OpenGlRenderer::renderSheetSection(float x, float y, int sheetId, Rect source, boost::shared_ptr<firemelon::RenderEffects> renderEffects)
{
	// Render a clipped rectangle from a sprite sheet at location x, y.
	boost::shared_ptr<OpenGlSpriteSheet> openGlSheet = boost::static_pointer_cast<OpenGlSpriteSheet>(sheets_[sheetId]);

	float scaleFactor = openGlSheet->getScaleFactor() * renderEffects->getScaleFactor();

	//If the texture exists
    if (textureAtlas_->getTextureId() != 0)
    {				
		// Use the sprite sheet offset and height/ width to determine the 
		// texture coordinates that will map to the requested image.
		int sheetOffsetX = openGlSheet->getSheetOffsetX();
		int sheetOffsetY = openGlSheet->getSheetOffsetY();


		// Vertex coordinates
		GLfloat quadWidth = source.w * scaleFactor;
		GLfloat quadHeight = source.h * scaleFactor;

		float quadLeft = (int)x;
		float quadRight = (int)(x + quadWidth);
		float quadTop = (int)y;
		float quadBottom = (int)(y + quadHeight);


		// Texture coordinates
		int textureHeight = textureAtlas_->getTextureHeight();
		int textureWidth = textureAtlas_->getTextureWidth();

		GLfloat texLeft   = (GLfloat)(sheetOffsetX + source.x) / (GLfloat)textureWidth;
		GLfloat texRight  = (GLfloat)(sheetOffsetX + source.x + source.w) / (GLfloat)textureWidth;
		GLfloat texTop    = (GLfloat)(sheetOffsetY + source.y) / (GLfloat)textureHeight;
		GLfloat texBottom = (GLfloat)(sheetOffsetY + source.y + source.h) / (GLfloat)textureHeight;

		float extentLeft   = renderEffects->getExtentLeft();
		float extentRight  = renderEffects->getExtentRight();
		float extentTop    = renderEffects->getExtentTop();
		float extentBottom = renderEffects->getExtentBottom();

		if (extentLeft != -1.0 || extentRight != 1.0 || extentTop != -1.0 || extentBottom != 1.0)
		{
			// Source coordinates
			float srcLeft = sheetOffsetX + source.x;
			float srcRight = sheetOffsetX + source.x + source.w;
			float srcTop = sheetOffsetY + source.y;
			float srcBottom = sheetOffsetY + source.y + source.h;

			float srcWidth = srcRight - srcLeft;
			float srcHalfWidth = (srcWidth / 2);
			float srcMidPointX = srcLeft + srcHalfWidth;

			float srcHeight = srcBottom - srcTop;
			float srcHalfHeight = (srcHeight / 2);
			float srcMidPointY = srcTop + srcHalfHeight;

			srcLeft = srcMidPointX + (extentLeft * srcHalfWidth);
			srcRight = srcMidPointX + (extentRight * srcHalfWidth);
			srcTop = srcMidPointY + (extentTop * srcHalfHeight);
			srcBottom = srcMidPointY + (extentBottom * srcHalfHeight);

			// Texture coordinates
			texLeft = (GLfloat)(srcLeft) / (GLfloat)textureWidth;
			texRight = (GLfloat)(srcRight) / (GLfloat)textureWidth;
			texTop = (GLfloat)(srcTop) / (GLfloat)textureHeight;
			texBottom = (GLfloat)(srcBottom) / (GLfloat)textureHeight;
			
			// Scale the quad based on the extents.
			srcLeft = x;
			srcRight = x + quadWidth;
			srcTop = y;
			srcBottom = y + quadHeight;

			srcWidth = srcRight - srcLeft;
			srcHalfWidth = (srcWidth / 2);
			srcMidPointX = srcLeft + srcHalfWidth;

			srcHeight = srcBottom - srcTop;
			srcHalfHeight = (srcHeight / 2);
			srcMidPointY = srcTop + srcHalfHeight;

			quadLeft = srcMidPointX + (extentLeft * srcHalfWidth);
			quadRight = srcMidPointX + (extentRight * srcHalfWidth);
			quadWidth = quadRight - quadLeft;

			quadTop = srcMidPointY + (extentTop * srcHalfHeight);
			quadBottom = srcMidPointY + (extentBottom * srcHalfHeight);
			quadHeight = quadBottom - quadTop;
		}

		//if (renderEffects->getMirrorHorizontal() == true)
		//{
		//	GLfloat swap = texLeft;
		//	texLeft = texRight;
		//	texRight = swap;
		//}

		Quad quad;

		quad.vertices[0].x = quadLeft;
		quad.vertices[0].y = quadTop;

		quad.vertices[1].x = quadRight;
		quad.vertices[1].y = quadTop;

		quad.vertices[2].x = quadRight;
		quad.vertices[2].y = quadBottom;

		quad.vertices[3].x = quadLeft;
		quad.vertices[3].y = quadBottom;

		// TODO apply any transforms to the quad before adding it.

		addQuad(quad, texLeft, texTop, texRight, texBottom, renderEffects);
    }
}

void OpenGlRenderer::renderSheetCell(float x, float y, int sheetId, int sourceX, int sourceY)
{
	boost::shared_ptr<RenderEffects> renderEffects = boost::shared_ptr<firemelon::RenderEffects>(new RenderEffects);

	renderSheetCell(x, y, sheetId, sourceX, sourceY, renderEffects);
}

void OpenGlRenderer::renderSheetCell(float x, float y, int sheetId, int sourceX, int sourceY, boost::shared_ptr<RenderEffects> renderEffects)
{
	int size = sheets_.size();

	// Render a clipped rectangle from a sprite sheet at location x, y.
	if (sheetId >= 0 && sheetId < size)
	{
		boost::shared_ptr<OpenGlSpriteSheet> openGlSheet = boost::static_pointer_cast<OpenGlSpriteSheet>(sheets_[sheetId]);
		
		float scaleFactor = openGlSheet->getScaleFactor() * renderEffects->getScaleFactor();

		//If the texture exists
		if (textureAtlas_->getTextureId() != 0)
		{
			// Use the sprite sheet offset and height/ width within the atlas to determine the 
			// texture coordinates that will map to the requested image.
			int sheetOffsetX = openGlSheet->getSheetOffsetX();
			int sheetOffsetY = openGlSheet->getSheetOffsetY();

			// Use the sprite sheet data to calculate the source rect.
			int rows = openGlSheet->getRows();
			int cols = openGlSheet->getColumns();

			int cellHeight = openGlSheet->getCellHeight();
			int cellWidth = openGlSheet->getCellWidth();
			int padding = openGlSheet->getPadding();

			FRect source;

			int srcX = sourceX;

			if (srcX > cols)
			{
				srcX = cols;
			}

			int srcY = sourceY;

			if (srcY > rows)
			{
				srcY = rows;
			}

			//int divider = 1; // need to figure out what to do with this. I could just make it a rule that every sheet has a divided of 1,
			// in fact I may have to, because I added the 1 pixel border, and I'm going to have to use 1 pixel of that when I get the 
			// texture atlas coordinates. EVery image will assume it has 1 extra pixel around the edges and offset by that amount
			// when rendering. and then any operation on graphics will need to account for this.

			// This is kind of tricky because tile sheets don't have cells like sprite sheets they have regions, so for these they would not
			// account for dividers

			source.x = srcX * (cellWidth);
			source.y = srcY * (cellHeight);
			source.h = cellHeight;
			source.w = cellWidth;

			// Determine the vertex coordinates for the quad that will be rendered to.
			GLfloat quadWidth  = source.w * scaleFactor;
			GLfloat quadHeight = source.h * scaleFactor;

			float quadLeft   = (int)x;
			float quadRight  = (int)(x + quadWidth);
			float quadTop    = (int)y;
			float quadBottom = (int)(y + quadHeight);

			// Texture coordinates
			int textureHeight = textureAtlas_->getTextureHeight();
			int textureWidth = textureAtlas_->getTextureWidth();

			GLfloat texLeft =   (GLfloat)(sheetOffsetX + source.x) / (GLfloat)textureWidth;
			GLfloat texRight =  (GLfloat)(sheetOffsetX + source.x + source.w) / (GLfloat)textureWidth;
			GLfloat texTop =    (GLfloat)(sheetOffsetY + source.y) / (GLfloat)textureHeight;
			GLfloat texBottom = (GLfloat)(sheetOffsetY + source.y + source.h) / (GLfloat)textureHeight;

			// Build the quad vertex coordinates and texture coordinates based on the extents.
			float extentLeft = renderEffects->getExtentLeft();
			float extentRight = renderEffects->getExtentRight();
			float extentTop = renderEffects->getExtentTop();
			float extentBottom = renderEffects->getExtentBottom();

			if (extentLeft != -1.0 || extentRight != 1.0 || extentTop != -1.0 || extentBottom != 1.0)
			{
				// Source coordinates
				float srcLeft = sheetOffsetX + source.x;
				float srcRight = sheetOffsetX + source.x + source.w;
				float srcTop = sheetOffsetY + source.y;
				float srcBottom = sheetOffsetY + source.y + source.h;
				
				float srcWidth = srcRight - srcLeft;
				float srcHalfWidth = (srcWidth / 2);
				float srcMidPointX = srcLeft + srcHalfWidth;

				float srcHeight = srcBottom - srcTop;
				float srcHalfHeight = (srcHeight / 2);
				float srcMidPointY = srcTop + srcHalfHeight;

				srcLeft = srcMidPointX + (extentLeft * srcHalfWidth);
				srcRight = srcMidPointX + (extentRight * srcHalfWidth);
				srcTop = srcMidPointY + (extentTop * srcHalfHeight);
				srcBottom = srcMidPointY + (extentBottom * srcHalfHeight);

				// Texture coordinates
				texLeft = (GLfloat)(srcLeft) / (GLfloat)textureWidth;
				texRight = (GLfloat)(srcRight) / (GLfloat)textureWidth;
				texTop = (GLfloat)(srcTop) / (GLfloat)textureHeight;
				texBottom = (GLfloat)(srcBottom) / (GLfloat)textureHeight;

				// Scale the quad based on the extents.
				srcLeft = quadLeft;
				srcRight = quadRight;
				srcTop = quadTop;
				srcBottom = quadBottom;

				srcWidth = srcRight - srcLeft;
				srcHalfWidth = (srcWidth / 2);
				srcMidPointX = srcLeft + srcHalfWidth;

				srcHeight = srcBottom - srcTop;
				srcHalfHeight = (srcHeight / 2);
				srcMidPointY = srcTop + srcHalfHeight;

				// Set the quad data.
				quadLeft = srcMidPointX + (extentLeft * srcHalfWidth);
				quadRight = srcMidPointX + (extentRight * srcHalfWidth);
				quadWidth = quadRight - quadLeft;

				quadTop = srcMidPointY + (extentTop * srcHalfHeight);
				quadBottom = srcMidPointY + (extentBottom * srcHalfHeight);
				quadHeight = quadBottom - quadTop;
			}

			//			if (renderEffects->getMirrorHorizontal() == true)
			//			{
			//				GLfloat swap = texLeft;
			//				texLeft = texRight;
			//				texRight = swap;
			//			}

			Quad quad;

			quad.vertices[0].x = quadLeft;
			quad.vertices[0].y = quadTop;

			quad.vertices[1].x = quadRight;
			quad.vertices[1].y = quadTop;

			quad.vertices[2].x = quadRight;
			quad.vertices[2].y = quadBottom;

			quad.vertices[3].x = quadLeft;
			quad.vertices[3].y = quadBottom;

			// TODO apply any transforms to the quad before adding it.

			addQuad(quad, texLeft, texTop, texRight, texBottom, renderEffects);
		}
	}
	else
	{
		std::cout << "Unable to render. Sprite sheet ID " << sheetId << " not found." << std::endl;
	}
}

void OpenGlRenderer::renderSheetCell(Quad quad, int sheetId, int sourceX, int sourceY, boost::shared_ptr<RenderEffects> renderEffects)
{
	int size = sheets_.size();

	// Render a clipped rectangle from a sprite sheet into the specified quad
	if (sheetId >= 0 && sheetId < size)
	{
		boost::shared_ptr<OpenGlSpriteSheet> openGlSheet = boost::static_pointer_cast<OpenGlSpriteSheet>(sheets_[sheetId]);
		
		float scaleFactor = openGlSheet->getScaleFactor() * renderEffects->getScaleFactor();

		// Make sure the texture exists
		if (textureAtlas_->getTextureId() != 0)
		{
			// Texture atlas dimensions
			int textureHeight = textureAtlas_->getTextureHeight();

			int textureWidth = textureAtlas_->getTextureWidth();

			// Use the sprite sheet offset and height / width within the atlas to determine the texture coordinates that will map to the requested image.
			int sheetOffsetX = openGlSheet->getSheetOffsetX();

			int sheetOffsetY = openGlSheet->getSheetOffsetY();

			// Calculate the source rect within the sprite sheet..
			int rows = openGlSheet->getRows();

			int cols = openGlSheet->getColumns();

			int cellHeight = openGlSheet->getCellHeight();

			int cellWidth = openGlSheet->getCellWidth();

			int padding = openGlSheet->getPadding();

			FRect source;

			int srcX = sourceX;

			if (srcX > cols)
			{
				srcX = cols;
			}

			int srcY = sourceY;

			if (srcY > rows)
			{
				srcY = rows;
			}

			// If there is a padding, assume it is a padding of 2. Only paddings of 0 and 2 are valid.
			bool usePadding = padding > 0;
			
			if (usePadding > 0 || true)
			{
				source.x = (srcX * (cellWidth + 2));

				source.y = (srcY * (cellHeight + 2));

				source.h = cellHeight + 2;

				source.w = cellWidth + 2;

				// Adjust the sheet offset by 1 so that it starts at the border.
				sheetOffsetX--;

				sheetOffsetY--;
			}
			else
			{
				source.x = srcX * (cellWidth);

				source.y = srcY * (cellHeight);

				source.h = cellHeight;

				source.w = cellWidth;
			}
			
			// Gather the alpha mask sheet data if it's used. Eventually this will be replaced and the alpha channel itself will be used.
			boost::shared_ptr<OpenGlSpriteSheet> openGlAlphaMaskSheet = nullptr;

			boost::shared_ptr<AlphaMask> alphaMask = renderEffects->getAlphaMask();

			int alphaMaskSheetId = alphaMask->getSheetId();

			int alphaMaskSheetOffsetX = 0;
			int alphaMaskSheetOffsetY = 0;

			int alphaMaskRows = 0;
			int alphaMaskCols = 0;

			FRect alphaMaskSource;

			bool useAlphaMask = false;

			if (alphaMaskSheetId > -1)
			{
				int alphaMaskSrcX = alphaMask->getSheetCellColumn();

				int alphaMaskSrcY = alphaMask->getSheetCellRow();

				if (alphaMaskSrcX != -1 && alphaMaskSrcY != -1)
				{
					useAlphaMask = true;

					boost::shared_ptr<OpenGlSpriteSheet> openGlAlphaMaskSheet = boost::static_pointer_cast<OpenGlSpriteSheet>(sheets_[alphaMaskSheetId]);

					alphaMaskSheetOffsetX = openGlAlphaMaskSheet->getSheetOffsetX();

					alphaMaskSheetOffsetY = openGlAlphaMaskSheet->getSheetOffsetY();

					int alphaMaskRows = openGlAlphaMaskSheet->getRows();

					int alphaMaskCols = openGlAlphaMaskSheet->getColumns();

					// Construct the source rect from the cell column & row.

					if (alphaMaskSrcX > cols)
					{
						alphaMaskSrcX = cols;
					}


					if (alphaMaskSrcY > rows)
					{
						alphaMaskSrcY = rows;
					}

					alphaMaskSource.x = alphaMaskSrcX * cellWidth;
					alphaMaskSource.y = alphaMaskSrcY * cellHeight;
					alphaMaskSource.h = cellHeight;
					alphaMaskSource.w = cellWidth;
				}
			}

			// Texture coordinates
			GLfloat texLeft = 0;
			GLfloat texRight = 0;
			GLfloat texTop = 0;
			GLfloat texBottom = 0;

			// Modify the source rect based on the extents, if only a subset of the image should be rendered.
			float extentLeft = renderEffects->getExtentLeft();
			float extentRight = renderEffects->getExtentRight();
			float extentTop = renderEffects->getExtentTop();
			float extentBottom = renderEffects->getExtentBottom();

			if (extentLeft != -1.0 || extentRight != 1.0 || extentTop != -1.0 || extentBottom != 1.0)
			{
				// Source coordinates
				float srcLeft = sheetOffsetX + source.x;
				float srcRight = sheetOffsetX + source.x + source.w;
				float srcTop = sheetOffsetY + source.y;
				float srcBottom = sheetOffsetY + source.y + source.h;
				
				float srcWidth = srcRight - srcLeft;
				float srcHalfWidth = (srcWidth / 2);
				float srcMidPointX = srcLeft + srcHalfWidth;

				float srcHeight = srcBottom - srcTop;
				float srcHalfHeight = (srcHeight / 2);
				float srcMidPointY = srcTop + srcHalfHeight;

				srcLeft = srcMidPointX + (extentLeft * srcHalfWidth);
				srcRight = srcMidPointX + (extentRight * srcHalfWidth);
				srcTop = srcMidPointY + (extentTop * srcHalfHeight);
				srcBottom = srcMidPointY + (extentBottom * srcHalfHeight);

				// Texture coordinates
				texLeft = (GLfloat)(srcLeft) / (GLfloat)textureWidth;
				texRight = (GLfloat)(srcRight) / (GLfloat)textureWidth;
				texTop = (GLfloat)(srcTop) / (GLfloat)textureHeight;
				texBottom = (GLfloat)(srcBottom) / (GLfloat)textureHeight;
				
				// Resize the quad.

				// Convert the extents from a range of -1 to 1 to a range of 0 to 1 (i.e. normalized, or a percentage).

				// First, add one to both extremes, then divide each by 2. 
				// Examples:
				// A range of [-1 to 1] is transformed to [0 to 2] and then [0 to 1]
				// A range of [0 to 1] becomes [1 to 2] and then [0.5 to 1]
				// A range of [0.25 to 0.75] becomes [1.25 to 1.75] and then [0.625 to 0.875]
				extentLeft += 1.0;
				
				extentRight += 1.0;

				extentBottom += 1.0;

				extentTop += 1.0;
				
				extentLeft /= 2.0;

				extentRight /= 2.0;

				extentBottom /= 2.0;

				extentTop /= 2.0;

				// Vertex coordinates
				GLfloat quadWidth = source.w * scaleFactor;
				GLfloat quadHeight = source.h * scaleFactor;

				Vertex2 topEdge;

				topEdge.x = quad.vertices[1].x - quad.vertices[0].x;
				topEdge.y = quad.vertices[1].y - quad.vertices[0].y;

				Vertex2 leftEdge;

				leftEdge.x = quad.vertices[3].x - quad.vertices[0].x;
				leftEdge.y = quad.vertices[3].y - quad.vertices[0].y;

				// Skew the "width" of the box first. Then skew the "height".
				Vertex2 horizontalSkew1;

				horizontalSkew1.x = topEdge.x * extentLeft;
				horizontalSkew1.y = topEdge.y * extentLeft;

				Vertex2 horizontalSkew2;

				horizontalSkew2.x = topEdge.x * extentRight;
				horizontalSkew2.y = topEdge.y * extentRight;

				Vertex2 verticalSkew1;

				verticalSkew1.x = leftEdge.x * extentTop;
				verticalSkew1.y = leftEdge.y * extentTop;

				Vertex2 verticalSkew2;

				verticalSkew2.x = leftEdge.x * extentBottom;
				verticalSkew2.y = leftEdge.y * extentBottom;

				// Calculate the skewed points by taking the first point and moving it along the skew vectors.

				// "Top left" corner.
				Vertex2 tlc = quad.vertices[0];

				// "Bottom left" corner.
				Vertex2 blc = quad.vertices[3];

				// Adjust the coordinates to "squeeze" the width.
				quad.vertices[0].x = tlc.x + horizontalSkew1.x;
				quad.vertices[0].y = tlc.y + horizontalSkew1.y;

				quad.vertices[1].x = tlc.x + horizontalSkew2.x;
				quad.vertices[1].y = tlc.y + horizontalSkew2.y;

				quad.vertices[3].x = blc.x + horizontalSkew1.x;
				quad.vertices[3].y = blc.y + horizontalSkew1.y;

				quad.vertices[2].x = blc.x + horizontalSkew2.x;
				quad.vertices[2].y = blc.y + horizontalSkew2.y;

				// Adjust the coordinats to "squeeze" the height.
				tlc = quad.vertices[0];

				// "Top right" corner.
				Vertex2 trc = quad.vertices[1];
				
				quad.vertices[0].x = tlc.x + verticalSkew1.x;
				quad.vertices[0].y = tlc.y + verticalSkew1.y;

				quad.vertices[1].x = trc.x + verticalSkew1.x;
				quad.vertices[1].y = trc.y + verticalSkew1.y;

				quad.vertices[3].x = tlc.x + verticalSkew2.x;
				quad.vertices[3].y = tlc.y + verticalSkew2.y;

				quad.vertices[2].x = trc.x + verticalSkew2.x;
				quad.vertices[2].y = trc.y + verticalSkew2.y;
			}
			else
			{
				// Texture coordinates
				texLeft = (GLfloat)(sheetOffsetX + source.x) / (GLfloat)textureWidth;

				texRight = (GLfloat)(sheetOffsetX + source.x + source.w) / (GLfloat)textureWidth;

				texTop = (GLfloat)(sheetOffsetY + source.y) / (GLfloat)textureHeight;

				texBottom = (GLfloat)(sheetOffsetY + source.y + source.h) / (GLfloat)textureHeight;
			}

			addQuad(quad, texLeft, texTop, texRight, texBottom, renderEffects);
		}
	}
	else
	{
		std::cout << "Unable to render. Sprite sheet ID " << sheetId << " not found." << std::endl;
	}
}

void OpenGlRenderer::drawRect(float x, float y, int width, int height, float red, float green, float blue, float alpha)
{
	if ((x + screenWidth_ >= 0 || x <= screenWidth_) && (y + screenHeight_ >= 0 || y <= screenHeight_))
	{
		// Add vertices to draw lines to a VBO		
		ColorVertex3D rectLines[4];

		rectLines[0].pos.x = x;
		rectLines[0].pos.y = y;
		rectLines[0].pos.z = 0.0f;
		rectLines[0].rgba.r = red;
		rectLines[0].rgba.g = green;
		rectLines[0].rgba.b = blue;
		rectLines[0].rgba.a = alpha;
	
		rectLines[1].pos.x = x + width;
		rectLines[1].pos.y = y;
		rectLines[1].pos.z = 0.0f;
		rectLines[1].rgba.r = red;
		rectLines[1].rgba.g = green;
		rectLines[1].rgba.b = blue;
		rectLines[1].rgba.a = alpha;

		rectLines[2].pos.x = x + width;
		rectLines[2].pos.y = y + height;
		rectLines[2].pos.z = 0.0f;
		rectLines[2].rgba.r = red;
		rectLines[2].rgba.g = green;
		rectLines[2].rgba.b = blue;
		rectLines[2].rgba.a = alpha;

		rectLines[3].pos.x = x;
		rectLines[3].pos.y = y + height;
		rectLines[3].pos.z = 0.0f;
		rectLines[3].rgba.r = red;
		rectLines[3].rgba.g = green;
		rectLines[3].rgba.b = blue;
		rectLines[3].rgba.a = alpha;
	
		lineVertexData_.push_back(rectLines[0]);
		lineVertexData_.push_back(rectLines[1]);
		lineVertexData_.push_back(rectLines[2]);
		lineVertexData_.push_back(rectLines[3]);

		lineIndexData_.push_back(lineVertexData_.size() - 4);	
		lineIndexData_.push_back(lineVertexData_.size() - 3);
	
		lineIndexData_.push_back(lineVertexData_.size() - 3);
		lineIndexData_.push_back(lineVertexData_.size() - 2);
	
		lineIndexData_.push_back(lineVertexData_.size() - 2);
		lineIndexData_.push_back(lineVertexData_.size() - 1);

		lineIndexData_.push_back(lineVertexData_.size() - 1);
		lineIndexData_.push_back(lineVertexData_.size() - 4);
	}
}

void OpenGlRenderer::fillRect(float x, float y, int width, int height, float red, float green, float blue, float alpha)
{
	// Implement when necessesary.
 }

boost::shared_ptr<SpriteSheet> OpenGlRenderer::getSheet(int sheetID)
{
	int size = sheets_.size();
	if (sheetID >= 0 && sheetID < size)
	{
		return sheets_[sheetID];
	}

	return nullptr;
}

void OpenGlRenderer::fillScreen(unsigned int color)
{
	//SDL_FillRect(screen_, 0, color);
}

void OpenGlRenderer::sceneBegin()
{
	quadsAddedPerUpdate_ = 0;

	glClearColor(0.0f, 0.0f, 0.0f, 0.0f);

	// Clear color buffer
	glClear(GL_COLOR_BUFFER_BIT);

	vertexData_.clear();
	indexData_.clear();

	lineVertexData_.clear();
	lineIndexData_.clear();
}

void OpenGlRenderer::sceneComplete()
{
	//glBindFramebuffer(GL_FRAMEBUFFER, frameBufferId_);
	//glViewport(0, 0, 1024, 768);
	
	// Render the textured quads.	
	GLuint vertexCount = vertexData_.size();

	if (vertexCount > 0)
	{
		glUseProgram(programId_);
	
		GLuint textureId = textureAtlas_->getTextureId();

		updateVbo();

		glBindTexture(GL_TEXTURE_2D, textureId);

		glBindVertexArray(texturedQuadVao_);
		glDrawElements(GL_QUADS, vertexCount, GL_UNSIGNED_INT, NULL);

		glBindVertexArray(NULL);
	}
	
	// Render the untextured geometry.	
	GLuint lineVertexCount = lineVertexData_.size();
	GLuint lineIndexCount = lineIndexData_.size();

	if (lineVertexCount > 0)
	{
		glUseProgram(lineProgramId_);
	
		updateLineVbo();
		
		glBindVertexArray(linesVao_);
		glDrawElements(GL_LINES, lineIndexCount, GL_UNSIGNED_INT, NULL);

		glBindVertexArray(NULL);		
	}

	//std::cout << "quadsAddedPerUpdate_=" << quadsAddedPerUpdate_ << std::endl;
	SDL_GL_SwapWindow(window_);
}

bool OpenGlRenderer::initializeScreen()
{	
	if (window_ != NULL)
	{
		SDL_DestroyWindow(window_);
	}

	// Create the window via SDL
	window_ = SDL_CreateWindow("Firemelon Engine", 
							   SDL_WINDOWPOS_CENTERED, 
							   SDL_WINDOWPOS_CENTERED, 
							   screenWidth_, 
							   screenHeight_, 
							   SDL_WINDOW_SHOWN | SDL_WINDOW_OPENGL);
	

    if (window_ == NULL)
    {
		std::cout<<"Window creation failed with error: "<<SDL_GetError()<<std::endl;
    }

	SDL_ShowCursor(1);
	
	screen_ = SDL_GetWindowSurface(window_);
	
	if (screen_ == nullptr)
	{
		return false;
	}

	// Create the renderer.
	sdlRenderer_ = SDL_CreateRenderer(window_, -1, SDL_RENDERER_ACCELERATED);

    if (sdlRenderer_ == nullptr)
    {
        std::cout<<"Renderer creation failed with error: "<<SDL_GetError()<<std::endl;
		
		return false;
    }
	
	if (initializeOpenGl() == false)
	{
		return false;
	}

	if (initializeDevIl() == false)
	{
		return false;
	}

	return true;
}

bool OpenGlRenderer::initializeDevIl()
{	
    //Initialize DevIL
	ilInit();
    iluInit();
    ilClearColour(255, 255, 255, 000);

    //Check for error
    ILenum ilError = ilGetError();

    if( ilError != IL_NO_ERROR )
    {
        std::cout<<"Error initializing DevIL: "<<iluErrorString(ilError)<<std::endl;
        return false;
    }

	return true;
}

bool OpenGlRenderer::initializeOpenGl()
{
	//SDL_GL_SetAttribute(SDL_GL_CONTEXT_MAJOR_VERSION, 4);
	//SDL_GL_SetAttribute(SDL_GL_CONTEXT_MINOR_VERSION, 5);
	SDL_GL_SetAttribute(SDL_GL_CONTEXT_PROFILE_MASK, SDL_GL_CONTEXT_PROFILE_CORE);

	openGlContext_ = SDL_GL_CreateContext(window_);

	if (openGlContext_ == NULL)
	{
		std::cout << "OpenGL context creation failed with error: " << SDL_GetError() << std::endl;
	}

	//Initialize GLEW
	GLenum glewError = glewInit();

	if (glewError != GLEW_OK)
	{
		std::cout << "Error initializing GLEW: " << glewGetErrorString(glewError) << std::endl;
		return false;
	}

	//Make sure OpenGL 2.1 is supported
	if (!GLEW_VERSION_2_1)
	{
		std::cout << "OpenGL 2.1 not supported" << std::endl;
		return false;
	}

	std::cout << "GLEW version: " << glewGetString(GLEW_VERSION) << std::endl;

	//Set the viewport
	glViewport(0.f, 0.f, screenWidth_, screenHeight_);

	//Initialize clear color
	glClearColor(0.f, 0.f, 0.f, 1.f);

	//Enable texturing
	glEnable(GL_TEXTURE_2D);

	//Set blending
	glEnable(GL_BLEND);
	glDisable(GL_DEPTH_TEST);
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

	//Check for error
	GLenum error = glGetError();

	if (error != GL_NO_ERROR)
	{
		std::cout << "OpenGL renderer initialization failed with error: " << gluErrorString(error) << std::endl;

		return false;
	}

	std::cout << "OpenGL version " << glGetString(GL_VERSION) << std::endl;
	std::cout << "GLSL version " << glGetString(GL_SHADING_LANGUAGE_VERSION) << std::endl;
}

bool OpenGlRenderer::initializeShader()
{
	GLenum error;

	programId_ = loadShaders("vertex", "fragment_texture", "geometry");
	
	lineProgramId_ = loadShaders("vertex", "fragment", "geometry");

	glUseProgram(programId_);
	
	vertexPos2dLocation_ = glGetAttribLocation(programId_, "vertexPos3D");
	
	colorLocation_ = glGetAttribLocation(programId_, "color_in");
	blendPercentLocation_ = glGetAttribLocation(programId_, "blendPercent_in");
	blendColorLocation_ = glGetAttribLocation(programId_, "blendColor_in");
	fadeBlendPercentLocation_ = glGetAttribLocation(programId_, "fadeBlendPercent_in");
	fadeBlendColorLocation_ = glGetAttribLocation(programId_, "fadeBlendColor_in");
	texCoordLocation_ = glGetAttribLocation(programId_, "texCoord_in");
	alphaMaskTexCoordLocation_ = glGetAttribLocation(programId_, "alphaMaskTexCoord_in");
	useAlphaMaskLocation_ = glGetAttribLocation(programId_, "useAlphaMask_in");
	outlineColorLocation_ = glGetAttribLocation(programId_, "outlineColor_in");
	texUnitLocation_ = glGetUniformLocation(programId_, "textureUnit");
	radialFadeOriginCoordLocation_ = glGetAttribLocation(programId_, "radialFadeOriginCoord_in");
	radialFadeDistanceLocation_ = glGetAttribLocation(programId_, "radialFadeDistance_in");
	projectionMatrixLocation_ = glGetUniformLocation(programId_, "projectionMatrix");    
	modelViewMatrixLocation_ = glGetUniformLocation(programId_, "modelViewMatrix");
	windowSizeLocation_ = glGetUniformLocation(programId_, "windowSize");
	textureSizeLocation_ = glGetUniformLocation(programId_, "textureSize");

	// Initialize the projection matrix
	projectionMatrix_ = glm::ortho<GLfloat>(0.0, screenWidth_, screenHeight_, 0.0, 1.0, -1.0);
	glUniformMatrix4fv(projectionMatrixLocation_, 1, GL_FALSE, glm::value_ptr(projectionMatrix_));
	
    //Initialize modelview
	modelViewMatrix_ = glm::mat4();
    glUniformMatrix4fv(modelViewMatrixLocation_, 1, GL_FALSE, glm::value_ptr(modelViewMatrix_));

	// Initialize the screen size.
	windowSize_ = glm::ivec2();
	glUniform2i(windowSizeLocation_, screenWidth_, screenHeight_);

	textureSize_ = glm::ivec2();
	glUniform2i(textureSizeLocation_, bounds_.w, bounds_.h);

	glUniform1i(texUnitLocation_, 0);
	
	glUseProgram(lineProgramId_);
		
	vertexPos2dLinesLocation_ = glGetAttribLocation(lineProgramId_, "vertexPos3D");
	colorLinesLocation_ = glGetAttribLocation(lineProgramId_, "color_in");
	projectionMatrixLinesLocation_ = glGetUniformLocation(lineProgramId_, "projectionMatrix");    
	modelViewMatrixLinesLocation_ = glGetUniformLocation(lineProgramId_, "modelViewMatrix");
	
	projectionMatrixLines_ = glm::ortho<GLfloat>(0.0, screenWidth_, screenHeight_, 0.0, 1.0, -1.0);
	glUniformMatrix4fv(projectionMatrixLinesLocation_, 1, GL_FALSE, glm::value_ptr(projectionMatrixLines_));	

	modelViewMatrix_ = glm::mat4();
    glUniformMatrix4fv(modelViewMatrixLinesLocation_, 1, GL_FALSE, glm::value_ptr(modelViewMatrixLines_));

	// Initialize the vertex buffer and index buffer objects that
	// will be used to render the quads.
	bool vboInitOk = initVbo();

	if (vboInitOk == false)
	{
		return false;
	}

	//Generate textured quad VAO
    glGenVertexArrays(1, &texturedQuadVao_);

    //Bind vertex array
    glBindVertexArray(texturedQuadVao_);

	//Check for error
	error = glGetError();

	if (error != GL_NO_ERROR)
	{
		std::cout << "Error binding vertex array: " << gluErrorString(error) << std::endl;
	}

	// Enable vertex attributes.
	glEnableVertexAttribArray(vertexPos2dLocation_);
	glEnableVertexAttribArray(colorLocation_);
	glEnableVertexAttribArray(blendPercentLocation_);
	glEnableVertexAttribArray(blendColorLocation_);
	glEnableVertexAttribArray(fadeBlendPercentLocation_);
	glEnableVertexAttribArray(fadeBlendColorLocation_);
	glEnableVertexAttribArray(radialFadeOriginCoordLocation_);
	glEnableVertexAttribArray(radialFadeDistanceLocation_);
	glEnableVertexAttribArray(texCoordLocation_);
	glEnableVertexAttribArray(alphaMaskTexCoordLocation_);
	glEnableVertexAttribArray(useAlphaMaskLocation_);
	glEnableVertexAttribArray(outlineColorLocation_);

	//Check for error
	error = glGetError();

	if (error != GL_NO_ERROR)
	{
		std::cout << "Error enabling vertex attributes: " << gluErrorString(error) << std::endl;
		return false;
	}

	//Set vertex data
	glBindBuffer(GL_ARRAY_BUFFER, vertexBufferId_);

	glVertexAttribPointer(vertexPos2dLocation_, 
						  3, 
						  GL_FLOAT, 
						  GL_FALSE, 
						  sizeof(VertexData3D), 
						  (GLvoid*)offsetof(VertexData3D, pos));

	glVertexAttribPointer(colorLocation_, 
						  4, 
						  GL_FLOAT, 
						  GL_FALSE, 
						  sizeof(VertexData3D), 
						  (GLvoid*)offsetof(VertexData3D, rgba));

	glVertexAttribPointer(texCoordLocation_,
						  2,
						  GL_FLOAT,
						  GL_FALSE,
						  sizeof(VertexData3D),
						  (GLvoid*)offsetof(VertexData3D, texCoord));

	glVertexAttribPointer(alphaMaskTexCoordLocation_,
						  2,
						  GL_FLOAT,
						  GL_FALSE,
						  sizeof(VertexData3D),
						  (GLvoid*)offsetof(VertexData3D, alphaMaskTexCoord));

	glVertexAttribPointer(useAlphaMaskLocation_,
						  1,
						  GL_INT,
						  GL_FALSE,
						  sizeof(VertexData3D),
						  (GLvoid*)offsetof(VertexData3D, useAlphaMask));
	
	glVertexAttribPointer(outlineColorLocation_,
						  4,
						  GL_FLOAT,
						  GL_FALSE,
						  sizeof(VertexData3D),
						  (GLvoid*)offsetof(VertexData3D, rgbaOutline));

	glVertexAttribPointer(fadeBlendPercentLocation_,
				  		  1,
						  GL_FLOAT,
						  GL_FALSE,
						  sizeof(VertexData3D),
						  (GLvoid*)offsetof(VertexData3D, fadeBlendPercent));

	glVertexAttribPointer(fadeBlendColorLocation_,
						  4,
						  GL_FLOAT,
						  GL_FALSE,
						  sizeof(VertexData3D),
						  (GLvoid*)offsetof(VertexData3D, rgbaFadeBlend));

	glVertexAttribPointer(blendPercentLocation_,
						  1,
						  GL_FLOAT,
						  GL_FALSE,
						  sizeof(VertexData3D),
						  (GLvoid*)offsetof(VertexData3D, blendPercent));

	glVertexAttribPointer(blendColorLocation_,
						  4,
						  GL_FLOAT,
						  GL_FALSE,
						  sizeof(VertexData3D),
						  (GLvoid*)offsetof(VertexData3D, rgbaBlend));

	glVertexAttribPointer(radialFadeOriginCoordLocation_,
						  2,
						  GL_FLOAT,
						  GL_FALSE,
						  sizeof(VertexData3D),
						  (GLvoid*)offsetof(VertexData3D, radialFadeOriginCoord));
	
	glVertexAttribPointer(radialFadeDistanceLocation_,
						  2,
						  GL_FLOAT,
						  GL_FALSE,
						  sizeof(VertexData3D),
						  (GLvoid*)offsetof(VertexData3D, radialFadeDistance));


	//Check for error
	error = glGetError();

	if (error != GL_NO_ERROR)
	{
		std::cout << "Error setting vertex data: " << gluErrorString(error) << std::endl;
		return false;
	}


	glVertexPointer(2, GL_FLOAT, 0, NULL);

	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, indexBufferId_);
		
    //Unbind VAO
    glBindVertexArray(NULL);

	//Generate lines VAO
    glGenVertexArrays(1, &linesVao_);

    //Bind vertex array
    glBindVertexArray(linesVao_);

	// Enable vertex attributes.
	glEnableVertexAttribArray(vertexPos2dLinesLocation_);

	glEnableVertexAttribArray(colorLinesLocation_);

	//Set vertex data
	glBindBuffer(GL_ARRAY_BUFFER, lineVertexBufferId_);
	
	glVertexAttribPointer(vertexPos2dLinesLocation_, 
						  3, 
						  GL_FLOAT, 
						  GL_FALSE, 
						  sizeof(ColorVertex3D), 
						  (GLvoid*)offsetof(ColorVertex3D, pos));

	glVertexAttribPointer(colorLinesLocation_, 
						  4, 
						  GL_FLOAT, 
						  GL_FALSE, 
						  sizeof(ColorVertex3D), 
						  (GLvoid*)offsetof(ColorVertex3D, rgba));
	

	glVertexPointer(2, GL_FLOAT, 0, NULL);

	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, lineIndexBufferId_);
	
    //Unbind VAO
    glBindVertexArray(NULL);

	//Check for error
	error = glGetError();

	if (error != GL_NO_ERROR)
	{
		std::cout << "Error initializing shaders: " << gluErrorString(error) << std::endl;
		return false;
	}

	return true;
}

int OpenGlRenderer::getSheetIDByName(std::string sheetname)
{
	return sheetNameIDMap_[sheetname];
}

boost::shared_ptr<SpriteSheet> OpenGlRenderer::getSheetByName(std::string sheetname)
{
	int sheetIndex = sheetNameIDMap_[sheetname];

	return sheets_[sheetIndex];
}

void OpenGlRenderer::setScreenSize(int width, int height)
{
	screenWidth_ = width;
	screenHeight_ = height;
}

int OpenGlRenderer::getScreenHeight()
{
	return screenHeight_;
}

int OpenGlRenderer::getScreenWidth()
{
	return screenWidth_;
}

void OpenGlRenderer::setIsFullscreenPy(bool value)
{
	PythonReleaseGil unlocker;

	setIsFullscreen(value);
}

void OpenGlRenderer::setIsFullscreen(bool value)
{
	isFullscreen_ = value;

	if (isFullscreen_ == true)
	{
		int result = SDL_SetWindowFullscreen(window_, SDL_WINDOW_FULLSCREEN);


		if (result == -1)
		{
			std::cout << "Setting fullscreen failed with error: " << SDL_GetError() << std::endl;
		}
		//SDL_SetWindowPosition(window_, 100, 100);
		//glViewport(0, 0, screenWidth_ + (screenWidth_ * .25), screenHeight_ + (screenHeight_ * 0.25) );
		//glViewport(0, 0, 1600, 900);
		//1600×900
	}
	else
	{
		SDL_SetWindowFullscreen(window_, 0);
		//SDL_SetWindowPosition(window_, SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED);
	}
}

bool OpenGlRenderer::getIsFullscreenPy()
{
	PythonReleaseGil unlocker;

	return getIsFullscreen();
}

bool OpenGlRenderer::getIsFullscreen()
{
	return isFullscreen_;
}

std::string OpenGlRenderer::readString(std::string filepath)
{
	std::string result = "";

    std::ifstream stream(filepath, std::ios::in);
    	
	if(stream.is_open())
    {
        std::string line = "";

        while (getline(stream, line))
		{
            result += "\n" + line;
		}

        stream.close();
    }

	return result;
}

// Create and load the code for a vertex and fragment shader, 
// compile and link, and return the program ID.
GLuint OpenGlRenderer::loadShaders(std::string vertexShaderName, std::string fragmentShaderName, std::string geometryShaderName)
{
	std::string shaderPath = "Source\\Shaders\\";
	std::string shaderExtension = ".glsl";

	std::string vertexShaderFullPath = shaderPath + vertexShaderName + shaderExtension;
	std::string fragmentShaderFullPath = shaderPath + fragmentShaderName + shaderExtension;
	//std::string geometryShaderFullPath = shaderPath + geometryShaderName + shaderExtension;
	
	// Read the code for the shaders into strings.
	std::string vertexShaderCode = readString(vertexShaderFullPath);
	std::string fragmentShaderCode = readString(fragmentShaderFullPath);
	//std::string geometryShaderCode = readString(geometryShaderFullPath);
    
	// Create the shaders
    GLuint vertexShaderId = glCreateShader(GL_VERTEX_SHADER);
    GLuint fragmentShaderId = glCreateShader(GL_FRAGMENT_SHADER);
	//GLuint geometryShaderId = glCreateShader(GL_GEOMETRY_SHADER);

	GLint Result = GL_FALSE;
    int InfoLogLength;
 
    // Compile Vertex Shader
    char const* vertexSourcePointer = vertexShaderCode.c_str();
    glShaderSource(vertexShaderId, 1, &vertexSourcePointer, NULL);
    glCompileShader(vertexShaderId);
 
    // Check Vertex Shader
    glGetShaderiv(vertexShaderId, GL_COMPILE_STATUS, &Result);

    glGetShaderiv(vertexShaderId, GL_INFO_LOG_LENGTH, &InfoLogLength);
	
	if (InfoLogLength > 0)
	{
		std::vector<char> vertexShaderErrorMessage(InfoLogLength);

		glGetShaderInfoLog(vertexShaderId, InfoLogLength, NULL, &vertexShaderErrorMessage[0]);

		std::cout<<&vertexShaderErrorMessage[0]<<std::endl;
	}


	//// Compile Geometry Shader
	//char const* geometrySourcePointer = geometryShaderCode.c_str();
	//glShaderSource(geometryShaderId, 1, &geometrySourcePointer, NULL);
	//glCompileShader(geometryShaderId);

	//// Check Geometry Shader
	//glGetShaderiv(geometryShaderId, GL_COMPILE_STATUS, &Result);
	//glGetShaderiv(geometryShaderId, GL_INFO_LOG_LENGTH, &InfoLogLength);

	//if (InfoLogLength > 0)
	//{
	//	std::vector<char> geometryShaderErrorMessage(InfoLogLength);
	//	glGetShaderInfoLog(geometryShaderId, InfoLogLength, NULL, &geometryShaderErrorMessage[0]);
	//	std::cout << &geometryShaderErrorMessage[0] << std::endl;
	//}


    // Compile Fragment Shader
    char const* fragmentSourcePointer = fragmentShaderCode.c_str();
    glShaderSource(fragmentShaderId, 1, &fragmentSourcePointer, NULL);
    glCompileShader(fragmentShaderId);
 
    // Check Fragment Shader
    glGetShaderiv(fragmentShaderId, GL_COMPILE_STATUS, &Result);
    glGetShaderiv(fragmentShaderId, GL_INFO_LOG_LENGTH, &InfoLogLength);
	
	if (InfoLogLength > 0)
	{
		std::vector<char> fragmentShaderErrorMessage(InfoLogLength);
		glGetShaderInfoLog(fragmentShaderId, InfoLogLength, NULL, &fragmentShaderErrorMessage[0]);
		std::cout<<&fragmentShaderErrorMessage[0]<<std::endl;
	}

    // Link
    GLuint programId = glCreateProgram();
    glAttachShader(programId, vertexShaderId);
	glAttachShader(programId, fragmentShaderId);
	//glAttachShader(programId, geometryShaderId);
    glLinkProgram(programId);
 
    // Check the program
    glGetProgramiv(programId, GL_LINK_STATUS, &Result);
    glGetProgramiv(programId, GL_INFO_LOG_LENGTH, &InfoLogLength);
	
	if (InfoLogLength > 0)
	{
		std::vector<char> programErrorMessage( std::max(InfoLogLength, int(1)) );
		glGetProgramInfoLog(programId, InfoLogLength, NULL, &programErrorMessage[0]);
		std::cout<<&programErrorMessage[0]<<std::endl;
	}

    glDeleteShader(vertexShaderId);
	glDeleteShader(fragmentShaderId);
	//glDeleteShader(geometryShaderId);
 
    return programId;
}

void OpenGlRenderer::addQuad(Quad vertexCoordinates, GLfloat texLeft, GLfloat texTop, GLfloat texRight, GLfloat texBottom, firemelon::RenderEffectsPtr renderEffects)
{
	quadsAddedPerUpdate_++;

	ColorRgbaPtr color = renderEffects->getHueColor();

	float hueRed = color->getR();
	float hueGreen = color->getG();
	float hueBlue = color->getB();
	float hueAlpha = color->getA();

	int radialFadeOriginX = renderEffects->getAlphaGradientRadialCenterPoint()->getX();
	int radialFadeOriginY = renderEffects->getAlphaGradientRadialCenterPoint()->getY();
	float radialFadeDistance = renderEffects->getAlphaGradientRadius();

	//Set vertex data
	VertexData3D vData[4];

	//Texture coordinates
	vData[0].texCoord.s = texLeft;
	vData[0].texCoord.t = texTop;

	vData[1].texCoord.s = texRight;
	vData[1].texCoord.t = texTop;

	vData[2].texCoord.s = texRight;
	vData[2].texCoord.t = texBottom;

	vData[3].texCoord.s = texLeft;
	vData[3].texCoord.t = texBottom;

	// 0 = Don't use alpha mask, 1 = use alpha mask.
	GLint useAlphaMaskFlag = 0;

	// Remove this. Alpha mask is getting phased out.
	//// Alpha Mask Texture coordinates
	//if (useAlphaMask == true)
	//{
	//	useAlphaMaskFlag = 1;
	//}

	vData[0].useAlphaMask = useAlphaMaskFlag;
	vData[0].alphaMaskTexCoord.s = 0;
	vData[0].alphaMaskTexCoord.t = 0;

	vData[1].useAlphaMask = useAlphaMaskFlag;
	vData[1].alphaMaskTexCoord.s = 0;
	vData[1].alphaMaskTexCoord.t = 0;

	vData[2].useAlphaMask = useAlphaMaskFlag;
	vData[2].alphaMaskTexCoord.s = 0;
	vData[2].alphaMaskTexCoord.t = 0;

	vData[3].useAlphaMask = useAlphaMaskFlag;
	vData[3].alphaMaskTexCoord.s = 0;
	vData[3].alphaMaskTexCoord.t = 0;


	vData[0].pos.x = vertexCoordinates.vertices[0].x;
	vData[0].pos.y = vertexCoordinates.vertices[0].y;
	vData[0].pos.z = 0.0f;

	vData[1].pos.x = vertexCoordinates.vertices[1].x;
	vData[1].pos.y = vertexCoordinates.vertices[1].y;
	vData[1].pos.z = 0.0f;

	vData[2].pos.x = vertexCoordinates.vertices[2].x;
	vData[2].pos.y = vertexCoordinates.vertices[2].y;
	vData[2].pos.z = 0.0f;

	vData[3].pos.x = vertexCoordinates.vertices[3].x;
	vData[3].pos.y = vertexCoordinates.vertices[3].y;
	vData[3].pos.z = 0.0f;


	// Blend percent	
	if (getEnableFade() == true)
	{
		vData[0].fadeBlendPercent = fadeColorPercent_;
		vData[1].fadeBlendPercent = fadeColorPercent_;
		vData[2].fadeBlendPercent = fadeColorPercent_;
		vData[3].fadeBlendPercent = fadeColorPercent_;
	}
	else
	{
		vData[0].fadeBlendPercent = 0.0f;
		vData[1].fadeBlendPercent = 0.0f;
		vData[2].fadeBlendPercent = 0.0f;
		vData[3].fadeBlendPercent = 0.0f;
	}

	float blendPercent = renderEffects->getBlendPercent();

	vData[0].blendPercent = blendPercent;
	vData[1].blendPercent = blendPercent;
	vData[2].blendPercent = blendPercent;
	vData[3].blendPercent = blendPercent;

	//Vertex colors	
	vData[0].rgba.r = hueRed;
	vData[0].rgba.g = hueGreen;
	vData[0].rgba.b = hueBlue;
	vData[0].rgba.a = hueAlpha;

	
	vData[1].rgba.r = hueRed;
	vData[1].rgba.g = hueGreen;
	vData[1].rgba.b = hueBlue;
	vData[1].rgba.a = hueAlpha;
	
	vData[2].rgba.r = hueRed;
	vData[2].rgba.g = hueGreen;
	vData[2].rgba.b = hueBlue;
	vData[2].rgba.a = hueAlpha;

	vData[3].rgba.r = hueRed;
	vData[3].rgba.g = hueGreen;
	vData[3].rgba.b = hueBlue;
	vData[3].rgba.a = hueAlpha;

	//Vertex blend colors	
	vData[0].rgbaFadeBlend.r = fadeColor_.r;
	vData[0].rgbaFadeBlend.g = fadeColor_.g;
	vData[0].rgbaFadeBlend.b = fadeColor_.b;
	vData[0].rgbaFadeBlend.a = fadeColor_.a;

	vData[1].rgbaFadeBlend.r = fadeColor_.r;
	vData[1].rgbaFadeBlend.g = fadeColor_.g;
	vData[1].rgbaFadeBlend.b = fadeColor_.b;
	vData[1].rgbaFadeBlend.a = fadeColor_.a;

	vData[2].rgbaFadeBlend.r = fadeColor_.r;
	vData[2].rgbaFadeBlend.g = fadeColor_.g;
	vData[2].rgbaFadeBlend.b = fadeColor_.b;
	vData[2].rgbaFadeBlend.a = fadeColor_.a;

	vData[3].rgbaFadeBlend.r = fadeColor_.r;
	vData[3].rgbaFadeBlend.g = fadeColor_.g;
	vData[3].rgbaFadeBlend.b = fadeColor_.b;
	vData[3].rgbaFadeBlend.a = fadeColor_.a;

	ColorRgbaPtr blendColor = renderEffects->getBlendColor();

	float blendRed = blendColor->getR();
	float blendGreen = blendColor->getG();
	float blendBlue = blendColor->getB();
	float blendAlpha = blendColor->getA();

	vData[0].rgbaBlend.r = blendRed;
	vData[0].rgbaBlend.g = blendGreen;
	vData[0].rgbaBlend.b = blendBlue;
	vData[0].rgbaBlend.a = blendAlpha;

	vData[1].rgbaBlend.r = blendRed;
	vData[1].rgbaBlend.g = blendGreen;
	vData[1].rgbaBlend.b = blendBlue;
	vData[1].rgbaBlend.a = blendAlpha;

	vData[2].rgbaBlend.r = blendRed;
	vData[2].rgbaBlend.g = blendGreen;
	vData[2].rgbaBlend.b = blendBlue;
	vData[2].rgbaBlend.a = blendAlpha;

	vData[3].rgbaBlend.r = blendRed;
	vData[3].rgbaBlend.g = blendGreen;
	vData[3].rgbaBlend.b = blendBlue;
	vData[3].rgbaBlend.a = blendAlpha;

	// Sprite outline color	
	ColorRgbaPtr outlineColor = renderEffects->getOutlineColor();

	float outlineRed = outlineColor->getR();
	float outlineGreen = outlineColor->getG();
	float outlineBlue = outlineColor->getB();
	float outlineAlpha = outlineColor->getA();

	vData[0].rgbaOutline.r = outlineRed;
	vData[0].rgbaOutline.g = outlineGreen;
	vData[0].rgbaOutline.b = outlineBlue;
	vData[0].rgbaOutline.a = outlineAlpha;
	
	vData[1].rgbaOutline.r = outlineRed;
	vData[1].rgbaOutline.g = outlineGreen;
	vData[1].rgbaOutline.b = outlineBlue;
	vData[1].rgbaOutline.a = outlineAlpha;

	vData[2].rgbaOutline.r = outlineRed;
	vData[2].rgbaOutline.g = outlineGreen;
	vData[2].rgbaOutline.b = outlineBlue;
	vData[2].rgbaOutline.a = outlineAlpha;

	vData[3].rgbaOutline.r = outlineRed;
	vData[3].rgbaOutline.g = outlineGreen;
	vData[3].rgbaOutline.b = outlineBlue;
	vData[3].rgbaOutline.a = outlineAlpha;

	int vertexCount = vertexData_.size();

	indexData_.push_back(vertexCount);
	indexData_.push_back(vertexCount + 1);
	indexData_.push_back(vertexCount + 2);
	indexData_.push_back(vertexCount + 3);

	vertexData_.push_back(vData[0]);
	vertexData_.push_back(vData[1]);
	vertexData_.push_back(vData[2]);
	vertexData_.push_back(vData[3]);
}

void OpenGlRenderer::updateVbo()
{
	// Update the VBO contents. If the size of the array has increased, allocate a new VBO.
	// Otherwise update the current VBO with the vertex data for this frame.
	int size = vertexData_.size();

	if (size > 0)
	{
		VertexData3D* vData = &vertexData_[0];
		GLuint* iData = &indexData_[0];

		if (size > vertexBufferSize_)
		{
			// Allocate a new VBO and IBO to fit the new data size.
			vertexBufferSize_ = size;

			// Destroy the old VBO and IBO
			freeVbo();

			//Create new VBO
			glGenBuffers(1, &vertexBufferId_);
			glBindBuffer(GL_ARRAY_BUFFER, vertexBufferId_);
			glBufferData(GL_ARRAY_BUFFER, size * sizeof(VertexData3D), vData, GL_DYNAMIC_DRAW);
		
			//Create new IBO
			glGenBuffers(1, &indexBufferId_);
			glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, indexBufferId_);
			glBufferData(GL_ELEMENT_ARRAY_BUFFER, size * sizeof(GLuint), iData, GL_DYNAMIC_DRAW);
		
			// Bind the new VBO and IBO to the VAO.
			glBindVertexArray(texturedQuadVao_);
			
			//Set vertex data
			glBindBuffer(GL_ARRAY_BUFFER, vertexBufferId_);

			glVertexAttribPointer(vertexPos2dLocation_, 
								  3, 
								  GL_FLOAT, 
								  GL_FALSE, 
								  sizeof(VertexData3D), 
								  (GLvoid*)offsetof(VertexData3D, pos));

			glVertexAttribPointer(colorLocation_, 
								  4, 
								  GL_FLOAT, 
								  GL_FALSE, 
								  sizeof(VertexData3D), 
								  (GLvoid*)offsetof(VertexData3D, rgba));

			glVertexAttribPointer(texCoordLocation_,
								  2,
								  GL_FLOAT,
								  GL_FALSE,
								  sizeof(VertexData3D),
								  (GLvoid*)offsetof(VertexData3D, texCoord));
			
			glVertexAttribPointer(alphaMaskTexCoordLocation_,
								  2,
								  GL_FLOAT,
								  GL_FALSE,
								  sizeof(VertexData3D),
								  (GLvoid*)offsetof(VertexData3D, alphaMaskTexCoord));
			
			glVertexAttribPointer(useAlphaMaskLocation_,
								  1,
								  GL_INT,
								  GL_FALSE,
								  sizeof(VertexData3D),
								  (GLvoid*)offsetof(VertexData3D, useAlphaMask));
						
			glVertexAttribPointer(outlineColorLocation_,
								  4,
								  GL_FLOAT,
								  GL_FALSE,
								  sizeof(VertexData3D),
								  (GLvoid*)offsetof(VertexData3D, rgbaOutline));

			glVertexAttribPointer(blendPercentLocation_,
								  1,
								  GL_FLOAT,
								  GL_FALSE,
								  sizeof(VertexData3D),
								  (GLvoid*)offsetof(VertexData3D, blendPercent));

			glVertexAttribPointer(blendColorLocation_,
								  4,
								  GL_FLOAT,
								  GL_FALSE,
								  sizeof(VertexData3D),
								  (GLvoid*)offsetof(VertexData3D, rgbaBlend));

			glVertexAttribPointer(fadeBlendPercentLocation_,
								  1,
								  GL_FLOAT,
								  GL_FALSE,
								  sizeof(VertexData3D),
								  (GLvoid*)offsetof(VertexData3D, fadeBlendPercent));

			glVertexAttribPointer(fadeBlendColorLocation_,
								  4,
								  GL_FLOAT,
								  GL_FALSE,
								  sizeof(VertexData3D),
								  (GLvoid*)offsetof(VertexData3D, rgbaFadeBlend));

			glVertexAttribPointer(radialFadeOriginCoordLocation_,
			 					  2,
								  GL_FLOAT,
								  GL_FALSE,
								  sizeof(VertexData3D),
								  (GLvoid*)offsetof(VertexData3D, radialFadeOriginCoord));

			glVertexAttribPointer(radialFadeDistanceLocation_,
								  2,
								  GL_FLOAT,
								  GL_FALSE,
								  sizeof(VertexData3D),
								  (GLvoid*)offsetof(VertexData3D, radialFadeDistance));

			glVertexPointer(2, GL_FLOAT, 0, NULL);

			glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, indexBufferId_);
		
			//Unbind VAO
			glBindVertexArray(NULL);

			//Unbind buffers
			glBindBuffer(GL_ARRAY_BUFFER, NULL);
			glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, NULL);
		}
		else
		{	
			// Bind vertex buffer.
			glBindBuffer(GL_ARRAY_BUFFER, vertexBufferId_);

			// Update vertex buffer data.
			glBufferSubData(GL_ARRAY_BUFFER, 0, size * sizeof(VertexData3D), vData);

			// Bind index buffer.
			glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, indexBufferId_);

			// Update index buffer.		
			glBufferSubData(GL_ELEMENT_ARRAY_BUFFER, 0, size * sizeof(GLuint), iData);			
		}
	}
}

void OpenGlRenderer::updateLineVbo()
{
	// Update the VBO contents. If the size of the array has increased, allocate a new VBO.
	// Otherwise update the current VBO with the vertex data for this frame.
	int size = lineVertexData_.size();
	int indexSize = lineIndexData_.size();

	if (indexSize > 0)
	{
		ColorVertex3D* vData = &lineVertexData_[0];
		GLuint* iData = &lineIndexData_[0];

		if (indexSize > lineVertexBufferSize_)
		{
			// Allocate a new VBO and IBO to fit the new data size.
			lineVertexBufferSize_ = size;
			lineIndexBufferSize_ = indexSize;

			// Destroy the old VBO and IBO
			freeLineVbo();

			//Create new VBO
			glGenBuffers(1, &lineVertexBufferId_);
			glBindBuffer(GL_ARRAY_BUFFER, lineVertexBufferId_);
			glBufferData(GL_ARRAY_BUFFER, size * sizeof(ColorVertex3D), vData, GL_DYNAMIC_DRAW);
		
			//Create new IBO
			glGenBuffers(1, &lineIndexBufferId_);
			glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, lineIndexBufferId_);
			glBufferData(GL_ELEMENT_ARRAY_BUFFER, indexSize * sizeof(GLuint), iData, GL_DYNAMIC_DRAW);
		
			// Bind the new VBO and IBO to the VAO.
			glBindVertexArray(linesVao_);
			
			//Set vertex data
			glBindBuffer(GL_ARRAY_BUFFER, lineVertexBufferId_);

			glVertexAttribPointer(vertexPos2dLinesLocation_, 
								  3, 
								  GL_FLOAT, 
								  GL_FALSE, 
								  sizeof(ColorVertex3D), 
								  (GLvoid*)offsetof(ColorVertex3D, pos));

			glVertexAttribPointer(colorLinesLocation_, 
								  4, 
								  GL_FLOAT, 
								  GL_FALSE, 
								  sizeof(ColorVertex3D), 
								  (GLvoid*)offsetof(ColorVertex3D, rgba));

			glVertexPointer(2, GL_FLOAT, 0, NULL);

			glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, lineIndexBufferId_);
		
			//Unbind VAO
			glBindVertexArray(NULL);

			//Unbind buffers
			glBindBuffer(GL_ARRAY_BUFFER, NULL);
			glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, NULL);
		}
		else
		{	
			glBindVertexArray(linesVao_);
			
			// Bind vertex buffer.
			glBindBuffer(GL_ARRAY_BUFFER, lineVertexBufferId_);

			// Update vertex buffer data.
			glBufferSubData(GL_ARRAY_BUFFER, 0, size * sizeof(ColorVertex3D), vData);

			// Bind index buffer.
			glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, lineIndexBufferId_);

			// Update index buffer.		
			glBufferSubData(GL_ELEMENT_ARRAY_BUFFER, 0, indexSize * sizeof(GLuint), iData);
			
			//Unbind VAO
			glBindVertexArray(NULL);
		}
	}
}
  
void OpenGlRenderer::freeVbo()
{
	//Free VBO and IBO
	if (vertexBufferId_ != 0)
	{
		glDeleteBuffers(1, &vertexBufferId_);
		glDeleteBuffers(1, &indexBufferId_);
		
		vertexBufferId_ = 0;
		indexBufferId_ = 0;
	}
}

void OpenGlRenderer::freeVao()
{
	if( texturedQuadVao_ != 0 )
	{
		glDeleteVertexArrays(1, &texturedQuadVao_);
		
		texturedQuadVao_ = 0;
	}
}

void OpenGlRenderer::freeLineVao()
{
	if (linesVao_ != 0)
	{
		glDeleteVertexArrays(1, &linesVao_);
		
		linesVao_ = 0;
	}
}

void OpenGlRenderer::freeLineVbo()
{
	//Free VBO and IBO
	if (lineVertexBufferId_ != 0)
	{
		glDeleteBuffers(1, &lineVertexBufferId_);
		glDeleteBuffers(1, &lineIndexBufferId_);

		lineVertexBufferId_ = 0;
		lineIndexBufferId_ = 0;
	}
}

bool OpenGlRenderer::initVbo()
{
	if (vertexBufferId_ == 0)
	{
		// Start with a buffer size of 500. Re-allocate a larger buffer if
		// it becomes necessary later.
		VertexData3D vData[500];		
		GLuint iData[500];

		//Create VBO
		glGenBuffers(1, &vertexBufferId_);
		glBindBuffer(GL_ARRAY_BUFFER, vertexBufferId_);
		glBufferData(GL_ARRAY_BUFFER, 500 * sizeof(VertexData3D), vData, GL_DYNAMIC_DRAW);
		
		//Check for error
		GLenum error = glGetError();

		if (error != GL_NO_ERROR)
		{
			std::cout << "Error creating vertex buffer: " << gluErrorString(error) << std::endl;
			return false;
		}

		//Create IBO
		glGenBuffers(1, &indexBufferId_);
		glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, indexBufferId_);
		glBufferData(GL_ELEMENT_ARRAY_BUFFER, 500 * sizeof(GLuint), iData, GL_DYNAMIC_DRAW);

		//Check for error
		error = glGetError();

		if (error != GL_NO_ERROR)
		{
			std::cout << "Error creating vertex index buffer: " << gluErrorString(error) << std::endl;
			return false;
		}
		
		//Unbind buffers
		glBindBuffer(GL_ARRAY_BUFFER, NULL);
		glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, NULL);

		// Do the same thing for a second buffer for rendering lines.
		ColorVertex3D vLinesData[500];		
		GLuint iLinesData[500];

		//Create VBO
		glGenBuffers(1, &lineVertexBufferId_);
		glBindBuffer(GL_ARRAY_BUFFER, lineVertexBufferId_);
		glBufferData(GL_ARRAY_BUFFER, 500 * sizeof(ColorVertex3D), vLinesData, GL_DYNAMIC_DRAW);

		//Check for error
		error = glGetError();

		if (error != GL_NO_ERROR)
		{
			std::cout << "Error creating line buffer: " << gluErrorString(error) << std::endl;
			return false;
		}

		//Create IBO
		glGenBuffers(1, &lineIndexBufferId_);
		glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, lineIndexBufferId_);
		glBufferData(GL_ELEMENT_ARRAY_BUFFER, 500 * sizeof(GLuint), iLinesData, GL_DYNAMIC_DRAW);

		//Check for error
		error = glGetError();

		if (error != GL_NO_ERROR)
		{
			std::cout << "Error creating line index buffer: " << gluErrorString(error) << std::endl;
			return false;
		}

		//Unbind buffers
		glBindBuffer(GL_ARRAY_BUFFER, NULL);
		glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, NULL);
	}

	return true;
}

// The addImageRect() function takes a PackingRect as input and determines which of
// the location candidates will produce the least amount of unused space. After 
// determining the location, it sets the rect's x and y position and adds it to
// the locatedRects list.
void OpenGlRenderer::addImageRect(PackingRect &rect)
{
	// Update the total area of the located rects.
	locatedRectArea_ += (rect.h * rect.w);

	if (locatedRects_.size() == 0)
	{
		// If this is the first rect being added, just add it to the list
		// and initialize the bounds rect to the same size as it.
		rect.x = 0;
		rect.y = 0;

		locatedRects_.push_back(rect);

		bounds_.w = rect.w;
		bounds_.h = rect.h;
	}
	else
	{
		// Build a list of potential locations to place this rect.
		// Try placing this rect at each candidate location, as well appending it to
		// the right and bottom of the bounds. Whichever one results in the least 
		// amount of unused area should be chosen.
		
		// First try placing it at each of the location candidates.
		std::vector<PotentialLocation> potentialLocations;

		int size = locationCandidates_.size();

		for (int i = 0; i < size; i++)
		{
			LocationCandidate currentCandidate = locationCandidates_[i];

			PotentialLocation pl;
			pl.rectLocation.x = currentCandidate.rect.x;
			pl.rectLocation.y = currentCandidate.rect.y;
			pl.rectLocation.w = rect.w;
			pl.rectLocation.h = rect.h;

			pl.rectArea = currentCandidate.area;

			if (isAreaFree(pl.rectLocation) == true)
			{
				// Get what the new bounds would be if this location is selected.
				pl.newBoundsRect = newBounds(pl.rectLocation);
				pl.boundsArea = pl.newBoundsRect.w * pl.newBoundsRect.h;

				potentialLocations.push_back(pl);
			}
		}

		// Next try placing this rect on the right edge, expanding the bounds 
		// outward to the right by rect width.
		PotentialLocation plRight;
		plRight.rectLocation.x = bounds_.w;
		plRight.rectLocation.y = 0;
		plRight.rectLocation.w = rect.w;
		plRight.rectLocation.h = rect.h;

		plRight.newBoundsRect = newBounds(plRight.rectLocation);
		plRight.boundsArea = plRight.newBoundsRect.w * plRight.newBoundsRect.h;

		potentialLocations.push_back(plRight);

		// Lastly, try placing this rect on the bottom edge, expanding the bounds 
		// downward by rect height.
		PotentialLocation plBottom;
		plBottom.rectLocation.x = 0;
		plBottom.rectLocation.y = bounds_.h;
		plBottom.rectLocation.w = rect.w;
		plBottom.rectLocation.h = rect.h;

		plBottom.newBoundsRect = newBounds(plBottom.rectLocation);
		plBottom.boundsArea = plBottom.newBoundsRect.w * plBottom.newBoundsRect.h;

		potentialLocations.push_back(plBottom);
		
		// All of the potential location data is now set.
		// Determine which location will result in the smallest unused area.

		// Initialize to the largest possible value.
		unsigned int unusedArea = 0xFFFFFFFF;

		// The location that will be selected.
		PotentialLocation plFinalSelection;

		size = potentialLocations.size();
		for (int i = 0; i < size; i++)
		{			
			// Subtract the area of the potential new bounds from the total area
			// of the located rects to determine which option will have the least
			// amount of unused space.
			unsigned int totalUnusedArea = potentialLocations[i].boundsArea - locatedRectArea_;

			if (totalUnusedArea < unusedArea)
			{
				// If this amount of unused area is currently the smallest, consider 
				// this to be the current best option to select.
				plFinalSelection = potentialLocations[i];

				unusedArea = totalUnusedArea;
			}
			else if (totalUnusedArea == unusedArea)
			{
				// If the amount of unused area doesn't change, choose the one whose
				// expanded rect has less area.
				if (potentialLocations[i].rectArea < plFinalSelection.rectArea)
				{
					plFinalSelection = potentialLocations[i];
				}
			}
		}

		// Set the input rect's location to the selected location data, and set the
		// texture bounds to the new bounds data.
		rect.x = plFinalSelection.rectLocation.x;
		rect.y = plFinalSelection.rectLocation.y;

		locatedRects_.push_back(rect);
		
		bounds_.w = plFinalSelection.newBoundsRect.w;
		bounds_.h = plFinalSelection.newBoundsRect.h;
		
		// Build the new list of location candidates.
		generateLocationCandidates();
	}
}

// The generateLocationCandidates() function builds a list of location candidates
// that an image rect might be placed at. Currently it chooses them by looping
// through each of the located rects and tests whether the area immediately
// to the right or below are unoccupied. This provides decent results, but
// other methods could provide even more optimal packing results.
void OpenGlRenderer::generateLocationCandidates()
{
	locationCandidates_.clear();

	// Loop through the list of located rects. For each one,
	// create two location candidate rects, one directly 
	// below and one directly to the right. 

	int size = locatedRects_.size();

	for (int i = 0; i < size; i++)
	{
		PackingRect currentLocatedRect = locatedRects_[i];

		// Start with a rect directly to the right.
		LocationCandidate lcRight;
		lcRight.rect.y = currentLocatedRect.y;
		lcRight.rect.x = currentLocatedRect.x + currentLocatedRect.w;
		
		// If there's already a fixed rect at this point, or it is at the bounds
		// then it is not a valid candidate.
		if (isPointFree(lcRight.rect) == true)
		{
			inflateRect(lcRight.rect);
			
			lcRight.area = lcRight.rect.w * lcRight.rect.h;

			locationCandidates_.push_back(lcRight);
		}

		// Now try a rect directly below.
		LocationCandidate lcBottom;
		lcBottom.rect.y = currentLocatedRect.y + currentLocatedRect.h;
		lcBottom.rect.x = currentLocatedRect.x;
		
		// If there's already a fixed rect at this point, or it is at the bounds 
		// then it is not a valid candidate.
		if (isPointFree(lcBottom.rect) == true)
		{
			inflateRect(lcBottom.rect);

			lcBottom.area = lcBottom.rect.w * lcBottom.rect.h;

			locationCandidates_.push_back(lcBottom);
		}
	}
}

// Inflate a rect starting from the top left point of the parameter rect r.
// Start by expanding the dimensions outward to the right and left.
// Once the left and right bounds of this rect have been located expand a rect
// upwards and downwards to determine the best fit.
void OpenGlRenderer::inflateRect(PackingRect& r)
{
	// First determine the left and right bounds of the inflated rect.
	Line leftwardExpansionLine;
	leftwardExpansionLine.pt1.x = r.x;
	leftwardExpansionLine.pt1.y = r.y;

	leftwardExpansionLine.pt2.x = -100;
	leftwardExpansionLine.pt2.y = r.y;

	Point hitPointLeft = raycast(leftwardExpansionLine);

	Line rightwardExpansionLine;
	rightwardExpansionLine.pt1.x = r.x + 1;
	rightwardExpansionLine.pt1.y = r.y;
			
	rightwardExpansionLine.pt2.x = bounds_.w;
	rightwardExpansionLine.pt2.y = r.y;

	Point hitPointRight = raycast(rightwardExpansionLine);
	
	// Then find the top and bottom bounds of the inflated rect.

	// To do this, create two rects, one that expands downward and one that expands upward.
	// Loop through all located rects, testing for collisions. 
	// For the downward expanding rect, if the colliding rect is below, resize it so
	// it is no longer colliding.
	// For the upward expanding rect, if the colliding rect is above, resize it the same way.
	// Use these values to build a rect that expands to fill as much area as it can from the
	// original location candidate point.
	PackingRect downwardRect;
	downwardRect.x = hitPointLeft.x + 1;
	downwardRect.y = leftwardExpansionLine.pt1.y;

	downwardRect.w = (hitPointRight.x - (hitPointLeft.x + 1));
	downwardRect.h = bounds_.h - leftwardExpansionLine.pt1.y;

	PackingRect upwardRect;
	upwardRect.x = hitPointLeft.x + 1;
	upwardRect.y = 0;

	upwardRect.w = (hitPointRight.x - (hitPointLeft.x + 1));
	upwardRect.h = leftwardExpansionLine.pt1.y;

	int size = locatedRects_.size();
	for (int i = 0; i < size; i++)
	{
		PackingRect currentLocatedRect = locatedRects_[i];

		if (rectCollision(downwardRect, currentLocatedRect) == true)
		{
			if (downwardRect.y <= currentLocatedRect.y)
			{
				downwardRect.h = currentLocatedRect.y - downwardRect.y;
			}
		}

		if (rectCollision(upwardRect, currentLocatedRect) == true)
		{
			int upwardRectBottom = upwardRect.y + upwardRect.h;
			int locatedRectBottom = currentLocatedRect.y + currentLocatedRect.h;

			if (upwardRectBottom >= locatedRectBottom)
			{				
				upwardRect.y = locatedRectBottom;
				upwardRect.h = upwardRectBottom - locatedRectBottom;
			}
		}
	}

	// Now there is enough data to construct the inflated rect.
	r.x = hitPointLeft.x + 1;
	r.y = upwardRect.y;

	r.w = hitPointRight.x - (hitPointLeft.x + 1);
	r.h = (downwardRect.y + downwardRect.h) - upwardRect.y;
}

// The isPointFree() function takes a rect and tests whether the top left most
// point intersects with any located rects or is outside the current texture bounds.
bool OpenGlRenderer::isPointFree(PackingRect r)
{
	// If the point at (r.x, r.y) intersects with any located rects, then
	// the point is not free.
	int size = locatedRects_.size();
	for (int i = 0; i < size; i++)
	{
		PackingRect currentLocatedRect = locatedRects_[i];

		if (r.x >= currentLocatedRect.x && 
			r.x < (currentLocatedRect.x + currentLocatedRect.w))
		{				
			if (r.y >= currentLocatedRect.y && 
				r.y < (currentLocatedRect.y + currentLocatedRect.h))
			{
				return false;
			}
		}
	}

	// Also, if the point is at or beyond the bounds, it is not free.
	if (r.x >= bounds_.w || r.y >= bounds_.h)
	{
		return false;
	}

	return true;
}

// The isAreaFree() function takes a rect and tests whether it intersects
// with any located rects. For this function being outside the texture 
// bounds is acceptable.
bool OpenGlRenderer::isAreaFree(PackingRect r)
{
	// Loop through the located rects and test if this rect intersects with any of them.
	int size = locatedRects_.size();
	for (int j = 0; j < size; j++)
	{
		PackingRect currentLocatedRect = locatedRects_[j];

		if (rectCollision(r, currentLocatedRect) == true)
		{
			// Collision is found, thus the area is not free.
			return false;
		}
	}

	return true;
}

// Returns true if two rectangles overlap.
bool OpenGlRenderer::rectCollision(PackingRect r1, PackingRect r2)
{	
	if ((r1.x + r1.w) > r2.x && r1.x < (r2.x + r2.w))
	{
		if (r1.y < (r2.y + r2.h) && (r1.y + r1.h) > r2.y)
		{
			return true;
		}
	}

	return false;
}

// The newBounds() function takes a PackingRect as input and calculates what
// the new texture bounds would be, if this rect was to be added to the
// texture
PackingRect OpenGlRenderer::newBounds(PackingRect rect)
{
	PackingRect newBounds;

	newBounds.x = 0;
	newBounds.y = 0;

	if (rect.x + rect.w > bounds_.w)
	{
		newBounds.w = rect.x + rect.w;
	}
	else
	{
		newBounds.w = bounds_.w;
	}
				
	if (rect.y + rect.h > bounds_.h)
	{
		newBounds.h = rect.y + rect.h;
	}
	else
	{
		newBounds.h = bounds_.h;
	}

	return newBounds;
}

// Loop through each located rect and test if the line is colliding. Find all
// intersection points and choose the one closest to the ray origin.
Point OpenGlRenderer::raycast(Line ray)
{
	std::vector<Point> intersectionPoints;

	int size = locatedRects_.size();
	for (int j = 0; j <= size; j++)
	{
		PackingRect currentLocatedRect;

		if (j == size)
		{
			// Special case - Use the bounding rect
			currentLocatedRect = bounds_;
		}
		else
		{
			currentLocatedRect = locatedRects_[j];
		}

		// Convert the rect into the four lines that make up its border.
		// Find the intersection between the ray and each rect line.
		Line lines[4];
		
		lines[0].pt1.x = currentLocatedRect.x;
		lines[0].pt1.y = currentLocatedRect.y;
		lines[0].pt2.x = currentLocatedRect.x + (currentLocatedRect.w - 1);
		lines[0].pt2.y = currentLocatedRect.y;

		lines[1].pt1.x = lines[0].pt2.x;
		lines[1].pt1.y = lines[0].pt2.y;
		lines[1].pt2.x = lines[0].pt2.x;
		lines[1].pt2.y = lines[0].pt2.y + (currentLocatedRect.h - 1);

		lines[2].pt1.x = lines[1].pt2.x;
		lines[2].pt1.y = lines[1].pt2.y;
		lines[2].pt2.x = lines[1].pt2.x - (currentLocatedRect.w - 1);
		lines[2].pt2.y = lines[1].pt2.y;

		lines[3].pt1.x = lines[2].pt2.x;
		lines[3].pt1.y = lines[2].pt2.y;
		lines[3].pt2.x = lines[2].pt2.x;
		lines[3].pt2.y = lines[2].pt2.y - (currentLocatedRect.h - 1);

		Point p1, p2, p3, p4;

		p1.x = ray.pt1.x;
		p1.y = ray.pt1.y;

		p2.x = ray.pt2.x;
		p2.y = ray.pt2.y;
		
		Line first;
		first.pt1 = p1;
		first.pt2 = p2;

		// Loop through each line and find the intersection, if it exists.
		for (int i = 0; i < 4; i++)
		{
			p3.x = lines[i].pt1.x;
			p3.y = lines[i].pt1.y;

			p4.x = lines[i].pt2.x;
			p4.y = lines[i].pt2.y;

			Line second;
			second.pt1 = p3;
			second.pt2 = p4;

			Point intersectionPoint;

			if (lineIntersection(first, second, intersectionPoint) == true)
			{
				intersectionPoints.push_back(intersectionPoint);
			}
		}
	}

	Point closestPoint;
	float shortestDistance = 0.0;

	// Loop through the intersection points and find the one closest to the ray's origin point.
	size = intersectionPoints.size();
	for (int i = 0; i < size; i++)
	{
		Point currentPoint = intersectionPoints[i];

		float currentDistance = distance(currentPoint, ray.pt1);

		// For the first iteration, initialize the shortest distance to whatever the distance is.
		if (i == 0)
		{
			shortestDistance = currentDistance;
			closestPoint = currentPoint;
		}
		else
		{
			if (currentDistance < shortestDistance)
			{
				shortestDistance = currentDistance;
				closestPoint = currentPoint;
			}
		}
	}

	return closestPoint;
}

// Returns whether two lines intersect. If they do, the intersectionPoint parameter
// is set to the point at which they intersect.
bool OpenGlRenderer::lineIntersection(Line first, Line second, Point& intersectionPoint)
{
	// Construct the vectors from the lines.
	Point v;
	v.x = first.pt2.x - first.pt1.x;
	v.y = first.pt2.y - first.pt1.y;

	Point u;
	u.x = second.pt2.x - second.pt1.x;
	u.y = second.pt2.y - second.pt1.y;

	Point w;
	w.x = second.pt1.x - first.pt1.x;
	w.y = second.pt1.y - first.pt1.y;
	
	// If they are parallel, they either don't intersect, or they
	// intersect at every point. Either way, disregard it.
	if (perpDot(v, u) == 0)
	{
		return false;
	}

	float perpdot1 = perpDot(u, w);
	float perpdot2 = perpDot(u, v);

	// t is a scalar that scales the vector v to the point
	// where it intersects with vector u
	float t = perpdot1 / perpdot2;

	intersectionPoint.x = first.pt1.x + (t * v.x);
	intersectionPoint.y = first.pt1.y + (t * v.y);

	// If the point is lies on both line segments, it intersects.
	bool isOnLineOne = isPointOnLine(intersectionPoint, first);
	bool isOnLineTwo = isPointOnLine(intersectionPoint, second);

	if (isOnLineOne && isOnLineTwo)
	{
		return true;
	}
	else
	{
		return false;
	}
}

// Returns whether a given point exists on a given line.
bool OpenGlRenderer::isPointOnLine(Point p, Line l)
{	
	if (l.pt1.x > l.pt2.x)
	{
		// If the point is within the x components of the line.
		if (p.x >= l.pt2.x && p.x <= l.pt1.x)
		{
			if (l.pt1.y < l.pt2.y)
			{
				// If the point is within the y components of the line.
				if (p.y >= l.pt1.y && p.y <= l.pt2.y)
				{
					return true;
				}
			}
			else
			{
				// If the point is within the y components of the line.
				if (p.y >= l.pt2.y && p.y <= l.pt1.y)
				{
					return true;
				}
			}
		}
	}
	else
	{
		// If the point is within the x components of the line.
		if (p.x >= l.pt1.x && p.x <= l.pt2.x)
		{
			if (l.pt1.y < l.pt2.y)
			{
				// If the point is within the y components of the line.
				if (p.y >= l.pt1.y && p.y <= l.pt2.y)
				{
					return true;
				}
			}
			else
			{
				// If the point is within the y components of the line.
				if (p.y >= l.pt2.y && p.y <= l.pt1.y)
				{
					return true;
				}
			}
		}
	}

	return false;
}

float OpenGlRenderer::perpDot(Point p1, Point p2)
{
	return (p1.y * p2.x) - (p1.x * p2.y);
}

float OpenGlRenderer::dot(Point p1, Point p2)
{
	return (p1.x * p2.x) + (p1.y * p2.y);
}

float OpenGlRenderer::distance(Point p1, Point p2)
{
	int temp1 = p2.x - p1.x;
	int temp2 = p2.y - p1.y;

	return std::sqrt((temp1 * temp1) + (temp2 * temp2));
}

void OpenGlRenderer::setFadeColor(float red, float green, float blue)
{
	if (red > 1.0f)
	{
		fadeColor_.r = 1.0f;
	}
	else if (red < 0.0f)
	{
		fadeColor_.r = 0.0f;
	}
	else
	{
		fadeColor_.r =red;
	}
	
	if (green > 1.0f)
	{
		fadeColor_.g = 1.0f;
	}
	else if (green < 0.0f)
	{
		fadeColor_.g = 0.0f;
	}
	else
	{
		fadeColor_.g = green;
	}

	if (blue > 1.0f)
	{
		fadeColor_.b = 1.0f;
	}
	else if (blue < 0.0f)
	{
		fadeColor_.b = 0.0f;
	}
	else
	{
		fadeColor_.b = blue;
	}
}

void OpenGlRenderer::setFadeOpacity(float opacity)
{
	if (opacity > 1.0f)
	{
		fadeColorPercent_ = 1.0f;
	}
	else if (opacity < 0.0f)
	{
		fadeColorPercent_ = 0.0f;
	}
	else
	{
		fadeColorPercent_ = opacity;
	}
}

float OpenGlRenderer::getFadeOpacity()
{
	return fadeColorPercent_;
}

void OpenGlRenderer::saveScreenshot(std::string fileName)
{
	ILuint imgID = 0;

	ilGenImages(1, &imgID);

	ilBindImage(imgID);

	unsigned char* buffer = new unsigned char[screenWidth_ * screenHeight_ * 4];

	glReadBuffer(GL_FRONT_LEFT);
	glPixelStorei(GL_UNPACK_ALIGNMENT, 1);
	glPixelStorei(GL_PACK_ALIGNMENT, 1);

	glReadPixels(0, 0, screenWidth_, screenHeight_, GL_RGBA, GL_UNSIGNED_BYTE, buffer);

	ILboolean success = ilTexImage((ILuint)screenWidth_, (ILuint)screenHeight_,
									1, 4,
									IL_RGBA,
									IL_UNSIGNED_BYTE,
									buffer);

	// Save the current image.
ilEnable(IL_FILE_OVERWRITE);

	success = ilSave(IL_PNG, fileName.c_str());

	if (success == IL_FALSE)
	{
		ILenum error = ilGetError();

		std::cout << "Failed to save screenshot: " << iluErrorString(error) << std::endl;
	}

	ilDeleteImages(1, &imgID);
}