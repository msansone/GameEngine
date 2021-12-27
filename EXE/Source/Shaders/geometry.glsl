#version 330 core

layout(points) in;
layout(points, max_vertices = 1) out;

in VS_OUT {
    vec4 color;
    vec2 texCoords;
    vec2 alphaMaskTexCoords;
    int useAlphaMask;
    float blendPercent;
    vec4 blendColor;
    float fadeBlendPercent;
    vec4 fadeBlendColor;
} gs_in[]; 

out GS_OUT
{
	vec4 color;
    vec2 texCoords;
    vec2 alphaMaskTexCoords;
    int useAlphaMask;
	float blendPercent;
	vec4 blendColor;
	float fadeBlendPercent;
	vec4 fadeBlendColor;
} gs_out;

void main()
{
	// Pass along color_in to fragment shader without modification.
	gs_out.color = vec4(1.0, 1.0, 1.0, 1.0); //gl_in[0].color;
	
	gs_out.blendColor = vec4(1.0, 1.0, 1.0, 1.0); //gl_in[0].blendColor;

	gs_out.blendPercent = 1.0; //gl_in[0].blendPercent;
	
	gs_out.texCoords = gs_in[0].texCoords;
	
	gs_out.alphaMaskTexCoords = gs_in[0].alphaMaskTexCoords;
	
	gs_out.useAlphaMask = gs_in[0].useAlphaMask;

    gl_Position = gl_in[0].gl_Position;

    EmitVertex();

    EndPrimitive();
}