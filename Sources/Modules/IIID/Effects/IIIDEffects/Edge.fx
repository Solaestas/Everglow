sampler uImage0 : register(s0);

int EdgeNumber;
//float4 Range1[];
//float4 Range2[];
//float4 EdgeColor[];

float4 edge(float2 coords : TEXCOORD0) : COLOR0
{
    //Range1 = Range1[EdgeNumber];
   // Range1 = Range2[EdgeNumber];
   // EdgeColor = EdgeColor[EdgeNumber];
    float4 color = tex2D(uImage0, coords);
    if (color.a != 0)
        return color;
    // 获取每个像素的正确大小
    float dx = 2;
    float dy = 2;
    bool flag = false;
    // 对周围8格进行判定
    for (int i = -1; i <= 1; i++)
    {
        for (int j = -1; j <= 1; j++)
        {
            float4 c = tex2D(uImage0, (coords * 1000 + float2(dx * i, dy * j)) / 1000);
            // 如果任何一个像素有颜色
            if (any(c))
            {
                if (c.r + c.b + c.g >= 1.5)
                {
                    return float4(0.3, 0.05, 0, 1);

                }
                else
                {
                    return float4(0, 0, 0, 1);
                }
            }
        }
    }
    return color;
}

technique Technique1
{
    pass Edge
    {
        PixelShader = compile ps_3_0 edge();
    }
}