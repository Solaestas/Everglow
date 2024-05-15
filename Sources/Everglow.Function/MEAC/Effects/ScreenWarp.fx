sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float strength;
float4 PSFunction(float2 coords : TEXCOORD0) : COLOR0//用一张图片（uImage1）去扭曲uImage0，r代表方向，g代表大小。
{
    float4 color = tex2D(uImage0,coords);
    float4 color2 = tex2D(uImage1, coords);
    if (!any(color2))
        return color;
    else
    {
        float2 vec = float2(0, 0); //表示移动的向量
        float rot = color2.r * 6.28;//把r(0~1)转化为弧度制的角度(0~2*pi)
        vec = float2(cos(rot), sin(rot)) * color2.g * strength;
        return tex2D(uImage0, coords + vec);
    }
    
}
float4 PSFunction2(float2 coords : TEXCOORD0) : COLOR0 
{
    float4 color = tex2D(uImage0, coords);
    float4 color2 = tex2D(uImage1, coords);
    if (!any(color2))
        return color;
    else
    {
        float2 vec = float2(0, 0); //表示移动的向量
        vec = float2(color2.r - 0.5, color2.g - 0.5) * strength * color2.b;
        return tex2D(uImage0, coords + vec);
    }
    
}
technique Technique1
{
    pass KScreen0
    {
        PixelShader = compile ps_3_0 PSFunction();
    }
    pass KScreen1
    {
        PixelShader = compile ps_3_0 PSFunction2();
    }
}