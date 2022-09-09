sampler2D originalTexture : register(s0);
sampler2D noise : register(s1);
sampler2D wave : register(s2);
sampler2D water : register(s3);

float4 cb0;
float4 cb1;
float4 cb2;
float4 cb3;
float4 cb4;
float4 cb5;
float4 cb6;
float4 cb7;
float4 cb8;
float4 cb9;
float4 cb10;
float uThreashhold;
float uPower;
float3 uColor;

float RandXY(float2 coord)
{
	return frac(cos(coord.x * (12.9898) + coord.y * (4.1414)) * 43758.5453);
}

float4 PixelShaderFunction(float2 texCoord : TEXCOORD) : COLOR0
{
	float4 r0 = float4(0.100000, 0.200000, -0.500000, -0.300000);
	float4 r1 = float4(1.000000, 1.000000, 0.100000, 0);
	float4 r2 = float4(0.001000, 100.000000, 500.000000, 2.000000);
	float4 r3 = float4(-2.000000, 3.000000, 0, 0.333333);
	float4 r4;
	float4 r5;
	float4 r6;
	float4 r7;
	
	r4.xy = cb6.xy;
	r4.zw = r4.yx * texCoord.yx;
	r4.zw = r4.zw + cb7.yx;
	r4.zw = r4.zw * cb0.yx;
	r5.x = r0.x * r4.w;
	r0.xy = r0.xyxx;
	r1.w = r0.x * r4.z;
	r5.y = r1.w + cb1.x;
	r6.x = r0.y * r4.w;
	r0.y = r0.y * r4.z;
	r6.y = r0.y + cb2.x;
	r4.zw = cb3.xy;
	r4.zw = r4.zw * texCoord.xy;
	r4.zw = r4.zw + cb10.xy;
	r5.xyzw = tex2D(noise, r5.xy).xyzw;
	r5.xy = r5.xy;
	r6.xyzw = tex2D(noise, r6.xy).xyzw;
	r6.xy = r6.xy;
	r7.xyzw = tex2D(wave, r4.zw).xzyw;
	r7.xy = r7.xy;
	
	r4.z += (RandXY(r4.zw) - 0.5) * r7.x * 0.01;
	r4.w += (RandXY(r4.wz) - 0.5) * r7.x * 0.01;
	float ws = tex2D(wave, r4.zw).x;
	float ws2 = ws * 2 - 1;
	r4.zw = r0.zz + r5.xy;
	r5.xy = r0.zz + r6.xy;
	r5.xy = -r5.xy;
	r4.zw = r4.zw + r5.xy;
	r0.xy = r0.xx * r4.zw;
	r0.z = r0.z + r7.x;
	r1.w = r7.y * -0.400000;
	r1.w = r1.x + r1.w;
	r0.z = r0.z * r1.w;
	r1.w = -r0.z;
	r0.w = max(r0.w, r1.w);
	r0.w = min(r0.w, 0.300000);
	r1.w = r0.y * cb9.x;
	r5.y = r0.w + r1.w;
	r5.x = r0.x * cb9.x;
	r0.x = dot(r0.xy, r0.xy);
	r0.x = r3.z + r0.x;
	r0.y = (r0.x == 0.000000);
	r0.w = -r0.x;
	r0.x = max(r0.w, r0.x);
	r0.x = 1 / sqrt(r0.x);
	r0.x = r0.y ? 99999996802856924650656260769173209088.000000 : r0.x;
	r0.y = (r0.x == 0.000000);
	r0.x = 1.000000 / r0.x;
	r0.x = r0.y ? 99999996802856924650656260769173209088.000000 : r0.x;
	r0.y = r0.z * 0.300000;
	r0.x = r0.x + r0.y;
	r0.yz = r1.yz * r5.xy;
	r2.x = r2.x;
	r0.w = r0.z * cb4.y;
	r0.w = r2.x + r0.w;
	r0.yz = r0.yz * cb4.xy;
	r0.yz = r0.yz + texCoord.xy;
	r0.w = r2.z * r0.w;
	r0.w = max(r0.w, 0.000000);
	r0.w = min(r0.w, 1.000000);
	r1.y = r3.x * r0.w;
	r1.y = r3.y + r1.y;
	r0.w = r0.w * r0.w;
	r0.w = r0.w * r1.y;
	r1.yz = r4.xy * r0.yz;
	r1.yz = r1.yz + cb8.xy;
	r1.yz = r1.yz * cb5.xy;
	r2.xz = r4.xy * texCoord.xy;
	r2.xz = r2.xz + cb8.xy;
	r2.xz = r2.xz * cb5.xy;
	r4.xyzw = tex2D(originalTexture, r0.yz).xyzw;
	r5.xyzw = tex2D(water, r1.yz).wxyz;
	r5.x = r5.x;
	r6.xyzw = tex2D(water, r2.xz).wxyz;
	r6.x = r6.x;
	float x = (1.35 - ws * 2.);
	r7.xyzw = tex2D(originalTexture, texCoord.xy).xyzw;
	r0.y = r2.y * r5.x;
	r0.y = min(r1.x, r0.y);
	r0.z = r2.y * r6.x;
	r0.z = min(r1.x, r0.z);
	r1.y = -r0.z;
	r1.y = r0.y + r1.y;
	r0.z = r0.z + r0.y;
	r0.z = r2.y * r0.z;
	r0.z = min(r1.x, r0.z);
	r0.w = r2.w * r0.w;
	r0.w = r1.y + r0.w;
	r1.z = -r1.y;
	r1.y = max(r1.z, r1.y);
	r0.w = r0.w - 1.000000;
	r1.z = -r0.w;
	r0.w = max(r0.w, r1.z);
	r0.w = -r0.w;
	r0.w = r1.x + r0.w;
	r0.w = r0.w * r1.y;
	r0.w = max(r0.w, 0.000000);
	r0.w = min(r0.w, 1.000000);
	r0.w = -r0.w;
	r0.w = r1.x + r0.w;
	r0.z = r0.w * r0.z;
	r1.xyzw = -r7.xyzw;
	r1.xyzw = r1.xyzw + r4.xyzw;
	r1.xyzw = r0.zzzz * r1.xyzw;
	r1.xyzw = r1.xyzw + r7.xyzw;
	r0.z = r1.x + r1.y;
	r0.z = r0.z + r1.z;
	r0.x = r0.z * r0.x;
	r0.x = r0.x * r0.y;
	r0.x = r0.x * r0.w;
	r0.xyz = r3.wwww * r0.xxxx;
	r1.xyz = r1.xyzx + r0.xyzx;
	if (ws2 > uThreashhold)
	{
		float value = r6.x > 0 ? 1.0 : 0.0;
		float value2 =  pow(exp(pow(x - .50, 2.) * -20.) * value, uPower);
		r1.xyz += uColor * value2;
	}
	r1.w = r1.w;
	return r1.xyzw;
}

technique Technique1
{
	pass Display
	{
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
}