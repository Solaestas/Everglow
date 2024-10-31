sampler2D uImage : register(s0);

float uChargeProgress;
float uTime;
float2 uImageSize;

struct PSInput
{
	float4 Pos : SV_POSITION;
	float4 Color : COLOR0;
	float3 Texcoord : TEXCOORD0;
};

float4 PixelShaderFunction(PSInput input) : COLOR0
{
	float4 mainTex = tex2D(uImage, input.Texcoord.xy);
	if (!any(mainTex))
	{
		return float4(0, 0, 0, 0);
	}
	
	float timeFactorWithTexCoordX = (uTime * 0.05f) + 2 * input.Texcoord.x;
	float timeFactorWithTexCoordY = (uTime * 0.05f) + 2 * input.Texcoord.y;

	float redComponent = 0.2f + 0.2f * sin(timeFactorWithTexCoordX * 1.5f);
	float greenComponent = 0.6f + 0.2f * cos(timeFactorWithTexCoordY);
	float blueComponent = 0.7f + 0.3f * sin(timeFactorWithTexCoordX);
	float4 shineColor = float4(redComponent, greenComponent, blueComponent, 1);
	return shineColor * uChargeProgress * uChargeProgress;
}

technique Technique1
{
	pass Pixel
	{
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
}