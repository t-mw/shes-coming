Shader "Unlit/Mask"
{
	Properties{
		[IntRange] _Masked("Mask", Range(0,255)) = 1 
	}
		//IMPORTANT! This does onl work with forward rendering path!!
		SubShader{
			Tags{ "RenderType" = "Transparent" "Queue" = "Geometry-5"}

			//Blend SrcAlpha OneMinusSrcAlpha
			ZWrite off
			ZTest always

			Pass{

				Stencil {
					Ref [_Masked]
					Comp always
					Pass replace
				}

			}
	}
}