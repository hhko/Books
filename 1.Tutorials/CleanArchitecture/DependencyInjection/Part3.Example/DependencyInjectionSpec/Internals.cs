using DependencyInjectionSpec.Fakes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Xunit;

namespace DependencyInjectionSpec
{
    public class Internals
    {
        private ServiceProvider CreateServiceProvider(IServiceCollection services)
        {
            return services.BuildServiceProvider();
        }

        //
        // IServiceScopeFactory은 표준 인터페이스이다.
        //
        [Fact]
        public void ServiceProvider_Registers_ServiceScopeFactory()
        {
            // Arrange
            var collection = new TestServiceCollection();
            var provider = CreateServiceProvider(collection);

            // Act
            var scopeFactory = provider.GetService<IServiceScopeFactory>();

            // Assert
            Assert.NotNull(scopeFactory);
        }
    }
}
