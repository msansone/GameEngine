#version 330 core

out vec4 fragColor;

in VS_OUT
{
	vec4 color;
    vec2 texCoords;
    vec2 alphaMaskTexCoords;
    int useAlphaMask;
	float blendPercent;
	vec4 blendColor;	
	float fadeBlendPercent;
	vec4 fadeBlendColor;	
	vec2 radialFadeOriginCoord;
	float radialFadeDistance;
} fs_in;

void main() 
{ 
	fragColor = fs_in.color;
}