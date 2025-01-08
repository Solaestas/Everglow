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

float2x2 rotate2d(float angle)
{
	return float2x2(cos(angle), -sin(angle),
                sin(angle), cos(angle));
}

float random(in float2 st)
{
	return fract(sin(dot(st.xy,
                         float2(12.9898, 78.233)))
                * 43758.5453123);
}

float noise(float2 st)
{
	float2 i = floor(st);
	float2 f = fract(st);
	float2 u = f * f * (3.0 - 2.0 * f);
	return lerp(lerp(random(i + float2(0.0, 0.0)),
                     random(i + float2(1.0, 0.0)), u.x),
                lerp(random(i + float2(0.0, 1.0)),
                     random(i + float2(1.0, 1.0)), u.x), u.y);
}

float4 Projectile(PSInput input) : COLOR0
{
	float4 mainTex = tex2D(uImage, input.Texcoord.xy);
	
	float2 st = input.Texcoord.xy / u_resolution.xy;
	st.x *= u_resolution.x / u_resolution.y;
    
	st = st - float2(0.5, 0.5);
	st = asin(st / 0.5) * 0.5 * 3.14 * 0.25;
	float3 color = float3(0, 0, 0);

    // Scale
	st *= 16.;
	
	st += u_time * 0.2 * float2(1, 1);

	st = mul(rotate2d(noise(st)), st);

    // Tile the space
	float2 i_st = floor(st);
	float2 f_st = fract(st);

	float m_dist = 1.; // minimum distance

	for (int y = -1; y <= 1; y++)
	{
		for (int x = -1; x <= 1; x++)
		{
            // Neighbor place in the grid
			float2 neighbor = float2(float(x), float(y));

            // Random position from current + neighbor place in the grid
			float2 po = tex2D(uNoiseSampler, (i_st + neighbor) * 0.05).xy;

			// Animate the point
			po = 0.5 + 0.5 * sin(u_time + 6.2831 * po);

			// Vector between the pixel and the point
			float2 diff = neighbor + po
			- f_st;

            // Distance to the point
			float dist = length(diff);

            // Keep the closer distance
			m_dist = min(m_dist, dist);
		}
	}

    // Draw the min distance (distance field)
	color += m_dist;

    // Draw cell center
	color += 1. - step(.02, m_dist);

	return float4(color, 0.3) * mainTex * input.Color;
}

float4 Burst(PSInput input) : COLOR0
{
	float4 mainTex = tex2D(uImage, input.Texcoord.xy);
	
	float2 st = input.Texcoord.xy / u_resolution.xy;
	st = float2(0.5, 0.5) - st;
	st.y = 1. / st.y;
	st.x = 1. / st.x;
	st *= 2.;
	float a = fmod(atan(st) + u_time * 0.1 + tex2D(uNoiseSampler, st * 0.05).xy, 6.28);
	float r = 0.8;
	float d = sin(a * 25.) + 0.9;
	float width = sin(a * 5.) + 1.7;
	float v = smoothstep(r - 0.05, r - width, d);
	
	return float4(float3(v, v, v), 1.0) * input.Color * mainTex;
}

technique Technique1
{
	pass Projectile
	{
		PixelShader = compile ps_3_0 Projectile();
	}


	pass Burst
	{
		PixelShader = compile ps_3_0 Burst();
	}
}