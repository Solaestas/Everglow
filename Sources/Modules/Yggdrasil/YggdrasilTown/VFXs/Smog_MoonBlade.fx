sampler2D uImage : register(s0);
texture2D uPowder;
sampler2D uPowderTex = sampler_state
{
    Texture = <uPowder>;
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};
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
    float4 color = tex2D(uNoiseSampler, input.Texcoord.xy);
    float light = min((1 - color.r) * (1 - input.Color.r) + input.Color.r, 1);
    float4 flame = tex2D(uImage, float2(light, 0.5));

    float4 fadePowder = tex2D(uPowderTex, input.Texcoord.xy);
    float fadeLight = fadePowder.r;
    if (input.Texcoord.z > fadeLight)
        return flame * fadePowder.r * 4;
    return float4(0, 0, 0, 0);
}

technique Technique1
{
    pass Test
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}