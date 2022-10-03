float4x4 uModel;
float4x4 uModelNormal;
float4x4 uViewProjection;

float3 uCameraPosition;
float3 uLightDirection;
float3 uLightIntensity;
float uNormalIntensity;

sampler2D _MainTex : register(s0);
sampler2D _NormalTex : register(s1);
sampler2D _MaterialTex : register(s2);
sampler2D _EmissionTex : register(s3);

#define FLT_MIN  1.175494351e-38
#define PI 3.14159265358
#define INV_PI 0.31830988618379
#define Sq(x) x * x
#define real float
#define real2 float2
#define real3 float3
#define real4 float4

float square(float x)
{
	return x * x;
}

//-----------------------------------------------------------------------------
// Fresnel term
//-----------------------------------------------------------------------------

real F_Schlick(real f0, real f90, real u)
{
	real x = 1.0 - u;
	real x2 = x * x;
	real x5 = x * x2 * x2;
	return (f90 - f0) * x5 + f0; // sub mul mul mul sub mad
}

real F_Schlick(real f0, real u)
{
	return F_Schlick(f0, 1.0, u); // sub mul mul mul sub mad
}

real3 F_Schlick(real3 f0, real f90, real u)
{
	real x = 1.0 - u;
	real x2 = x * x;
	real x5 = x * x2 * x2;
	return f0 * (1.0 - x5) + (f90 * x5); // sub mul mul mul sub mul mad*3
}

real3 F_Schlick(real3 f0, real u)
{
	return F_Schlick(f0, 1.0, u); // sub mul mul mul sub mad*3
}

// Does not handle TIR.
real F_Transm_Schlick(real f0, real f90, real u)
{
    real x = 1.0 - u;
    real x2 = x * x;
    real x5 = x * x2 * x2;
	return (1.0 - f90 * x5) - f0 * (1.0 - x5); // sub mul mul mul mad sub mad
}

// Does not handle TIR.
real F_Transm_Schlick(real f0, real u)
{
	return F_Transm_Schlick(f0, 1.0, u); // sub mul mul mad mad
}

// Does not handle TIR.
real3 F_Transm_Schlick(real3 f0, real f90, real u)
{
    real x = 1.0 - u;
    real x2 = x * x;
    real x5 = x * x2 * x2;
	return (1.0 - f90 * x5) - f0 * (1.0 - x5); // sub mul mul mul mad sub mad*3
}

// Does not handle TIR.
real3 F_Transm_Schlick(real3 f0, real u)
{
	return F_Transm_Schlick(f0, 1.0, u); // sub mul mul mad mad*3
}



//-----------------------------------------------------------------------------
// GGX term
//-----------------------------------------------------------------------------

// Ref: Understanding the Masking-Shadowing Function in Microfacet-Based BRDFs, p. 19, 29.
// p. 84 (37/60)
real G_MaskingSmithGGX(real NdotV, real roughness)
{
    // G1(V, H)    = HeavisideStep(VdotH) / (1 + Lambda(V)).
    // Lambda(V)        = -0.5 + 0.5 * sqrt(1 + 1 / a^2).
    // a           = 1 / (roughness * tan(theta)).
    // 1 + Lambda(V)    = 0.5 + 0.5 * sqrt(1 + roughness^2 * tan^2(theta)).
    // tan^2(theta) = (1 - cos^2(theta)) / cos^2(theta) = 1 / cos^2(theta) - 1.
    // Assume that (VdotH > 0), e.i. (acos(LdotV) < Pi).
	
	return 1.0 / (0.5 + 0.5 * sqrt(1.0 + Sq(roughness) * (1.0 / Sq(NdotV) - 1.0)));
}

real GetSmithJointGGXPartLambdaV(real NdotV, real roughness)
{
    real a2 = Sq(roughness);
	return sqrt((-NdotV * a2 + NdotV) * NdotV + a2);
}

// Note: V = G / (4 * NdotL * NdotV)
// Ref: http://jcgt.org/published/0003/02/03/paper.pdf
real V_SmithJointGGX(real NdotL, real NdotV, real roughness, real partLambdaV)
{
    real a2 = Sq(roughness);

    // Original formulation:
    // lambda_v = (-1 + sqrt(a2 * (1 - NdotL2) / NdotL2 + 1)) * 0.5
    // lambda_l = (-1 + sqrt(a2 * (1 - NdotV2) / NdotV2 + 1)) * 0.5
    // G        = 1 / (1 + lambda_v + lambda_l);

    // Reorder code to be more optimal:
    real lambdaV = NdotL * partLambdaV;
    real lambdaL = NdotV * sqrt((-NdotL * a2 + NdotL) * NdotL + a2);

    // Simplify visibility term: (2.0 * NdotL * NdotV) /  ((4.0 * NdotL * NdotV) * (lambda_v + lambda_l))
	return 0.5 / max(lambdaV + lambdaL, FLT_MIN);
}

