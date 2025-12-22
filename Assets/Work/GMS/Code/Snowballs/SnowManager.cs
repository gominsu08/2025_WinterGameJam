using UnityEngine;

namespace Work.GMS.Code.Snowballs
{
    public class SnowManager : MonoBehaviour
    {
        [Header("Terrain")]
        public Terrain terrain;

        [Header("Snow Settings")]
        public float removeRadius = 0.3f;      // 월드 기준 반경
        public float maxDigDepth = 0.002f;      // Heightmap 파임 깊이

        TerrainData data;

        int alphaWidth;
        int alphaHeight;
        int heightRes;

        // 원본 백업 (Play 종료 복원용)
        float[,,] originalAlphamaps;
        float[,] originalHeights;

        [Header("Snow Overlay")]
        public Renderer snowRenderer;
        public int snowMaskResolution = 1024;

        RenderTexture snowMask;
        Material snowMat;

        void Awake()
        {
            // 🔹 TerrainData 복제 (원본 보호)
            TerrainData runtimeData = Instantiate(terrain.terrainData);
            terrain.terrainData = runtimeData;
            data = runtimeData;

            alphaWidth = data.alphamapWidth;
            alphaHeight = data.alphamapHeight;
            heightRes = data.heightmapResolution;

            // 🔹 원본 상태 백업
            originalAlphamaps = data.GetAlphamaps(0, 0, alphaWidth, alphaHeight);
            originalHeights = data.GetHeights(0, 0, heightRes, heightRes);

            snowMask = new RenderTexture(snowMaskResolution, snowMaskResolution, 0, RenderTextureFormat.R8);
            snowMask.wrapMode = TextureWrapMode.Clamp;
            snowMask.filterMode = FilterMode.Bilinear;
            snowMask.Create();

            snowMat = snowRenderer.material;
            snowMat.SetTexture("_SnowMask", snowMask);
            Debug.Log(snowMat.GetTexture("_SnowMask"));
            ClearSnowMask();
        }

        void ClearSnowMask()
        {
            RenderTexture.active = snowMask;
            GL.Clear(true, true, Color.white);
            RenderTexture.active = null;
        }

        void OnDestroy()
        {
            RestoreTerrain();
        }

        void OnDisable()
        {
            RestoreTerrain();
        }

        void RestoreTerrain()
        {
            if (data == null) return;

            data.SetAlphamaps(0, 0, originalAlphamaps);
            data.SetHeights(0, 0, originalHeights);
        }

        // --------------------------------------------------
        // 눈 존재 여부 체크
        // --------------------------------------------------
        public bool HasSnow(RaycastHit hit)
        {
            if (!(hit.collider is TerrainCollider))
                return false;

            if (data.alphamapLayers < 1)
                return false;

            Vector2 alphaCoord = GetAlphaCoord(hit);
            int x = Mathf.Clamp((int)alphaCoord.x, 0, alphaWidth - 1);
            int y = Mathf.Clamp((int)alphaCoord.y, 0, alphaHeight - 1);

            float[,,] map = data.GetAlphamaps(x, y, 1, 1);
            return map[0, 0, 0] > 0.1f; // 0번 = Snow
        }

        // --------------------------------------------------
        // 외부에서 호출하는 메인 함수
        // --------------------------------------------------
        public void RemoveAndDeformSnow(RaycastHit hit, float ballSize)
        {
            if (!(hit.collider is TerrainCollider))
                return;

            if (data.alphamapLayers < 2)
            {
                Debug.LogError("Terrain Layer는 최소 2개(Snow, Ground)가 필요합니다.");
                return;
            }

            RemoveSnowAlphamap(hit, ballSize);
            DeformSnowHeight(hit, ballSize, hit.normal);
            DigSnow(hit.point, (ballSize / 614.4f )* 2.5f);
        }

        // --------------------------------------------------
        // 1️⃣ Alphamap 눈 제거
        // --------------------------------------------------
        void RemoveSnowAlphamap(RaycastHit hit, float ballSize)
        {
            Vector2 center = GetAlphaCoord(hit);
            int cx = (int)center.x;
            int cy = (int)center.y;

            int radius = Mathf.RoundToInt(removeRadius * ballSize * alphaWidth / data.size.x);
            radius = Mathf.Max(1, radius);

            int maxRadius = Mathf.Min(alphaWidth, alphaHeight) / 2;
            radius = Mathf.Min(radius, maxRadius);

            int startX = Mathf.Clamp(cx - radius, 0, alphaWidth - 1);
            int startY = Mathf.Clamp(cy - radius, 0, alphaHeight - 1);
            int endX = Mathf.Clamp(cx + radius, 0, alphaWidth - 1);
            int endY = Mathf.Clamp(cy + radius, 0, alphaHeight - 1);

            int width = endX - startX + 1;
            int height = endY - startY + 1;

            if (width <= 0 || height <= 0)
                return;

            float[,,] map = data.GetAlphamaps(startX, startY, width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float dx = (startX + x) - cx;
                    float dy = (startY + y) - cy;

                    if (dx * dx + dy * dy > radius * radius)
                        continue;

                    map[y, x, 0] = 0f; // Snow 제거
                    map[y, x, 1] = 1f; // Ground
                }
            }

