sampler uImage0 : register(s0);

float2 uImageSize0;
float uDelta;


//float bi_linear(float2 coords, float2 resolution)
//{
//	float color1 = tex2D(uImage0, coords + float2(-resolution.x, -resolution.y) * uDelta).r;
//	float color2 = tex2D(uImage0, coords + float2(-resolution.x, resolution.y) * uDelta).r;
//	float color3 = tex2D(uImage0, coords + float2(resolution.x, -resolution.y) * uDelta).r;
//	float color4 = tex2D(uImage0, coords + float2(resolution.x, resolution.y) * uDelta).r;
//	float2 v = fmod(coords, resolution) / resolution;
//	return lerp(lerp(color1, color3, v.x), lerp(color2, color4, v.x), v.y);
//}

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0{
	float dx = 1.0 / uImageSize0.x;
	float dy = 1.0 / uImageSize0.y;
	float3 color1 = tex2D(uImage0, coords + float2(dx, dy) * uDelta).rgb;
	float3 color2 = tex2D(uImage0, coords + float2(-dx, dy) * uDelta).rgb;
	float3 color3 = tex2D(uImage0, coords + float2(dx, -dy) * uDelta).rgb;
	float3 color4 = tex2D(uImage0, coords + float2(-dx, -dy) * uDelta).rgb;
	float3 c = (color1 + color2 + color3 + color4) * 0.25;
	return float4(c, 1);
}

technique Technique1
{
	pass Test
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}
