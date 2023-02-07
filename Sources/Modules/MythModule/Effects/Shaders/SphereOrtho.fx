sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
sampler uImage2 : register(s2);

/*
* 使用方法：	uTransform		国际惯例是顶点坐标变换矩阵，只会影响两个三角形的变换，也就是教程中的屏幕
*			circleCenter	球心的三维坐标，注意这个坐标的z分量需要是负数才能显示出来，因为我用的是另一个坐标系
*			radiusOfCircle	球的半径
*			uImage#			贴图，自己看着加，默认是uImage0要有一个
* 如有需要可以自己加参数
*/

float4x4 uTransform;
float uTime;
float3 uColor1, uColor2;

struct VSInput {
	float2 Pos : POSITION0;
	float4 Color : COLOR0;
	float3 Texcoord : TEXCOORD0;
};

struct PSInput {
	float4 Pos : SV_POSITION;
	float4 Color : COLOR0;
	float3 Texcoord : TEXCOORD0;
};


float4 PixelShaderFunction(PSInput input) : COLOR0 {
	float3 coord = input.Texcoord;
	float len = length(coord - float3(0, 0, 0));
	// 把不会打到球的像素剔除
	if (len > 1)
		return float4(0, 0, 0, 0);
	
	// 简陋的正交投影
	float r1 = sqrt(1 - coord.y * coord.y);
	float x = acos(coord.x / r1) * 2 / 3.1415926;
	float k = coord.y - uTime;
	return tex2D(uImage0, float2(x, coord.y)); 
}

PSInput VertexShaderFunction(VSInput input)  {
	PSInput output;
	output.Color = input.Color;
	output.Texcoord = input.Texcoord;
	output.Pos = mul(float4(input.Pos, 0, 1), uTransform);
	return output;
}


technique Technique1 {
	pass ColorBar {
		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}