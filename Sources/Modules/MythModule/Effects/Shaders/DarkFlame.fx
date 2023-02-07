sampler uImage0 : register(s0);
texture uImage1;
sampler2D s3 = sampler_state
{
    Texture = <uImage1>;
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};
float uTime;
float2 uImageSize;
float4 edge(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    float4 cT = tex2D(s3, coords);
    if (any(color))
        return color;
    // 获取每个像素的正确大小
    float dx = 1 / uImageSize.x;
    float dy = 1 / uImageSize.y;
    bool flag = false;
    float Stre = 0;
    // 对周围8格进行判定
    for (int i = -1; i <= 1; i++)
    {
        for (int j = -1; j <= 1; j++)
        {
            float4 c = tex2D(uImage0, coords + float2(dx * i, dy * j));
            // 如果任何一个像素有颜色
            if (any(c))
            {
                Stre += 0.05;
                // 不知道为啥，这里直接return会被编译器安排，所以只能打标记了
                flag = true;
            }
        }
    }
    if (flag)
        return float4(1,1,1,1);
    return color;
}
technique Technique1
{
    pass Test
    {
        PixelShader = compile ps_2_0 edge();
    }
}