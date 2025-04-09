Shader "Custom/MouseTransparency"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MouseVector ("Mouse Vector", Vector) = (0,0,0,0)
        _Radius ("Effect Radius", Float) = 0.5
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
        float _Radius;          // 阈值夹角
        float2 centerUV = float2(0.5, 0.5);

        fixed4 frag(v2f i) : SV_Target
        {
            // 计算当前像素的方向向量
            float2 pixelDir = normalize(i.uv - centerUV);

            // 计算鼠标的方向向量
            float2 mouseDir = normalize(_MouseVector.xy);

            float dotProduct = dot(pixelDir, mouseDir);
            float cosThreshold = cos(_Radius * 3.1415926 / 180.0); // 将角度转换为弧度并计算 cos 值

            // 判断点积是否大于等于余弦阈值
            float alpha = dotProduct >= cosThreshold ? 1.0 : 0.0;

            // 采样颜色并应用透明度
            fixed4 col = tex2D(_MainTex, i.uv);
            col.a *= alpha;

            return col;
        }

            ENDCG
        }
    }
}
