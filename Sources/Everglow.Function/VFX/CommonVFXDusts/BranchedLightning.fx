sampler2D uImage : register(s0);

texture uDotLight;
sampler uDotLightpSampler =
sampler_state
{
    Texture = <uDotLight>;
    MipFilter = LINEAR;
    MinFilter = LINEAR;
    MagFilter = LINEAR;
    AddressU = CLAMP;
    AddressV = CLAMP;
};

texture uDisplacement;
sampler uDisplacementSampler =
sampler_state
{
    Texture = <uDisplacement>;
    MipFilter = LINEAR;
    MinFilter = LINEAR;
    MagFilter = LINEAR;
    AddressU = CLAMP;
    AddressV = CLAMP;
};
float uDisplaceIntensity;
float uNoiseSize;
float uLineProportion;

float4 uEdgeColor;
float uBlurProportion;

float4x4 uTransform;

float uDisplacementShift;
float uDisplacementPeriod;

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

float PingPong(float val)
{
    val %= 2;
    return val < 1 ? val : (2 - val);
}

float4 PixelShaderFunction(PSInput input) : COLOR0
{
    float4 baseColor = (input.Color.a == 0 ? float4(1, 1, 1, 1) : uEdgeColor);
    
    if (input.Color.b == 0)
    {
        // 绘制原点
        float4 dotColor = tex2D(uDotLightpSampler, input.Texcoord.xy);
        dotColor.a = dotColor.r;
        return dotColor * baseColor;
    }

    float2 displaceCoord = (input.Texcoord.yz % uNoiseSize) / uNoiseSize;
    float rawDisplace = tex2D(uDisplacementSampler, displaceCoord).r;
    /*
    float rawDisplace = lerp(
        tex2D(uDisplacementSampler, displaceCoord).r, 
        tex2D(uDisplacementSampler, float2(1,1) - displaceCoord).r, 
        PingPong((uDisplacementShift % uDisplacementPeriod) / uDisplacementPeriod * 2));
    */
    float refinedDisplace = (rawDisplace - 0.5) * uDisplaceIntensity;
    float currentHalfWidthProportion = input.Texcoord.x * 0.5;
    float distFromCenter = abs(input.Color.r + refinedDisplace - 0.5);
    float blurProportion = uBlurProportion * (input.Color.a == 0 ? 1: 10);

    if (distFromCenter > (currentHalfWidthProportion + uBlurProportion))
        return float4(0, 0, 0, 0);
    
    if (distFromCenter > currentHalfWidthProportion)
        return baseColor * (1.0 - ((distFromCenter - currentHalfWidthProportion) / uBlurProportion));
    
    return baseColor;

    return float4(1, 1, 1, 1);
}

technique Technique1
{
    pass Test
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}