sampler2D uImage : register(s0);
sampler2D uImage1 : register(s1);

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
    output.Color = input.Color;
    output.Texcoord = input.Texcoord;
    output.Pos = mul(float4(input.Pos, 0, 1), uTransform);
    return output;
}

float4 PixelShaderFunction(PSInput input) : COLOR0
{
    float2 coordXY = input.Texcoord.xy;
    coordXY -= float2(0.5, 0.5);
    coordXY.y /= input.Texcoord.z;
    coordXY += float2(0.5, 0.5);
    coordXY.y = clamp(coordXY.y, 0, 1);
    float4 color = tex2D(uImage, coordXY);
    return color * input.Color;
}
float4 PixelShaderFunction_heatMap(PSInput input) : COLOR0
{
    float2 coordXY = input.Texcoord.xy;
    coordXY -= float2(0.5, 0.5);
    coordXY.y /= input.Texcoord.z;
    coordXY += float2(0.5, 0.5);
    coordXY.y = clamp(coordXY.y, 0, 1);
    float4 color0 = tex2D(uImage, coordXY);
    float4 color = tex2D(uImage1, float2(clamp(color0.r * input.Color.a, 0, 1), 0.5));
    return color * float4(input.Color.rgb, 1);
}
technique Technique1
{
    pass Test
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
    pass HeatMap
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction_heatMap();
    }
}