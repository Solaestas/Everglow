sampler2D uImage0 : register(s0);
sampler2D uImage1 : register(s1);
float4x4 uTransform;
float duration;
float dissolveRange;
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
//x是采样半径
//z是投影x的坐标0~1
float4 PixelShaderFunction(PSInput input) : COLOR0
{
    float newX = input.Texcoord.z - 0.5;
    float newY = input.Texcoord.y;
    newX *= 2;
    newX = acos(clamp(newX, -1, 1)) / 3.14159265359;
    float mulCoor = 1 - abs(newX - 0.5) * 2;
    float2 newCoord = float2(newX + input.Texcoord.x, newY);
    float4 color = tex2D(uImage0, newCoord);
    float4 color_orig = tex2D(uImage0, newCoord);
    color *= input.Color;
    color.rgb *= mulCoor;

    if (1 - color.r < duration)
        return color;
    if (1 - color.r < duration + 0.2)
        return color * (color_orig.r - duration) * 5;
    return float4(0, 0, 0, 0);
}
float4 PixelShaderFunction2(PSInput input) : COLOR0
{
    float newX = input.Texcoord.z - 0.5;
    float newY = input.Texcoord.y;
    newX *= 2;
    newX = acos(clamp(newX, -1, 1)) / 3.14159265359;
    float mulCoor = 1 - abs(newX - 0.5) * 2;
    float2 newCoord = float2(newX + input.Texcoord.x, newY);
    float4 color = tex2D(uImage0, newCoord);
    float4 color_orig = tex2D(uImage0, newCoord);
    color *= input.Color;
    color.rgba *= mulCoor;

    if (1 - color.a < duration)
        return color;
    if (1 - color.a < duration + dissolveRange)
        return color * (color_orig.r - duration) * (1 / dissolveRange);
    return float4(0, 0, 0, 0);
}
technique Technique1
{
    pass RedEffect
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
    pass DarkEffect
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction2();
    }
}