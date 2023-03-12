sampler uImage0 : register(s0);
sampler uImage1 : register(s1);

float4 PSFunction(float2 coords : TEXCOORD0) : COLOR0//用一张图片（uImage1）遮挡特效的绘制
{
    float4 color = tex2D(uImage0,coords);
    float4 color2 = tex2D(uImage1, coords);
    if (!any(color2))
        return color;
    else
    {
        float r0 = max(color.r - color2.a, 0);
        float g0 = max(color.g - color2.a, 0);
        float b0 = max(color.b - color2.a, 0);
        float a0 = max(color.a - color2.a, 0);

        return float4(r0, g0, b0, a0);
    }
    
}
technique Technique1
{
    pass KScreen0
    {
        PixelShader = compile ps_2_0 PSFunction();
    }
}