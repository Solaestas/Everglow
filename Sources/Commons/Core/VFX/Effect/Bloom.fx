sampler uImage0 : register(s0);
float2 uSize;
float4x4 uTransform;
float uIntensity;
float uDelta;
float uLimit;
float weight[5] = { 0.227027, 0.1945946, 0.1216216, 0.054054, 0.016216 };

struct VSInput
{
    float2 Pos : POSITION0;
    float4 Color : COLOR0;
    float2 Texcoord : TEXCOORD0;
};

struct PSInput
{
    float4 Pos : SV_POSITION;
    float4 Color : COLOR0;
    float2 Texcoord : TEXCOORD0;
};

PSInput VertexShaderFunction(VSInput input)
{
    PSInput output;
    output.Color = input.Color;
    output.Texcoord = input.Texcoord;
    output.Pos = mul(float4(input.Pos, 0, 1), uTransform);
    return output;
}

float4 BloomH(float2 coord : TEXCOORD0) : COLOR0{
    float3 color = tex2D(uImage0, coord) * weight[0];
    float2 offset = 1 / uSize;
    for (int i = 1; i < 5; ++i)
    {
        color += tex2D(uImage0, coord + float2(offset.x * i, 0.0)).rgb * weight[i] * uIntensity;
        color += tex2D(uImage0, coord - float2(offset.x * i, 0.0)).rgb * weight[i] * uIntensity;
    }
    
    return float4(color, 1);
}

float4 BloomV(float2 coord : TEXCOORD0) : COLOR0
{
    float3 color = tex2D(uImage0, coord) * weight[0];
    float2 offset = 1 / uSize;
    for (int i = 1; i < 5; ++i)
    {
        color += tex2D(uImage0, coord + float2(0.0, offset.y * i)).rgb * weight[i] * uIntensity;
        color += tex2D(uImage0, coord - float2(0.0, offset.y * i)).rgb * weight[i] * uIntensity;
    }
    
    return float4(color, 1);
}

float4 Blur(float2 coord : TEXCOORD0) : COLOR0
{
    float dx = 1.0 / uSize.x;
    float dy = 1.0 / uSize.y;
    float3 color1 = tex2D(uImage0, coord + float2(dx, dy) * uDelta).rgb;
    float3 color2 = tex2D(uImage0, coord + float2(-dx, dy) * uDelta).rgb;
    float3 color3 = tex2D(uImage0, coord + float2(dx, -dy) * uDelta).rgb;
    float3 color4 = tex2D(uImage0, coord + float2(-dx, -dy) * uDelta).rgb;
    float3 c = (color1 + color2 + color3 + color4) * 0.25;
    return float4(c, 1);
}

float4 GetLight(float2 coord : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coord);
    if (color.r * 0.4 + color.g * 0.4 + color.b * 0.2 < uLimit)
        return float4(1, 1, 1, 1);
    else
        return color;

}
technique Technique1
{
	pass BloomH
	{
		PixelShader = compile ps_3_0 BloomH();
        VertexShader = compile vs_3_0 VertexShaderFunction();
    }
    pass BloomV
    {
        PixelShader = compile ps_3_0 BloomV();
        VertexShader = compile vs_3_0 VertexShaderFunction();
    }
    pass GetLight
    {
        PixelShader = compile ps_3_0 GetLight();
        VertexShader = compile vs_3_0 VertexShaderFunction();
    }
    pass Blur
    {
        PixelShader = compile ps_3_0 Blur();
        VertexShader = compile vs_3_0 VertexShaderFunction();
    }
}