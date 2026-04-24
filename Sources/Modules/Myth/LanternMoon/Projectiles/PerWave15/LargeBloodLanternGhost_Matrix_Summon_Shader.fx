sampler2D uImage : register(s0);
sampler2D uImage1 : register(s1);
float4x4 uTransform;
float2 size1;

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
	float4 color = tex2D(uImage, input.Texcoord.xy);
	float4 color_dissolve = tex2D(uImage1, input.Texcoord.xy * size1);
	if (input.Texcoord.z < color_dissolve.r)
	{
		return float4(0, 0, 0, 0);
	}
	return color * input.Color;
}
technique Technique1
{
    pass Test
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}