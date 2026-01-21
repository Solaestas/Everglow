sampler2D uImage : register(s0);
sampler2D uImage1 : register(s1);
sampler2D uImage2 : register(s2);
float2 size1;
float2 size2;
float2 size3;
float uTime;
float lerpGolden;
float warpScale;
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

float4 PixelShaderFunction_GoldenShield(PSInput input) : COLOR0
{
    float4 color = tex2D(uImage, input.Texcoord.xy);
	if (!any(color))
	{
		return float4(0, 0, 0, 0);
	}
	float4 color1 = tex2D(uImage1, input.Texcoord.xy * size1);
	if (input.Texcoord.z > color1.r)
	{
		float2 warp = tex2D(uNoiseSampler, input.Texcoord.xy * size3 + float2(0, uTime)).rg - float2(0.5, 0.5);
		warp *= warpScale;
		float4 color2 = tex2D(uImage2, input.Texcoord.xy * size2 + warp);
		color2 = lerp(color2, color, lerpGolden);
		return color2 * input.Color;
	}
	else if (input.Texcoord.z > color1.r - 0.03)
	{
		return float4(1, 0.8, 0.4, 0) * input.Color;
	}
	else
	{
		return color * input.Color;
	}
	return float4(0, 0, 0, 0);
}

float4 PixelShaderFunction_DecayAndFade(PSInput input) : COLOR0
{
	float4 color = tex2D(uImage, input.Texcoord.xy);
	if (!any(color))
	{
		return float4(0, 0, 0, 0);
	}
	float4 color1 = tex2D(uImage1, input.Texcoord.xy * size1);
	if (input.Texcoord.z > color1.r)
	{
		return float4(0, 0, 0, 0);
	}
	else if (input.Texcoord.z > color1.r - 0.5)
	{
		float value = (input.Texcoord.z - color1.r + 0.5) * 2;
		value = 1 - value;
		value = smoothstep(0, 1, value);
		float2 warp = tex2D(uNoiseSampler, input.Texcoord.xy * size3 + float2(0, uTime)).rg - float2(0.5, 0.5);
		warp *= warpScale * (1 - value);
		color = tex2D(uImage, input.Texcoord.xy + warp) * value;
		return color * input.Color;
	}
	else
	{
		return color * input.Color;
	}
	return float4(0, 0, 0, 0);
}

float4 PixelShaderFunction(PSInput input) : COLOR0
{
	float4 color = tex2D(uImage, input.Texcoord.xy);
	return color * input.Color;
}

technique Technique1
{
    pass GoldenSheild
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
		PixelShader = compile ps_3_0 PixelShaderFunction_GoldenShield();
	}
	pass DecayAndFade
	{
		VertexShader = compile vs_3_0 VertexShaderFunction();
		PixelShader = compile ps_3_0 PixelShaderFunction_DecayAndFade();
	}
	pass Normal
	{
		VertexShader = compile vs_3_0 VertexShaderFunction();
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
}