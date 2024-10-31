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

	float dx = 1 / uImageSize.x;
	float dy = 1 / uImageSize.y;
	bool flag = false;
	int width = 1;
	for (int i = -width; i <= width; i++)
	{
		for (int j = -width; j <= width; j++)
		{
			float checkx = input.Texcoord.x + dx * i;
			float checky = input.Texcoord.y + dy * i;
			float4 c = tex2D(uImage, input.Texcoord.xy + float2(dx * i, dy * j));
			if (any(mainTex) && (!any(c) || checkx > 1 || checkx < 0 || checky > 1 || checky < 0))
			{
				flag = true;
			}
		}
	}
	if (flag)
	{
		float timeFactorWithTexCoordX = (uTime * 0.05f) + 2 * input.Texcoord.x;
		float timeFactorWithTexCoordY = (uTime * 0.05f) + 2 * input.Texcoord.y;

		float redComponent = 0.2f + 0.2f * sin(timeFactorWithTexCoordX * 1.5f);
		float greenComponent = 0.6f + 0.2f * cos(timeFactorWithTexCoordY);
		float blueComponent = 0.7f + 0.3f * sin(timeFactorWithTexCoordX);
		float4 shineColor = float4(redComponent, greenComponent, blueComponent, 1);
		return shineColor * uChargeProgress * uChargeProgress + mainTex * input.Color * (1 - uChargeProgress * uChargeProgress);
	}
	return mainTex * input.Color;
}

technique Technique1
{
	pass Pixel
	{
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
}