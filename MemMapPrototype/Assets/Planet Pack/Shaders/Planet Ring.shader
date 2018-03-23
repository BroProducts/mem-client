Shader "Space Builder/PlanetRing"
{

Properties {
	_Color ("Color", Color) = (1,1,1,1)
	_MainTex ("Gradient", 2D) = "white" {}
	_Power ("power",Float) = 1.2 
}

SubShader {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }

		ZWrite On
		Lighting On
		//Cull Off
		LOD 300
		Blend One SrcAlpha 
		//Blend SrcAlpha OneMinusSrcAlpha
		
		
		CGPROGRAM
		#pragma surface surf Lambert addshadow
		sampler2D _MainTex;
		fixed4 _Color;
		float _Power;
		
		struct Input {
			float2 uv_MainTex;
		};
		
		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb * _Power;
			o.Alpha = c.a;
		}
		ENDCG
	}

}
