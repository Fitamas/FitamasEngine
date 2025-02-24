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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitamas.Entities
{
    public class Aspect
    {
        internal Aspect()
        {
            AllSet = new BitVector32();
            ExclusionSet = new BitVector32();
            OneSet = new BitVector32();
        }

        public BitVector32 AllSet;
        public BitVector32 ExclusionSet;
        public BitVector32 OneSet;

        public static AspectBuilder All(params Type[] types)
        {
            return new AspectBuilder().All(types);
        }

        public static AspectBuilder One(params Type[] types)
        {
            return new AspectBuilder().One(types);
        }

        public static AspectBuilder Exclude(params Type[] types)
        {
            return new AspectBuilder().Exclude(types);
        }

        public bool IsInterested(BitVector32 componentBits)
        {
            if (AllSet.Data != 0 && (componentBits.Data & AllSet.Data) != AllSet.Data)
                return false;

            if (ExclusionSet.Data != 0 && (componentBits.Data & ExclusionSet.Data) != 0)
                return false;

            if (OneSet.Data != 0 && (componentBits.Data & OneSet.Data) == 0)
                return false;

            return true;
        }
    }
}
