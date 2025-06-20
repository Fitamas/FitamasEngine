using Fitamas.DebugTools;
using Fitamas.ECS;
using Fitamas.Serialization;
using System.Collections.Generic;

namespace Fitamas.Scene
{
    [AssetMenu(fileName: "NewScene.scene", title: "Scene")]
    public class Scene : MonoContentObject
    {
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
