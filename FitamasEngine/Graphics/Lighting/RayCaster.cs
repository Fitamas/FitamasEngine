using Fitamas.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitamas.Graphics.Lighting
{
    public abstract class RayCaster
    {
        public int renderScale = 2;
        public float baseRayOffset = 0.5f;
        public float baseRayLength = 2.0f;

        //protected static readonly int m_emitterSceneID = Shader.PropertyToID("emitterScene");
        //protected static readonly int m_radianceMapID = Shader.PropertyToID("radianceMap");
        //protected static readonly int m_renderScaleID = Shader.PropertyToID("renderScale");
        //protected static readonly int m_rayOffsetID = Shader.PropertyToID("rayOffset");
        //protected static readonly int m_stepSizeID = Shader.PropertyToID("stepSize");
        //protected static readonly int m_samplesCountID = Shader.PropertyToID("quarterSampleCount");
        //protected static readonly int m_farRadianceMapID = Shader.PropertyToID("farRadianceMap");
        //protected static readonly int m_nearRadianceMapID = Shader.PropertyToID("nearRadianceMap");

        //protected ComputeShader m_creationShader;
        //protected int m_creationKernel;

        //protected ComputeShader m_mergingShader;
        //protected int m_mergingKernel;

        //protected ComputeShader m_finalizationShader;
        //protected int m_finalizationKernel;

        //protected Vector2Int m_LitSceneRes;
        //protected RenderTexture m_litScene;
        //protected Vector3Int m_finalizationThreadGroupCount;

        //protected struct RadianceLayerInfo
        //{
        //    public Vector3Int resolution;
        //    public int probeSpacing;
        //    public float rayOffset;
        //    public float stepSize;
        //    public int quarterSampleCount;

        //    public RadianceLayerInfo(
        //        Vector3Int resolution,
        //        int probeSpacing,
        //        float rayOffset,
        //        float stepSize,
        //        int quarterSampleCount
        //    )
        //    {
        //        this.resolution = resolution;
        //        this.probeSpacing = probeSpacing;
        //        this.rayOffset = rayOffset;
        //        this.stepSize = stepSize;
        //        this.quarterSampleCount = quarterSampleCount;
        //    }

        //    public override string ToString()
        //    {
        //        return $"resolution: {resolution} probeSpacing: {probeSpacing} rayOffset: {rayOffset} stepSize: {stepSize} quarterSampleCount: {quarterSampleCount}";
        //    }
        //};

        private Camera m_camera;


        //protected abstract void InitializeRadianceMaps(RadianceLayerInfo[] layerInfos);

        protected abstract void CreateRadianceCascade();

        protected abstract void MergeRadianceCascade();

        protected abstract void FinalizeRadiance();

        protected abstract string shaderPath
        {
            get;
        }

        //private void Start()
        //{
        //    m_camera = GetComponent<Camera>();
        //    SetupRenderTexture();
        //    LoadShaders();
        //    SetupRadianceMaps();
        //}

        //private void SetupRenderTexture()
        //{

        //    var emitterSceneRes = new Vector2Int(m_camera.pixelWidth, m_camera.pixelHeight);
        //    m_LitSceneRes = emitterSceneRes / renderScale;

        //    m_litScene = new RenderTexture(
        //        m_LitSceneRes.x, m_LitSceneRes.y,
        //        24,
        //        RenderTextureFormat.ARGBFloat,
        //        RenderTextureReadWrite.Linear
        //    );
        //    m_litScene.enableRandomWrite = true;
        //    m_litScene.Create();

        //}

        //private void LoadShaders()
        //{
        //    var basePath = shaderPath;

        //    m_creationShader = Resources.Load<ComputeShader>(basePath + "creation");
        //    m_creationKernel = m_creationShader.FindKernel("CreationKernel");

        //    m_mergingShader = Resources.Load<ComputeShader>(basePath + "merging");
        //    m_mergingKernel = m_mergingShader.FindKernel("MergingKernel");

        //    m_finalizationShader = Resources.Load<ComputeShader>(basePath + "finalization");
        //    m_finalizationKernel = m_finalizationShader.FindKernel("FinalizationKernel");

        //    m_finalizationShader.SetTexture(m_finalizationKernel, "litScene", m_litScene);
        //    m_finalizationThreadGroupCount = CalcWorkingGroupSize(
        //        m_finalizationShader,
        //        m_finalizationKernel,
        //        m_litScene.width,
        //        m_litScene.height,
        //        1
        //    );
        //}

        //protected Vector3Int CalcWorkingGroupSize(ComputeShader shader, int kernelIndex, int workSizeX, int workSizeY, int workSizeZ)
        //{

        //    uint threadGroupsX, threadGroupsY, threadGroupsZ;
        //    shader.GetKernelThreadGroupSizes(
        //        kernelIndex,
        //        out threadGroupsX,
        //        out threadGroupsY,
        //        out threadGroupsZ
        //    );

        //    return new Vector3Int(
        //        (workSizeX + (int)threadGroupsX - 1) / (int)threadGroupsX,
        //        (workSizeY + (int)threadGroupsY - 1) / (int)threadGroupsY,
        //        (workSizeZ + (int)threadGroupsZ - 1) / (int)threadGroupsZ
        //    );
        //}

        //void SetupRadianceMaps()
        //{

        //    var diagonalLength = new Vector2Int(m_litScene.width, m_litScene.height).magnitude;
        //    var layerCount = (int)Math.Ceiling(Math.Log(diagonalLength, 4)) + 1;

        //    var layerInfos = new RadianceLayerInfo[layerCount];

        //    var layerInfo = new RadianceLayerInfo(
        //        new(m_litScene.width, m_litScene.height, 4),
        //        2,
        //        baseRayOffset,
        //        0.0f,
        //        0
        //    );

        //    float rayLength = baseRayLength;

        //    for (int i = 0; i != layerCount; ++i)
        //    {

        //        layerInfo.quarterSampleCount = Math.Min((int)Math.Ceiling(rayLength / 4), 64);
        //        layerInfo.stepSize = rayLength / (layerInfo.quarterSampleCount * 4);

        //        layerInfos[i] = layerInfo;

        //        // Debug.Log($"layer[{i}]: {layerInfo}");

        //        layerInfo.resolution = new(
        //            (layerInfo.resolution.x + 1) / 2,
        //            (layerInfo.resolution.y + 1) / 2,
        //            layerInfo.resolution.z * 4
        //        );
        //        layerInfo.probeSpacing *= 2;
        //        layerInfo.rayOffset += rayLength;

        //        rayLength *= 4;
        //    }

        //    InitializeRadianceMaps(layerInfos);
        //}

        //private void OnRenderImage(RenderTexture src, RenderTexture dst)
        //{

        //    m_creationShader.SetTexture(m_creationKernel, m_emitterSceneID, src);

        //    CreateRadianceCascade();
        //    MergeRadianceCascade();
        //    FinalizeRadiance();

        //    Graphics.Blit(m_litScene, dst);
        //}
    }

    public class LightingEffect
    {
        private readonly Effect _effect;
        private readonly GraphicsDevice _graphicsDevice;

        public Effect Effect => _effect;

        public struct LightData
        {
            public Vector2 Position;
            public Vector3 Color;
            public float Radius;
            public float Intensity;
        }

        public LightingEffect(Effect effect, GraphicsDevice graphicsDevice)
        {
            _effect = effect;
            _graphicsDevice = graphicsDevice;
        }

        public float AmbientIntensity
        {
            get => _effect.Parameters["AmbientIntensity"].GetValueSingle();
            set => _effect.Parameters["AmbientIntensity"].SetValue(value);
        }

        public Color AmbientColor
        {
            get
            {
                Vector3 value = _effect.Parameters["AmbientColor"].GetValueVector3();
                return new Color(value);
            }
            set => _effect.Parameters["AmbientColor"].SetValue(value.ToVector3());
        }

        public void SetLightMask(Texture2D texture)
        {
            _effect.Parameters["LightMask"].SetValue(texture);
        }

        public void SetLights(LightData[] lights, int activeLightsCount)
        {
            if (lights == null || lights.Length == 0)
            {
                _effect.Parameters["ActiveLightsCount"].SetValue(0);
                return;
            }

            // Убедитесь, что массив не превышает MAX_LIGHTS из шейдера
            int count = System.Math.Min(lights.Length, 8);

            // Преобразуем данные для шейдера
            Vector2[] positions = new Vector2[count];
            Vector4[] colors = new Vector4[count];
            float[] radii = new float[count];
            float[] intensities = new float[count];

            for (int i = 0; i < count; i++)
            {
                positions[i] = lights[i].Position;
                colors[i] = new Vector4(lights[i].Color, 1);
                radii[i] = lights[i].Radius;
                intensities[i] = lights[i].Intensity;
            }

            _effect.Parameters["PointLightPosition"].SetValue(positions);
            _effect.Parameters["PointLightColor"].SetValue(colors);
            _effect.Parameters["PointLightRadius"].SetValue(radii);
            _effect.Parameters["PointLightIntensity"].SetValue(intensities);
            _effect.Parameters["ActiveLightsCount"].SetValue(activeLightsCount);

            //Apply();
        }

        public void Apply()
        {
            _effect.CurrentTechnique.Passes[0].Apply();
        }
    }
}
