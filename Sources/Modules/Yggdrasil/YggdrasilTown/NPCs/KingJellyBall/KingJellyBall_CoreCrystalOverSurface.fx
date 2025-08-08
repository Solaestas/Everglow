sampler2D uImage0 : register(s0);
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
	float4 c0 = tex2D(uImage0, input.Texcoord.xy);
	float4 c1 = tex2D(uHeatMapSampler, input.Texcoord.xy);
	float value = c1.r;
	if (value < input.Texcoord.z + 0.05 && value > input.Texcoord.z)
	{
		return float4(0.2, 1.4 ,2, 2) * input.Color;
	}
	if (value > input.Texcoord.z)
	{
		return c0 * input.Color;
	}
	return float4(0, 0 ,0 ,0);
}
technique Technique1
{
    pass Test
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}
