sampler uImage0 : register(s0);

float4x4 uTransform;
float uTime;

float4 PixelShaderFunction(float4 drawColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float4 BackG = tex2D(uImage0, float2(coords.x + uTime, clamp(coords.y,0,1)));
	return BackG * drawColor;
}

technique Technique1
{
	pass Test
	{
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
}