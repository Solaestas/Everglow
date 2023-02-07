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
    float2 Bp0 = float2(0.5 + 0.48 * sin(uRot + 1.5707963267949), 0.5 - 0.48 * sin(uRot));
    float2 Bp1 = float2(0.5 + 0.48 * sin(uRot + 2.0943951023932 + 1.5707963267949), 0.5 - 0.48 * sin(uRot + 2.0943951023932));
    float2 Bp2 = float2(0.5 + 0.48 * sin(uRot + 4.1887902047864 + 1.5707963267949), 0.5 - 0.48 * sin(uRot + 4.1887902047864));
    
    float dis0 = length(coords - Bp0.xy);
    if (dis0 < 0.14)
    {
        return float4(0, 0, 0, 0);
    }
    float dis1 = length(coords - Bp1.xy);
    if (dis1 < 0.14)
    {
        return float4(0, 0, 0, 0);
    }
    float dis2 = length(coords - Bp2.xy);
    if (dis2 < 0.14)
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