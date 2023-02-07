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
float uRot;
float Col;
float radiu;
float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    if (!any(color))
        return color;
    float4 color3 = float4(color.rgb * Col, color.r * Col);
    float2 Bp0 = float2(0.5 ,0.5) - coords;
    
    float dis0 = length(Bp0.xy);
    float stre = abs(dis0 - radiu);
    if (stre < 0.14)
    {
        return float4(color3.rgba * sqrt((0.14 - stre) / 0.14f));
    }
    return float4(0, 0, 0, 0);
}

technique Technique1
{
    pass Test
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}