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

using System;
using System.IO;
using System.Reflection;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Extended.Content
{
    public static class ContentReaderExtensions
    {
        private static readonly FieldInfo _contentReaderGraphicsDeviceFieldInfo = typeof(ContentReader).GetTypeInfo().GetDeclaredField("graphicsDevice");

        public static GraphicsDevice GetGraphicsDevice(this ContentReader contentReader)
        {
            return (GraphicsDevice)_contentReaderGraphicsDeviceFieldInfo.GetValue(contentReader);
        }

        public static string RemoveExtension(string path)
        {
            return Path.ChangeExtension(path, null).TrimEnd('.');
        }

        public static string GetRelativeAssetName(this ContentReader contentReader, string relativeName)
        {
            var assetDirectory = Path.GetDirectoryName(contentReader.AssetName);
            var assetName = RemoveExtension(Path.Combine(assetDirectory, relativeName).Replace('\\', '/'));

            return ShortenRelativePath(assetName);
        }

        public static string ShortenRelativePath(string relativePath)
        {
            var ellipseIndex = relativePath.IndexOf("/../", StringComparison.Ordinal);
            while (ellipseIndex != -1)
            {
                var lastDirectoryIndex = relativePath.LastIndexOf('/', ellipseIndex - 1) + 1;
                relativePath = relativePath.Remove(lastDirectoryIndex, ellipseIndex + 4 - lastDirectoryIndex);
                ellipseIndex = relativePath.IndexOf("/../", StringComparison.Ordinal);
            }

            return relativePath;
        }
    }
}