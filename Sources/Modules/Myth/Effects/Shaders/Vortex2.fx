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
float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    if (!any(color))
        return color;
    float4 color3 = float4(color.rgb * Col, color.r * Col);
    float2 Bp0 = float2(0.5, 0.5);
    
    float dis0 = length(coords - Bp0.xy);
    if (dis0 < 0.22)
    {
        return float4(0, 0, 0, 0);
    }
    if (dis0 > 0.4)
    {
        return float4(0, 0, 0, 0);
    }
    return color3;
}

technique Technique1
{
    pass Test
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}