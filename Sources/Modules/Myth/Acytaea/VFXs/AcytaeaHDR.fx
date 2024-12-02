sampler2D uImage : register(s0);
float4x4 uTransform;
texture uShade;
sampler uShadeSampler =
sampler_state
{
    Texture = <uShade>;
    MipFilter = LINEAR;
    MinFilter = LINEAR;
    MagFilter = LINEAR;
    AddressU = WRAP;
    AddressV = WRAP;
};
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
    float4 light = tex2D(uImage, input.Texcoord.xy);
    return float4(0, 0, 0, 1 - light.g);
}
float4 PixelShaderFunction2(PSInput input) : COLOR0
{
    float4 color = tex2D(uImage, input.Texcoord.xy);
    float4 colorShade = tex2D(uShadeSampler, input.Texcoord.xy);
    float mulR = colorShade.r * input.Color.r * 8;
    return float4(color.r * mulR, color.gb / mulR, color.a * input.Color.a);
}

technique Technique1
{
    pass DarkCover
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
    pass Vivid
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction2();
    }
}