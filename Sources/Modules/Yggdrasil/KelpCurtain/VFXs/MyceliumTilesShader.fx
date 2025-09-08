sampler2D uImage0 : register(s0);
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

struct VSInput
{
    float2 Pos : POSITION0;
    float4 Color : COLOR0;
    float3 Texcoord : TEXCOORD0;
    float3 Texcoord2 : TEXCOORD1;
};

struct PSInput
{
    float4 Pos : SV_POSITION;
    float4 Color : COLOR0;
    float3 Texcoord : TEXCOORD0;
    float3 Texcoord2 : TEXCOORD1;
};

float3x3 m_rot(float angle)
{
	float c = cos(angle);
	float s = sin(angle);
	return float3x3(c, s, 0, -s, c, 0, 0, 0, 1);
}
float3x3 m_trans(float x, float y)
{
	return float3x3(1., 0., 0., 0., 1., 0, -x, -y, 1.);
}
float3x3 m_scale(float s)
{
	return float3x3(s, 0, 0, 0, s, 0, 0, 0, 1);
}
float mod_hlsl(float x, float y)
{
	return x - y * floor(x / y);
}

PSInput VertexShaderFunction(VSInput input)
{
    PSInput output;
    output.Color = input.Color;
    output.Texcoord = input.Texcoord;
    output.Texcoord2 = input.Texcoord2;
    output.Pos = mul(float4(input.Pos, 0, 1), uTransform);
    return output;
}

float4 PixelShaderFunction(PSInput input) : COLOR0
{
	if (!any(tex2D(uImage0, input.Texcoord2.xy)))
	{
		return float4(0, 0, 0, 0);
	}
	float2 pos = input.Texcoord.xy;

	pos *= 6.0;
	pos.y *= -1;
	float3 p = float3(pos, 1.0); // 使用 float4 替代 float3
	float d = 1.0;
	float iter = mod_hlsl(floor(input.Texcoord.z), 20.0); // 如果 input.Texcoord.z 为负，考虑可能的负值情况
	float len = frac(input.Texcoord.z); // 获取小数部分

	for (int i = 0; i < 20; ++i)
	{
		if (i <= int(iter + 0.5))
		{
			d = min(d, (length(max(abs(p.xy) - float2(0.01, 1.0), 0.0))) / p.z); // 使用 p.w 来替代 p.z
			p.x = abs(p.x);
            // 确保矩阵乘法的顺序和维度正确
			p = mul(p, m_trans(0, 1)); // 首先应用平移
			p = mul(p, m_rot(0.26 * 3.1415926535)); // 然后旋转
			p = mul(p, m_scale(1.22)); // 最后缩放
		}
		else
		{
			d = min(d, (length(max(abs(p.xy) - float2(0.01, len), 0.0))) / p.z);
		}
	}

	d = smoothstep(0.1, 0.15, d);
	float4 color = float4(1.0 - d, 1.0 - d, 1.0 - d, d);
	float4 color2 = tex2D(uNoiseSampler, input.Texcoord.xy);
	if (input.Texcoord2.z > color2.r)
	{
		return float4(0, 0, 0, 0);
	}
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