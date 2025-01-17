using Fitamas.Serializeble;
using System.Collections.Generic;

namespace Fitamas.Animation
{
    public class AnimationTree : MonoObject
    {
        [SerializableField] private Dictionary<string, AnimationClip> animations = new Dictionary<string, AnimationClip>();
        [SerializableField] private List<IPlayable> playables = new List<IPlayable>();
        [SerializableField] private List<ConnectionPlayable> connections = new List<ConnectionPlayable>();
        public AnimationTree(string id, AnimationClip[] animations)
        {
            foreach (AnimationClip clip in animations)
            {
                this.animations[clip.Name] = clip;
            }
        }

        public AnimationTree()
        {

        }

        public void AddPlayable(IPlayable playable)
        {
            playables.Add(playable);
        }

        public void AddConnection(int A, int B)
        {
            connections.Add(new ConnectionPlayable(A, B));
        }

        public AnimationNode[] CreateNodes()
        {
            AnimationNode[] nodes = new AnimationNode[playables.Count];
            for (int i = 0; i < playables.Count; i++)
            {
                nodes[i] = playables[i].Create(this);
            }

            foreach (var connection in connections) 
            {
                nodes[connection.IdA].AddNode(nodes[connection.IdB]);
            }

            return nodes;
        }

        public AnimationClip GetAnimation(string id)
        {
            if (animations.ContainsKey(id))
            {
                return animations[id];
            }
            else
            {
                return null;
            }
        }
    }

    public interface IPlayable
    {
        AnimationNode Create(AnimationTree tree);
    }

    public struct ConnectionPlayable
    {
        public int IdA;
        public int IdB;

        public ConnectionPlayable(int A, int B)
        {
            IdA = A;
            IdB = B;
        }
    }

    public struct PlayableMixer : IPlayable
    {
        public AnimationNode Create(AnimationTree tree)
        {
            throw new System.NotImplementedException();
        }
    }

    public struct PlayableClip : IPlayable
    {
        public float weight;
        public float speed;
        public string animationName;
        public string name;

        public AnimationNode Create(AnimationTree tree)
        {
            if (name == "")
            {
                name = "node";
            }

            AnimationClip clip = tree.GetAnimation(animationName);
            AnimationClipNode clipNode = new AnimationClipNode(name, clip);

            return clipNode;
        }
    }
}
