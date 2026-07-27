sampler uImage0 : register(s0);


float rand1;
float rand2;
float rand3;

float4x4 uTransform;
//float4 uDissolveColor;

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


float r(float2 p)
{
	
	return frac(cos(p.x * rand1 + p.y * rand2) * rand3);
}

float n(float2 p)
{
	float2 fn = floor(p);
	float2 sn = smoothstep(0, 1, frac(p));
    
	float h1 = lerp(r(fn), r(fn + float2(1, 0)), sn.x);
	float h2 = lerp(r(fn + float2(0, 1)), r(fn + float2(1, 1)), sn.x);
	return lerp(h1, h2, sn.y);
}

float noise(float2 p)
{
	return n(p / 32.0) * 0.58 +
           n(p / 16.0) * 0.2 +
           n(p / 8.0) * 0.1 +
           n(p / 4.0) * 0.05;
}

float4 background(float2 pos)
{
	return float4(0,0,0,0);
}

float4 PixelShaderFunction(PSInput input) : COLOR0
{
	float2 p = input.Texcoord.xy;
	float t = input.Texcoord.z;

    // Get input texture color
	float4 texColor = tex2D(uImage0, input.Texcoord.xy);

    // 使用简化的噪声，只调用一次n函数
	float2 noisePos = 2500.0 * p / 32.0; 
	float noiseValue = noise(noisePos); // 相当于原来噪声函数的第一层

    // fade to black
	float4 c = //float4(noiseValue, noiseValue, noiseValue, noiseValue);
	lerp(texColor, float4(0, 0, 0, 0),smoothstep(t + 0.1, t - 0.1, noiseValue));

    // 简化燃烧效果，只使用一个简单的噪声
	c.rgb = clamp(c.rgb + step(c.a, 0.1) * 1.6 * noiseValue *
                   float3(1.2, 0.5, 0.0), 0.0, 1.0);

    // 如果alpha大于0.01，使用c，否则使用黑色背景
	if (c.a < 0.01)
		c = float4(0, 0, 0, 0);
	return c;
}

/*
float4 PixelShaderFunction(PSInput input) : COLOR0
{
	
	float2 p = input.Texcoord.xy;
	float2 normalizedUV = input.Texcoord.xy;
    
	float t = fmod(input.Texcoord.z * 0.15, 1.2);
    
    // Get input texture color
	float4 texColor = tex2D(uImage0, input.Texcoord.xy);
    
    // fade to black
	float4 c = lerp(texColor, float4(0, 0, 0, 0),
                    smoothstep(t + 0.1, t - 0.1, noise(p * 0.4)));
    
    // burning on the edges (when c.a < .1)
	c.rgb = clamp(c.rgb + step(c.a, 0.1) * 1.6 * noise(2000.0 * normalizedUV) *
                   float3(1.2, 0.5, 0.0), 0.0, 1.0);
    
    // fancy background under burned texture
	c.rgba = c.rgba * step(0.01, c.a) + background(0.1 * normalizedUV) * step(c.a, 0.01);
    
	return c;




	
	/*float4 c = tex2D(uImage0, input.Texcoord.xy);
	if (!any(c))
		return float4(0, 0, 0, 0);
	c *= input.Color;
	float4 c1 = tex2D(uImage1, input.Texcoord.xy);
	float light = max(max(c1.r, c1.g), c1.b);
	float value = (light - input.Texcoord.z) * 2.5;
	if (light < input.Texcoord.z)
		return float4(0, 0, 0, 0);
	if (light < input.Texcoord.z + 0.4)
		return uDissolveColor * value + c * (1 - value);
	return c;
}*/

technique Technique1
{
	pass Test
	{
		VertexShader = compile vs_3_0 VertexShaderFunction();
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
}