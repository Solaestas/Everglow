sampler2D uImage : register(s0);
texture uHeatMap;
sampler uHeatMapSampler =
sampler_state
{
    Texture = <uHeatMap>;
    MipFilter = LINEAR;
    MinFilter = LINEAR;
    MagFilter = LINEAR;
    AddressU = CLAMP;
    AddressV = CLAMP;
};
texture uNoise;
sampler uNoiseSampler =
sampler_state
{
    Texture = <uNoise>;
    MipFilter = LINEAR;
    MinFilter = LINEAR;
    MagFilter = LINEAR;
    AddressU = WRAP;
    AddressV = WRAP;
};
//Noise_x由Color.r代替
//Noise_y由Color.g代替
//utime由Color.b代替

float4x4 uTransform;

struct VSInput
{
    float2 Pos : POSITION0;
    float4 Color : COLOR0;
    float3 Texcoord : TEXCOORD0;
};

struct PSInput
{
    float4 Pos : SV_POSITION;
    float4 Color : COLOR0;
    float3 Texcoord : TEXCOORD0;
};

PSInput VertexShaderFunction(VSInput input)
{
    PSInput output;
    output.Color = input.Color;
    output.Texcoord = input.Texcoord;
    output.Pos = mul(float4(input.Pos, 0, 1), uTransform);
    return output;
}

float4 PixelShaderFunction(PSInput input) : COLOR0
{
    float4 colorNoise = tex2D(uNoiseSampler, input.Texcoord.xy);
	float4 colorNoise2 = tex2D(uNoiseSampler, input.Pos.xy * 0.005);
	float light = 1 - colorNoise.r;
    float4 colorHeatMap = tex2D(uHeatMapSampler, float2(light - input.Color.r, 0.1f));
    colorHeatMap *= input.Color.aaaa;
	if (input.Texcoord.z > 0.91 + colorNoise2.r * 0.1)
	{
		return colorHeatMap = float4(0, 0, 0, 0);
	}
	if (input.Texcoord.z > 0.9 + colorNoise2.r * 0.1)
	{
		return colorHeatMap = tex2D(uHeatMapSampler, float2(0.4 + colorNoise2.r * 0.04, 0));
	}
	return colorHeatMap;
}

technique Technique1
{
    pass Test
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}