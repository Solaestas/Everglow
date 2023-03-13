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

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
    float gauss[3][3] =
    {
        Stren, Stren * 1.5, Stren,
        Stren * 1.5, 1 - Stren * 10, Stren * 1.5,
        Stren, Stren * 1.5, Stren
    };
    float4 color = tex2D(uImage0, coords);
    if (!any(color))
        return color;
    float dx = 2 / uScreenResolution.x;
    float dy = 2 / uScreenResolution.y;
    color = float4(0, 0, 0, 0);
    for (int i = -1; i <= 1; i++)
    {
        for (int j = -1; j <= 1; j++)
        {
            color += gauss[i + 1][j + 1] * tex2D(uImage0, float2(coords.x + dx * i, coords.y + dy * j));
        }
    }
    return color;
    
}
technique Technique1
{
    pass Test
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}