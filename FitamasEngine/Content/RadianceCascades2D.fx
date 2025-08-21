/*
MIT License

Copyright (c) 2024 Youssef Afella

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

#define TAU 6.28318530718

sampler2D _MainTex;

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
     
float2 CalculateRayRange(uint index, uint count)
{
                //A relatively cheap way to calculate ray ranges instead of using pow()
                //The values returned : 0, 3, 15, 63, 255
                //Dividing by 3       : 0, 1, 5, 21, 85
                //and the distance between each value is multiplied by 4 each time

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

    [unroll]
    //[loop]
    for (int i = 0; i < 16; i++) //TODO 32
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
    float2 pixelIndex = floor(input.UV.xy * _CascadeResolution);
    
    uint blockSqrtCount = pow(2, _CascadeLevel); //Another way to write 1 << _CascadeLevel
    
    float2 blockDim = _CascadeResolution / blockSqrtCount;
    float2 block2DIndex = floor(pixelIndex / blockDim);
    float blockIndex = block2DIndex.x + block2DIndex.y * blockSqrtCount;
    
    float2 coordsInBlock = fmod(pixelIndex, blockDim);
    
    float4 finalResult = 0;
                
    float2 rayOrigin = (coordsInBlock + 4) * blockSqrtCount;
    float2 rayRange = CalculateRayRange(_CascadeLevel, _CascadeCount);
    
    [unroll]
    for (int i = 0; i < 4; i++)
    {
        float angleStep = TAU / (blockSqrtCount * blockSqrtCount * 4);
        float angleIndex = blockIndex * 4 + i;
        float angle = (angleIndex + 0.5) * angleStep;
    
        float2 rayDirection = float2(cos(angle), sin(angle));
                    
        float4 radiance = SampleRadianceSDF(rayOrigin / _CascadeResolution, rayDirection, rayRange);
                    
        if (radiance.a != 0)
        {
            if (_CascadeLevel + 1 != _CascadeCount)
            {
                            //Merging with the Upper Cascade (_MainTex)
                float2 position = coordsInBlock * 0.5 + 4;
                float2 positionOffset = float2(fmod(angleIndex, blockSqrtCount * 2), floor(angleIndex / (blockSqrtCount * 2)));

                position = clamp(position, 8, blockDim * 0.5 - 8);
                float2 uv = (position + positionOffset * blockDim * 0.5) / _CascadeResolution;
            
                //float4 rad = tex2Dlod(_MainTex, float4(uv, 0, 0));
                float4 rad = tex2D(_MainTex, uv);

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
              
        finalResult += radiance * 0.25;
    }

    return finalResult;
}

float4 DistancePS(VertexShaderOutput input) : COLOR
{
    float2 jf = tex2D(_MainTex, input.UV).rg;
    float2 dir = (jf - input.UV) * _Aspect;
    float value = length(dir);

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
            float2 peekUV = input.UV + float2(x, y) * _Aspect.yx * float2(_StepSize, _StepSize);

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

technique CascadesDrawing
{
	pass Pass0
	{
        VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL CascadePS();
    }
};