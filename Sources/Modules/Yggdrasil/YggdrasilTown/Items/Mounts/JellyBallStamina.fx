sampler2D uImage : register(s0);
float duration;

struct PSInput
{
    float4 Pos : SV_POSITION;
    float4 Color : COLOR0;
    float3 Texcoord : TEXCOORD0;
};

float4 PixelShaderFunction(PSInput input) : COLOR0
{
    float4 mainTex = tex2D(uImage, input.Texcoord.xy);
	if (input.Texcoord.y < duration)
		return float4(0, 0, 0, 0);
	return mainTex * input.Color;
}

technique Technique1
{
    pass Test
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}