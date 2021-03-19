using System;
using System.Collections.Generic;
using System.Text;

namespace DependencyInjectionSpec.Fakes
{
    public class ServiceAcceptingFactoryService
    {
        public ServiceAcceptingFactoryService(
            ScopedFactoryService scopedService,
            IFactoryService transientService)
        {
            ScopedService = scopedService;
            TransientService = transientService;
        }

        public ScopedFactoryService ScopedService { get; private set; }

        public IFactoryService TransientService { get; private set; }
    }
}
