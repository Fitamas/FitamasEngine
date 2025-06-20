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

using Fitamas.Collections;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.ECS
{
    public class WorldBuilder
    {
        private Bag<IExecutor> executors = new Bag<IExecutor>();
        private Bag<ISystem> systems = new Bag<ISystem>();
        private Game game;

        public WorldBuilder(Game game)
        {
            executors = new Bag<IExecutor>();
            systems = new Bag<ISystem>();
            this.game = game;         
        }

        public WorldBuilder AddSystem(ISystem system)
        {
            systems.Add(system);
            return this;
        }

        public WorldBuilder AddExecutor(IExecutor executor)
        {
            executors.Add(executor);
            return this;
        }

        public GameWorld Build()
        {
            var world = new GameWorld(executors);

            foreach (var system in systems)
                world.RegisterSystem(system);

            return world;
        }
    }
}
