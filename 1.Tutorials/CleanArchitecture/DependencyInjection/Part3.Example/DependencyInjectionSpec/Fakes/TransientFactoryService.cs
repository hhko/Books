using System;
using System.Collections.Generic;
using System.Text;

namespace DependencyInjectionSpec.Fakes
{
    public class TransientFactoryService : IFactoryService
    {
        public IFakeService FakeService { get; set; }

        public int Value { get; set; }
    }
}
