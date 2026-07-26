sampler2D uImage0 : register(s0);
sampler2D uImage1 : register(s1);

float4x4 uTransform;
float uSize;
float uThredshold;

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
	float4 c0 = tex2D(uImage0, (input.Pos.xy - input.Texcoord.xy) * uSize + float2(0, input.Texcoord.z * 0.3));
	float4 c1 = tex2D(uImage1, (input.Pos.xy - input.Texcoord.xy) * uSize + float2(0, input.Texcoord.z));
	float value = c0.r * c1.r;
	if (value > uThredshold + 0.3)
	{
		return float4(1, 1, 1, 1) * input.Color;
	}
	if (value > uThredshold + 0.15)
	{
		return float4(0.3, 0.5, 0.7, 1) * input.Color;
	}
	if (value > uThredshold)
	{
		return float4(0, 0.1, 0.5, 1) * input.Color;
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