real DV_SmithJointGGX(real NdotH, real NdotL, real NdotV, real roughness, real partLambdaV)
{
    real a2 = Sq(roughness);
    real s = (NdotH * a2 - NdotH) * NdotH + 1.0;

    real lambdaV = NdotL * partLambdaV;
    real lambdaL = NdotV * sqrt((-NdotL * a2 + NdotL) * NdotL + a2);

    real2 D = real2(a2, s * s); // Fraction without the multiplier (1/Pi)
    real2 G = real2(1, lambdaV + lambdaL); // Fraction without the multiplier (1/2)

    // This function is only used for direct lighting.
    // If roughness is 0, the probability of hitting a punctual or directional light is also 0.
    // Therefore, we return 0. The most efficient way to do it is with a max().
	return INV_PI * 0.5 * (D.x * G.x) / max(D.y * G.y, FLT_MIN);
}

// Ref: Diffuse Lighting for GGX + Smith Microsurfaces, p. 113.
real3 DiffuseGGXNoPI(real3 albedo, real NdotV, real NdotL, real NdotH, real LdotV, real roughness)
{
    real facing = 0.5 + 0.5 * LdotV; // (LdotH)^2
    real rough = facing * (0.9 - 0.4 * facing) * (0.5 / NdotH + 1);
    real transmitL = F_Transm_Schlick(0, NdotL);
    real transmitV = F_Transm_Schlick(0, NdotV);
    real smooth = transmitL * transmitV * 1.05; // Normalize F_t over the hemisphere
    real single = lerp(smooth, rough, roughness); // Rescaled by PI
    real multiple = roughness * (0.1159 * PI); // Rescaled by PI

	return single + albedo * multiple;
}

real3 DiffuseGGX(real3 albedo, real NdotV, real NdotL, real NdotH, real LdotV, real roughness)
{
    // Note that we could save 2 cycles by inlining the multiplication by INV_PI.
	return INV_PI * DiffuseGGXNoPI(albedo, NdotV, NdotL, NdotH, LdotV, roughness);
}

float3 F_Schlick_S(float3 F0, float HdotV)
{
	return F0 + (1.0 - F0) * pow(1.0 - HdotV, 5);
}

float D_GGX(float NdotH, float alpha)
{
	float a2 = square(alpha);
	return a2 / (PI * square(square(NdotH) * (a2 - 1.0) + 1.0));
}

float V_SmithGGXCorrelated(float NdotL, float NdotV, float alpha)
{
	float a2 = square(alpha);
	float GGXL = NdotV * sqrt((-NdotL * a2 + NdotL) * NdotL + a2);
	float GGXV = NdotL * sqrt((-NdotV * a2 + NdotV) * NdotV + a2);
	return 0.5 / max(1e-5, (GGXV + GGXL));
}


struct VSInput
{
    float3 Pos : POSITION0;
    float3 Texcoord : TEXCOORD0;
    float3 Normal : NORMAL0;
	float3 Tangent : NORMAL1;
};

struct PSInput
{
    float4 Pos : SV_POSITION;
    float3 Texcoord : TEXCOORD0;
	float3 WorldPos : TEXCOORD1;
	float4 ClipSpacePos : TEXCOORD2;
    float3 Normal : NORMAL0;
	float3 Tangent : NORMAL1;
};

PSInput VertexShaderFunction(VSInput input)
{
    PSInput output;
    output.Texcoord = float3(input.Texcoord.x, abs(input.Texcoord.y-1), input.Texcoord.z);

	float4 posWS = mul(float4(input.Pos, 1), uModel);
	float4 posCS = mul(posWS, uViewProjection);
	output.Pos = posCS;
	output.WorldPos = posWS.xyz;
	output.ClipSpacePos = posCS;
    output.Normal = mul(float4(input.Normal, 0), uModelNormal).xyz;
	output.Tangent = mul(float4(input.Tangent, 0), uModelNormal).xyz;
    return output;
}



