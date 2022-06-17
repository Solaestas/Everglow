sampler2D dustPrevTexture : register(s0);

float4 PixelShaderFunction(float2 texCoord : TEXCOORD) : COLOR0
{
	float4 data = tex2D(dustPrevTexture, texCoord);
	if (data.r > 0)
	{
		data.g -= 0.03;
		if (data.g <= 0)
		{
			return float4(0, 0, 0, 0);
		}
		return data;
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