sampler2D notUsed : register(s0);
sampler2D waterDisortion : register(s1);
sampler2D waterTarget : register(s2);

float2 uResolution;
float2 uTargetPos;
float2 uInvWaterSize;

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
	float4 data = tex2D(waterDisortion, texCoord * uZoom + uOffset);
	float value = data.r * 2 - 1;

	float2 coord = uResolution * texCoord + uTargetPos;
	coord *= uInvWaterSize;
	
	if (tex2D(waterTarget, coord).w < 0.01)
	{
		return float4(0, 0, 0, 0);
	}
	
	if (value > uThreasholdMin && value < uThreasholdMax)
	{
		float x = RandXY(texCoord.xy);
		float x2 = RandXY(float2(x, uVFXTime));
		
		if (x2 < uSpawnChance)
		{
			return float4(1, 1, 1, 1);
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