#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D Texture;
matrix WorldViewProj;
float Time;

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

sampler2D SpriteTextureSampler = sampler_state
{
    Texture = <Texture>;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

	output.Position = mul(input.Position, WorldViewProj);
    output.TextureCoordinates = input.TextureCoordinates;
	output.Color = input.Color;

	return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
    //vec2 uv = fragCoord / iResolution.xy;

    //// Time varying pixel color
    //vec3 col = cos(iTime + uv.xyx + vec3(0, 2, 4));

    //// Output to screen
    //fragColor = vec4(col, 1.0);
	
    // Sample the texture
    float4 color = tex2D(SpriteTextureSampler, input.TextureCoordinates) * input.Color;

    // Calculate the grayscale value based on human perception of colors.
    //float grayscale = dot(color.rgb, float3(0.1, 0.1, 0.1));

    // create a grayscale color vector (same value for R, G, and B)
    //float3 grayscaleColor = float3(grayscale, grayscale, grayscale);

    // Linear interpolation between he grayscale color and the original color's
    // rgb values based on the saturation parameter.
    //float3 finalColor = lerp(grayscale, color.rgb, Saturation);
    
    // Return the final color with the original alpha value.
    //return color; //float4(finalColor, color.a);
    
    color.a *= sin(Time * 2.0);
	
    return /*float4(grayscaleColor, color.a);*/ color;
}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};