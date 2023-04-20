sampler uImage : register(s0);

float4 PixelShaderFunction(float2 texCoord : TEXCOORD0) : COLOR0
{
    return tex2D(uImage, texCoord);
}

technique Technique1
{
	pass Default
	{
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
}