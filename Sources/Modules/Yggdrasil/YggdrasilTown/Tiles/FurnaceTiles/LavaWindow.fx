sampler2D uImage0 : register(s0);
texture uLight;
float uSize;
float uTime;
sampler uLightSampler =
sampler_state
{
	Texture = <uLight>;
	MipFilter = POINT;
	MinFilter = POINT;
	MagFilter = POINT;
	AddressU = WRAP;
	AddressV = WRAP;
};
texture uHeatmap;
sampler uHeatmapSampler =
sampler_state
{
	Texture = <uHeatmap>;
	MipFilter = POINT;
	MinFilter = POINT;
	MagFilter = POINT;
	AddressU = CLAMP;
	AddressV = CLAMP;
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
    output.Color = input.Color;
    output.Texcoord = input.Texcoord;
    output.Pos = mul(float4(input.Pos, 0, 1), uTransform);
    return output;
}

float4 PixelShaderFunction(PSInput input) : COLOR0
{
    float4 drawColor=input.Color;
    float2 uv=input.Texcoord.xy;
    float4 col = tex2D(uImage0, uv);
	float lightColor = tex2D(uLightSampler, uv * uSize + float2(0, uTime * 0.05)).r;
	float uLavaLevelHeight = 0.25;
	for (int k = 0; k < 8;k++)
	{
		uLavaLevelHeight += 0.04 * pow(2, -k) * sin(uTime * cos(k * 3.73) + uv.x * 4 * pow(2, k));
	}
	float4 lavaColor = tex2D(uHeatmapSampler, lightColor);
	if (uv.y < uLavaLevelHeight)
	{
		return float4(91 / 255, 43 / 255, 40/ 255, 1);
	}
	if (uv.y < uLavaLevelHeight + 0.03)
	{
		lavaColor = tex2D(uHeatmapSampler, 0.99);
	}
	return col * lavaColor * input.Color;
}

technique Technique1
{
    pass Shader2D
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}