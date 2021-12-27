#version 330 core

uniform sampler2D textureUnit;

uniform ivec2 textureSize;

uniform ivec2 windowSize;

out vec4 fragColor;

//in GS_OUT
in VS_OUT
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
} fs_in;

// Light test data
struct Light
{
  vec2 position;
  float radius;
  float dropoff;
};

// Used when expirimenting with different things.
vec4 sepia = vec4(1.2, 1.0, 0.6, 1.0); 
vec4 white = vec4(1.0, 1.0, 1.0, 1.0); 
vec4 red = vec4(1.0, 0.0, 0.0, 1.0); 
vec4 green = vec4(0.0, 1.0, 0.0, 1.0); 
vec4 blue = vec4(0.0, 0.0, 1.0, 1.0); 
vec4 magenta = vec4(1.0, 0.0, 1.0, 1.0); 

in vec4 gl_FragCoord;

void main() 
{
    vec4 debugColor = magenta;

    vec4 colorFinal = fs_in.color;

	vec4 textureSample = texture(textureUnit, fs_in.texCoords);

	vec4 alphaMaskTextureSample = texture(textureUnit, fs_in.alphaMaskTexCoords);
	
    vec2 texelSize = vec2(1.0f / textureSize.x, 1.0f / textureSize.y);

	// Get the neighboring texel values. Texture coordinates go from 0.0 to 1.0
	vec4 pixelUp    = texture(textureUnit, fs_in.texCoords - vec2(0, texelSize.y));

    vec4 pixelDown  = texture(textureUnit, fs_in.texCoords + vec2(0, texelSize.y));

    vec4 pixelLeft  = texture(textureUnit, fs_in.texCoords - vec2(texelSize.x, 0));

    vec4 pixelRight = texture(textureUnit, fs_in.texCoords + vec2(texelSize.x, 0));

	// If this pixel is transparent, and any neighboring pixels are not, it is an edge pixel.
    if (textureSample.a == 0 && (pixelUp.a > 0 || pixelDown.a > 0 || pixelLeft.a > 0 || pixelRight.a > 0))
    {
		colorFinal.rgba = fs_in.outlineColor;				
    }
	else
	{
		// If the texture fragment is transparent, it should not be blended, because it would create an undesirable visual effect.
		if (textureSample.a > 0)
		{
			colorFinal = textureSample * fs_in.color;

			colorFinal = mix(colorFinal, fs_in.blendColor, fs_in.blendPercent);

			colorFinal = mix(colorFinal, fs_in.fadeBlendColor, fs_in.fadeBlendPercent);

			// If the color is transparent, don't blend the alpha.
			if (fs_in.color.a == 0)
			{
				colorFinal.a = 0;
			}
		}
		else
		{ 
			colorFinal = textureSample * fs_in.color;
		}		
	}

	// Apply any transparency effects.
	//if (fs_in.useAlphaMask > 0)
	//{
	//	if (alphaMaskTextureSample.a > 0.0f)
	//	{
	//		colorFinal.a = alphaMaskTextureSample.r;
	//	}
	//}
	
	fragColor = colorFinal;	
}