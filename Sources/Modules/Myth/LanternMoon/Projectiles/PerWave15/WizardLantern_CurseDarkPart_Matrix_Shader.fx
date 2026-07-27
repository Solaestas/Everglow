sampler2D uImage : register(s0);
sampler2D uImage1 : register(s1);
sampler2D uImage2 : register(s2);
float4x4 uTransform;
float2 size1;
float2 size2;
float2 warpSize;
float uTime;

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
	float4 color_warp = tex2D(uImage1, input.Texcoord.xy * size1 + float2(uTime, 0));
	float2 newCoord = input.Texcoord.xy + color_warp.rg * warpSize;
	float4 color = tex2D(uImage, newCoord);
	float light = max(max(color.r, color.g), color.b);
	if (light < 1 - input.Texcoord.z)
	{
		return float4(0, 0, 0, 0);
	}
	color *= input.Color;
	color.rgb = smoothstep(0, 1, color.rgb);
	color.rgb = smoothstep(0, 1, color.rgb);
	float4 color_dissolve = tex2D(uImage2, newCoord * size2);
	color *= color_dissolve.r * 2;
	color.a *= input.Color.a;
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