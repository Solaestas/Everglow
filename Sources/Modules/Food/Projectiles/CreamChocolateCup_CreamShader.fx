sampler2D uImage : register(s0);
texture uNoise;
sampler uNoiseSampler =
sampler_state
{
    Texture = <uNoise>;
    MipFilter = LINEAR;
    MinFilter = LINEAR;
    MagFilter = LINEAR;
    AddressU = WRAP;
    AddressV = WRAP;
};

float4x4 uTransform;

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
    output.Color = input.Color;

    output.Texcoord = input.Texcoord;
    output.Pos = mul(float4(input.Pos, 0, 1), uTransform);
    return output;
}

float4 PixelShaderFunction(PSInput input) : COLOR0
{
    float2 coord = input.Texcoord.xy - float2(0.5, 0.5);
    coord.y = coord.y / input.Texcoord.z;
    coord += float2(0.5, 0.5);
    coord.y = clamp(coord.y, 0, 1);
    float light = tex2D(uNoiseSampler, coord).r + input.Color.a * 1.5; 
    float4 tex = tex2D(uImage, coord);
    float4 mulColor = float4(input.Color.rgb, 1);
    if (light <= 1)
    {
        return tex * mulColor;
    }
    else if(light <= 1.1)
    {
        return tex * mulColor * float4(0.96, 0.92, 0.9, 1);
    }
    else if (light <= 1.3)
    {
        return tex * mulColor * float4(0.8, 0.7, 0.6, 1);
    }
    else
    {
        return float4(0, 0, 0, 0);

    }
}
technique Technique1
{
    pass Test
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}