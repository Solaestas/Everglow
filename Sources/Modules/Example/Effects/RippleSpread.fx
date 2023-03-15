sampler uImage0 : register(s0);

// 每横纵像素所占据的纹理坐标值
float2 uDeltaXY;
// 波纹扩散过程的阻尼
float uDamping;

float4 PixelShaderFunction(float2 texCoord : TEXCOORD0) : COLOR0
{
	float left = tex2D(uImage0, texCoord + float2(-uDeltaXY.x, 0)).x;
	float right = tex2D(uImage0, texCoord + float2(uDeltaXY.x, 0)).x;
	float up = tex2D(uImage0, texCoord + float2(0, -uDeltaXY.y)).x;
	float down = tex2D(uImage0, texCoord + float2(0, uDeltaXY.y)).x;
	
	float2 center = tex2D(uImage0, texCoord).xy;
	
	float v = -(center.y - .5) * 2. + (left + right + up + down - 2.);
	v *= uDamping;
	v = v * 0.5 + 0.5;
	return float4(v, center.x, 0, 1);
}

technique Technique1
{
	pass Spread
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}