sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float m;
float n;
float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
    float4 c = tex2D(uImage0,coords);

	float a=max(c.r,max(c.g,c.b));
	float4 c1 = tex2D(uImage1,float2(coords.x + m,coords.y));
	float s = clamp((1.5 - a),0,1);
	return float4(c1.r*s,c1.g*s,c1.b*s,s);
}

technique Technique1
{
	pass Tentacle
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}   
}