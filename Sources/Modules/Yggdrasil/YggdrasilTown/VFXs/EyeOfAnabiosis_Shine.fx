sampler2D uImage : register(s0);

float uChargeProgress;
float uTime;
float2 uImageSize;

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

	float redComponent = 0.2f + 0.1f * sin(timeFactorWithTexCoordX * 1.5f);
	float greenComponent = 0.8f + 0.2f * cos(timeFactorWithTexCoordY);
	float blueComponent = 0.7f + 0.3f * sin(timeFactorWithTexCoordX);
	float4 shineColor = float4(redComponent, greenComponent, blueComponent, 1);
	float alpha = 1 - abs(distance(input.Texcoord.xy, float2(0.5f, 0.5f))) / 0.5f;
	return shineColor * uChargeProgress * uChargeProgress * float4(alpha.rrr, 0);
}

float4 PixelShaderFunction_MagicCircle(PSInput input) : COLOR0
{
	float4 mainTex = tex2D(uImage, input.Texcoord.xy);
	float4 noise = tex2D(uNoiseSampler, float2(input.Texcoord.x * 0.1 + uTime * 0.0015f, input.Texcoord.y * 0.5f));
	
	float timeFactorWithTexCoordX = (uTime * 0.05f) + 2 * input.Texcoord.x;
	float timeFactorWithTexCoordY = (uTime * 0.05f) + 2 * input.Texcoord.y * 2;

	float redComponent = 0.2f + 0.1f * sin(timeFactorWithTexCoordX * 1.5f);
	float greenComponent = 0.8f + 0.2f * cos(timeFactorWithTexCoordY);
	float blueComponent = 0.7f + 0.3f * sin(timeFactorWithTexCoordX);
	float4 shineColor = float4(redComponent, greenComponent, blueComponent, 1);
	if ((input.Texcoord.x < 0.8f || input.Texcoord.x > 0.2f) && input.Texcoord.y > 0.9)
	{
		return shineColor;
	}
	return shineColor * float4(noise.rgb, 0);
}

technique Technique1
{
	pass Ball_Pixel
	{
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}

	pass MagicCircle_Pixel
	{
		PixelShader = compile ps_3_0 PixelShaderFunction_MagicCircle();
	}
}