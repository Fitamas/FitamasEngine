#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

sampler texture0;

float power;
float radius;

float4 mainPS(float2 texCoord : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(texture0, texCoord);
	float2 dist = (texCoord - 0.5f) * radius;
	dist.x = 1 - dot(dist, dist) * power;
	color.rgb *= dist.x;

	return color;
}

technique Vignette
{
	pass P0
	{
		PixelShader = compile ps_3_0 mainPS();
	}
};