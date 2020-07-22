Shader "Custom/rimSHading"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
		_RimColor("RimColor", Color) = (1,1,1,1)
		_Normal("Normal",2D) = ""{}
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Size ("StripeCount", Range(0,1)) = 0.5
		_StripeSize("StripeSize",Range(0,1))=0.4
		_DiffuseIntensity("IntensityDiff",Range(0,10))=1
		_EmissiveIntensity("EmissionIntensity",Range(0,10))=1
		_RimPow("RimPow",Range(0,10)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
		pass {
			ZWrite On
			ColorMask 0

		}
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Lambert alpha:fade

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
		sampler2D _Normal;
        struct Input
        {
			float2 uv_Normal;
            float2 uv_MainTex;
			float3 viewDir;
			float3 worldPos;
			float3 localPos;
        };

		fixed4 _RimColor;
        fixed4 _Color;
		
		half _DiffuseIntensity;
		half _EmissiveIntensity;
		half _RimPow;
		half _Size;
		half _StripeSize;
      
      

        void surf (Input IN, inout SurfaceOutput o)
        {
            // Albedo comes from a texture tinted by color
			half dotProd = 1-saturate(dot(IN.viewDir,o.Normal));
			dotProd = pow(dotProd,_RimPow);
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color*_DiffuseIntensity;
			fixed3 a = (dotProd > 0.8) ? _RimColor : (dotProd>0.5?c:_RimColor);
			o.Emission = ((frac(IN.uv_MainTex.y*_Size*10)>_StripeSize)?_RimColor:a)*dotProd;
			o.Alpha = dotProd;
			//o.Normal = UnpackNormal();

            // Metallic and smoothness come from slider variables
         
         
        }
        ENDCG
    }
    FallBack "Diffuse"
}
