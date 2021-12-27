#version 130

uniform sampler2D textureUnit;

in vec4 color;

in vec4 blendColor;
in float blendPercent;

out vec4 fragColor;
in vec2 texCoord;

// Used when expirimenting with different things.
vec4 sepia = vec4(1.2, 1.0, 0.6, 1.0); 
vec4 white = vec4(1.0, 1.0, 1.0, 1.0); 
vec4 red = vec4(1.0, 0.0, 0.0, 1.0); 
vec4 green = vec4(0.0, 1.0, 0.0, 1.0); 
vec4 blue = vec4(0.0, 0.0, 1.0, 1.0); 
vec4 magenta = vec4(1.0, 0.0, 1.0, 1.0); 

void main() 
{ 
    vec4 colorFinal = color;
	vec4 textureSample = texture(textureUnit, texCoord);

	// If the texture fragment is transparent, it should not be blended, because it would create an undesirable visual effect.
	if (textureSample.a > 0)
	{
	    colorFinal = textureSample * color;
	    colorFinal = mix(colorFinal, blendColor, blendPercent);
    }
	else
	{ 
	    colorFinal = textureSample * color;
	}

	fragColor = colorFinal;
}