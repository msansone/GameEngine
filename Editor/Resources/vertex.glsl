#version 330 core

//Transformation Matrices
uniform mat4 projectionMatrix;
uniform mat4 modelViewMatrix;

//Vertex position attribute
in vec3 vertexPos3D;

// Color attribute
in vec4 color_in;

// Blend attribute
in vec4 blendColor_in;
in vec4 fadeBlendColor_in;

// Blend percent
in float blendPercent_in;
in float fadeBlendPercent_in;

// Texture coordinate attribute
in vec2 texCoord_in;

// Alpha Mask Texture coordinate attribute
in vec2 alphaMaskTexCoord_in;

in int useAlphaMask_in;

in vec4 outlineColor_in;

// Radial Fade Origin.
in vec2 radialFadeOriginCoord_in;

// Radial Fade Distance.
in float radialFadeDistance_in;

out VS_OUT
{
	vec4 color;
    vec2 texCoords;
    vec2 alphaMaskTexCoords;
    int useAlphaMask;
	vec4 outlineColor;
	float blendPercent;
	vec4 blendColor;
	float fadeBlendPercent;
	vec4 fadeBlendColor;
	vec2 radialFadeOriginCoord;
	float radialFadeDistance;
} vs_out;

void main() 
{  
	// Pass along color_in to fragment shader without modification.

	vs_out.color = color_in;

	// Pass along the blending data without modification.
	vs_out.blendPercent = blendPercent_in;

	vs_out.blendColor = blendColor_in;

	vs_out.fadeBlendPercent = fadeBlendPercent_in;

	vs_out.fadeBlendColor = fadeBlendColor_in;
	
	// Pass along the radial fade data without modification.
	vs_out.radialFadeDistance = radialFadeDistance_in;

	vs_out.radialFadeOriginCoord = radialFadeOriginCoord_in;

	// Pass along the texture coordinates without modification.
    vs_out.texCoords = texCoord_in;

	vs_out.alphaMaskTexCoords = alphaMaskTexCoord_in;

    vs_out.useAlphaMask = useAlphaMask_in;

	vs_out.outlineColor = outlineColor_in;

    // Process vertex
	gl_Position = projectionMatrix * modelViewMatrix * vec4(vertexPos3D.x, vertexPos3D.y, vertexPos3D.z, 1.0);
}