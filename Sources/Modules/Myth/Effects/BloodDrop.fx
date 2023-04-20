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
float rotation;
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
	float2 coo = coords.xy - float2(0.5, 0.5);
	float4 c = tex2D(uImage0, coords.xy);
	c *= environmentLight;
	c.a *= 0.6f;
	float4 c1 = tex2D(uPerlinTex, coords.xy);
	float light = max(max(c1.r,c1.g),c1.b);
	float value = (light - alphaValue) * 2.5;

	float cosTheta = dot(coo, float2(-0.5, -0.5)) / (length(coo) * 0.707);
	float cosRotation = cos(rotation);
	float deltaCos = abs(cosRotation - cosTheta);
	float lengthCoord = length(coo);

	if(light < alphaValue)
	    return float4(0, 0, 0, 0);
	if (light < alphaValue + 0.05)
		return float4(0, 0, 0, 1);
	if (lengthCoord < 0.2 * (1 - alphaValue) && deltaCos < 0.3)
		return float4(environmentLight.rgb * 0.6, environmentLight.a);
	if(light < alphaValue + 0.4)	    
	    return float4(0.9, 0, 0.4 * value, 1) * (1 - value) * environmentLight + c * value;
	return c;
}

technique Technique1
{
	pass Test
	{
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
}