#version 130

//Transformation Matrices
uniform mat4 projectionMatrix;
uniform mat4 modelViewMatrix;

//Vertex position attribute
in vec2 vertexPos2D;

// Color attribute
in vec4 color_in;
out vec4 color;

// Blend attribute
in vec4 blendColor_in;
out vec4 blendColor;

// Blend percent
in float blendPercent_in;
out float blendPercent;

// Texture coordinate attribute
in vec2 texCoord_in;
out vec2 texCoord;

void main() 
{  
	//color = vec4(color_in, 1.0);
	color = color_in;
	
	// Pass along the blending data.
	blendPercent = blendPercent_in;
	blendColor = blendColor_in;

	// Process texCoord
    texCoord = texCoord_in;
    
    // Process vertex
    gl_Position = projectionMatrix * modelViewMatrix * vec4(vertexPos2D.x, vertexPos2D.y, 0.0, 1.0);
}