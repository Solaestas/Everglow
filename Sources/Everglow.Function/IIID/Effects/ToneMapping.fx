sampler2D _MainTex : register(s0);

struct PSInput
{
    float2 Texcoord : TEXCOORD0;
};

#define rcp(x) 1.0 / (x)

float3 FastTonemapPerChannel(float3 c)
{
	return c * rcp(max(c.r, max(c.g, c.b)) + 1.0);
}


// Neutral tonemapping (Hable/Hejl/Frostbite)
// Input is linear RGB
// More accuracy to avoid NaN on extremely high values.
float3 NeutralCurve(float3 x, float a, float b, float c, float d, float e, float f)
{
	return ((x * (a * x + c * b) + d * e) / (x * (a * x + b) + d * f)) - e / f;
}

float3 NeutralTonemap(float3 x)
{
    // Tonemap
	const float a = 0.2;
	const float b = 0.29;
	const float c = 0.24;
	const float d = 0.272;
	const float e = 0.02;
	const float f = 0.3;
	const float whiteLevel = 5.3;
	const float whiteClip = 1.0;

	float3 whiteScale = (1.0).xxx / NeutralCurve(whiteLevel, a, b, c, d, e, f);
	x = NeutralCurve(x * whiteScale, a, b, c, d, e, f);
	x *= whiteScale;

    // Post-curve white point adjustment
	x /= whiteClip.xxx;

	return x;
}

float4 PixelShaderFunction_Fast(PSInput input) : SV_Target0
{
	float4 c = tex2D(_MainTex, input.Texcoord);
	c.xyz = FastTonemapPerChannel(c.xyz);
	return float4(pow(c.xyz, 1 / 2.2), c.a);
}

float4 PixelShaderFunction_Neutral(PSInput input) : SV_Target0
{
	float4 c = tex2D(_MainTex, input.Texcoord);
	c.xyz = NeutralTonemap(c.xyz);
	return float4(pow(c.xyz, 1 / 2.2), c.a);
}


technique ToneMapping
{
    pass Fast
    {
        PixelShader = compile ps_3_0 PixelShaderFunction_Fast();
    }
	pass Neutral
	{
		PixelShader = compile ps_3_0 PixelShaderFunction_Neutral();
	}
}
