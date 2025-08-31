sampler uImage0 : register(s0);
sampler uImage1 : register(s1);

float2 uInvImageSize;
bool uHorizontal;
float uDelta;

float uIntensity;

// float weight[5] = {0.227027, 0.1945946, 0.1216216, 0.054054, 0.016216};

static float offset[3] = { 0, 1.38461533, 3.23076704 };
static float weight[3] = { 0.227027, 0.3162162, 0.07027 };

float4 PS_GaussianBlur(float2 coords : TEXCOORD0) : COLOR0
{
	float dx = uInvImageSize.x;
	float dy = uInvImageSize.y;
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


float4 PS_Box(float2 coords : TEXCOORD0) : COLOR0
{
	float dx = uInvImageSize.x;
	float dy = uInvImageSize.y;
	float4 color1 = tex2D(uImage0, coords + float2(dx, dy) * uDelta);
	float4 color2 = tex2D(uImage0, coords + float2(-dx, dy) * uDelta);
	float4 color3 = tex2D(uImage0, coords + float2(dx, -dy) * uDelta);
	float4 color4 = tex2D(uImage0, coords + float2(-dx, -dy) * uDelta);
	float4 c = (color1 + color2 + color3 + color4) * 0.25;
	return c;
}


float4 PS_BlendBloom(float2 coords : TEXCOORD0) : COLOR0
{
	return tex2D(uImage0, coords) + tex2D(uImage1, coords) * uIntensity;
}


technique Technique1
{
	pass GBlur
	{
		PixelShader = compile ps_2_0 PS_GaussianBlur();
	}

	pass Box
	{
		PixelShader = compile ps_2_0 PS_Box();
	}

	pass Blend
	{
		PixelShader = compile ps_2_0 PS_BlendBloom();
	}
}
