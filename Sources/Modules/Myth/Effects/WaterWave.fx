sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float m;
float n;
float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
    float4 c = tex2D(uImage0,coords);
	float a=max(c.r,max(c.g,c.b));
    if(a>m)//明度超过m的部分被替换为背景图片
	{
        float4 c1 = tex2D(uImage1, coords + float2(c.r * sin(c.g * 6.28 + 3.1416) / 50.0, c.r * sin(c.g* 6.28) / 50.0));
		return c1;
	}
	else
		return c*a;
}

technique Technique1
{
	pass Tentacle
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}   
}