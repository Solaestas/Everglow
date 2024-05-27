sampler2D uImage : register(s0);
texture uHeatMap;
sampler uHeatMapSampler =
sampler_state
{
    Texture = <uHeatMap>;
    MipFilter = LINEAR;
    MinFilter = LINEAR;
    MagFilter = LINEAR;
    AddressU = CLAMP;
    AddressV = CLAMP;
};
texture uHeatMap2;
sampler uHeatMap2Sampler =
sampler_state
{
    Texture = <uHeatMap2>;
    MipFilter = LINEAR;
    MinFilter = LINEAR;
    MagFilter = LINEAR;
    AddressU = CLAMP;
    AddressV = CLAMP;
};
texture uNoise;
sampler uNoiseSampler =
sampler_state
{
    Texture = <uNoise>;
    MipFilter = LINEAR;
    MinFilter = LINEAR;
    MagFilter = LINEAR;
    AddressU = WRAP;
    AddressV = WRAP;
};
//utime由Color.b代替

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
    float4 colorNoise = tex2D(uNoiseSampler, input.Texcoord.xy);
    float4 colorHalo = tex2D(uImage, input.Color.xy);
    float light = colorHalo.r * colorNoise.r * 2;
    float4 colorHeatMap = tex2D(uHeatMapSampler, float2(light, input.Color.b));
    colorHeatMap.w *= 0;
    float4 colorHeatMap2 = tex2D(uHeatMap2Sampler, float2(light, input.Color.b));
    colorHeatMap.w *= input.Color.a;
    float4 colorHeatMap3 = colorHeatMap2 + colorHeatMap;
    return colorHeatMap3;
}

technique Technique1
{
    pass Test
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}