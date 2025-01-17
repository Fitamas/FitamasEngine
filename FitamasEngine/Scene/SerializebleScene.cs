using Fitamas.DebugTools;
using Fitamas.Entities;
using Fitamas.Serializeble;
using System.Collections.Generic;

namespace Fitamas.Scene
{
    public class SceneProperties
    {

    }

    [AssetMenu(fileName: "NewScene.scene", title: "Scene")]
    public class SerializebleScene : MonoObject
    {
        public List<GameObject> GameObjects;
        public SceneProperties SceneProperties;

        public SerializebleScene()
        {
            GameObjects = new List<GameObject>();
            SceneProperties = new SceneProperties();
        }
    }
}
