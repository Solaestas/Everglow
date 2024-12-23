sampler2D uImage : register(s0);
texture uHeatMap;
sampler uHeatMapSampler =
sampler_state
{
	Texture = <uHeatMap>;
	MipFilter = LINEAR;
	MinFilter = LINEAR;
	MagFilter = LINEAR;
	AddressU = CLAMP;
	AddressV = CLAMP;
};
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

float u_time;
float2 u_resolution;

struct PSInput
{
	float4 Pos : SV_POSITION;
	float4 Color : COLOR0;
	float3 Texcoord : TEXCOORD0;
	float3 Texcoord2 : TEXCOORD1;
};

float2 fract(float2 st)
{
	return st - floor(st);
}

float2 random2(float2 st)
{
	st = float2(dot(st, float2(127.1, 311.7)),
              dot(st, float2(269.5, 183.3)));
	return -1.0 + 2.0 * fract(sin(st) * 43758.5453123);
}

float noise(float2 st)
{
	float2 i = floor(st);
	float2 f = fract(st);

	float2 u = f * f * (3.0 - 2.0 * f);

	return lerp(lerp(dot(random2(i + float2(0.0, 0.0)), f - float2(0.0, 0.0)),
                     dot(random2(i + float2(1.0, 0.0)), f - float2(1.0, 0.0)), u.x),
                lerp(dot(random2(i + float2(0.0, 1.0)), f - float2(0.0, 1.0)),
                     dot(random2(i + float2(1.0, 1.0)), f - float2(1.0, 1.0)), u.x), u.y);
}

float2x2 rotate2d(float angle)
{
	return float2x2(cos(angle), -sin(angle),
                sin(angle), cos(angle));
}

float4 PixelShaderFunction(PSInput input) : COLOR0
{
	float4 mainTex = tex2D(uImage, input.Texcoord.xy);
	
	float2 st = input.Texcoord.xy / u_resolution.xy;
	st = float2(0.5, 0.5) - st;
	st.y = 1. / st.y;
	st.x = 1. / st.x;
	st *= 2.;
	float a = fmod(atan(st) + u_time * 0.1 + noise(st * 0.05), 6.28);
	float r = 0.8;
	float d = sin(a * 25.) + 0.9;
	float width = sin(a * 5.) + 1.7;
	float v = smoothstep(r - 0.05, r - width, d);
	
	return float4(float3(v, v, v), 1.0) * input.Color * mainTex;
}

technique Technique1
{
	pass Test
	{
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
}