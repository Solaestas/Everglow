sampler2D uImage : register(s0);

float uTime;

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

texture uNoiseMap;
sampler uNoiseMapSampler =
sampler_state
{
	Texture = <uNoiseMap>;
	MipFilter = LINEAR;
	MinFilter = LINEAR;
	MagFilter = LINEAR;
	AddressU = WRAP;
	AddressV = WRAP;
};

struct PSInput
{
	float4 Pos : SV_POSITION;
	float4 Color : COLOR0;
	float3 Texcoord : TEXCOORD0;
};

float4 PixelShader1(PSInput input) : COLOR0
{
	float4 resultColor = tex2D(uImage, input.Texcoord.xy);
	if (!any(resultColor) && length(float2(0.5, 0.5) - input.Texcoord.xy) <= 0.5)
	{
		resultColor = tex2D(uNoiseMapSampler, input.Texcoord.xy + float2(0, uTime));
	}
	else if (resultColor.r >= 0.9)
	{
		resultColor = tex2D(uHeatMapSampler, input.Texcoord.xy) * tex2D(uNoiseMapSampler, input.Texcoord.xy + float2(0, uTime));
	}
	else if (any(resultColor))
	{
		resultColor = float4(1, 1, 1, 0) - resultColor;
	}
	return resultColor * input.Color;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_3_0 PixelShader1();
	}
}