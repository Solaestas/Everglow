sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float3 uColor;
float uOpacity;
float3 uSecondaryColor;
float uTime;
float2 uScreenResolution;
float2 uScreenPosition;
float2 uTargetPosition;
float2 uImageOffset;
float uIntensity;
float uProgress;
float2 uDirection;
float2 uZoom;
float2 uImageSize0;
float2 uImageSize1;
float Stren;
float k0;
float b0;
float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    if (!any(color))
        return color;
    // pos 就是中心了
    float2 pos = float2(0.5, 0.5);
    // offset 是中心到当前点的向量
    float2 offset = (coords - pos);
    // 因为长宽比不同进行修正
    float2 rpos = offset * float2(uScreenResolution.x / uScreenResolution.y, 1);
    float dis = length(rpos);
    float4 color2 = tex2D(uImage0, pos + offset);
    float x0 = coords.x;
    float y0 = coords.y;
    float upvalue = abs(k0 * x0 - y0 + b0);
    float downvalue = sqrt(k0 * k0 + 1);
    float Ds = upvalue / downvalue;
    // 向量长度缩短0.8倍
    if (Ds < 0.03f)
    {
        tex2D(uImage0, pos + offset + float2(0.03 - Ds, (0.03 - Ds) * k0));
    }
    return color2;
}
technique Technique1
{
    pass Test
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}