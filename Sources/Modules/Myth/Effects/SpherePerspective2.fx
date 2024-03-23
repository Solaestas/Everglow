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

float3 circleCenter;
float radiusOfCircle;
float uTime;

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
    float A = dot(dir, dir);
    float B = 2 * dot(dir, P);
    float C = dot(P, P) - radiusOfCircle * radiusOfCircle;
	
	// 解方程
    float det = B * B - 4 * A * C;
    if (det < 0)
        return float4(0, 0, 0, 0);
    float sqdet = sqrt(det);
    float t1 = (-B + sqdet) / (2 * A);
    float t2 = (-B - sqdet) / (2 * A);
    float t = t1 < t2 ? t1 : t2;
    float3 ligS = float3(-1, 1, 1) / sqrt(3);
	// 求 theta 和 phi ，对应 x, y
    float3 hitpos = dir * t - circleCenter;
    
    float3 N = normalize(hitpos);
    float x = atan2(hitpos.z, hitpos.x) / 3.14159 + 1.0 + uTime;
    float y = hitpos.y / radiusOfCircle + 1.0;
    float x0 = atan2(hitpos.z, hitpos.x) / 3.14159 + 1.0;
    float y0 = hitpos.y / radiusOfCircle + 1.0;
    
	
	// 因为我坐标是 [-1, 1] 这个区间的，所以先要翻正再取模
    float xx = fmod(x + 1, 1.0);
    float yy = fmod(y + 1, 1.0);
    float xx0 = fmod(x0 + 1, 1.0);
    float yy0 = fmod(y0 + 1, 1.0);
    float costheta = dot(ligS, N);
    float4 Cz = tex2D(uImage0, float2(xx, yy));
    return Cz;

}

technique Technique1
{
    pass ColorBar
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}