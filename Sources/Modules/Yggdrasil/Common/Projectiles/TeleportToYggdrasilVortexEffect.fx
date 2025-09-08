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
float uTimer;
float4 uColor0;
float4 uColor1;
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
	float2 center = float2(0.5, 0.5);
	float2 inputVector = input.Texcoord.xy - center;
	float theta = atan2(inputVector.y ,inputVector.x) + 3.14159265359;
	float r = length(inputVector) * 2;
	theta += (1 - r) * input.Texcoord.z;
	float newX = theta / 6.2831853;
	float newY = r + uTimer;
	float2 newCoord = float2(newX, newY);
    float4 color = tex2D(uImage, newCoord);
	float factor = (sin(theta) + 1) * 0.5;
	color *= lerp(uColor0, uColor1, float4(factor, factor, factor, factor));
	color *= input.Color;
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