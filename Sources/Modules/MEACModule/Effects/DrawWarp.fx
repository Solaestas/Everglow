sampler uShapeTex : register(s0);

float4 PixelShaderFunction(float4 drawColor : Color0,float3 coord : TEXCOORD0) : COLOR0
{
	float4 c = tex2D(uShapeTex, float2(coord.x , coord.y));//Ö÷ÎÆÀí
    float4 finalColor = float4(drawColor.r, drawColor.g, 0, 1) * c.r;
	return finalColor;
}

technique Technique1 
{
	pass Trail0 
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}
