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
float ValB;
float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    float4 c1 = tex2D(uImage0, float2(coords.x, coords.y));
    if (color.r * 0.3 + color.g * 0.6 + color.b * 0.1  > ValB)
    {
        return color;
    }
    return float4(color.r * (1 - ValB) * (1 - ValB), color.g * (1 - ValB) * (1 - ValB), color.b * sqrt(1 - ValB), color.a);
}

technique Technique1
{
    pass Test
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}