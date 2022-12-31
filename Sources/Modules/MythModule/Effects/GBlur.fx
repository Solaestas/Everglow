sampler uImage0 : register(s0);
float2 uImageSize0;
bool uHorizontal;
float uDelta;

// float weight[5] = {0.227027, 0.1945946, 0.1216216, 0.054054, 0.016216};

static float offset[3] = { 0, 1.38461533, 3.23076704 };
static float weight[3] = { 0.227027, 0.3162162, 0.07027 };

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
	float dx = 1.0 / uImageSize0.x;
	float dy = 1.0 / uImageSize0.y;
	float4 origin = tex2D(uImage0, coords);
	float4 color = weight[0] * tex2D(uImage0, coords);
	for (int i = 1; i < 3; i++)
	{
		if (uHorizontal)
		{
			color += weight[i] * tex2D(uImage0, coords + float2(offset[i] * dx * uDelta, 0));
			color += weight[i] * tex2D(uImage0, coords - float2(offset[i] * dx * uDelta, 0));
		}
		else
		{
			color += weight[i] * tex2D(uImage0, coords + float2(0, offset[i] * dy * uDelta));
			color += weight[i] * tex2D(uImage0, coords - float2(0, offset[i] * dy * uDelta));
		}
	}
	return color;
}

technique Technique1
{
	pass Test
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}
