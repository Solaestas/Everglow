sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float3 axle;
float4x4 uTransform;
float2 corr;
struct VertexIn
{
	float3 pos : POSITION;
	float3 texCoord : TEXCOORD;
	float3 normal : NORMAL;
};

struct VertexOut
{
	float4 posH : SV_POSITION;
	float3 texCoord : TEXCOORD;
	float3 normal : TEXCOORD1;
	float3 posOut : TEXCOORD2;
};


VertexOut VS(VertexIn vIn)
{
	VertexOut vout;
	vout.posH = mul(float4(vIn.pos, 1), uTransform);
	vout.texCoord = vIn.texCoord;
	vout.normal = normalize(vIn.normal);
	vout.posOut = vIn.pos;
	return vout;
}
float4 PixelShaderFunction(float3 texCoord :TEXCOORD,float3 normal :TEXCOORD,float3 posOut : TEXCOORD2) : COLOR0
{
    float4 c = tex2D(uImage0, texCoord.xy);
	float3 Tolig = float3(-0.02, -0.02, 0.15) - posOut / 500;
    float lig0 = dot(normalize(Tolig), normal);
    float kdis = length(Tolig) / 4;
    float disr = (1 - kdis * kdis);
    disr = max(0, disr);
    disr *= disr;
    float lightvalumn = max(0, lig0) * disr;
	return float4(c.r * lightvalumn,c.g * lightvalumn,c.b * lightvalumn,1);
}

technique Technique1
{
	pass Tentacle
	{
	    VertexShader = compile vs_2_0 VS();
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}   
}