sampler screenImage : register(s0);
sampler bloomImage : register(s1);
float2 uImageSize0;
float3 uAbsorption;
float uBloomIntensity;
float uBloomAbsorption;


float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
	float3 origin = tex2D(screenImage, coords).rgb;
	float3 bloom = tex2D(bloomImage, coords).rgb * uBloomIntensity;
	float2 actual = coords - float2(0.5, 0.5);
	actual = actual * uImageSize0;
	float3 t = exp(-length(actual) * uAbsorption);
	float tb = exp(-uBloomAbsorption);
	
	return float4(lerp(bloom, origin, tb) * t, 1);
}

technique Technique1
{
	pass Test
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}
