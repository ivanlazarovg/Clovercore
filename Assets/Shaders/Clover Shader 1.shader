// Made with Amplify Shader Editor v1.9.9.8
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Clover Shader 1"
{
	Properties
	{
		_Height( "Height", Float ) = 1.74
		_CenterPosition( "CenterPosition", Vector ) = ( 0, 0, 0, 0 )
		_MainColor( "MainColor", Color ) = ( 0.1606444, 0.3584906, 0.1910484, 0 )
		_SecondColor( "SecondColor", Color ) = ( 0.2731598, 0.6037736, 0.2135992, 0 )
		_GrowWarp( "GrowWarp", Range( -1, 1 ) ) = 0
		_ColorBandLength( "ColorBandLength", Range( 0, 2 ) ) = 0.4214205
		[HDR] _GrowGlowColor( "GrowGlowColor", Color ) = ( 0, 0, 0, 0 )
		_GrowWarpPower( "GrowWarpPower", Float ) = 1
		_ColorWarp( "ColorWarp", Float ) = 3.79
		_WindStrength( "WindStrength", Float ) = 0
		_WindScale( "WindScale", Float ) = 4
		_WindSpeed( "Wind Speed", Float ) = 2
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.5
		#define ASE_VERSION 19908
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
		};

		uniform float _WindStrength;
		uniform float _WindSpeed;
		uniform float _WindScale;
		uniform float _Height;
		uniform float3 _CenterPosition;
		uniform float _GrowWarpPower;
		uniform float _GrowWarp;
		uniform float4 _MainColor;
		uniform float4 _SecondColor;
		uniform float4 _GrowGlowColor;
		uniform float _ColorBandLength;
		uniform float _ColorWarp;


		//https://www.shadertoy.com/view/XdXGW8
		float2 GradientNoiseDir( float2 x )
		{
			const float2 k = float2( 0.3183099, 0.3678794 );
			x = x * k + k.yx;
			return -1.0 + 2.0 * frac( 16.0 * k * frac( x.x * x.y * ( x.x + x.y ) ) );
		}
		
		float GradientNoise( float2 UV, float Scale )
		{
			float2 p = UV * Scale;
			float2 i = floor( p );
			float2 f = frac( p );
			float2 u = f * f * ( 3.0 - 2.0 * f );
			return lerp( lerp( dot( GradientNoiseDir( i + float2( 0.0, 0.0 ) ), f - float2( 0.0, 0.0 ) ),
					dot( GradientNoiseDir( i + float2( 1.0, 0.0 ) ), f - float2( 1.0, 0.0 ) ), u.x ),
					lerp( dot( GradientNoiseDir( i + float2( 0.0, 1.0 ) ), f - float2( 0.0, 1.0 ) ),
					dot( GradientNoiseDir( i + float2( 1.0, 1.0 ) ), f - float2( 1.0, 1.0 ) ), u.x ), u.y );
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float mulTime45 = _Time.y * _WindSpeed;
			float2 temp_cast_0 = (mulTime45).xx;
			float2 uv_TexCoord43 = v.texcoord.xy + temp_cast_0;
			float gradientNoise42 = GradientNoise(uv_TexCoord43,_WindScale);
			gradientNoise42 = gradientNoise42*0.5 + 0.5;
			float saferPower57 = abs( v.texcoord.xy.y );
			float WindDisplacement49 = ( _WindStrength * ( saturate(  (0.0 + ( gradientNoise42 - 0.55 ) * ( 1.0 - 0.0 ) / ( 1.0 - 0.55 ) ) ) * pow( saferPower57 , 2.0 ) ) );
			float3 ase_positionWS = mul( unity_ObjectToWorld, v.vertex );
			float DistanceGrowth51 = saturate( ( ( _Height - distance( _CenterPosition , ase_positionWS ) ) - v.texcoord.xy.y ) );
			float3 ase_normalOS = v.normal.xyz;
			float3 saferPower33 = abs( ( ( 1.0 - DistanceGrowth51 ) * ase_normalOS ) );
			float3 temp_cast_1 = (_GrowWarpPower).xxx;
			v.vertex.xyz += ( WindDisplacement49 + ( pow( saferPower33 , temp_cast_1 ) * _GrowWarp ) );
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 transform40 = mul(unity_ObjectToWorld,float4( 0,0,0,1 ));
			float4 lerpResult28 = lerp( _MainColor , _SecondColor , abs( sin( ( transform40.x * 100.0 ) ) ));
			o.Albedo = lerpResult28.rgb;
			float3 ase_positionWS = i.worldPos;
			float DistanceGrowth51 = saturate( ( ( _Height - distance( _CenterPosition , ase_positionWS ) ) - i.uv_texcoord.y ) );
			float saferPower38 = abs( ( ( 1.0 - DistanceGrowth51 ) - ( 1.0 - _ColorBandLength ) ) );
			o.Emission = ( _GrowGlowColor * saturate( pow( saferPower38 , _ColorWarp ) ) ).rgb;
			o.Alpha = DistanceGrowth51;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.5
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback Off
	CustomEditor "AmplifyShaderEditor.MaterialInspector"
}
/*ASEBEGIN
Version=19908
Node;AmplifyShaderEditor.CommentaryNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;54;-2066,78;Inherit;False;1576.833;811;Comment;9;5;4;6;3;1;7;9;10;51;Distance Growth;0.7924528,0.5270559,0.5270559,1;0;0
Node;AmplifyShaderEditor.CommentaryNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;62;-946,1550;Inherit;False;2564;560;Comment;13;56;46;47;48;49;41;57;60;42;61;45;43;58;WindDisplacement;0.1699448,0.6320754,0.2635038,1;0;0
Node;AmplifyShaderEditor.Vector3Node, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;5;-2000,496;Inherit;False;Property;_CenterPosition;CenterPosition;1;0;Create;True;0;0;0;False;0;False;0,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldPosInputsNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;4;-2016,704;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DistanceOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;6;-1712,640;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;3;-1904,352;Inherit;False;Property;_Height;Height;0;0;Create;True;0;0;0;False;0;False;1.74;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;58;-896,1632;Inherit;False;Property;_WindSpeed;Wind Speed;12;0;Create;True;0;0;0;False;0;False;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;1;-1872,128;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;7;-1600,352;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;45;-640,1648;Inherit;False;1;0;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;9;-1360,208;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;56;-272,1808;Inherit;False;Property;_WindScale;WindScale;11;0;Create;True;0;0;0;False;0;False;4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;43;-384,1616;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;10;-1088,208;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;42;0,1680;Inherit;False;Gradient;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;51;-731.1665,220.681;Inherit;False;DistanceGrowth;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;41;144,1888;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;60;224,1648;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0.55;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;64;574,782;Inherit;False;1172;395;Comment;8;11;12;34;14;33;13;63;66;GrowthDisplacement;0.9622642,0.8533795,0.4039694,1;0;0
Node;AmplifyShaderEditor.CommentaryNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;65;141.2808,-786;Inherit;False;1284.719;617.0669;Comment;7;40;30;26;8;24;29;28;Color Randomization;0.04503382,0.4205567,0.8679245,1;0;0
Node;AmplifyShaderEditor.PowerNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;57;384,1856;Inherit;True;True;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;61;432,1712;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;17;400,416;Inherit;False;Property;_ColorBandLength;ColorBandLength;6;0;Create;True;0;0;0;False;0;False;0.4214205;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;52;480,224;Inherit;False;51;DistanceGrowth;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;63;560,864;Inherit;False;51;DistanceGrowth;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;46;688,1728;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;48;784,1600;Inherit;False;Property;_WindStrength;WindStrength;10;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;18;832,400;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;15;736,224;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;11;656,992;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ObjectToWorldTransfNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;40;191.2808,-377.9331;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;66;800,864;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;16;1024,256;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0.66;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;39;1178.891,491.262;Inherit;False;Property;_ColorWarp;ColorWarp;9;0;Create;True;0;0;0;False;0;False;3.79;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;47;1056,1664;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;12;992,864;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;34;992,1008;Inherit;False;Property;_GrowWarpPower;GrowWarpPower;8;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;30;496,-368;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;100;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;49;1344,1664;Inherit;False;WindDisplacement;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;38;1360,256;Inherit;False;True;2;0;FLOAT;0;False;1;FLOAT;0.43;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;14;1440,1024;Inherit;False;Property;_GrowWarp;GrowWarp;5;0;Create;True;0;0;0;False;0;False;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;33;1248,864;Inherit;False;True;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SinOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;26;672,-384;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;19;1376,0;Inherit;False;Property;_GrowGlowColor;GrowGlowColor;7;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;True;0;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.SaturateNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;21;1632,272;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;50;1472,448;Inherit;False;49;WindDisplacement;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;8;848,-736;Inherit;False;Property;_MainColor;MainColor;2;0;Create;True;0;0;0;False;0;False;0.1606444,0.3584906,0.1910484,0;0,0,0,0;True;True;0;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.AbsOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;24;912,-368;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;29;848,-624;Inherit;False;Property;_SecondColor;SecondColor;3;0;Create;True;0;0;0;False;0;False;0.2731598,0.6037736,0.2135992,0;0,0,0,0;True;True;0;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.SimpleMultiplyOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;13;1552,848;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;20;1728,80;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;53;1808,208;Inherit;False;51;DistanceGrowth;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;55;1824,464;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;28;1248,-512;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;0;2016,48;Float;False;True;-1;3;AmplifyShaderEditor.MaterialInspector;0;0;Standard;Clover Shader 1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;0;False;;0;Custom;0.5;True;True;0;True;Transparent;;Geometry;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;4;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;17;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;16;FLOAT4;0,0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;6;0;5;0
WireConnection;6;1;4;0
WireConnection;7;0;3;0
WireConnection;7;1;6;0
WireConnection;45;0;58;0
WireConnection;9;0;7;0
WireConnection;9;1;1;2
WireConnection;43;1;45;0
WireConnection;10;0;9;0
WireConnection;42;0;43;0
WireConnection;42;1;56;0
WireConnection;51;0;10;0
WireConnection;60;0;42;0
WireConnection;57;0;41;2
WireConnection;61;0;60;0
WireConnection;46;0;61;0
WireConnection;46;1;57;0
WireConnection;18;0;17;0
WireConnection;15;0;52;0
WireConnection;66;0;63;0
WireConnection;16;0;15;0
WireConnection;16;1;18;0
WireConnection;47;0;48;0
WireConnection;47;1;46;0
WireConnection;12;0;66;0
WireConnection;12;1;11;0
WireConnection;30;0;40;1
WireConnection;49;0;47;0
WireConnection;38;0;16;0
WireConnection;38;1;39;0
WireConnection;33;0;12;0
WireConnection;33;1;34;0
WireConnection;26;0;30;0
WireConnection;21;0;38;0
WireConnection;24;0;26;0
WireConnection;13;0;33;0
WireConnection;13;1;14;0
WireConnection;20;0;19;0
WireConnection;20;1;21;0
WireConnection;55;0;50;0
WireConnection;55;1;13;0
WireConnection;28;0;8;0
WireConnection;28;1;29;0
WireConnection;28;2;24;0
WireConnection;0;0;28;0
WireConnection;0;2;20;0
WireConnection;0;9;53;0
WireConnection;0;11;55;0
ASEEND*/
//CHKSM=A3AE008A2EE5CA104793553B12B8C9B97F2C958C