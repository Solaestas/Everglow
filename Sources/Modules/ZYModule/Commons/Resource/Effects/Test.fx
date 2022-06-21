#define _PI_ radians(180.0)
#define _PIOVER3_ (_PI_ / 3)
#define _PIOVER6_ (_PI_ / 6)
#define _RANDOM_ 0
#define _HIT_MAX_ 100
sampler uImage : register(s0);
float uTime;
float uOpacity;
float2 uResolution;
texture2D uNoise;
sampler uNoiseSampler =
sampler_state
{
    Texture = <uNoise>;
    MipFilter = LINEAR;
    MinFilter = LINEAR;
    MagFilter = LINEAR;
    AddressU = WRAP;
    AddressV = WRAP;
};

texture2D uColorBar;
sampler uColorBarSampler =
sampler_state
{
    Texture = <uColorBar>;
    MipFilter = LINEAR;
    MinFilter = LINEAR;
    MagFilter = LINEAR;
    AddressU = CLAMP;
    AddressV = CLAMP;
};
float wrap(float from, float to, float value)
{
    if(value > to)
    {
        value -= ceil((value - to) / (to - from)) * (to - from);
    }else
    {
        value += ceil((from - value) / (to - from)) * (to - from);
    }
    
    return value;
}
float4 PixelShaderFunction(float2 texCoord : TEXCOORD0) : COLOR0
{ 
    float4 color = tex2D(uImage, texCoord);
    if(uTime >= 120)
        return color;
    float intensity = uTime / 60 * step(uTime, 60) + (120 - uTime) / 60 * step(60, uTime);
    float2 worldCoord = (texCoord - float2(0.5, 0.5)) * uResolution; 
    float len = length(worldCoord);
    float angle = atan2(worldCoord.y, worldCoord.x) + _PI_;
    float t1 = tex2D(uNoiseSampler, float2(_RANDOM_, angle / 10)).x + 0.2;
    float3 hitColor = float3(0, 1, 0) * smoothstep(_HIT_MAX_ * t1 * intensity, 0, len) + float3(1, 1, 1) * smoothstep(_HIT_MAX_ * t1 * 0.4 * intensity, 0, len);
    //float3 lightColor = tex2D(uColorBarSampler, float2(intensity * (smoothstep(0.45, 0.5, texCoord.x) - smoothstep(0.5, 0.55, texCoord.x)), 0)) * step(texCoord.y, 0.5);
    return float4(color.rgb + hitColor, 1);
    //return color;
}

technique Technique1
{
    pass Test
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}