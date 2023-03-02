sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
sampler uImage2 : register(s2);

float4x4 uTransform;
float uTime;

struct VSInput
{
    float2 Pos : POSITION0;
    float4 Color : COLOR0;
    float4 Texcoord : TEXCOORD0;
};

struct PSInput
{
    float4 Pos : SV_POSITION;
    float4 Color : COLOR0;
    float4 Texcoord : TEXCOORD0;
};

float4 PixelShaderFunction(PSInput input) : COLOR0
{
    float4 coord = input.Texcoord;
    float y = uTime + coord.x;
    float4 c1 = tex2D(uImage1, float2(coord.x, coord.y));
    float4 c3 = tex2D(uImage2, float2(y, coord.y));
    c1 *= c3;
    float4 c = tex2D(uImage0, float2(c1.r, coord.a));
   
    if (c1.r < coord.x - 0.1)
        return float4(0, 0, 0, 0);
    return c * coord.z * 2;

}

PSInput VertexShaderFunction(VSInput input)
{
    PSInput output;
    output.Color = input.Color;
    output.Texcoord = input.Texcoord;
    output.Pos = mul(float4(input.Pos, 0, 1), uTransform);
    return output;
}


technique Technique1
{
    pass ColorBar
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}