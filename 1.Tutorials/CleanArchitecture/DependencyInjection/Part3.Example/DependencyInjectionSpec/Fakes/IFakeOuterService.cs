using System;
using System.Collections.Generic;
using System.Text;

namespace DependencyInjectionSpec.Fakes
{
    public interface IFakeOuterService
    {
        IFakeService SingleService { get; }

        IEnumerable<IFakeMultipleService> MultipleServices { get; }
    }
}
