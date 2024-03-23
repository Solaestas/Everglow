sampler uImage0 : register(s0);
texture2D tex0;
sampler2D uShapeTex = sampler_state
{
    Texture = <tex0>;
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};

float2 worldPos;
float textureWidth;

// 234, 90 为物块贴图总尺寸
const float2 TILE_SPRITE_DIMENTION = float2(234.0, 90.0);
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
PSInput VertexShaderFunction(VSInput input)
{
    PSInput output;
    output.Texcoord = input.Texcoord;
    output.Color = input.Color;
    output.Pos = float4(input.Pos, 0, 1);
    return output;
}

float4 Dissolve(PSInput input) : COLOR0
{
    float4 baseC = tex2D(uImage0, input.Texcoord.xy);
    // 18px为每个物块贴图帧所占用的总宽度（包括透明分隔），16px为贴图实际内容宽度
    float2 coordToTex = ((input.Texcoord.xy * TILE_SPRITE_DIMENTION) % 18) / 18;
    float2 texCoord = float2(worldPos % (textureWidth / 16)) / 32 + coordToTex / 32;
    float4 dissolveColor = tex2D(uShapeTex, texCoord);
    return dissolveColor * baseC.aaaa * input.Color;
}

technique Technique1
{
    pass newTexutre
    {
        PixelShader = compile ps_3_0 Dissolve();
    }
}
