sampler2D uImage0 : register(s0);
sampler2D uImage1 : register(s1);
float4x4 uTransform;
float uProcession;
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
    float newX = input.Texcoord.x % 1;
    float newY = input.Texcoord.y - 0.5;

    newY *= 1 / sin(input.Texcoord.z * 3.141592653589793238);
    newY += 0.5;
    newY = clamp(newY, 0, 1);
    float2 newCoord = float2(newX, newY);
    float4 color = tex2D(uImage0, newCoord);
    color *= input.Color;
    return color;
}
float4 PixelShaderFunction2(PSInput input) : COLOR0
{
    float newX = input.Texcoord.x % 1;
    float newY = input.Texcoord.y - 0.5;

    newY *= 1 / sin(input.Texcoord.z * 3.141592653589793238);
    newY += 0.5;
    newY = clamp(newY, 0, 1);
    float2 newCoord = float2(newX, newY);
    float4 color = tex2D(uImage0, newCoord);
    float4 color2 = tex2D(uImage1, float2(color.r, 0.5));
    color2.a *= 0;
    return color2;
}
technique Technique1
{
    pass Test
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
    pass Test2
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction2();
    }
}