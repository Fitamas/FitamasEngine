using Fitamas.ImGuiNet.Serialization;
using Fitamas.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace Fitamas.ImGuiNet.Readers
{
    public interface IResourceReader
    {
        public bool CanRead(Type objectType);
        object Read(FitamasContentManager manager, string absolutePath, Type objectType);
    }

    public abstract class ResourceReader<T> : IResourceReader
    {
        private Type targetType;

        public ResourceReader()
        {
            targetType = typeof(T);
        }

        public bool CanRead(Type objectType)
        {
            return targetType.IsAssignableFrom(objectType);
        }

        public object Read(FitamasContentManager manager, string absolutePath, Type objectType)
        {
            return Read(manager, absolutePath, objectType, default);
        }

        public abstract T Read(FitamasContentManager manager, string absolutePath, Type objectType, T existingInstance);
    }

    public class MonoObjectReader : ResourceReader<MonoContentObject>
    {
        public override MonoContentObject Read(FitamasContentManager manager, string absolutePath, Type objectType, MonoContentObject existingInstance)
        {
            MonoContentObject result = (MonoContentObject)Activator.CreateInstance(objectType, true);
            result.LoadData(absolutePath);
            return result;
        }
    }

    public class Texture2DReader : ResourceReader<Texture2D>
    {
        public override Texture2D Read(FitamasContentManager manager, string absolutePath, Type objectType, Texture2D existingInstance)
        {
            IGraphicsDeviceService graphicsDeviceService = manager.ServiceProvider.GetService(typeof(IGraphicsDeviceService)) as IGraphicsDeviceService;
            Stream stream = !Path.IsPathRooted(absolutePath) ? TitleContainer.OpenStream(absolutePath) : File.OpenRead(absolutePath);
            if (stream != null)
            {
                using (stream)
                {
                    return Texture2D.FromStream(graphicsDeviceService.GraphicsDevice, stream, DefaultColorProcessors.PremultiplyAlpha);
                }
            }

            return existingInstance;
        }
    }
}
