sampler2D uImage0 : register(s0);

float4 PixelShaderFunction(float2 texCoord : TEXCOORD) : COLOR0
{
	float h = tex2D(uImage0, texCoord).x;
	float sh = 1.35 - h*2.;
	float3 c =
       float3(exp(pow(sh - .75, 2.) * -10.),
            exp(pow(sh-.50,2.)*-20.),
            exp(pow(sh-.25,2.)*-10.));
	return float4(c, 1);
}

technique Technique1
{
	pass Display
	{
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
}