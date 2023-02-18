sampler uImage0 : register(s0);

sampler uNoiseTex : register(s1);

float2 worldPos;
float textureWidth;

float progress;

// 234, 90 为物块贴图总尺寸
const float2 TILE_SPRITE_DIMENTION = float2(234.0, 90.0);


float4 Dissolve(float2 coords : TEXCOORD0) : COLOR0
{
    float4 baseC = tex2D(uImage0, coords);
    // 18px为每个物块贴图帧所占用的总宽度（包括透明分隔），16px为贴图实际宽度
    float4 dissolveColor = tex2D(uNoiseTex, (((coords * TILE_SPRITE_DIMENTION) % 18 + worldPos) % textureWidth) / textureWidth);
    float brightNess = min(1, max(0, dissolveColor.r + (1.0 - 2 * progress)));
    return float4(baseC.rgb, brightNess * baseC.a);
}

technique Technique1
{
    pass NoiseDissolve
    {
        PixelShader = compile ps_2_0 Dissolve();
    }
}
