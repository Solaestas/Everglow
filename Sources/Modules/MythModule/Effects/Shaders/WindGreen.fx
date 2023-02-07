sampler uImage0 : register(s0);
float minr;
float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    if (color.r + color.g + color.b > minr)        
        return color;
    return float4(0, 0, 0, 0);
}
technique Technique1
{
    pass Test
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}