            data.SetAlphamaps(startX, startY, map);
            
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                RenderTexture.active = snowMask;
                GL.Clear(true, true, Color.black);
                RenderTexture.active = null;    
            }
        }

        public void DigSnow(Vector3 worldPos, float radius)
        {
            Debug.Log("DigSnow called");

            Vector3 local = worldPos - terrain.transform.position;

            float u = local.x / terrain.terrainData.size.x;
            float v = local.z / terrain.terrainData.size.z;
            Debug.Log($"UV: {u}, {v}");
            RenderTexture.active = snowMask;

            GL.PushMatrix();
            GL.LoadOrtho();

            Material mat = new Material(Shader.Find("Hidden/Internal-Colored"));
            mat.SetPass(0);

            GL.Begin(GL.TRIANGLES);
            GL.Color(Color.black);

            int segments = 32;
            Vector2 center = new Vector2(u, v);

            for (int i = 0; i < segments; i++)
            {
                float a0 = (i / (float)segments) * Mathf.PI * 2f;
                float a1 = ((i + 1) / (float)segments) * Mathf.PI * 2f;

                Vector2 p0 = center;
                Vector2 p1 = center + new Vector2(Mathf.Cos(a0), Mathf.Sin(a0)) * radius;
                Vector2 p2 = center + new Vector2(Mathf.Cos(a1), Mathf.Sin(a1)) * radius;

                GL.Vertex3(p0.x, p0.y, 0);
                GL.Vertex3(p1.x, p1.y, 0);
                GL.Vertex3(p2.x, p2.y, 0);
            }

            GL.End();
            GL.PopMatrix();
            //snowMat.SetTexture("_SnowMask", snowMask);
            RenderTexture.active = null;
        }

        // --------------------------------------------------
        // 2️⃣ Heightmap 눈 파임
        // --------------------------------------------------
        void DeformSnowHeight(RaycastHit hit, float ballSize, Vector3 normal)
        {
            Vector3 local = hit.point - terrain.transform.position;

            int cx = Mathf.RoundToInt(local.x / data.size.x * heightRes);
            int cy = Mathf.RoundToInt(local.z / data.size.z * heightRes);

            float slope = Mathf.Clamp01(Vector3.Dot(normal, Vector3.up));
            float depth = maxDigDepth * ballSize * slope;

            int radius = Mathf.RoundToInt(removeRadius * ballSize * heightRes / data.size.x);
            radius = Mathf.Max(1, radius);

            int maxRadius = heightRes / 2;
            radius = Mathf.Min(radius, maxRadius);

            int startX = Mathf.Clamp(cx - radius, 0, heightRes - 1);
            int startY = Mathf.Clamp(cy - radius, 0, heightRes - 1);
            int endX = Mathf.Clamp(cx + radius, 0, heightRes - 1);
            int endY = Mathf.Clamp(cy + radius, 0, heightRes - 1);

            int width = endX - startX + 1;
            int height = endY - startY + 1;

            if (width <= 0 || height <= 0)
                return;

            float[,] heights = data.GetHeights(startX, startY, width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float dx = (startX + x) - cx;
                    float dy = (startY + y) - cy;
                    float dist = Mathf.Sqrt(dx * dx + dy * dy);

                    if (dist > radius)
                        continue;

                    float falloff = 1f - (dist / radius);
                    falloff = Mathf.SmoothStep(0f, 1f, falloff);

                    heights[y, x] -= depth * falloff;
                    heights[y, x] = Mathf.Clamp01(heights[y, x]);
                }
            }
           
            data.SetHeights(startX, startY, heights);
        }

        // --------------------------------------------------
        // 공통 유틸 : Terrain Alphamap 좌표 계산
        // --------------------------------------------------
        Vector2 GetAlphaCoord(RaycastHit hit)
        {
            Vector3 local = hit.point - terrain.transform.position;

            float u = Mathf.Clamp01(local.x / data.size.x);
            float v = Mathf.Clamp01(local.z / data.size.z);

            return new Vector2(
                u * alphaWidth,
                v * alphaHeight
            );
        }
    }
}