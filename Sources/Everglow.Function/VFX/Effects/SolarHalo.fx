sampler uImage0 : register(s0);
texture2D uRainbow;
sampler2D uRainbowTex = sampler_state
{
    Texture = <uRainbow>;
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};
float4x4 uTransform;
float2 uSunMoonPos;
float4 uSunMoonLight;
float uScreenYDevideByX;
float uHaloSize;
float uHaloRange;

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

float4 HaloRing(PSInput input) : COLOR0
{
    float4 color = tex2D(uImage0, input.Texcoord);
    float2 toSun = input.Texcoord - uSunMoonPos;
    toSun.x *= uScreenYDevideByX;
    float4 rainbow = tex2D(uRainbowTex, float2(0.5, length(toSun) * uHaloSize) - uHaloRange) * uSunMoonLight;
    color.rgb += rainbow.rgb * color.rgb;
    color.a = 0;
    return color;
}

float4 None(PSInput input) : COLOR0
{
    float4 color = tex2D(uImage0, input.Texcoord);
    return color;
}

technique Technique1
{
	pass HaloRing
	{
		PixelShader = compile ps_3_0 HaloRing();
        VertexShader = compile vs_3_0 VertexShaderFunction();
    }
    pass None
    {
        PixelShader = compile ps_3_0 None();
        VertexShader = compile vs_3_0 VertexShaderFunction();
    }
}