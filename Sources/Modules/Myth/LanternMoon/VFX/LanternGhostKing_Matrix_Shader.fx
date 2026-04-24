sampler2D uImage : register(s0);
sampler2D uImage1 : register(s1);
float2 size1;
float uTime;
float warpScale;
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
	float4 color = tex2D(uImage, input.Texcoord.xy);
	if (input.Texcoord.z > 1)
	{
		return float4(0, 0, 0, 0);
	}
	else if (input.Texcoord.z > 0)
	{
		float value = input.Texcoord.z;
		float2 warp = tex2D(uImage1, input.Texcoord.xy * size1).rg - float2(0.5, 0.5);
		warp *= warpScale * value;
		color = tex2D(uImage, input.Texcoord.xy + warp);
		return color * input.Color;
	}
	else
	{
		return color * input.Color;
	}
	return float4(0, 0, 0, 0);
}

technique Technique1
{
	pass Fade
	{
		VertexShader = compile vs_3_0 VertexShaderFunction();
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
}