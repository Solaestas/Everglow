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
//Noise_x由Color.r代替
//Noise_y由Color.g代替
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
	float2 newCoord = input.Texcoord.xy - float2(0.5, 0.5);
	newCoord.y /= input.Texcoord.z;
	newCoord += float2(0.5, 0.5);
	if (newCoord.y < 0 || newCoord.y > 1)
		return float4(0, 0, 0, 0);
	float4 colorNoise = tex2D(uNoiseSampler, newCoord);
    float light = 1 - colorNoise.r * (1 - input.Color.b);
    float4 colorHeatMap = tex2D(uHeatMapSampler, float2(light, 0));
    colorHeatMap *= float4(input.Color.ggg, 1);
    return colorHeatMap;
}

technique Technique1
{
    pass Test
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}