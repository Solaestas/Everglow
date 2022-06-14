sampler uImage0 : register(s0);

float4x4 uTransform;
float uTime;
float alpha;

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
    float4 BackG = tex2D(uImage0, float2(coords.x + uTime, clamp(coords.y,0,1)));
	return BackG * alpha;
}

technique Technique1
{
	pass Test
	{
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
}