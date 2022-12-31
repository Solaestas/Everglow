sampler2D notUsed : register(s0);
sampler2D waterDisortion : register(s1);
sampler2D waterTarget : register(s2);

float2 uResolution;
float2 uTargetPos;
float2 uInvWaterSize;
float2 uWaterDisortionTargetSize;

float2 uZoom;
float2 uOffset;
float uThreasholdMin;
float uThreasholdMax;
float uSpawnChance;
float uVFXTime;

float RandXY(float2 coord)
{
	return frac(cos(coord.x * (12.9898) + coord.y * (4.1414)) * 43758.5453);
}

float4 PixelShaderFunction(float2 texCoord : TEXCOORD) : COLOR0
{
	float2 waterCoord = texCoord * uZoom + uOffset;
	float4 data = tex2D(waterDisortion, waterCoord);
	
	float2 chanceCoord = floor(waterCoord * uWaterDisortionTargetSize);
	float2 offset = waterCoord * uWaterDisortionTargetSize - chanceCoord;
	
	float value = data.r * 2 - 1;

	float2 coord = uResolution * texCoord + uTargetPos;
	coord *= uInvWaterSize;

	if (tex2D(waterTarget, coord).w < 0.01)
	{
		return float4(0, 0, 0, 0);
	}
	
	if (value > uThreasholdMin && value < uThreasholdMax)
	{
		float x = RandXY(chanceCoord);
		float x2 = RandXY(float2(x, uVFXTime));
		
		if (x2 < uSpawnChance)
		{
			bool xIn = abs(offset.x - 0.5) < 0.25;
			bool yIn = abs(offset.y - 0.5) < 0.25;
			if (xIn && yIn)
			{
				return float4(1, 1, 1, 1);
			}
			else if (!xIn && !yIn)
			{
				return float4(0, 0, 0, 0);
			}
			else
			{
				return float4(0.5, 0.5, 0.5, 1);
			}
		}
	}
	return float4(0, 0, 0, 0);
}

technique Technique1
{
	pass Display
	{
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
}