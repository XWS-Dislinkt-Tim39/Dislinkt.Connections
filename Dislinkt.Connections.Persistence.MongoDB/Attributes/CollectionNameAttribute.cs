using System;
using System.Collections.Generic;
using System.Text;

namespace Dislinkt.Connections.Persistence.MongoDB.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CollectionNameAttribute : Attribute
    {
        public CollectionNameAttribute(string name)
        {
            Name = name;
        }
        public string Name { get; }

    }
}
