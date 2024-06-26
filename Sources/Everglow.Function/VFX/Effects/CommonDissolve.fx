sampler2D uImage0 : register(s0);
sampler2D uImage1 : register(s1);
float uvMulti;
float uDissolve;
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
    float4 drawColor=input.Color;
    float2 uv=input.Texcoord.xy;
    float4 col = tex2D(uImage0, uv);
    float uvm=0.3;
    if(uvMulti!=0)
        uvm=uvMulti;
    float4 noise=tex2D(uImage1, uv*uvm);
    float n=max(noise.r,max(noise.g,noise.b));
    float a=smoothstep(-0.1,0.1,n-uDissolve);
    col*=a;
    return col*drawColor;
}
technique Technique1
{
    pass Test
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}