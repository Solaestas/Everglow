sampler2D uImage0 : register(s0);
texture2D tex1;
sampler2D uColorTex = sampler_state
{
    Texture = <tex1>;
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};
float4x4 uTransform;
float uScreenWidth;
float uScreenHeight;

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
    float2 newPos = float2(input.Pos.x / uScreenWidth, input.Pos.y / uScreenHeight);
    float2 newTexcoord = input.Texcoord.xy;
    newTexcoord.x -= input.Color.r * input.Color.b / input.Texcoord.z * 0.001;
    newTexcoord.y -= input.Color.g * input.Color.b / input.Texcoord.z * 0.001;
    float4 cScreen = tex2D(uColorTex, newTexcoord + newPos);
    return cScreen;
}

technique Technique1
{
    pass PureColor
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}