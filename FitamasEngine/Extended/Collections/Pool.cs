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

namespace Fitamas.Collections
{
    public class Pool<T> where T : class
    {
        private readonly Func<T> _createItem;
        private readonly Action<T> _resetItem;
        private readonly Deque<T> _freeItems;
        private readonly int _maximum;

        public Pool(Func<T> createItem, Action<T> resetItem, int capacity = 16, int maximum = int.MaxValue)
        {
            _createItem = createItem;
            _resetItem = resetItem;
            _maximum = maximum;
            _freeItems = new Deque<T>(capacity);
        }

        public Pool(Func<T> createItem, int capacity = 16, int maximum = int.MaxValue)
            : this(createItem, _ => { }, capacity, maximum)
        {
        }

        public int AvailableCount => _freeItems.Count;

        public T Obtain()
        {
            if (_freeItems.Count > 0)
                return _freeItems.Pop();

            return _createItem();
        }

        public void Free(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            if (_freeItems.Count < _maximum)
                _freeItems.AddToBack(item);

            _resetItem(item);
        }

        public void Clear()
        {
            _freeItems.Clear();
        }
    }
}
