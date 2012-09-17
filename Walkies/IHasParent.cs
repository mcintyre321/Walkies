using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Walkies
{
    public interface IHasParent 
    {
        object Parent {get;}
    }

    public class ParentAttribute : Attribute
    {
    }

    
}
