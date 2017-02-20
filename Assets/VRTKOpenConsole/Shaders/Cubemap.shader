// Shader created with Shader Forge v1.34 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.34;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:6213,x:32719,y:32712,varname:node_6213,prsc:2|emission-4860-OUT;n:type:ShaderForge.SFN_Cubemap,id:3243,x:32338,y:32561,ptovrint:False,ptlb:node_3243,ptin:_node_3243,varname:node_3243,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,cube:ec4f2bec38d636a4ca013528d6a8450e,pvfc:0;n:type:ShaderForge.SFN_Fresnel,id:2951,x:31955,y:32879,varname:node_2951,prsc:2|EXP-8084-OUT;n:type:ShaderForge.SFN_Add,id:4860,x:32488,y:32764,varname:node_4860,prsc:2|A-3243-RGB,B-8859-OUT;n:type:ShaderForge.SFN_Slider,id:8084,x:31581,y:32868,ptovrint:False,ptlb:fresnel,ptin:_fresnel,varname:node_8084,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:3.951827,max:10;n:type:ShaderForge.SFN_Append,id:3642,x:32099,y:32680,varname:node_3642,prsc:2|A-2951-OUT,B-2951-OUT;n:type:ShaderForge.SFN_Append,id:8859,x:32280,y:32797,varname:node_8859,prsc:2|A-3642-OUT,B-2951-OUT;proporder:3243-8084;pass:END;sub:END;*/

Shader "Custom/Cubemap" {
    Properties {
        _node_3243 ("node_3243", Cube) = "_Skybox" {}
        _fresnel ("fresnel", Range(0, 10)) = 3.951827
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        LOD 200
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform samplerCUBE _node_3243;
            uniform float _fresnel;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                UNITY_FOG_COORDS(2)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
////// Lighting:
////// Emissive:
                float node_2951 = pow(1.0-max(0,dot(normalDirection, viewDirection)),_fresnel);
                float3 emissive = (texCUBE(_node_3243,viewReflectDirection).rgb+float3(float2(node_2951,node_2951),node_2951));
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
