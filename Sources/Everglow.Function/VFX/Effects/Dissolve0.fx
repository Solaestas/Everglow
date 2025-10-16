sampler2D uImage0 : register(s0);
sampler2D uImage1 : register(s1);
float _DissolveFactor;
float4 PixelShaderFunction(float2 uv : TEXCOORD0,float4 drawColor:COLOR0) : COLOR0
{
    float4 col = tex2D(uImage0, uv);
    float4 noise = tex2D(uImage1, uv*0.3);
    float n = (noise.r+noise.g+noise.b)/3;

    float a=smoothstep(-0.1,0.1,n-_DissolveFactor);
    col*=a;
    return col*drawColor;
}
technique Technique1
{
    pass Test
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}