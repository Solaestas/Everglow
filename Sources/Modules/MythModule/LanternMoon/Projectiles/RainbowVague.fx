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

float2 ProjPos;
float waveSize;
float waveWidth;
float darkness;
float2 ProjCen;
texture uImage2;
sampler2D s3 = sampler_state
{
    Texture = <uImage2>;
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0 
{
    float2 offset = (coords - ProjPos);
    float2 rpos = offset * float2(uScreenResolution.x / uScreenResolution.y, 1);
    float dis = length(rpos);

    float2 offsetC = (coords - ProjCen);
    float2 rposC = offsetC * float2(uScreenResolution.x / uScreenResolution.y, 1);
    float disC = length(rposC);

    float stre = (waveWidth - clamp(abs(dis - waveSize), 0, waveWidth)) / waveWidth;
    //扭曲波段
    float CosStre = (cos(stre * 3.1415926535898) + 1) * 0.1 * waveWidth + 1 - 0.2 * waveWidth;
    float4 CNow = tex2D(uImage0, coords);   
    float4 CYellow = float4(1,0.8,0,1) - CNow;
    float CenterLig = clamp(1 - disC * 5, 0, 1) * (1 - CYellow.r);
    float light = ((1 - CosStre) * darkness + 1 - darkness) + CenterLig;
    light = clamp(light, 0, 1);
    float4 Cdraw = tex2D(uImage0, ProjPos + offset * CosStre) * light;
    float k0 = max(abs(dot(float2(cos(waveSize * 1.7),sin(waveSize * 1.7)), normalize(offset))) - 0.2,0.000001);
    float k2 = k0;
    float4 CRainbow = tex2D(s3, float2(clamp(0.5 + (dis - waveSize) / waveWidth / waveWidth / k2 / 1.5,0,1),0.5));
    Cdraw.rgb += CRainbow.rgb * waveWidth * waveWidth * 9 * (k0);
    return Cdraw;
}
technique Technique1
{
    pass Test
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}