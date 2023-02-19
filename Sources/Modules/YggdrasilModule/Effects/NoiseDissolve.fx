sampler uImage0 : register(s0);

sampler uNoiseTex : register(s1);

float2 worldPos;
float textureWidth;

float progress;
float2 edgeThreshold;
float4 edgeColor;

// 234, 90 为物块贴图总尺寸
const float2 TILE_SPRITE_DIMENTION = float2(234.0, 90.0);


float4 Dissolve(float2 coords : TEXCOORD0) : COLOR0
{
    float4 baseC = tex2D(uImage0, coords);
    // 18px为每个物块贴图帧所占用的总宽度（包括透明分隔），16px为贴图实际内容宽度
    float4 dissolveColor = tex2D(uNoiseTex, (((coords * TILE_SPRITE_DIMENTION) % 18 + worldPos) % textureWidth) / textureWidth);
    float brightNess = min(1.0, max(0.0, 0.1 + dissolveColor.r - progress));
    
    if (brightNess >= edgeThreshold.x && brightNess <= edgeThreshold.y)
    {
        float interpolate = (brightNess - edgeThreshold.x) / (edgeThreshold.y - edgeThreshold.x);
        return edgeColor * (1 - interpolate) + baseC * interpolate;
    } else if (brightNess <= 0)
        return float4(0, 0, 0, 0);
    else
        return baseC;
}

technique Technique1
{
    pass NoiseDissolve
    {
        PixelShader = compile ps_2_0 Dissolve();
    }
}
