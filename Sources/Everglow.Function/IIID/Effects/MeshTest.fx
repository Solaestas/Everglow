float4x4 uModel;
float4x4 uViewProjection;
float4x4 uModelNormal;

float4 uColor0;
float4 uColor1;
float4 uColor2;
float3 uLightS;
float3 uAxis;
float uTime;
float uMoveZ;


texture texImage;
texture glowImage;

sampler2D s3 = sampler_state
{
    Texture = <texImage>;
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};

sampler2D s4 = sampler_state
{
    Texture = <glowImage>;
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};

struct VSInput
{
    float3 Pos : POSITION0;
    float3 Texcoord : TEXCOORD0;
    float3 Normal : NORMAL0;
};

struct PSInput
{
    float4 Pos : SV_POSITION;
    float3 Color : COLOR0;
    float3 Texcoord : TEXCOORD0;
    float3 Normal : NORMAL0;
};

float3 SpinWithAxis(float3 orig, float3 axis, float rotation)
{
    axis = normalize(axis);
    float k = cos(rotation);
    return orig * k + cross(axis, orig * sin(rotation)) + dot(axis, orig) * axis * (1 - k);
}
float GetLight(float3 v1, float3 v2)
{
    return max(dot(v1, v2) / length(v1) / length(v2), 0);
}

PSInput VertexShaderFunction(VSInput input)
{
    PSInput output;
    output.Color = float3(1, 1, 1);
	// GetLight(uLightS, SpinWithAxis(input.Normal, uAxis, uTime)) * uColor0;
    output.Texcoord = input.Texcoord;
	
    float4x4 MVP = mul(uModel, uViewProjection);
    output.Pos = mul(float4(input.Pos, 1), MVP);
    output.Normal = mul(float4(input.Normal, 0), uModelNormal).xyz;
    return output;
}

float4 PixelShaderFunction(PSInput input) : COLOR0
{
    if (input.Texcoord.x > 4000 && input.Texcoord.y > 4000)
    {
        float4 c0 = tex2D(s4, (input.Texcoord.xy - float2(4000, 4000)));
        return float4(c0.rgb, 1);
    }
    else
    {
        float4 c0 = tex2D(s3, input.Texcoord.xy);
        return float4(c0.rgb * GetLight(input.Normal, uLightS), 1);
    }
}


technique Technique1
{
    pass Test
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}
