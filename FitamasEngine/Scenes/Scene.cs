using Fitamas.DebugTools;
using Fitamas.ECS;
using Fitamas.Serialization;
using System.Collections.Generic;

namespace Fitamas.Scenes
{
    [AssetType(fileName: "NewScene", title: "Scene")]
    public class Scene : MonoContentObject
    {
        public static readonly string MainScene = "Scenes\\MainScene";

        public List<EntityData> Entities;
        public SceneProperties Properties;

        public Scene()
        {
            Entities = new List<EntityData>();
            Properties = new SceneProperties();
        }
    }

    public class SceneProperties
    {

    }
}
