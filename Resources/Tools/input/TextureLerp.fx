sampler2D Texture1 : register(s0);
Texture2D lerptarget;
sampler2D Texture2 = sampler_state
{
    Texture = <lerptarget>;
};
float lerp;

float4 PixelShaderFunction(float2 texCoord : TEXCOORD,float4 texColor:COLOR0) : COLOR0
{
    float4 data1 = tex2D(Texture1, texCoord);
    float4 data2 = tex2D(Texture2, texCoord);
    float r = data1.r + (data2.r - data1.r) * lerp;
    float g = data1.g + (data2.g - data1.g) * lerp;
    float b = data1.b + (data2.b - data1.b) * lerp;
    float a = data1.a + (data2.a - data1.a) * lerp;
    return float4(r, g, b, a) * texColor;
}

technique Technique1
{
    pass Lerp
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}