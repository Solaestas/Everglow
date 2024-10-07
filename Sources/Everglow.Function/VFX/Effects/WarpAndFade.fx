sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float strength;
float deltaY;
float fade;
float mulSize;

float4 PSFunction(float2 coords : TEXCOORD0) : COLOR0 //用一张图片（uImage1）去扭曲uImage0，r - 0.5代表x，g - 0.5代表y,b代表倍率。
{
    float4 color = tex2D(uImage0, coords);
	float4 color2 = tex2D(uImage1, float2(coords.x * mulSize, coords.y * mulSize + deltaY));
	float2 vec = float2(0, 0); //表示移动的向量
	vec = float2(color2.r - 0.5, color2.g - 0.5) * strength * color2.b;
	return tex2D(uImage0, coords + vec) - float4(fade, fade, fade, fade);
}
technique Technique1
{
    pass warp
    {
        PixelShader = compile ps_3_0 PSFunction();
    }
}