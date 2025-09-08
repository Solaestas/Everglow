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

struct PSInput
{
	float4 Pos : SV_POSITION;
	float4 Color : COLOR0;
	float3 Texcoord : TEXCOORD0;
	float3 Texcoord2 : TEXCOORD1;
};

float4 UniverseWorm(PSInput input) : COlOR0
{
	float4 mainColor = tex2D(uImage, input.Texcoord.xy);
	float4 black = float4(0, 0, 0, 1);
	
	float2 texCoord = input.Texcoord.xy;
	texCoord.y /= 200;
	float4 heatColor = tex2D(uHeatMapSampler, texCoord * 0.01);

	float4 resultColor = lerp(black, heatColor, mainColor.r / 0.02);
	resultColor = lerp(resultColor, float4(heatColor.xyz, 0), mainColor.r);
	return resultColor;
}

technique Technique1
{
	pass Worm
	{
		PixelShader = compile ps_3_0 UniverseWorm();

	}
}