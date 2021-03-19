using System;
using System.Collections.Generic;
using System.Text;

namespace DependencyInjectionSpec.Fakes
{
    public interface IFactoryService
    {
        IFakeService FakeService { get; }

        int Value { get; }
    }
}
