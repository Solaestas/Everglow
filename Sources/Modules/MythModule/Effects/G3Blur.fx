sampler uImage0 : register(s0);
float2 uImageSize0;
float uDelta;
float uIntensity;

float gauss[3][3] = {
	0.075, 0.124, 0.075,
	0.124, 0.204, 0.124,
	0.075, 0.124, 0.075
};


float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0 
{
	float dx = 1.0 / uImageSize0.x;
	float dy = 1.0 / uImageSize0.y;
	float origin = tex2D(uImage0, coords).r;
	float color = 0.0;
	for (int i = -1; i <= 1; i++)
	{
		for (int j = -1; j <= 1; j++)
		{
			color += gauss[i + 1][j + 1] * tex2D(uImage0, coords + float2(dx * i, dy * j) * uDelta).r;
		}
	}
	return float4(color * uIntensity, 0, 0, 1);
}

technique Technique1
{
	pass Test
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}
