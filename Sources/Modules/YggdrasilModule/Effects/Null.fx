sampler uImage0 : register(s0);
sampler uImage1 : register(s1);

struct PSInput
{
    float4 Pos : SV_POSITION;
    float4 Color : COLOR0;
    float3 Texcoord : TEXCOORD0;
};
float4 PSFunction(PSInput input) : COLOR0
{
    float4 c0 = tex2D(uImage0, input.Texcoord) * input.Color;

    return c0;   
}
technique Technique1
{
    pass Test
    {
        PixelShader = compile ps_3_0 PSFunction();
    }
}