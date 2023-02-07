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
float x1;
float y1;
float Strds;
float DMax;
float LeMax;
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

    if (y0 <= k0 * x0 + b0)
    {
        if (Ds < DMax)
        {
            float2 Di = (x1 - x0, y1 - y0);
            float Le = length(Di);
            if (Le > LeMax)
            {
                Le = LeMax;
            }
            float Le2 = (LeMax - Le) / 100;
            float Str = (DMax - Ds) * (DMax - Ds) * (DMax - Ds) * 10000000;
            float2 Newx = (Str / downvalue * Le2 * Strds, Str * k0 / downvalue * Le2 * Strds);
            if (length(Newx) > 0.4 + (1 + sin(x0 * 100 + y0 * 100 + uTime)) * 0.1f)
            {
                float r = (0.4 + (1 + sin(x0 * 100 + y0 * 100 + uTime)) * 0.1f) / (length(Newx) * length(Newx) * length(Newx)) * 0.3f;
                return float4(r, 0, 0, 0);
            }
            return tex2D(uImage0, pos + offset - Newx);
        }         
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