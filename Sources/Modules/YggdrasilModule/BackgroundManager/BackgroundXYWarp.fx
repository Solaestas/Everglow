﻿sampler uImage0 : register(s0);

float4x4 uTransform;
float uTime;

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


float4 PixelShaderFunction(float3 coords : TEXCOORD0) : COLOR0
{
	float4 BackG = tex2D(uImage0, float2(coords.x - floor(coords.x), coords.y - floor(coords.y)));
	return BackG;
}

technique Technique1
{
	pass Test
	{
		VertexShader = compile vs_3_0 VertexShaderFunction();
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
}