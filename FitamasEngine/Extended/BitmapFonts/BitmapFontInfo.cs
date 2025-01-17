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

using System.Xml.Serialization;

namespace MonoGame.Extended.Content.Pipeline.BitmapFonts
{
    // ---- AngelCode BmFont XML serializer ----------------------
    // ---- By DeadlyDan @ deadlydan@gmail.com -------------------
    // ---- There's no license restrictions, use as you will. ----
    // ---- Credits to http://www.angelcode.com/ -----------------	
    public class BitmapFontInfo
    {
        [XmlAttribute("face")]
        public string Face { get; set; }

        [XmlAttribute("size")]
        public int Size { get; set; }

        [XmlAttribute("bold")]
        public int Bold { get; set; }

        [XmlAttribute("italic")]
        public int Italic { get; set; }

        [XmlAttribute("charset")]
        public string CharSet { get; set; }

        [XmlAttribute("unicode")]
        public int Unicode { get; set; }

        [XmlAttribute("stretchH")]
        public int StretchHeight { get; set; }

        [XmlAttribute("smooth")]
        public int Smooth { get; set; }

        [XmlAttribute("aa")]
        public int SuperSampling { get; set; }

        [XmlAttribute("padding")]
        public string Padding { get; set; }

        [XmlAttribute("spacing")]
        public string Spacing { get; set; }

        [XmlAttribute("outline")]
        public int OutLine { get; set; }
    }
}