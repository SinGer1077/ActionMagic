Shader "Unlit/TestShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float HexDist(float2 st) {
                st = abs(st);
                float c = dot(st, normalize(float2(1., 1.73)));
                c = max(c, st.x);
                return c;
            }

            float2 mod(float x, float y) {
                return x - y * floor(x / y);
            }

            float4 HexCoords(float2 st) {
                float2 r = float2(1, 1.73);
                float2 h = r * 0.5;
                float2 a = mod(st, r) - h;
                float2 b = mod(st - h, r) - h;
                float2 gv;
                if (length(a) < length(b)) {
                    gv = a;
                }
                else {
                    gv = b;
                }
                float x = atan2(gv.x, gv.y);
                float y = 0.5 - HexDist(gv);
                float2 id = st - gv;

                return float4(x, y, id.x, id.y);
            }

            float4 mainImage(v2f i) {

                //float2 st = (i.uv.xy * 2.0 - u_resolution.xy) / u_resolution.y;
                float2 st = (i.uv - 0.5) * 2.0;
                float2 st0 = st;
                float3 finalColor = float3(0.4118, 0.1451, 0.1451);
                float3 background = float3(1.0, 1.0, 1.0);
                //finalColor *= length(sin(_Time.y))*float3(0.9451, 0.0471, 0.0471);
                st *= 5.;

                float4 hc = HexCoords(st);
                hc.y *= 0.5;
                //float c = smoothstep(.05, .1, hc.y * sin(hc.z * hc.w + _Time.y));
                float c = smoothstep(0.0, .01, hc.y);
                if (hc.y * sin(hc.z * hc.w) > 0.01)
                    //if ( hc.y * sin(hc.z * hc.w + _Time.y) > 0.01)   
                    c -= 0.97;
                else
                    c -= .94;

                if (c > -0.0)
                    finalColor = float3(0.0588, 0.0706, 0.6941);
                else {
                    float distance = length(st) * exp(-length(st));
                    distance = sin(distance * 8.0 + _Time.y * 4.0) / 50.0;
                    distance = pow(0.01 / distance, 2.0);
                    finalColor *= distance * float3(0.8706, 0.0471, 0.9451);
                }

                finalColor += c;


                float4 col = float4(finalColor, 1.0);
                return col;
            }

            float4 frag(v2f i) : SV_Target
            {
                // sample the texture
                //fixed4 col = tex2D(_MainTex, i.uv);
                float4 col = mainImage(i);
                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
