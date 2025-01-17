/*
    The MIT License (MIT)

    Copyright (c) 2015-2024:
    - Dylan Wilson (https://github.com/dylanwilson80)
    - Lucas Girouard-Stranks (https://github.com/lithiumtoast)
    - Christopher Whitley (https://github.com/aristurtledev)

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
*/

using System.IO;
using Fitamas.Serializeble;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Serialization;

namespace MonoGame.Extended.Content
{
    //public interface IContentLoader<out T>
    //{
    //    T Load(ContentManager contentManager, string path);
    //}

    //public interface IContentLoader
    //{
    //    T Load<T>(ContentManager contentManager, string path);
    //}

    public static class ContentManagerExtensions
    {
        public const string DirectorySeparatorChar = "/";

        public static Stream OpenStream(this ContentManager contentManager, string path)
        {
            return TitleContainer.OpenStream(contentManager.RootDirectory + DirectorySeparatorChar + path);
        }

        public static Stream OpenStream(this ObjectManager objectManager, string path)
        {
            return TitleContainer.OpenStream(objectManager.RootDirectory + DirectorySeparatorChar + path);
        }

        public static GraphicsDevice GetGraphicsDevice(this ContentManager contentManager)
        {
            // http://konaju.com/?p=21
            var serviceProvider = contentManager.ServiceProvider;
            var graphicsDeviceService = (IGraphicsDeviceService) serviceProvider.GetService(typeof(IGraphicsDeviceService));
            return graphicsDeviceService.GraphicsDevice;
        }

        /// <summary>
        /// Loads the content using a custom content loader.
        /// </summary>
        //public static T Load<T>(this ContentManager contentManager, string path, JsonContentLoader contentLoader)
        //{
        //    return contentLoader.Load<T>(contentManager, path);
        //}

        /// <summary>
        /// Loads the content using a custom content loader.
        /// </summary>
        //public static T Load<T>(this ContentManager contentManager, string path, JsonContentLoader<T> contentLoader)
        //{
        //    return contentLoader.Load(contentManager, path);
        //}
    }
}