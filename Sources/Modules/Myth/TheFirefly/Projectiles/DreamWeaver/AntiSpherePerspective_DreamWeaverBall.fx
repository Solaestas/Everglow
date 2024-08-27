sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
sampler uImage2 : register(s2);

//这个是负球面映射
/*
* 使用方法：	uTransform		国际惯例是顶点坐标变换矩阵，只会影响两个三角形的变换，也就是教程中的屏幕
*			circleCenter	球心的三维坐标，注意这个坐标的z分量需要是负数才能显示出来，因为我用的是另一个坐标系
*			radiusOfCircle	球的半径
*			uImage#			贴图，自己看着加，默认是uImage0要有一个
* 如有需要可以自己加参数
*/


float4x4 uTransform;
float3 circleCenter;
float radiusOfCircle;
float uTime;
float uPower;

struct VSInput
{
    float2 Pos : POSITION0;
    float4 Color : COLOR0;
    float3 Texcoord : TEXCOORD0;
};

struct PSInput
{
    float4 Pos : SV_POSITION;
    float4 Color : COLOR0;
    float3 Texcoord : TEXCOORD0;
};

float4 PixelShaderFunction(PSInput input) : COLOR0
{
    float3 coord = input.Texcoord;
	
    float3 dir = float3(coord.x, coord.y, -1);
    float3 P = -circleCenter;
    //视线 模的平方
    float A = dot(dir, dir);
    //？ 忘了
    float B = 2 * dot(dir, P);
    //能看到的最远一圈 模的平方
    float C = dot(P, P) - radiusOfCircle * radiusOfCircle;
	
	// 解方程
    float det = B * B - 4 * A * C;
    if (det < 0)
        return float4(0, 0, 0, 0);
    float sqdet = sqrt(det);
    float t1 = (-B + sqdet) / (2 * A);
    float t2 = (-B - sqdet) / (2 * A);
    float t = t1 > t2 ? t1 : t2;
    float3 ligS = float3(0, 0, 1);
	// 求 theta 和 phi ，对应 x, y
    float3 hitpos = dir * t - circleCenter;
    
    float x0 = atan2(hitpos.z, hitpos.x) / 3.14159 + 1.0;
    float y0 = hitpos.y / radiusOfCircle + 1.0;
    float3 N = normalize(hitpos);
    //删注解扭曲

    float4 Cwarp = tex2D(uImage1, float2(uTime * 1.8 + x0, sin(uTime) + y0));
    //删注解随时间变化
    float x = atan2(hitpos.z, hitpos.x) / 3.14159 + 1.0 + uTime + Cwarp.r * 0.05;
    float y = hitpos.y / radiusOfCircle + 1.0 + uTime + Cwarp.g * 0.05;
    

	
	// 因为我坐标是 [-1, 1] 这个区间的，所以先要翻正再取模
    float xx = fmod(x + 1, 1.0);
    float yy = fmod(y + 1, 1.0);

    if (hitpos.y > uPower + tex2D(uImage2, float2(xx, yy)).r * 0.05)
    {
        return float4(0, 0, 0, 0);
    }
    
    float costheta = dot(ligS, N);
    float4 Cz = tex2D(uImage0, float2(xx, yy));
    if (hitpos.y < uPower + tex2D(uImage2, float2(xx, yy)).r * 0.05 && hitpos.y > uPower + tex2D(uImage2, float2(xx, yy)).r * 0.05 - 0.04)
    {
        return input.Color.rgba;
    }
    return Cz * input.Color;

}

PSInput VertexShaderFunction(VSInput input)
{
    PSInput output;
    output.Color = input.Color;
    output.Texcoord = input.Texcoord;
    output.Pos = mul(float4(input.Pos, 0, 1), uTransform);
    return output;
}


technique Technique1
{
    pass ColorBar
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}