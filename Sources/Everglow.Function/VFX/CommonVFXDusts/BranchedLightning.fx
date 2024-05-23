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
    AddressU = Wrap;
    AddressV = Wrap;
};
float uDisplaceIntensity;
float uNoiseSize;
float uLineProportion;

float4 uEdgeColor;
float uBlurProportion;

float4x4 uTransform;

float uDisplacementShift;
float uTransitPeriod;
float uDeformPeriod;

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
    float4 baseColor = (input.Color.a == 0 ? float4(1, 1, 1, 1) : uEdgeColor);
    
    if (input.Color.b == 0)
    {
        // 绘制原点
        float4 dotColor = tex2D(uDotLightpSampler, input.Texcoord.xy);
        dotColor.a = dotColor.r;
        return dotColor * baseColor;
    }
    
    float noiseX = input.Texcoord.y;
    float noiseRange = input.Texcoord.z;
    float texU = input.Color.r;
    float texV = input.Color.g;

    
    float dispLerp = texU - (uDisplacementShift / uTransitPeriod) % 2.0;
    if (dispLerp < 0 && dispLerp >= -1) {
        dispLerp *= -1;
    } else if (dispLerp < -1) {
        dispLerp += 2;
    }
    
    float2 displaceCoord = float2(
        noiseX + lerp(0, noiseRange / uNoiseSize, dispLerp),
        uDisplacementShift / uDeformPeriod);
    float rawDisplace = tex2D(uDisplacementSampler, displaceCoord).r;

	// 每段闪电两端无位移 (位移量乘 1-(2texU - 1)^6, texU在[0,1]上)
    float refinedDisplace = (rawDisplace - 0.5) * uDisplaceIntensity * (-pow(2 * texU - 1, 6) + 1);
    float currentHalfWidthProportion = input.Texcoord.x * 0.5;
    float distFromCenter = abs(texV + refinedDisplace - 0.5);
    float blurProportion = uBlurProportion * (input.Color.a == 0 ? 1: 10);

    // 绘制边缘模糊
    if (distFromCenter > (currentHalfWidthProportion + uBlurProportion))
        return float4(0, 0, 0, 0);
    
    if (distFromCenter > currentHalfWidthProportion)
        return baseColor * (1.0 - ((distFromCenter - currentHalfWidthProportion) / uBlurProportion));
    
    return baseColor;
}

technique Technique1
{
    pass Test
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}