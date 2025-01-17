/*
    MIT License

    Contacts:
    Email: andreyvavilichev@gmail.com
    LinkedIn: https://www.linkedin.com/in/vavilichevgd/

    Copyright (c) 2024 Andrey Vavilichev

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

namespace Fitamas.Container
{
    public abstract class DIEntry : IDisposable
    {
        protected DIContainer Container { get; }
        protected bool IsSingleton { get; set; }

        protected DIEntry() { }
        
        protected DIEntry(DIContainer container)
        {
            Container = container;
        }

        public T Resolve<T>()
        {
            return ((DIEntry<T>)this).Resolve();
        }

        public DIEntry AsSingle()
        {
            IsSingleton = true;

            return this;
        }

        public abstract void Dispose();
    }
    
    public class DIEntry<T> : DIEntry
    {
        private Func<DIContainer, T> Factory { get; }
        private T _instance;
        private IDisposable _disposableInstance;
        
        public DIEntry(DIContainer container, Func<DIContainer, T> factory) : base(container)
        {
            Factory = factory;
        }

        public DIEntry(T value)
        {
            _instance = value;

            if (_instance is IDisposable disposableInstance)
            {
                _disposableInstance = disposableInstance;
            }
            
            IsSingleton = true;
        }

        public T Resolve()
        {
            if (IsSingleton)
            {
                if (_instance == null)
                {
                    _instance = Factory(Container);
                    
                    if (_instance is IDisposable disposableInstance)
                    {
                        _disposableInstance = disposableInstance;
                    }
                }

                return _instance;
            }

            return Factory(Container);
        }

        public override void Dispose()
        {
            _disposableInstance?.Dispose();
        }
    }
}