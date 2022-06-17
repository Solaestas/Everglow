sampler2D dustInfo : register(s0);

float3 uColor;
float2 uResolution;
float2 uResolutionInv;

float4 PixelShaderFunction(float2 texCoord : TEXCOORD) : COLOR0
{
	float4 data = tex2D(dustInfo, texCoord);
	float2 worldCoord = texCoord * float2(uResolution.x, uResolution.y);
	
	if (data.r > 0)
	{
		return float4(uColor, data.g);
	}
	//else
	//{
	//	float2 left = tex2D(dustInfo, texCoord + float2(-uResolutionInv.x, 0)).xy;
	//	float2 right = tex2D(dustInfo, texCoord + float2(uResolutionInv.x, 0)).xy;
	//	float2 up = tex2D(dustInfo, texCoord + float2(0, -uResolutionInv.y)).xy;
	//	float2 down = tex2D(dustInfo, texCoord + float2(0, uResolutionInv.y)).xy;
		
	//	float v = 0;
	//	if (left.r > 0)
	//	{
	//		v += left.g;
	//	}
	//	if (right.r > 0)
	//	{
	//		v += right.g;
	//	}
	//	if (up.r > 0)
	//	{
	//		v += up.g;
	//	}
	//	if (down.r > 0)
	//	{
	//		v += down.g;
	//	}
	//	return float4(1, 1, 1, saturate(v * 2));
	//}

	return float4(0, 0, 0, 0);
}

technique Technique1
{
	pass Display
	{
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
}