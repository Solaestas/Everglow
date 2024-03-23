sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
sampler uImage2 : register(s2);
float strength;

struct PSInput
{
    float4 Pos : SV_POSITION;
    float4 Color : COLOR0;
    float2 Texcoord : TEXCOORD0;
};

float4 PSFunction(PSInput input) : COLOR0
{
    float2 coords = input.Texcoord;
    float4 color = tex2D(uImage0, coords);
    float4 color2 = tex2D(uImage1, coords);
    float4 color3 = tex2D(uImage2, coords);
    if (!any(color2))
        return color;
    else
    {
        float2 vec = float2(0, 0); //表示移动的向量
        vec = float2(color2.r - 0.5, color2.g - 0.5) * strength * color2.b;
        float4 color4 = tex2D(uImage0, coords + vec * color3.r);
        return color4 * input.Color;
    }
    
}
technique Technique1
{
    pass SwayGrass
    {
        PixelShader = compile ps_3_0 PSFunction();
    }
}