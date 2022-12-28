sampler uImage0 : register(s0);
texture uImage1;
sampler2D s3 = sampler_state
{
    Texture = <uImage1>;
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};
float minr;
float4 BackCol;
float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    float4 colorGrey = tex2D(s3, coords);
    float delta = (minr - colorGrey.r + coords.y) / 0.45;
    delta = clamp(delta, 0, 1);
    float4 TrueC = color * BackCol;
    if (!any(color))
        return float4(0, 0, 0, 0);
    if (colorGrey.r < minr)
        return float4(0, 0, 0, 0);
    if (colorGrey.r < minr + 0.45)        
        return float4(0, 0, 0, 1 - delta) + TrueC * (1 - delta);
    return TrueC;
}
technique Technique1
{
    pass Test
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}