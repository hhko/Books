using System;
using System.Collections.Generic;
using System.Text;

namespace DependencyInjectionSpec.Fakes
{
    public class ClassWithAbstractClassConstraint<T> : IFakeOpenGenericService<T>
        where T : AbstractClass
    {
        public ClassWithAbstractClassConstraint(T value) => Value = value;

        public T Value { get; }
    }
}
