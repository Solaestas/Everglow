sampler uImage0 : register(s0);
texture2D uPerlin;
sampler2D uPerlinTex = sampler_state
{
    Texture = <uPerlin>;
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};
texture2D uTrail;
sampler2D uTrailTex = sampler_state
{
    Texture = <uTrail>;
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};
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
    float4 color = tex2D(uImage0, input.Texcoord.xy);
    float4 fadePowder = tex2D(uPerlinTex, input.Texcoord.xy);
    float fadeLight = fadePowder.r;
    if (input.Texcoord.z > fadeLight)
        return color * input.Color * (fadePowder.r * 3);
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