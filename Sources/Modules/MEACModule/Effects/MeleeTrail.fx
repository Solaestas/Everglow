texture2D tex0;
sampler2D uShapeTex = sampler_state
{
    Texture = <tex0>;
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};
texture2D tex1;
sampler2D uColorTex = sampler_state
{
    Texture = <tex1>;
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
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
    output.Texcoord = input.Texcoord;
    output.Color = input.Color;
    output.Pos = mul(float4(input.Pos, 0, 1), uTransform);
    return output;
}

float4 PixelShaderFunction(PSInput input) : COLOR0
{
    float3 coord = input.Texcoord;
    float4 c = tex2D(uShapeTex, float2(coord.x, coord.y)); //主纹理
    c *= tex2D(uColorTex, float2(c.r * coord.z, 0.5)); //乘颜色图
    return c * coord.z;

}

float4 PixelShaderFunction2(PSInput input) : COLOR0
{
    float3 coord = input.Texcoord;
    float4 c = tex2D(uShapeTex, float2(coord.x, coord.y)); //主纹理
    c = tex2D(uColorTex, float2(c.r * coord.z, 0.5)); //取颜色
    float a = 1 - (c.r - 1) * 0.7f;
    c *= a; //乘上透明度
    return c;
}

technique Technique1
{
    pass Trail
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
    pass Trail0
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction2();
    }
}
