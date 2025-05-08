sampler2D uImage : register(s0);
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
texture uFade;
sampler uFadeSampler =
sampler_state
{
	Texture = <uFade>;
	MipFilter = LINEAR;
	MinFilter = LINEAR;
	MagFilter = LINEAR;
	AddressU = WRAP;
	AddressV = WRAP;
};

float uTime;
float uSize;
float4 uLight;
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
	float4 colorBase = tex2D(uImage, input.Texcoord.xy) * input.Color.rrrr;
	float2 noiseCoord = input.Texcoord.xy * uSize;
    float4 colorNoise = tex2D(uNoiseSampler, noiseCoord);
	float4 colorFade = tex2D(uFadeSampler, input.Texcoord.xy);
	float light = colorNoise.r;
	light += abs((uTime + noiseCoord.y) % 1 - 0.5) * 2 - 1;
	if (any(colorBase))
	{
		if (colorFade.r < input.Color.g - 0.1)
		{
			return float4(0, 0, 0, 0);
		}
		if (colorFade.r < input.Color.g)
		{
			return float4(1, 1, 1, 1);
		}
		float4 addColor = float4(0, 0, 0, 0);
		if (light > 0.2)
		{
			addColor = uLight;
		}
		colorBase += addColor;
		return colorBase;
	}
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