sampler uImage0 : register(s0);
texture uImage1;
sampler2D s3 = sampler_state
{
    Texture = <uImage1>;
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};
texture uImage2;
sampler2D s4 = sampler_state
{
    Texture = <uImage2>;
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};
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
float xc;
float yc;
float rot;
float Strds;
float Ome;
float DMax;
float color0;
float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    float4 cT = tex2D(s3, coords);
    if (!any(color))
        return color;
    float4 c6 = float4(0, 0, 0, 0);
    float2 pos = float2(0.5, 0.5);
    float2 Cen = float2(xc, yc);
    float2 offset = (coords - pos);
    float2 offset2 = (Cen - coords);
    float2 r0 = DMax * float2(sin(rot + 1.5707), sin(rot)) / float2(sqrt(uScreenResolution.x / uScreenResolution.y), 1);
    float2 r1 = DMax * float2(sin(rot), sin(rot - 1.5707));
    float2 r2 = DMax * float2(sin(rot + 1.5707), sin(rot));
    float2 r3 = DMax * float2(sin(rot), sin(rot - 1.5707));
    float2 rpos2 = offset2 * float2(uScreenResolution.x / uScreenResolution.y, 1);

    float dis = length(rpos2);
    float4 color2 = tex2D(uImage0, pos + offset);
    float CO = (rpos2.x * r0.x + rpos2.y * r0.y) / (DMax * dis);
    float CO2 = (rpos2.x * r2.x + rpos2.y * r2.y) / (DMax * dis);
    float SI = (rpos2.x * r1.x + rpos2.y * r1.y) / (DMax * dis);
    float MaxT = (0.4 - Ome) / 0.4;
    float4 cT2 = tex2D(s3, float2(acos(CO2) / 1.5707 / (1 - MaxT), 1 - dis / DMax));
    float4 cTc = tex2D(s4, float2(0.5 + dis * 71, 0.5  - dis * 71));

    float2 V = float2(0, 0);
    
    float4 cT3 = float4(0, 0, 0, 0);
    if (1 - acos(CO2) / 1.5707 > MaxT + 0.1 && 1 - acos(SI) / 1.5707 > 0)
    {
        V = 1 * (rpos2.y, rpos2.x) * CO * Ome;
        cT3 = cT2;

    }
    c6 = tex2D(uImage0, pos + offset - V * cT3.r);
    cTc *= cT3;
    cTc *= color0;
    if (dis < DMax)
    {
        return cTc;
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