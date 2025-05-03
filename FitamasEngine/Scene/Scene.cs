using Fitamas.DebugTools;
using Fitamas.Entities;
using Fitamas.Serialization;
using System.Collections.Generic;

namespace Fitamas.Scene
{
    public class SceneProperties
    {

    }

    [AssetMenu(fileName: "NewScene.scene", title: "Scene")]
    public class Scene : MonoObject
    {
        public List<GameObject> GameObjects;
        public SceneProperties SceneProperties;

        public Scene()
        {
            GameObjects = new List<GameObject>();
            SceneProperties = new SceneProperties();
        }
    }
}
