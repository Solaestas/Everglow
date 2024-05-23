sampler2D uImage0 : register(s0);
sampler2D uImage1 : register(s1);
float4 PixelShaderFunction(float2 uv : TEXCOORD0,float4 drawColor:COLOR0) : COLOR0
{
    float4 col = tex2D(uImage0, uv);
    float n=tex2D(uImage1, uv*0.3).r;

    float a=smoothstep(-0.1,0.1,n-drawColor.a);
    col*=a;
    return col*float4(drawColor.rgb,1.);
}
technique Technique1
{
    pass Test
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}