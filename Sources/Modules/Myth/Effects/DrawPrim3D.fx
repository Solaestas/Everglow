sampler uImage0 : register(s0);
float4x4 uTransform;
struct VSInput
{
    float3 Pos : POSITION0;
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
    output.Texcoord = input.Texcoord;
    output.Pos = mul(float4(input.Pos, 1), uTransform);
    output.Color = input.Color;
    return output;
}

float4 PixelShaderFunction(PSInput input) : COLOR0
{
    float4 c = tex2D(uImage0, input.Texcoord.xy);
    c *= input.Color;
    return c;

}


technique Technique1
{
    pass Base
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }

}

