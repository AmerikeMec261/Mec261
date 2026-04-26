Shader "Custom/WaterSurface_URP"
{
    Properties
    {
        // ── COLOR & PROFUNDIDAD ─────────────────────────
        _ShallowColor       ("Color agua superficial",  Color)  = (0.1, 0.6, 0.7, 0.6)
        _DeepColor          ("Color agua profunda",     Color)  = (0.02, 0.15, 0.35, 0.9)
        _DepthDistance      ("Distancia de profundidad",Float)  = 2.0

        // ── NORMAL MAP (ondas) ──────────────────────────
        // Usa dos normal maps superpuestos moviéndose en distinta dirección
        _NormalMap          ("Normal Map",              2D)     = "bump" {}
        _NormalMap2         ("Normal Map 2 (capa B)",   2D)     = "bump" {}
        _NormalScale        ("Intensidad normales",     Float)  = 0.6
        _WaveSpeedA         ("Velocidad capa A",        Vector) = (0.03, 0.02, 0, 0)
        _WaveSpeedB         ("Velocidad capa B",        Vector) = (-0.02, 0.03, 0, 0)

        // ── ONDAS GÉOMÉTRICAS (Gerstner) ────────────────
        _WaveAmplitude      ("Amplitud de ondas",       Float)  = 0.15
        _WaveFrequency      ("Frecuencia de ondas",     Float)  = 1.2
        _WaveSpeed          ("Velocidad ondas geom.",   Float)  = 1.5

        // ── REFLEXIÓN ───────────────────────────────────
        _ReflectionTex      ("Reflection Texture",      2D)     = "white" {}
        _ReflectionStrength ("Fuerza de reflexión",     Range(0,1)) = 0.6
        _ReflectionDistort  ("Distorsión reflexión",    Float)  = 0.08

        // ── REFRACCIÓN ──────────────────────────────────
        // Necesita la textura opaca de la cámara (_CameraOpaqueTexture)
        _RefractionStrength ("Fuerza de refracción",    Float)  = 0.05

        // ── ESPUMA ──────────────────────────────────────
        _FoamColor          ("Color espuma",            Color)  = (1,1,1,1)
        _FoamThreshold      ("Umbral espuma",           Range(0,2)) = 0.4
        _FoamSpeed          ("Velocidad espuma",        Float)  = 1.2

        // ── SPECULAR / FRESNEL ──────────────────────────
        _SpecularColor      ("Color especular",         Color)  = (1,1,1,1)
        _Smoothness         ("Smoothness",              Range(0,1)) = 0.92
        _FresnelPower       ("Fresnel power",           Float)  = 4.0
    }

    SubShader
    {
        Tags
        {
            "RenderType"     = "Transparent"
            "Queue"          = "Transparent-10"   // antes que otros transparentes
            "RenderPipeline" = "UniversalPipeline"
        }

        // Grab pass equivalente en URP: usamos _CameraOpaqueTexture
        // (habilitar en URP Asset → Opaque Texture ✓)

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Back

        Pass
        {
            Name "WaterForward"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex   vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareOpaqueTexture.hlsl"

            // ════════════════════════════════════════════
            // PROPIEDADES → CBUFFER
            // ════════════════════════════════════════════
            CBUFFER_START(UnityPerMaterial)
                float4 _ShallowColor;
                float4 _DeepColor;
                float  _DepthDistance;

                float4 _NormalMap_ST;
                float4 _NormalMap2_ST;
                float  _NormalScale;
                float4 _WaveSpeedA;
                float4 _WaveSpeedB;

                float  _WaveAmplitude;
                float  _WaveFrequency;
                float  _WaveSpeed;

                float4 _ReflectionTex_ST;
                float  _ReflectionStrength;
                float  _ReflectionDistort;
                float  _RefractionStrength;

                float4 _FoamColor;
                float  _FoamThreshold;
                float  _FoamSpeed;

                float4 _SpecularColor;
                float  _Smoothness;
                float  _FresnelPower;
            CBUFFER_END

            // Texturas (fuera del CBUFFER)
            TEXTURE2D(_NormalMap);    SAMPLER(sampler_NormalMap);
            TEXTURE2D(_NormalMap2);   SAMPLER(sampler_NormalMap2);
            TEXTURE2D(_ReflectionTex);SAMPLER(sampler_ReflectionTex);

            // ════════════════════════════════════════════
            // ESTRUCTURAS
            // ════════════════════════════════════════════
            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float4 tangentOS  : TANGENT;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS   : SV_POSITION;
                float2 uv            : TEXCOORD0;
                float3 positionWS    : TEXCOORD1;   // world space position
                float3 normalWS      : TEXCOORD2;
                float3 tangentWS     : TEXCOORD3;
                float3 bitangentWS   : TEXCOORD4;
                float4 screenPos     : TEXCOORD5;   // para refracción y profundidad
                float3 viewDirWS     : TEXCOORD6;
                float  fogFactor     : TEXCOORD7;
            };

            // ════════════════════════════════════════════
            // FUNCIONES AUXILIARES
            // ════════════════════════════════════════════

            // Ruido procedural simple (sin textura externa)
            float Hash(float2 p)
            {
                return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453);
            }
            float SmoothNoise(float2 p)
            {
                float2 i = floor(p);
                float2 f = frac(p);
                f = f*f*(3.0-2.0*f);
                return lerp(
                    lerp(Hash(i), Hash(i+float2(1,0)), f.x),
                    lerp(Hash(i+float2(0,1)), Hash(i+float2(1,1)), f.x),
                    f.y);
            }

            // ── ONDA DE GERSTNER ─────────────────────────
            // Simula olas oceánicas con movimiento circular de partículas
            // dir    = dirección de la ola (normalizada)
            // amp    = amplitud (altura)
            // freq   = frecuencia espacial
            // speed  = velocidad de fase
            // t      = tiempo
            // pos    = posición en XZ a desplazar (in/out)
            // retorna el desplazamiento en Y
            float GerstnerWave(float2 dir, float amp, float freq,
                               float speed, float t, inout float3 pos)
            {
                dir = normalize(dir);
                float k     = freq * TWO_PI;           // número de onda
                float phase = k * dot(dir, pos.xz) - speed * t;

                // Desplazamiento horizontal (efecto circular)
                float steepness = 0.5;
                pos.x += steepness * amp * dir.x * cos(phase);
                pos.z += steepness * amp * dir.y * cos(phase);

                // Retorna desplazamiento vertical
                return amp * sin(phase);
            }

            // ════════════════════════════════════════════
            // VERTEX SHADER
            // Desplaza vértices con ondas de Gerstner
            // ════════════════════════════════════════════
            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                float3 posOS = IN.positionOS.xyz;
                float  t     = _Time.y;

                // Sumamos 4 olas de Gerstner en distintas direcciones
                float3 pos = posOS;
                float waveY = 0;
                waveY += GerstnerWave(float2(1.0,  0.5), _WaveAmplitude,       _WaveFrequency,      _WaveSpeed,      t, pos);
                waveY += GerstnerWave(float2(-0.4, 0.9), _WaveAmplitude*0.6,   _WaveFrequency*1.5,  _WaveSpeed*0.8,  t, pos);
                waveY += GerstnerWave(float2(0.3, -0.7), _WaveAmplitude*0.4,   _WaveFrequency*2.2,  _WaveSpeed*1.2,  t, pos);
                waveY += GerstnerWave(float2(0.8,  0.2), _WaveAmplitude*0.25,  _WaveFrequency*3.0,  _WaveSpeed*0.6,  t, pos);

                pos.y += waveY;

                // Transformar al clip space
                OUT.positionHCS  = TransformObjectToHClip(pos);
                OUT.positionWS   = TransformObjectToWorld(pos);
                OUT.uv           = IN.uv;

                // TBN (Tangent-Bitangent-Normal) para normal mapping
                OUT.normalWS    = TransformObjectToWorldNormal(IN.normalOS);
                OUT.tangentWS   = TransformObjectToWorldDir(IN.tangentOS.xyz);
                OUT.bitangentWS = cross(OUT.normalWS, OUT.tangentWS)
                                  * IN.tangentOS.w
                                  * GetOddNegativeScale();

                OUT.screenPos  = ComputeScreenPos(OUT.positionHCS);
                OUT.viewDirWS  = GetWorldSpaceViewDir(OUT.positionWS);

                OUT.fogFactor  = ComputeFogFactor(OUT.positionHCS.z);

                return OUT;
            }

            // ════════════════════════════════════════════
            // FRAGMENT SHADER
            // ════════════════════════════════════════════
            half4 frag(Varyings IN) : SV_Target
            {
                float  t    = _Time.y;
                float2 uv   = IN.uv;

                // ── NORMAL MAP (dos capas animadas) ──────
                // Capa A: se mueve en dirección _WaveSpeedA
                float2 uvA = uv * _NormalMap_ST.xy
                           + _NormalMap_ST.zw
                           + _WaveSpeedA.xy * t;

                // Capa B: se mueve en dirección opuesta (crea interferencia)
                float2 uvB = uv * _NormalMap2_ST.xy
                           + _NormalMap2_ST.zw
                           + _WaveSpeedB.xy * t;

                // Descodificar normal maps (de [0,1] a [-1,1])
                float3 normalA = UnpackNormalScale(
                    SAMPLE_TEXTURE2D(_NormalMap,  sampler_NormalMap,  uvA), _NormalScale);
                float3 normalB = UnpackNormalScale(
                    SAMPLE_TEXTURE2D(_NormalMap2, sampler_NormalMap2, uvB), _NormalScale);

                // Combinar normales (método Whiteout blending)
                float3 blendedNormal = normalize(float3(
                    normalA.xy + normalB.xy,
                    normalA.z  * normalB.z
                ));

                // Transformar normal del tangent space al world space
                float3x3 TBN = float3x3(
                    normalize(IN.tangentWS),
                    normalize(IN.bitangentWS),
                    normalize(IN.normalWS)
                );
                float3 normalWS = normalize(mul(blendedNormal, TBN));

                // ── PROFUNDIDAD → COLOR ──────────────────
                // Leer la profundidad de la escena detrás del agua
                float2 screenUV = IN.screenPos.xy / IN.screenPos.w;

                float sceneDepthRaw = SampleSceneDepth(screenUV);
                float sceneDepth    = LinearEyeDepth(sceneDepthRaw,
                                          _ZBufferParams);
                float surfaceDepth  = IN.screenPos.w;   // profundidad de la superficie

                // Diferencia de profundidad normalizada
                float depthDiff = saturate((sceneDepth - surfaceDepth) / _DepthDistance);

                // Mezclar color superficial y profundo según depthDiff
                float4 waterColor = lerp(_ShallowColor, _DeepColor, depthDiff);

                // ── REFRACCIÓN ───────────────────────────
                // Desplaza las UVs de pantalla usando la normal para
                // simular la distorsión del fondo bajo el agua
                float2 refrOffset = blendedNormal.xy * _RefractionStrength;
                float2 refrUV     = screenUV + refrOffset;

                // Clamp para evitar artefactos en bordes
                refrUV = clamp(refrUV, 0.001, 0.999);

                float3 refraction = SampleSceneColor(refrUV).rgb;

                // ── FRESNEL ──────────────────────────────
                // Cuánto refleja vs. cuánto refracta según el ángulo de visión
                // Vista perpendicular → refracción (ves el fondo)
                // Vista rasante → reflexión (ves el cielo)
                float3 viewDir = normalize(IN.viewDirWS);
                float  NdotV   = saturate(dot(normalWS, viewDir));
                float  fresnel = pow(1.0 - NdotV, _FresnelPower);

                // ── REFLEXIÓN ────────────────────────────
                // Distorsionar UVs de reflexión con la normal
                float2 reflUV = screenUV + blendedNormal.xy * _ReflectionDistort;
                reflUV.y = 1.0 - reflUV.y;  // voltear verticalmente
                float3 reflection = SAMPLE_TEXTURE2D(_ReflectionTex,
                                        sampler_ReflectionTex,
                                        reflUV).rgb
                                  * _ReflectionStrength;

                // ── ESPUMA ───────────────────────────────
                // Aparece donde el agua es muy superficial (depthDiff bajo)
                float foamNoise = SmoothNoise(uv * 8.0 + t * _FoamSpeed);
                float foamNoise2= SmoothNoise(uv * 14.0 - t * _FoamSpeed * 0.7);
                float foamMask  = saturate(
                    (1.0 - depthDiff) / max(_FoamThreshold, 0.001)
                );
                // Animar bordes de espuma con ruido
                float foam = saturate(foamMask - foamNoise  * 0.4
                                               - foamNoise2 * 0.3);
                foam = smoothstep(0.0, 0.5, foam);

                // ── SPECULAR (Blinn-Phong simplificado) ──
                Light mainLight  = GetMainLight();
                float3 halfVec   = normalize(viewDir + mainLight.direction);
                float  NdotH     = saturate(dot(normalWS, halfVec));
                float  specPower = exp2(_Smoothness * 10.0 + 1.0);
                float  spec      = pow(NdotH, specPower) * _Smoothness;
                float3 specular  = spec * _SpecularColor.rgb * mainLight.color;

                // ── COMBINAR TODO ────────────────────────
                // Base: mezcla refracción (fondo distorsionado) con color del agua
                float3 finalColor = lerp(refraction, waterColor.rgb, waterColor.a * 0.6);

                // Añadir reflexión modulada por Fresnel
                finalColor = lerp(finalColor, reflection, fresnel * _ReflectionStrength);

                // Espuma encima
                finalColor = lerp(finalColor, _FoamColor.rgb, foam * _FoamColor.a);

                // Especular encima de todo
                finalColor += specular;

                // Alpha: más opaco en partes profundas, más transparente en superficie
                float alpha = lerp(_ShallowColor.a, _DeepColor.a, depthDiff);
                alpha = saturate(alpha + foam * 0.5 + spec * 0.3);

                // Aplicar niebla
                finalColor = MixFog(finalColor, IN.fogFactor);

                return half4(finalColor, alpha);
            }

            ENDHLSL
        }
    }

    // Fallback si URP no está disponible
    FallBack "Universal Render Pipeline/Lit"
}
