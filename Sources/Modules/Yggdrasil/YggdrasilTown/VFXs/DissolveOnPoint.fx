sampler uImage0 : register(s0);

float range1;
float range2;
float2 p1;
float2 p2;
float2 p3;
int frames;
struct PSInput
{
    float4 Pos : SV_POSITION;
    float4 Color : COLOR0;
    float3 Texcoord : TEXCOORD0;
};


float4 PixelShaderFunction(PSInput input) : COLOR0
{
    float2 coords = input.Texcoord.xy;	
	float4 c = tex2D(uImage0, coords) * input.Color;
	

	float d1 = length(p1 - float2(coords.x, fmod(coords.y * frames, 1)));
	float d2 = length(p2 - float2(coords.x, fmod(coords.y * frames, 1)));
	float d3 = length(p3 - float2(coords.x, fmod(coords.y * frames, 1)));

		
	if (d1 < range1 || d2 < range1 || d3 < range1)
	{
		return c;
	}
	if ((d1 < range2 || d2 < range2 || d3 < range2) && c.r > 0)
	{
		return c + float4(0.25 * d1, 0.05 * d2, 0.4 * d3, 1);
	}
	else
	{
		return float4(0, 0, 0, 0);

	}

}

technique Technique1
{
	pass Tentacle
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
   
}