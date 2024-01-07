sampler uImage0 : register(s0);

float4 edge(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = float4(0);
    if (any(color))
        return color;
    // ��ȡÿ�����ص���ȷ��С
    float dx = 1 / 500;
    float dy = 1 / 500;
    bool flag = false;
    // ����Χ8������ж�
    for (int i = -1; i <= 1; i++)
    {
        for (int j = -1; j <= 1; j++)
        {
            float4 c = tex2D(uImage0, coords + float2(dx * i, dy * j));
            // ����κ�һ����������ɫ
            if (any(c))
            {
                // ��֪��Ϊɶ������ֱ��return�ᱻ���������ţ�����ֻ�ܴ�����
                flag = true;
            }
        }
    }
    if (flag)
        return float4(0, 0, 0, 1);
    return color;
}

technique Technique1
{
    pass Edge
    {
        PixelShader = compile ps_2_0 edge();
    }
}