float4 PS_Forward_Lit(PSInput input, out float4 emissionTarget : SV_Target1,
							out float4 depthNormalTarget : SV_Target2) : SV_Target0
{
	emissionTarget = tex2D(_EmissionTex, input.Texcoord.xy);
	
	float4 albedo = tex2D(_MainTex, input.Texcoord.xy);
	float4 materialParams = tex2D(_MaterialTex, input.Texcoord.xy);
	float3 normalOS = normalize(tex2D(_NormalTex, input.Texcoord.xy).xyz * 2 - 1);
	normalOS.xy = normalOS.xy * uNormalIntensity;
	
	float3 GNormal = normalize(input.Normal);
	float3 BiTangent = normalize(cross(GNormal, input.Tangent));
	float3 Tangent = -normalize(cross(GNormal, BiTangent));
	float3 N = normalize(normalOS.x * Tangent + normalOS.y * BiTangent + normalOS.z * GNormal);

	depthNormalTarget = float4(N * 0.5 + 0.5, saturate((input.ClipSpacePos.w - input.ClipSpacePos.z) / input.ClipSpacePos.w));
	
	float3 L = uLightDirection;
	float3 V = normalize(uCameraPosition - input.WorldPos);
	float3 H = normalize(L + V);

	float NdotH = max(0, dot(N, H));
	float NdotL = max(0, dot(N, L));
	float NdotV = max(0, dot(N, V));
	float LdotV = max(0, dot(L, V));
	float HdotV = saturate(dot(H, V));
	
	float roughness = 0.3; //clamp(materialParams.r, 0.002, 1.0);
	float metallic = saturate(materialParams.g);
	float alpha = Sq(roughness);
	
	float3 baseF0 = 0.04;
	float3 f0 = lerp(baseF0, albedo.xyz, metallic);
	
	float Vis = V_SmithGGXCorrelated(NdotL, NdotV, alpha);
	float3 Ls = D_GGX(NdotH, alpha) * Vis * F_Schlick_S(f0, HdotV);
	float3 Ld = (1.0 - metallic) * albedo.xyz * INV_PI; //DiffuseGGX(albedo.xyz, NdotV, NdotL, NdotH, LdotV, 1.0);
	float3 Lit = (Ls + Ld) * NdotL * uLightIntensity;
	return float4(Lit, albedo.a);
}


//void PS_GBuffer_Prepass(PSInput input,
//                        out float4 albedoTarget : SV_Target0,
//                        out float4 normalTarget : SV_Target1,
//                        out float4 surfaceTarget : SV_Target2,
//                        out float4 emissionTarget : SV_Target3,
//                        out float4 worldPosTarget : SV_Target4)
//{
//	albedoTarget = tex2D(_MainTex, input.Texcoord.xy);
//	surfaceTarget = tex2D(_MaterialTex, input.Texcoord.xy);
//	emissionTarget = tex2D(_EmissionTex, input.Texcoord.xy);
    
//	float3 preNormalWS = normalize(input.Normal);
//	float3 normalOS = normalize(tex2D(_NormalTex, input.Texcoord.xy).xyz * 2 - 1);
//	float3 biTangent = cross(preNormalWS, input.Tangent);
    
//	float3x3 TBN = float3x3(input.Tangent, biTangent, preNormalWS);
//	float3 normalWS = mul(normalOS, TBN);
//	normalTarget = float4(preNormalWS * 0.5 + 0.5, 0);
//	worldPosTarget = float4(input.WorldPos, 1);
//}

float stepP(float x)
{
	if (x > 0.8)
	{
		x = 1.0;
	}
	else if (x > 0.6)
	{
		x = 0.8;
	}
	else if (x > 0.4)
	{
		x = 0.6;
	}
	else if (x > 0.2)
	{
		x = 0.4;
	}
	else if (x > 0.05)
	{
		x = 0.2;
	}
	return x;
}

float4 PS_ToonShading(PSInput input, out float4 emissionTarget : SV_Target1,
							out float4 depthNormalTarget : SV_Target2) : SV_Target0
{
	emissionTarget = tex2D(_EmissionTex, input.Texcoord.xy);
	
	float4 albedo = tex2D(_MainTex, input.Texcoord.xy);
	float4 materialParams = tex2D(_MaterialTex, input.Texcoord.xy);
	float3 normalOS = normalize(tex2D(_NormalTex, input.Texcoord.xy).xyz * 2 - 1);
	
	float3 N = normalize(input.Normal);
	float3 biTangent = cross(N, input.Tangent);
	float3x3 TBN = float3x3(input.Tangent, biTangent, N);
	float3 normalWS = mul(normalOS, TBN);
	
	depthNormalTarget = float4(normalWS * 0.5 + 0.5, saturate((input.ClipSpacePos.w - input.ClipSpacePos.z) / input.ClipSpacePos.w));
	
	float3 L = uLightDirection;
	float3 V = normalize(uCameraPosition - input.WorldPos);
	float3 H = normalize(L + V);

	float NdotH = max(0, dot(N, H));
	float NdotL = max(0, dot(N, L));
	float NdotV = max(0, dot(N, V));
	float LdotV = max(0, dot(L, V));
	float HdotV = saturate(dot(H, V));
	
	float s = pow(NdotH, 512);
	s = stepP(s);

	return float4((2 * s * albedo.xyz + stepP(NdotL) * albedo.xyz * INV_PI) * uLightIntensity, albedo.a);
}


technique Render
{
	pass Forward_Lit
	{
		VertexShader = compile vs_3_0 VertexShaderFunction();
		PixelShader = compile ps_3_0 PS_Forward_Lit();
	}

	pass Forward_Toon
	{
		VertexShader = compile vs_3_0 VertexShaderFunction();
		PixelShader = compile ps_3_0 PS_ToonShading();
	}
}
