sampler uImage0 : register(s0);
texture2D tex0;
sampler2D uPerlinTex = sampler_state
{
    Texture = <tex0>;
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};
float alphaValue;
float4 environmentLight;

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

float4 PixelShaderFunction(float3 coords : TEXCOORD0) : COLOR0
{
	float4 c = tex2D(uImage0, coords.xy);
	c *= environmentLight;
	if(c.r + c.g + c.a < 0.01)
	    return float4(0, 0, 0, 0);
	float4 c1 = tex2D(uPerlinTex, coords.xy);
	float light = max(max(c1.r,c1.g),c1.b);
	float value = (light - alphaValue) * 2.5;
	if(light < alphaValue)
	    return float4(0, 0, 0, 0);
	if(light < alphaValue + 0.4)	    
	    return float4(1, 0.5, 0, 0) * value + c * (1 - value);
	return c;
}

technique Technique1
{
	pass Test
	{
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
}