using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DependencyInjectionSpec.Fakes
{
    public class ClassWithInterfaceConstraint<T> : IFakeOpenGenericService<T>
        where T : IEnumerable
    {
        public ClassWithInterfaceConstraint(T value) => Value = value;

        public T Value { get; }
    }
}
