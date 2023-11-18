#ifndef GAUSSIAN_BLUR_INCLUDED
#define GAUSSIAN_BLUR_INCLUDED

void GaussianBlur3x3_float(UnityTexture2D Tex, UnitySamplerState SS, float2 UV, float2 TexelSize, out float4 Result)
{
    const float3 gaussian = float3(0.06,0.125,0.25);
    const float3 off = float3(0,1,-1);
    Result = 0;
    Result += SAMPLE_TEXTURE2D(Tex, SS, UV + off.zy * TexelSize) * gaussian.x; //Arriba izquierda
    Result += SAMPLE_TEXTURE2D(Tex, SS, UV + off.xy * TexelSize) * gaussian.x; //Arriba
    Result += SAMPLE_TEXTURE2D(Tex, SS, UV + off.yy * TexelSize) * gaussian.x; //Arriba Derecha
    Result += SAMPLE_TEXTURE2D(Tex, SS, UV + off.xz * TexelSize) * gaussian.x; //Izquierda
    Result += SAMPLE_TEXTURE2D(Tex, SS, UV) * gaussian.x;                      //Centro
    Result += SAMPLE_TEXTURE2D(Tex, SS, UV + off.yx * TexelSize) * gaussian.x; //Derecha
    Result += SAMPLE_TEXTURE2D(Tex, SS, UV + off.zz * TexelSize) * gaussian.x; //Abajo Izquierda
    Result += SAMPLE_TEXTURE2D(Tex, SS, UV + off.xz * TexelSize) * gaussian.x; //Abajo
    Result += SAMPLE_TEXTURE2D(Tex, SS, UV + off.yz * TexelSize) * gaussian.x; //Abajo Derecha
}

#endif