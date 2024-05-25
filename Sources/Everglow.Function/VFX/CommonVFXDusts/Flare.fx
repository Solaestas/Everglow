sampler2D uImage0 : register(s0);
sampler2D uImage1 : register(s1);
float4x4 uTransform;

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

float4 PixelShaderFunction(PSInput input) : COLOR0
{
    float4 drawColor=input.Color;
    float2 uv=input.Texcoord;
    float4 col = tex2D(uImage0, uv);
    float n=tex2D(uImage1, uv*0.3).r;
    float a=smoothstep(-0.1,0.1,n-drawColor.a);
    col*=a;
    return col*float4(drawColor.rgb,1.);
}

technique Technique1
{
    pass Shader2D
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}