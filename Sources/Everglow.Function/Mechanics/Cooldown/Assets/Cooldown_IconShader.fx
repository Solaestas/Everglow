sampler2D uImage : register(s0);
texture uCut;
sampler uCutSampler =
sampler_state
{
	Texture = <uCut>;
	MipFilter = LINEAR;
	MinFilter = LINEAR;
	MagFilter = LINEAR;
	AddressU = WRAP;
	AddressV = WRAP;
};

texture uBg;
sampler uBgSampler =
sampler_state
{
	Texture = <uBg>;
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

float4 PixelShaderFunction(PSInput input) : COLOR0
{
	float4 cutTex = tex2D(uCutSampler, input.Texcoord.xy);
	float4 bgTex = tex2D(uBgSampler, input.Texcoord.xy);
	float4 mainTex = tex2D(uImage, input.Texcoord.xy);
	float4 color = mainTex * mainTex.a + bgTex * (1 - mainTex.a);
	return color * (1 - cutTex.a) * input.Color.a;
}

technique Technique1
{
	pass Main
	{
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
}