using System;
using System.Collections.Generic;
using System.Linq;

namespace Fitamas.ECS
{
    public class TagsComponent : Component
    {
        public HashSet<string> Tags;

        public TagsComponent()
        {
            Tags = new HashSet<string>();
        }

        public TagsComponent(IEnumerable<string> tags)
        {
            Tags = tags.ToHashSet();
        }

        public bool Has(string tag)
        {
            return Tags.Contains(tag);
        }

        public bool HasAny(params string[] tags)
        {
            return tags.Any(Tags.Contains);
        }
    }
}
