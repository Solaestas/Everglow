sampler uImage0 : register(s0);
sampler uDepthImage : register(s1);

float uBias;
float2 uInvImageSize;
float4 _EdgeColor;
float4 _ZBufferParams;

const float Gx[9] =
{
	-1, 0, 1,
    -2, 0, 2,
    -1, 0, 1
};

const float Gy[9] =
{
	-1, -2, -1,
    0, 0, 0,
    1, 2, 1
};

const float Dx[9] = { -1, 0, 1, -1, 0, 1, -1, 0, 1 };
const float Dy[9] = { -1, -1, -1, 0, 0, 0, 1, 1, 1 };

float3 unpack_normal(float3 unpackedNormal)
{
	return normalize(unpackedNormal * 2 - 1);
}

float Linear01Depth(float z)
{
	return 1.0 / (_ZBufferParams.x * z + _ZBufferParams.y);
}

float LinearEyeDepth(float z)
{
	return 1.0 / (_ZBufferParams.z * z + _ZBufferParams.w);
}

float4 DownSample_Naive(float2 coords : TEXCOORD0) : COLOR0
{
	float dx = uInvImageSize.x * 0.5;
	float dy = uInvImageSize.y * 0.5;
	float4 topRight = tex2D(uImage0, coords + float2(dx, -dy));
	float4 topLeft = tex2D(uImage0, coords + float2(-dx, -dy));
	float4 botRight = tex2D(uImage0, coords + float2(dx, dy));
	float4 botLeft = tex2D(uImage0, coords + float2(-dx, dy));
	float4 c = (topRight + topLeft + botRight + botLeft) * 0.25;
	return c;
}

float4 Edge_HighLight_Inner(float2 coords : TEXCOORD0) : COLOR0
{
	float dx = uInvImageSize.x;
	float dy = uInvImageSize.y;
	float4 top = tex2D(uImage0, coords + float2(0, -dy));
	float4 bot = tex2D(uImage0, coords + float2(0, dy));
	float4 right = tex2D(uImage0, coords + float2(dx, 0));
	float4 left = tex2D(uImage0, coords + float2(-dx, 0));
	float4 self = tex2D(uImage0, coords);
	
	float2 edge = 0;
	for (int i = 0; i < 9; i++)
	{
		float z = Linear01Depth(1 - tex2D(uDepthImage, coords + float2(Dx[i] * dx, Dy[i] * dy)).w);
		edge.x += Gx[i] * z;
		edge.y += Gy[i] * z;
	}
	float strength = length(edge);
	
	// Normal Edge
	if (strength > uBias)
	{
		return float4(self.rgb * lerp(0.5, 0.1, strength), 1);
	}
	
	float4 topD = tex2D(uDepthImage, coords + float2(0, -dy));
	float4 botD = tex2D(uDepthImage, coords + float2(0, dy));
	float4 rightD = tex2D(uDepthImage, coords + float2(dx, 0));
	float4 leftD = tex2D(uDepthImage, coords + float2(-dx, 0));
	float4 selfD = tex2D(uDepthImage, coords);
	
	// Concave Edge
	float HDiff = max(0, dot(unpack_normal(leftD.xyz), unpack_normal(rightD.xyz)));
	float VDiff = max(0, dot(unpack_normal(topD.xyz), unpack_normal(botD.xyz)));
	if (HDiff < 0.95
		|| VDiff < 0.95)
	{
		return float4(self.rgb * lerp(0.5, 0.3, HDiff * VDiff), 1);
	}
	//if (abs((topD.w + botD.w + rightD.w + leftD.w) * 0.25 - selfD.w) > uBias)
	//{
	//	return float4(self.rgb * 0.5, 1);
	//}
	return self;
}


float4 Edge_HighLight_Outer(float2 coords : TEXCOORD0) : COLOR0
{
	float dx = uInvImageSize.x;
	float dy = uInvImageSize.y;
	float4 top = tex2D(uImage0, coords + float2(0, -dy));
	float4 bot = tex2D(uImage0, coords + float2(0, dy));
	float4 right = tex2D(uImage0, coords + float2(dx, 0));
	float4 left = tex2D(uImage0, coords + float2(-dx, 0));
	float4 self = tex2D(uImage0, coords);
	
	if (self.a != 0)
	{
		return self;
	}
	
	if (top.a != 0 || bot.a != 0 || right.a != 0 || left.a != 0)
	{
		return _EdgeColor;
	}
	return self;
}

technique ConcaveEdge
{
	pass DownSample_Naive
	{
		PixelShader = compile ps_2_0 DownSample_Naive();
	}
	pass Edge_HighLight_Inner
	{
		PixelShader = compile ps_3_0 Edge_HighLight_Inner();
	}
	pass Edge_HighLight_Outer
	{
		PixelShader = compile ps_3_0 Edge_HighLight_Outer();
	}
}
