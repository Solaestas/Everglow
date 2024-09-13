sampler2D uImage : register(s0);
texture uNoise;
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
//valueC由Color.r代替
//float valueC;
//utime由Color.g代替
//float utime;
float4x4 uTransform;

struct VSInput
{
    float2 Pos : POSITION0;
    float4 Color : COLOR0;
    float3 Texcoord : TEXCOORD0;
};

struct PSInput
{
    float4 Pos : SV_POSITION;
    float4 Color : COLOR0;
    float3 Texcoord : TEXCOORD0;
};

PSInput VertexShaderFunction(VSInput input)
{
    PSInput output;
    output.Color = input.Color;
    output.Texcoord = input.Texcoord;
    output.Pos = mul(float4(input.Pos, 0, 1), uTransform);
    return output;
}

float4 PixelShaderFunction(PSInput input) : COLOR0
{
    float4 color = tex2D(uNoiseSampler, float2(input.Texcoord.x, input.Texcoord.y + input.Color.g));
    float4 colorFlame = tex2D(uImage, input.Texcoord.xy);
    
    float valueD = input.Color.r + input.Texcoord.z * 0.5f;
    float4 smog = float4(0, 0, 0, max(color.r / valueD * 3 - valueD - 1.5, 0));
    float4 flame = tex2D(uImage, float2((1 - color.r) / (1 - valueD), 0.5));
    smog.a *= input.Color.a;
    flame.a *= input.Color.a;
    if (!any(color))
        return float4(0, 0, 0, 0);
    if (color.r >= valueD)
        return flame;
    else
        return smog;
}

technique Technique1
{
    pass Test
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}