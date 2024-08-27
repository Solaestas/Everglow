sampler2D uImage0 : register(s0);
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
float uNoiseSize; //不允许填0
float4x4 uTransform;
float2 drawOrigin;
float2 noiseCoord;

struct VSInput
{
    float2 Pos : POSITION0;
    float4 Color : COLOR0;
    float2 Texcoord : TEXCOORD0;
};

struct PSInput
{
    float4 Pos : SV_POSITION;
    float4 Color : COLOR0;
    float2 Texcoord : TEXCOORD0;
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
	//float4 newDrawOrigin = mul(float4(drawOrigin, 0, 1), uTransform);
	float distance = length(input.Pos.xy - drawOrigin.xy);
	float deltaY = drawOrigin.y - input.Pos.y;
	float value = distance * -0.0005 + 2 * -0.0014 * deltaY;
	if (value > 0)
	{
		float4 noise = tex2D(uNoiseSampler, input.Texcoord * uNoiseSize + noiseCoord);
		float2 addCoord = (noise.rg - float2(0.5, 0.5)) * (noise.b * value);
		float4 tex = tex2D(uImage0, input.Texcoord + addCoord) * input.Color;
		return tex * max(0.2 - value, 0) / 0.2;
	}
	return tex2D(uImage0, input.Texcoord) * input.Color;
}

technique Technique1
{
    pass Shader2D
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}