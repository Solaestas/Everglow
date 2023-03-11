﻿sampler screenImage : register(s0);
sampler bloomImage : register(s1);
float2 uImageSize0;
float3 uViewAbsorptionRatio;
float uBloomIntensity;
float uBloomScatteringRatio;
float uBloomAbsorptionRate;
bool uFogScatterWithDistance;

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
	float3 origin = tex2D(screenImage, coords).rgb;
	float3 bloom = tex2D(bloomImage, coords).rgb * uBloomIntensity;
	float2 actual = coords - float2(0.5, 0.5);
	actual = actual * uImageSize0;
	float3 t = 1 / (1 + (length(actual) * 0.1 + 1) * uViewAbsorptionRatio);
	float bloomFactor = 1.0 - 1.0 / (1 + (length(actual) * 0.1 + 1) * uBloomScatteringRatio);
	return float4(lerp(origin * t, bloom * lerp(1.0, t, uBloomAbsorptionRate), bloomFactor), 1);
}

technique Technique1
{
	pass Test
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}
