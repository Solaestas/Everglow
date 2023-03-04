sampler uImage0 : register(s0);
texture2D tex0;
sampler2D uShapeTex = sampler_state
{
    Texture = <tex0>;
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};
float4x4 uTransform;
float alphaValue;

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


float4 PixelShaderFunction(float3 coords : TEXCOORD0) : COLOR0
{
	float4 c = tex2D(uImage0, coords.xy);
	float light = max(max(c.r,c.g),c.b);
	if(light < alphaValue)
	    return float4(0, 0, 0, 0);
    float4 c1 = tex2D(uShapeTex, float2((c.r - alphaValue) / (1 - alphaValue),0));
	return c1;
}

technique Technique1
{
	pass Test
	{
		VertexShader = compile vs_3_0 VertexShaderFunction();
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
}