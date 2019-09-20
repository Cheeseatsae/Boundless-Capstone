Shader "Hidden/PixelateImageEffect"
{
    HLSLINCLUDE

    #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

    TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
    uniform half _TileSize;

	float2 fmod(float2 a, float2 b)
	{
		float2 c = frac(abs(a / b)) * abs(b);
		return abs(c);
	}

    float4 Frag(VaryingsDefault i) : SV_TARGET
    {
        float2 fragCoord = (i.texcoordStereo.xy) * _ScreenParams.xy;
		float4 c = 0;
		
		float2 uv = fragCoord.xy / _ScreenParams.xy;
		float2 size = 1.0 / float2(_ScreenParams.x, _ScreenParams.y) * _TileSize;

		float2 Pbase = uv - fmod(uv, size);
		float2 PCenter = Pbase + size / 2.0;
		float2 st = (uv - Pbase) / size;

		float4 tileColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, PCenter);
		c = tileColor;

		return c;
    }

    ENDHLSL

    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM
                #pragma vertex VertDefault
                #pragma fragment Frag
            ENDHLSL
        }
    }
}
