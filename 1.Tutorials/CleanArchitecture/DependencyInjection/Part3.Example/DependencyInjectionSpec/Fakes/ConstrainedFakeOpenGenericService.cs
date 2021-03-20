using System;
using System.Collections.Generic;
using System.Text;

namespace DependencyInjectionSpec.Fakes
{
    public class ConstrainedFakeOpenGenericService<TVal> : IFakeOpenGenericService<TVal>
        where TVal : PocoClass
    {
        public ConstrainedFakeOpenGenericService(TVal value)
        {
            Value = value;
        }
        public TVal Value { get; }
    }
}
