sampler2D notUsed : register(s0);
sampler2D waterDisortion : register(s1);

float2 uZoom;
float2 uOffset;
float uThreashold;

float4 PixelShaderFunction(float2 texCoord : TEXCOORD) : COLOR0
{
	float4 data = tex2D(waterDisortion, texCoord);
	if (data.r * 2 - 1 > uThreashold)
	{
		return float4(1, 1, 1, 1);
	}
	return float4(0, 0, 0, 0);
}

technique Technique1
{
	pass Display
	{
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
}