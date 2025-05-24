sampler2D uImage : register(s0);

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
	float2 v = float2(1, 1);
	float2 u = input.Texcoord.xy;
	u = .2 * (u + u - v) / v.y;
	float4 o;
	float4 z = o = input.Color * 5;
     
	for (float a = .5, t = input.Texcoord.z, i;
         ++i < 19.;
         o += (1. + cos(z + t))
            / length((1. + i * dot(v, v))
                   * sin(1.5 * u / (.5 - dot(u, u)) - 9. * u.yx + t))
         )
	{
		u = mul(u, float2x2(cos(i + 0.02 * t - float4(0, 11, 33, 0))));
		v = cos(++t - 7. * u * pow(a += .03, i)) - 5. * u,
        u += tanh(40. * dot(u, u) * cos(1e2 * u.yx + t)) / 2e2 + .2 * a * u + cos(4. / exp(dot(o, o) / 1e2) + t) / 3e2;
	}
	float4 result = (25.6 / (min(o, 13.) + 164. / o) - dot(u, u) / 250);
	result.a = input.Color.a;
	return result;
}

technique Technique1
{
    pass Test
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}