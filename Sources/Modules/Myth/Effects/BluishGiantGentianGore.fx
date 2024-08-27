sampler uImage0 : register(s0);
sampler uImage1 : register(s1);

float4x4 uTransform;
float4 uDissolveColor;

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
    float4 c = tex2D(uImage0, input.Texcoord.xy);
    if (!any(c))
        return float4(0, 0, 0, 0);
    c *= input.Color;
    float4 c1 = tex2D(uImage1, input.Texcoord.xy);
    float light = max(max(c1.r, c1.g), c1.b);
    float value = (light - input.Texcoord.z) * 2.5;
    if (light < input.Texcoord.z)
        return float4(0, 0, 0, 0);
    if (light < input.Texcoord.z + 0.4)
		return uDissolveColor * value + c * (1 - value);
    return c;
}

technique Technique1
{
    pass Test
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}