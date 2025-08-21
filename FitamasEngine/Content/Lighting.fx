#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

#define MAX_LIGHTS 8
#define LIGHT_CASCADE_LEVELS 3

int ActiveLightsCount;
float2 PointLightPosition[MAX_LIGHTS];
float4 PointLightColor[MAX_LIGHTS];
float PointLightRadius[MAX_LIGHTS];
float PointLightIntensity[MAX_LIGHTS];
//float3 PointLightDirection[MAX_LIGHTS];

float AmbientIntensity;
float3 AmbientColor;
texture LightMask;
texture MainTexture;
matrix WorldViewProj;

sampler TextureSampler = sampler_state
{
    Texture = <MainTexture>;
    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};

sampler LightMaskSampler = sampler_state
{
    Texture = <LightMask>;
    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};

//struct VertexShaderInput
//{
//    float4 Position : POSITION0;
//    float4 Color : COLOR0;
//    float2 TextureCoordinates : TEXCOORD0;
//};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION0;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
    float4 WorldPosition : TEXCOORD1;
};

VertexShaderOutput MainVS(float4 position : SV_POSITION, float4 color : COLOR0, float2 texCoord : TEXCOORD0)
{
    VertexShaderOutput output;
    output.Position = mul(position, WorldViewProj);
    output.Color = color;
    output.TextureCoordinates = texCoord;
    output.WorldPosition = position;
    return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 texColor = tex2D(TextureSampler, input.TextureCoordinates) * input.Color;
    float4 lighting = float4(AmbientColor * AmbientIntensity, 0);

    for (int i = 0; i < ActiveLightsCount; i++)
    {
        //lighting += /*input.Color.rgb;*/ CalculateRadianceCascade(Lights[i], pixelPos, input.TextureCoordinates);
        
        float distanceToLight = length(PointLightPosition[i] - input.WorldPosition.xy);
    
        if (distanceToLight > PointLightRadius[i])
        {
            continue;
        }
        
        float attenuation = 1.0 - smoothstep(0, PointLightRadius[i], distanceToLight);
        float4 lightValue = PointLightColor[i] * attenuation * PointLightIntensity[i];
    
        float mask = tex2D(LightMaskSampler, input.TextureCoordinates).r;
    
        for (int i = 1; i <= LIGHT_CASCADE_LEVELS; i++)
        {
            float cascadeFactor = 1.0 - (i / (float) (LIGHT_CASCADE_LEVELS + 1));
            float cascadeRadius = PointLightRadius[i] * cascadeFactor;
        
            if (distanceToLight < cascadeRadius)
            {
                float cascadeAttenuation = 1.0 - smoothstep(0, cascadeRadius, distanceToLight);
                lightValue += PointLightColor[i] * cascadeAttenuation * PointLightIntensity[i] * 0.5 * mask;
            }
        }
        
        lighting += lightValue;
    }
    
    texColor = lighting;
    
    return texColor;
}

technique BasicColorDrawing
{
    pass P0
    {
        VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};