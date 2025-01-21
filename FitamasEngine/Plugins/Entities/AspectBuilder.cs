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
using Fitamas.Collections;

namespace Fitamas.Entities
{
    public class AspectBuilder
    {
        public Bag<Type> AllTypes { get; }
        public Bag<Type> ExclusionTypes { get; }
        public Bag<Type> OneTypes { get; }

        public AspectBuilder()
        {
            AllTypes = new Bag<Type>();
            ExclusionTypes = new Bag<Type>();
            OneTypes = new Bag<Type>();
        }

        public AspectBuilder All(params Type[] types)
        {
            foreach (var type in types)
                AllTypes.Add(type);

            return this;
        }

        public AspectBuilder One(params Type[] types)
        {
            foreach (var type in types)
                OneTypes.Add(type);

            return this;
        }

        public AspectBuilder Exclude(params Type[] types)
        {
            foreach (var type in types)
                ExclusionTypes.Add(type);

            return this;
        }

        public Aspect Build(ComponentManager componentManager)
        {
            var aspect = new Aspect();
            Associate(componentManager, AllTypes, ref aspect.AllSet);
            Associate(componentManager, OneTypes, ref aspect.OneSet);
            Associate(componentManager, ExclusionTypes, ref aspect.ExclusionSet);
            return aspect;
        }

        // ReSharper disable once ParameterTypeCanBeEnumerable.Local
        private static void Associate(ComponentManager componentManager, Bag<Type> types, ref BitVector32 bits)
        {
            foreach (var type in types)
            {
                var id = componentManager.GetComponentTypeId(type);
                bits[1 << id] = true;
            }
        }
    }
}
