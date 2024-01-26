sampler uScreenBuffer : register(s0);
sampler2D uImage0 : register(s1);

// Shlick 菲涅尔项近似公式中的F0
float3 uFresnelF0;

// 高光项整体权重
float3 uKs;

// 屏幕缓冲区空间与模型空间距离缩放系数
float2 uScreenDistanceMultipler;

// 屏幕缓冲区的大小，直接用来映射纹理坐标
float2 uViewportSize;

// 模型矩阵
float4x4 uModel;

// 模型-法线矩阵
float4x4 uMNormal;

// 视角+投影矩阵
float4x4 uViewProj;


struct VSInput
{
    float3 Pos : POSITION0;
    float4 Color : COLOR0;
    float3 Texcoord : TEXCOORD0;
};

struct PSInput
{
    float4 Color : COLOR0;
    float4 Pos : SV_POSITION;
    float3 Pos_World : TEXCOORD1;
    float3 Texcoord : TEXCOORD2;
};

PSInput VertexShaderFunction(VSInput input)
{
    PSInput output;
	
    float3 world_pos = mul(float4(input.Pos, 1), uModel);
    output.Pos_World = world_pos;
    output.Pos = mul(float4(world_pos, 1), uViewProj);
    output.Color = input.Color;
    output.Texcoord = input.Texcoord;
    return output;
}

float4 PixelShaderFunction(PSInput input) : COLOR0
{
    float4 normalColor = tex2D(uImage0, input.Texcoord.xy);

    float3 N = normalize(normalColor.xyz - float3(0.5, 0.5, 0.5));
    float3 R = normalize(reflect(float3(0, 0, -1), N));
    float NdotV = max(0, N.z);
	//return float4(N * 0.5 + 0.5, 1.0);
    float move = -input.Pos_World.z / R.z;
	// 纹理坐标上下颠倒，需要注意
    float2 move_world_pos = input.Pos_World.xy + float2(R.x, R.y) * move * uScreenDistanceMultipler;
	// 映射回屏幕纹理坐标
    move_world_pos = move_world_pos / uViewportSize;
    float3 fresnel = uFresnelF0 + (1.0 - uFresnelF0) * pow(1.0 - NdotV, 5);
    float3 sceneHDR = pow(tex2D(uScreenBuffer, move_world_pos).rgb, 2.2);
    float3 hdr = sceneHDR * uKs * fresnel;
    return float4(pow(hdr, 1 / 2.2), 1.0);
}

technique Technique1
{
    pass Test
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}
