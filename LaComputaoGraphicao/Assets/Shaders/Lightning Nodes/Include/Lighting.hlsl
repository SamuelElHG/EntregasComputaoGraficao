#ifndef LIGHTING_INFO
#define LIGHTING_INFO

//#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
//#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

void GetMainLight_float(float3 PositionWS, out float3 LightDirectionWS, out float3 LightColor)
{
    LightDirectionWS = float3(1, 1, -1);
    LightColor = 1;
#ifdef UNIVERSAL_LIGHTING_INCLUDED
    //Creanme
    float4 shadowCoord = TransformWorldToShadowCoord(PositionWS);
    //Creanme
    
    Light mainLight = GetMainLight(shadowCoord);

    LightDirectionWS = mainLight.direction;
    LightColor = mainLight.color;
#endif
}

void GetMainLight_half(float3 PositionWS, out half3 LightDirectionWS, out half3 LightColor)
{
    LightDirectionWS = float3(1, 1, -1);
    LightColor = 1;
#ifdef UNIVERSAL_LIGHTING_INCLUDED
    //Creanme
    float4 shadowCoord = TransformWorldToShadowCoord(PositionWS);
    //Creanme
    
    Light mainLight = GetMainLight(shadowCoord);

    LightDirectionWS = mainLight.direction;
    LightColor = mainLight.color;
#endif
}
#endif