sampler uImage0 : register(s0);

uniform float uOffset;
float4 PixelShaderFunction(float4 drawColor : COLOR0,float2 coord : TEXCOORD0) : COLOR0
{
	
	float4 c = tex2D(uImage0, float2(coord.x , coord.y));
    float a = clamp((c.r+c.g+c.b)/3 + uOffset, 0, 1);
    float4 finalColor;
    if (a<0.5)
    {
        finalColor.rgb = lerp(float3(0,0,0),drawColor.rgb,a*2);
    }
    else
    {
        finalColor.rgb = lerp(drawColor.rgb, float3(1, 1, 1), (a - 0.5) * 2);
    }
    finalColor.a = c.a;
    return finalColor*drawColor.a;
}
float4 PixelShaderFunc(float4 drawColor : COLOR0, float2 coord : TEXCOORD0) : COLOR0
{
    float4 c = tex2D(uImage0, coord);
    if (any(c))
        return drawColor;
    return float4(0, 0, 0, 0);
}

technique Technique1 
{
	pass Colorize
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}
