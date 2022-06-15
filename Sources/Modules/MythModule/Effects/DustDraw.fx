sampler2D dustInfo : register(s0);

float2 uResolution;

float4 PixelShaderFunction(float2 texCoord : TEXCOORD) : COLOR0
{
	float4 data = tex2D(dustInfo, texCoord);
	float2 invRes = float2(1 / uResolution.x, 1 / uResolution.y);
	if (data.r > 0)
	{
		return float4(1, 1, 1, data.g);
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