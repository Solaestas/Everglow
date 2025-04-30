sampler2D uImage : register(s0);

struct PSInput
{
	float2 Pos : SV_POSITION;
	float4 Color : COLOR0;
	float3 Texcoord : TEXCOORD0;
};

float4 PixelShaderFunction(PSInput input) : COLOR0
{
	float4 color = tex2D(uImage, input.Texcoord.xy);
	if (any(color))
	{
		return input.Color;
	}
	return float4(0, 0, 0, 0);
}

technique Technique0
{
    pass Test
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}