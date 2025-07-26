﻿#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

#define TAU 6.28318530718

sampler2D _MainTex;

//sampler2D _CascadeTex;
sampler2D _ColorTex;
sampler2D _DistanceTex;

float2 _Aspect;
float _RayRange;

float2 _CascadeResolution;
uint _CascadeLevel;
uint _CascadeCount;

float _SkyRadiance;
float3 _SkyColor;
float3 _SunColor;
float _SunAngle;

matrix WorldViewProj;

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float2 UV : TEXCOORD0;
    float4 Color : COLOR0;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float2 UV : TEXCOORD0;
    float4 Color : COLOR0;
};

//v2f vert(appdata v)
//{
//    v2f o;
//    o.vertex = UnityObjectToClipPos(v.vertex);
//    o.uv = v.uv;

//    return o;
//}
            
float2 CalculateRayRange(uint index, uint count)
{
                //A relatively cheap way to calculate ray ranges instead of using pow()
                //The values returned : 0, 3, 15, 63, 255
                //Dividing by 3       : 0, 1, 5, 21, 85
                //and the distance between each value is multiplied by 4 each time

    //float maxValue = (1 << (count * 2)) - 1;
    //float start = (1 << (index * 2)) - 1;
    //float end = (1 << (index * 2 + 2)) - 1;
    
    float maxValue = pow(2, (count * 2)) - 1;
    float start = pow(2, (index * 2)) - 1;
    float end = pow(2, (index * 2 + 2)) - 1;

    float2 r = float2(start, end) / maxValue;
    return r * _RayRange;
}

float3 SampleSkyRadiance(float a0, float a1)
{
                // Sky integral formula taken from "Analytic Direct Illumination" - Mathis
                // https://www.shadertoy.com/view/NttSW7
    const float3 SkyColor = _SkyColor;
    const float3 SunColor = _SunColor;
    const float SunA = _SunAngle;
    const float SSunS = 8.0;
    const float ISSunS = 1 / SSunS;
    float3 SI = SkyColor * (a1 - a0 - 0.5 * (cos(a1) - cos(a0)));
    SI += SunColor * (atan(SSunS * (SunA - a0)) - atan(SSunS * (SunA - a1))) * ISSunS;
    return SI * 0.16;
}

            //Raymarching
float4 SampleRadianceSDF(float2 rayOrigin, float2 rayDirection, float2 rayRange)
{
    float t = rayRange.x;
    float4 hit = float4(0, 0, 0, 1);

    for (int i = 0; i < 32; i++)
    {
        float2 currentPosition = rayOrigin + t * rayDirection * _Aspect.yx;

        if (t > rayRange.y || currentPosition.x < 0 || currentPosition.y < 0 || currentPosition.x > 1 || currentPosition.y > 1)
        {
            break;
        }

        float distance = tex2D(_DistanceTex, currentPosition).r;

        if (distance < 0.001)
        {
            hit = float4(tex2D(_ColorTex, currentPosition).rgb, 0);
            break;
        }

        t += distance;
    }
    
    return hit;
}

VertexShaderOutput MainVS(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput) 0;

    output.Position = mul(input.Position, WorldViewProj);
    output.UV = input.UV;
    output.Color = input.Color;

    return output;
}

float4 CascadePS(VertexShaderOutput input) : COLOR
{
    //return float4(_SkyColor, 1);
    
    //return tex2D(_MainTex, input.UV);
    
    float2 pixelIndex = floor(input.UV * _CascadeResolution);
    
    uint blockSqrtCount = pow(2, _CascadeLevel);//    1 << _CascadeLevel; //Another way to write pow(2, _CascadeLevel)
    
    float2 blockDim = _CascadeResolution / blockSqrtCount;
    float2 block2DIndex = floor(pixelIndex / blockDim);
    float blockIndex = block2DIndex.x + block2DIndex.y * blockSqrtCount;
    
    float2 coordsInBlock = fmod(pixelIndex, blockDim);
    
    float4 finalResult = 0;
                
    float2 rayOrigin = (coordsInBlock + 0.5) * blockSqrtCount;
    float2 rayRange = CalculateRayRange(_CascadeLevel, _CascadeCount);
    
    for (int i = 0; i < 4; i++)
    {
        float angleStep = TAU / (blockSqrtCount * blockSqrtCount * 4);
        float angleIndex = blockIndex * 4 + i;
        float angle = (angleIndex + 0.5) * angleStep;
    
        float2 rayDirection = float2(cos(angle), sin(angle));
                    
        float4 radiance = SampleRadianceSDF(rayOrigin / _CascadeResolution, rayDirection, rayRange);
                    
        if (radiance.a != 0)
        {
            //if (_CascadeLevel != _CascadeCount - 1)
            if (_CascadeLevel + 1 != _CascadeCount)
            {
                            //Merging with the Upper Cascade (_MainTex)
                float2 position = coordsInBlock * 0.5 + 0.25;
                float2 positionOffset = float2(fmod(angleIndex, blockSqrtCount * 2), floor(angleIndex / (blockSqrtCount * 2)));

                position = clamp(position, 0.5, blockDim * 0.5 - 0.5);
            
                float4 rad = tex2D(_MainTex, (position + positionOffset * blockDim * 0.5) / _CascadeResolution);
                        
                radiance.rgb += rad.rgb * radiance.a;
                radiance.a *= rad.a;
            }
            else
            {
                            //if this is the Top Cascade and there is no other cascades to merge with, we merge it with the sky radiance instead
                float3 sky = SampleSkyRadiance(angle, angle + angleStep) * _SkyRadiance;
                radiance.rgb += (sky / angleStep) * 2;
            }
        }
        
        //float3 sky = _SkyColor;
        //radiance.rgb += _SkyColor;
        //radiance.a = 1;
                    
        finalResult += radiance * 0.25;
    }
    
    //finalResult += float4(0, 0, 1, 1);

    return finalResult;
}

float4 DistancePS(VertexShaderOutput input) : COLOR
{
    float2 jf = tex2D(_MainTex, input.UV).rg;
    float value = distance(input.UV, jf);
    return float4(value, 0, 0, 1);
}

float4 ScreenUVPS(VertexShaderOutput input) : COLOR
{
    return float4(input.UV * (1 - step(tex2D(_MainTex, input.UV).a, 0.5)), 0, 1);
}

float _StepSize;

float4 JumpFloodPS(VertexShaderOutput input) : COLOR
{
    float min_dist = 1;
    float2 min_dist_uv = float2(0, 0);

    [unroll]
    for (int y = -1; y <= 1; y++)
    {
        [unroll]
        for (int x = -1; x <= 1; x++)
        {
            float2 peekUV = input.UV + float2(x, y) * _Aspect.yx * _StepSize;

            float2 peek = tex2D(_MainTex, peekUV).xy;
            if (all(peek))
            {
                float2 dir = peek - input.UV;
                float dist = dot(dir, dir);
                if (dist < min_dist)
                {
                    min_dist = dist;
                    min_dist_uv = peek;
                }
            }
        }
    }

    return float4(min_dist_uv, 0, 1);
}

technique ScreenUVDrawing
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL ScreenUVPS();
    }
};

technique JumpFloodDrawing
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL JumpFloodPS();
    }
};

technique DistanceDrawing
{
    pass Pass0
    {
        VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL DistancePS();
    }
};

technique Drawing
{
	pass Pass0
	{
        VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL CascadePS();
    }
};