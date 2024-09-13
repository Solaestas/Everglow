sampler2D uImage0 : register(s0);

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
    float4 drawColor = input.Color;
    float2 coord = input.Texcoord.xy;
	float4 c = tex2D(uImage0, coord.xy);//主纹理
    float4 finalColor = float4(drawColor.r, drawColor.g * c.r, 0, c.a);
	return finalColor;
}

float4 PixelShaderFunction2(PSInput input) : COLOR0
{
    float4 drawColor = input.Color;
    float2 coordXY = input.Texcoord.xy;
    coordXY -= float2(0.5, 0.5);
    coordXY.y /= input.Texcoord.z;
    coordXY += float2(0.5, 0.5);
    coordXY.y = clamp(coordXY.y, 0, 1);
    float4 c = tex2D(uImage0, coordXY); //主纹理
    float4 finalColor = float4(drawColor.r, drawColor.g, c.r * drawColor.b, drawColor.a);
    return finalColor;
}

technique Technique1 
{
	pass Trail0 
	{
        VertexShader = compile vs_3_0 VertexShaderFunction();
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
    pass Trail1
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction2();
    }
}
