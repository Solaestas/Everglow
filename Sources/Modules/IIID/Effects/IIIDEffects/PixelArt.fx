sampler uImage0 : register(s0);

int _PixelSize;
float _PixelRatio;
float2 _PixelScale;

float4 Quad(float2 uv, float pixelSize, float pixelRatio)
{
    float uvX = ceil(uv.x *500)/500;
    float uvY = ceil(uv.y *500)/500;
    half2 coord = half2(uvX, uvY);
    return tex2D(uImage0, coord);
}

float4 PS_BlendBloom(float2 coords : TEXCOORD0) : COLOR0
{
   /* float3 rgb = (0,0,0);
    int n=0;
    // 对周围8格进行判定
    for (int i = -10; i <= 10; i++)
    {
        for (int j = -10; j <= 10; j++)
        {
            float4 c = tex2D(uImage0, (coords * 1000 + float2(i, j))/1000);
            if (any(c))
            {
                rgb = rgb + c.xyz;
                n++;
            }
        }
    }
    if (n == 0)
    {
        return (0, 0, 0, 0);

    }
    else
    {
        return float4(rgb / n,1);
    }
*/
    //float4 color = Quad(i.uv,_PixelSize,_PixelRatio);
    float4 color = Quad(coords, _PixelSize, _PixelRatio);
    return color;
}


technique Technique1
{
    pass Blend
    {
        PixelShader = compile ps_3_0 PS_BlendBloom();
    }
}
