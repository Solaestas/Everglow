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
float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    float4 colorGrey = tex2D(s3, coords);
    float delta = (minr + 0.15 - colorGrey.r) / 0.15;
    if (!any(color))
        return float4(0, 0, 0, 0);
    if (colorGrey.r < minr)
        return float4(0, 0, 0, 0);
    if (colorGrey.r < minr + 0.15)        
        return float4(1 * delta, 0.5 * delta, 0, 0);
    return float4(0, 0, 0, 0);
}
technique Technique1
{
    pass Test
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}