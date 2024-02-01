sampler2D uImage : register(s0);
sampler2D uImage1 : register(s1);
sampler2D uImage2 : register(s2);
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

float4x4 uTransform;
float uTime;

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
    float2 warp = tex2D(uNoiseSampler, input.Texcoord.xz).rg - float2(0.5, 0.5);
    warp *= (1 - input.Texcoord.y) * 0.15;
    float4 color = tex2D(uImage, input.Texcoord.xy + warp);
    float4 color2 = tex2D(uImage2, input.Texcoord.xz);
    float4 colorFlame = tex2D(uImage1, float2(sqrt(color.r * color2.r), 0.5));
    return colorFlame * input.Color;
}

technique Technique1
{
    pass Test
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}