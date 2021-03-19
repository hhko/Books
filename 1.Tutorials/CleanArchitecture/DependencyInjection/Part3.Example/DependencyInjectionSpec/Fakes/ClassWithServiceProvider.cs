using System;
using System.Collections.Generic;
using System.Text;

namespace DependencyInjectionSpec.Fakes
{
    public class ClassWithServiceProvider
    {
        public ClassWithServiceProvider(IServiceProvider serviceProvider)
        {
            // Scope의 ServiceProvider가 아니다.
            ServiceProvider = serviceProvider;
        }

        public IServiceProvider ServiceProvider { get; }
    }
}
