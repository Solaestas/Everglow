const float NoiseResolution = 4.0; //  0.4
const float Lacunarity = 2.0; // 2.0 // intuitive measure of gappiness / heterogenity or variability
const float Gain = 0.6; // 0.6
const float Ball_rad = 0.45; // 0.45
const float Ball_roll_spd = 0.5; // 0.5
const float Dark_lava_spd = 0.05; // 0.05
const float Dark_island_spd = 0.5; // 0.5

float uTime;

float2 random2D(float2 p)
{
	return frac(sin(float2(dot(p, float2(127.1, 311.7)),
                          dot(p, float2(269.5, 183.3)))) * 43758.5453);
}

float random1D(float2 p)
{
	return frac(sin(dot(p.xy, float2(12.9898, 78.233))) * 43758.5453123);
}

float rand(float2 n)
{
	return frac(cos(dot(n, float2(5.9898, 4.1414))) * 65899.89956);
}

// Add a bit of noise
float noise2D(float2 _pos)
{
	float2 i = floor(_pos); // integer
	float2 f = frac(_pos); // fracion

// define the corners of a tile
	float a = random1D(i);
	float b = random1D(i + float2(1.0, 0.0));
	float c = random1D(i + float2(0.0, 1.0));
	float d = random1D(i + float2(1.0, 1.0));

	// smooth Interpolation
	float2 u = smoothstep(0.0, 1.0, f);
    
	// lerp between the four corners
	return lerp(a, b, u.x) + (c - a) * u.y * (1.0 - u.x) + (d - b) * u.x * u.y;
}

// fracal brownian motion
float fbm(float2 _pos)
{
	_pos.y += uTime * Ball_roll_spd;
	_pos.x += sin(uTime * Ball_roll_spd);
	float ts = uTime * Dark_lava_spd;
	float val = 0.0;
	float amp = 0.4;
    
	// Loop of octaves
	for (int i = 0; i < 4; ++i) // set octave number to 4
	{
		val += amp * noise2D(_pos + ts);
		_pos *= Lacunarity;
		amp *= Gain;
	}
	return val;
}

float voronoiIQ(float2 _pos)
{
	_pos.y += uTime * Ball_roll_spd;
	_pos.x += sin(uTime * Ball_roll_spd);
	float2 p = floor(_pos);
	float2 f = frac(_pos);
	float res = 0.0;
	for (int j = -1; j <= 1; j++)
		for (int i = -1; i <= 1; i++)
		{
			float2 b = float2(i, j);
			float2 pnt = random2D(p + b);
			pnt = 0.5 + 0.5 * sin((uTime * Dark_island_spd) + 6.2831 * pnt);
			float2 r = float2(b) - f + pnt;
			float d = dot(r, r);
			res += exp(-32.0 * d); // quickly decaying exponential 
		}
	return -(1.0 / 32.0) * log(res);
}

struct PSInput
{
	float4 Pos : SV_POSITION;
	float4 Color : COLOR0;
	float3 Texcoord : TEXCOORD0;
};

float4 PShader(PSInput input):COLOR0
{
	float3 color = float3(0.0, 0.0, 0.0);
    
    // Normalized pixel coordinates (from 0 to 1)
	float2 pos1 = 1.1 * (input.Texcoord.xy - float2(0.5, 0.5)); // center what being drawn
	float3 pos = float3(pos1, sqrt(Ball_rad * Ball_rad - pos1.x * pos1.x - pos1.y * pos1.y) / NoiseResolution);
    
	float dist = distance(pos.xy, float2(0.0, 0.0));
	pos /= float3(1.0 * pos.z, 1.0 * pos.z, 1.0);

    // Draw outside bloom
	if (dist > (Ball_rad - Ball_rad * 0.125))
	{
		color = float3(0.0, 0.0, 0.0);
		color.r += 1.0 - smoothstep(Ball_rad - Ball_rad * 0.35, Ball_rad + 0.125, dist);
	}
	else // Draw main color
	{
		float cellular = voronoiIQ(pos.xy);
		color.rg = float2(cellular, cellular);
		
		float2 nfbm = float2(pos.x, pos.y - rand(float2(1., 5.)));
		float q = fbm(nfbm);
		float2 arg1 = float2(pos.xy + q + uTime * 0.15 - pos.x - pos.y);
    
		color.r += 0.25 + fbm(arg1);
	}
    
	return float4(color, 0.0);
}

technique Test
{
	pass Test
	{
		PixelShader = compile ps_3_0 PShader();
	}
}