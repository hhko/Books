using System;
using System.Collections.Generic;
using System.Text;

namespace DependencyInjectionSpec.Fakes
{
    public class ClassInheritingAbstractClass : AbstractClass
    {

    }

    public class ClassAlsoInheritingAbstractClass : AbstractClass
    {

    }

    public class ClassInheritingClassInheritingAbstractClass : ClassInheritingAbstractClass
    {

    }
}
