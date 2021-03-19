using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DependencyInjectionSpec
{
    internal class TestServiceCollection : List<ServiceDescriptor>, IServiceCollection
    {
    }
}
