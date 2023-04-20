sampler2D dustInfo : register(s0);

float3 uColor;
float2 uResolution;
float2 uResolutionInv;

float4 PixelShaderFunction(float2 texCoord : TEXCOORD) : COLOR0
{
	float4 data = tex2D(dustInfo, texCoord);
	float2 worldCoord = texCoord * float2(uResolution.x, uResolution.y);
	
	if (data.r > 0)
	{
		return float4(uColor, data.g);
	}

	return float4(0, 0, 0, 0);
}

technique Technique1
{
	pass Display
	{
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
}