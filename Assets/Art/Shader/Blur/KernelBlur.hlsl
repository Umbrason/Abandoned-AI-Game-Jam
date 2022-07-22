

sampler2D _SourceTex;
float2 _SourceTex_TexelSize;
SAMPLER(sampler_LinearClamp);
TEXTURE2D(_CameraColorTexture);
float2 _CameraColorTexture_TexelSize;

/*SamplerState _MainTexSampler = sampler_state
        {
            Filter = MIN_MAG_MIP_LINEAR;
            AddressU = Wrap;
            AddressV = Wrap;
        };
        */

void blur_float(UnityTexture2D BlurStrengthTex, float Spread, int KernelSize, float2 uv, float2 screenPos, out float3 Out)
{
    float3 sum = float3(0.0, 0.0, 0.0);
    float blurStrength = SAMPLE_TEXTURE2D(BlurStrengthTex,BlurStrengthTex.samplerstate, uv).w;    
    KernelSize = clamp(KernelSize, 1, 10);
    int upper = ((KernelSize - 1) / uint(2));
    int lower = -upper;    
    for (int x = lower; x <= upper; x++)
        {
            for (int y = lower; y <= upper; y++)
            {
                float2 offset = float2(_CameraColorTexture_TexelSize.x * x, _CameraColorTexture_TexelSize.y * y) * blurStrength * Spread;
		        sum += SAMPLE_TEXTURE2D(_CameraColorTexture,sampler_LinearClamp, screenPos + offset);                
            }
        }
    sum /= (KernelSize * KernelSize);
    Out = float3(sum);
}