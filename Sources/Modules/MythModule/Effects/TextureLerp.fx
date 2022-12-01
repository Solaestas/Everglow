sampler2D Texture1 : register(s0);
Texture2D lerptarget;
sampler2D Texture2 = sampler_state
{
    Texture = <lerptarget>;
};
float lerp;
texture uImage2;
sampler2D s3 = sampler_state
{
    Texture = <uImage2>;
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};

float4 PixelShaderFunction(float2 texCoord : TEXCOORD,float4 texColor:COLOR0) : COLOR0
{
    float4 data1 = tex2D(Texture1, texCoord);
    float4 data2 = tex2D(Texture2, texCoord);
    float r = data1.r + (data2.r - data1.r) * lerp;
    float g = data1.g + (data2.g - data1.g) * lerp;
    float b = data1.b + (data2.b - data1.b) * lerp;
    float a = data1.a + (data2.a - data1.a) * lerp;

    float light = max(max(data1.r , data1.g),data1.b);
    if(data1.a <= 0.3)
        return float4(0,0,0,0);
    float4 C = data1 * (1 - lerp) + tex2D(s3, float2(light, 0)) * lerp;
    return C * texColor;
}

technique Technique1
{
    pass Lerp
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}