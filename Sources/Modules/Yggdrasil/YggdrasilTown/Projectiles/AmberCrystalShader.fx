sampler2D uImage0 : register(s0);//Base color
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
float duration;
float uNoiseSize;//不允许填0
float2 uNoiseXY;
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
    float4 colorNoise = tex2D(uNoiseSampler, input.Texcoord.xy / uNoiseSize + uNoiseXY);
    float4 mainTex = tex2D(uImage0, input.Texcoord.xy);
	float4 heatMap = tex2D(uHeatMapSampler, float2(clamp((mainTex * colorNoise).r * 1.5f, 0, 1), 0.5));
    if (colorNoise.r < duration)
		return heatMap * input.Color;
    return float4(0, 0, 0, 0);
}

technique Technique1
{
    pass Test
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}