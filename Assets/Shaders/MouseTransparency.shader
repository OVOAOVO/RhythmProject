Shader "Custom/MouseTransparency"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MouseVector ("Mouse Vector", Vector) = (0,0,0,0)
        _RadiusInRadians ("Effect Radius", Float) = 0.5
    }
    SubShader
    {
        ZTest LEqual
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float4 _MouseVector; 
            float _RadiusInRadians;          // 锥形区域的半角（弧度）

            float2 centerUV = float2(0.5, 0.5); // 以中心点(0.5, 0.5)为参考点

            fixed4 frag(v2f i) : SV_Target
            {
                // 计算当前像素的方向向量，相对于纹理的中心
                float2 pixelDir = normalize(i.uv - centerUV);

                // 计算鼠标的方向向量
                float2 mouseDir = normalize(_MouseVector.xy);

                // 计算 pixelDir 与 mouseDir 的点积
                float dotProduct = dot(mouseDir, pixelDir);

                // 如果点积 >= cos(θ)，说明夹角 ≤ θ，即在扇形范围内
                bool isInsideAngle = dotProduct >= cos(_RadiusInRadians);

                // 如果在范围内，透明度为 1，否则为 0
                float alpha = isInsideAngle ? 1.0 : 0.0;

                // 采样纹理，并将透明度应用于颜色
                fixed4 col = tex2D(_MainTex, i.uv);
                col.a *= alpha;

                return col;
            }

            ENDCG
        }
    }
}
