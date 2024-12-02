sampler uImage0 : register(s0);

texture2D tex1;
sampler2D uStarrySky = sampler_state
{
    Texture = <tex1>;
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};
texture2D tex2;
sampler2D perlin = sampler_state
{
    Texture = <tex2>;
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};

float4x4 uTransform;
float uTime;

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


float4 PixelShaderFunction(float3 coords : TEXCOORD0, PSInput input) : COLOR0
{
	float2 p0coord = float2(coords.x * 0.2 + 0.5, coords.y * 0.2 + 0.5) + float2(cos(uTime) * 0.2, sin(uTime) * 0.2);
	float4 c0 = tex2D(perlin, float2(p0coord.x - floor(p0coord.x), p0coord.y - floor(p0coord.y)));


	float2 p2coord =  float2(coords.x * 0.5 + 0.25, coords.y * 0.5 + 0.25) + float2(cos(-uTime * 0.65) * 0.2, sin(-uTime * 0.65) * 0.2);
	float4 c2 = tex2D(perlin,  float2(p2coord.x - floor(p2coord.x), p2coord.y - floor(p2coord.y)));

	float4 c = tex2D(uStarrySky, float2(coords.x, coords.y) + float2(cos(c2.r * 6.283), sin(c2.r * 6.283)) * 0.01);

	float4 BackG = tex2D(uImage0, float2(coords.x, coords.y) + float2(cos(c0.r * 6.283), sin(c0.r * 6.283)) * 0.03);
	BackG.rgba *= input.Color.rgba;

	if(BackG.a > 0.75)
	    return float4(0,0,0,1) + c * (1 - (1 - BackG.a) * 4);



	return BackG * 1.3333;
}

technique Technique1
{
	pass Test
	{
		VertexShader = compile vs_3_0 VertexShaderFunction();
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
}