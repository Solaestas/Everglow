sampler2D uImage0 : register(s0);

float4x4 uTransform;

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
    float4 drawColor = input.Color;
    float2 coord = input.Texcoord;
	float4 c = tex2D(uImage0, coord.xy);//Ö÷ÎÆÀí
    float4 finalColor = float4(drawColor.r, drawColor.g * c.r, 0, c.a);
	return finalColor;
}

technique Technique1 
{
	pass Trail0 
	{
        VertexShader = compile vs_3_0 VertexShaderFunction();
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
}
