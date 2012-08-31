using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Walkies
{
    public interface IHasParent
    {
        object Parent { get; }
    }


    public interface IHasParent<out T> : IHasParent
    {
        new T Parent { get; }
    }
}
