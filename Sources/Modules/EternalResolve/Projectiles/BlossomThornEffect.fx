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
    AddressV = CLAMP;
};
float4x4 uTransform;
float uProcession;
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
    float newX = input.Texcoord.x % 1;
    float newY = input.Texcoord.y - 0.5;
    //newY *= 1 / (sin(log(-(input.Texcoord.z - 1) * 22.141 + 1)));
    //newY *= 1 / sin(log(input.Texcoord.z * 22.141 + 1));
    newY *= 1 / (sin(input.Texcoord.z * 3.141592653589793238) * pow(sin(input.Texcoord.z * 30) * sin(input.Texcoord.z * 30), 30));
    newY += 0.5;
    float2 newCoord = float2(newX, newY);
    float4 color = tex2D(uImage, newCoord);
    color *= input.Color;
    color *= sin(uProcession * 3.141592653589793238) * max(sin(uProcession * 3.141592653589793238), 0);
    return color;
}
technique Technique1
{
    pass Test
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}