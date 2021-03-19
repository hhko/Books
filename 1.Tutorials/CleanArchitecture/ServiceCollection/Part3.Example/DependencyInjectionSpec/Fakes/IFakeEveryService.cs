using System;
using System.Collections.Generic;
using System.Text;

namespace DependencyInjectionSpec.Fakes
{
    public interface IFakeEveryService :
        IFakeService
        //IFakeMultipleService,
        //IFakeScopedService,
        //IFakeServiceInstance,
        //IFakeSingletonService,
        //IFakeOpenGenericService<PocoClass>
    {
    }
}
