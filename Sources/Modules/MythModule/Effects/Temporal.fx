sampler curTexture : register(s0);
sampler prevTexture : register(s1);
float2 uImageSize0;
float2 uImageSize1;
float uAlpha;
float2 uOffset;

float3 sample(float2 coord)
{
	if (coord.x < 0 || coord.x > 1 || coord.y < 0 || coord.y > 1)
	{
		return 0;
	}
	return tex2D(prevTexture, coord).xyz;
}

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
	float2 pos1 = coords.xy * uImageSize0;
	pos1 += uOffset;
	
	float3 src = tex2D(curTexture, coords).xyz;
	float2 src2UV = pos1 / uImageSize1;

	return float4(lerp(sample(src2UV), src, uAlpha), 1);

}

technique Technique1
{
	pass Test
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}
