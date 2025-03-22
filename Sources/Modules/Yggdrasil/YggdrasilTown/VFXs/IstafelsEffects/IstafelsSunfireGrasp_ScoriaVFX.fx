#define OCTAVES 4
#define LACUNARITY 2.0
#define GAIN 0.5

sampler2D uImage : register(s0);
float uTime;
float uScale;
float uRand;
float uProgress;

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

struct PSInput
{
	float4 Pos : SV_POSITION;
	float4 Color : COLOR0;
	float3 Texcoord : TEXCOORD0;
};

float2 rotate2d(float2 uv, float angle)
{
	float2x2 mat = float2x2(cos(angle), -sin(angle), sin(angle), cos(angle));
	return 0.5 + mul(mat, (uv - 0.5));
}


float2 random2(float2 uv)
{
	uv = float2(dot(uv, float2(127.1, 311.7)), dot(uv, float2(269.5, 183.3)));
	float2 v = -1.0 + 2.0 * frac(sin(uv) * 43758.5453123);
	v = rotate2d(v, uTime); // rotated random direction
	return v;
}


float noise(float2 uv)
{
	float2 nuv = float2(0.5, 0.5) - uv;

	float2 i = floor(uv);
	float2 f = frac(uv);
    
    // four corners
	float a = dot(random2(i + float2(0.0, 0.0)), f);
	float b = dot(random2(i + float2(1.0, 0.0)), f - float2(1.0, 0.0));
	float c = dot(random2(i + float2(0.0, 1.0)), f - float2(0.0, 1.0));
	float d = dot(random2(i + float2(1.0, 1.0)), f - float2(1.0, 1.0));
    
    // interplation, cubic hermite
	float2 u = f * f * (3.0 - 2.0 * f);
    
	return lerp(lerp(a, b, u.x), lerp(c, d, u.x), u.y);
}

float fbm(float2 uv)
{
	float v = 0.0;
	float amplitude = 0.5;
	float frequency = 5.0;
    
	for (int i = 0; i < OCTAVES; ++i)
	{
		uv -= uTime * float2(0.06, 0.01);
		float n = abs(noise((uv) * frequency)); //abs(noise) - turbulence, create creases
        
        // creating ridge
		n = 0.9 - n; // invert so creases are at top
		n *= n; // sharpen creases
        
		v += amplitude * n;
		frequency *= LACUNARITY;
		amplitude *= GAIN;
	}
	return v;
}

float2 mixNoise(float2 p)
{
	float epsilon = .968785675;
	float noiseX = noise(float2(p.x + epsilon, p.y)) - noise(float2(p.x - epsilon, p.y));
	float noiseY = noise(float2(p.x, p.y + epsilon)) - noise(float2(p.x, p.y - epsilon));
	return float2(noiseX, noiseY);
}

float2x2 rot(float a)
{
	return float2x2(cos(a), sin(a), -sin(a), cos(a));
}

float4 NotCollidedPixelShader(PSInput input) : COLOR0
{
	float2 baseCoord = input.Texcoord.xy - float2(0.5, 0.5);
	baseCoord *= length(baseCoord);
	float3 uv = float3(baseCoord, sqrt(0.45 * 0.45 - baseCoord.x * baseCoord.x - baseCoord.y * baseCoord.y) / 4);
	
	float dist = distance(uv.xy, float2(0.0, 0.0));
	uv /= float3(1.0 * uv.z, 1.0 * uv.z, 1.0);
	
	uv /= uScale;
	uv += float3(uRand, uRand, uRand);
	
	float f = (fbm(uv.xy) * 2.0 - 1.0) * 2.0;
	
	float3 col = lerp(float3(0.81, 0.06, 0.13), float3(0.80, 0.40, 0.00), f * f);
  
	float4 color;
	
	float4 mainC = tex2D(uImage, input.Texcoord.xy);
	if (!any(mainC))
	{
		color = float4(0, 0, 0, 0);
	}
	else
	{
		float4 scoriaColor = float4((f * f * (8.0 - 5.0 * f)) * col, 1.0);
		
		float stoneFact = (1 - pow(length(baseCoord), 3)) * 0.2;
		float4 stoneColor = tex2D(uNoiseSampler, uv.xy) + float4(stoneFact, stoneFact, stoneFact, 1);
		
		color = lerp(stoneColor, scoriaColor, uProgress);
	}
    
	return color * input.Color;
}

float4 Pixel(PSInput input) : COLOR0
{
	float2 uv1 = input.Texcoord.xy;
	float2 uv = uv1 * uScale;
	
	float f = (fbm(uv.xy) * 2.0 - 1.0) * 2.0;
	
	float3 col = lerp(float3(0.81, 0.06, 0.13), float3(0.80, 0.40, 0.00), f * f);

	float4 scoriaColor = float4((f * f * (8.0 - 5.0 * f)) * col, 1.0);
	
	float3 col2 = float3(.212, 0.08, 0.03) / max(fbm(uv) * 2.2, 0.0001);
	
	col2 = pow(col2, float3(1.5, 1.5, 1.5));
	
	float4 coldScoriaColor = float4(col2, 1.);
		
	float4 stoneColor = tex2D(uNoiseSampler, uv1.xy) + float4(0.2, 0.2, 0.2, 1);
    
	float4 finalScoriaColor = lerp(coldScoriaColor, scoriaColor, uProgress);
	return lerp(stoneColor, finalScoriaColor, uProgress) * input.Color;
}

technique Tech
{
	pass Pass1
	{
		PixelShader = compile ps_3_0 Pixel();
	}

	pass NotCollided
	{
		PixelShader = compile ps_3_0 NotCollidedPixelShader();
	}
}
