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

        //
        //
        //
        [Fact]
        public void SelfResolveThenDispose()
        {
            // Arrange
            var collection = new TestServiceCollection();
            var provider = CreateServiceProvider(collection);

            // Act
            var serviceProvider = provider.GetService<IServiceProvider>();

            // Assert
            Assert.NotNull(serviceProvider);
            (provider as IDisposable)?.Dispose();
        }

        [Fact]
        public void SafelyDisposeNestedProviderReferences()
        {
            // Arrange
            var collection = new TestServiceCollection();
            collection.AddTransient<ClassWithNestedReferencesToProvider>();
            var provider = CreateServiceProvider(collection);

            // Act
            var nester = provider.GetService<ClassWithNestedReferencesToProvider>();

            // Assert
            Assert.NotNull(nester);
            nester.Dispose();
        }

        [Fact]
        public void AttemptingToResolveNonexistentService_Returns_Null()
        {
            // Arrange
            var collection = new TestServiceCollection();
            var provider = CreateServiceProvider(collection);

            // Act
            var service = provider.GetService<INonexistentService>();

            // Assert
            Assert.Null(service);
        }

        [Fact]
        public void NonexistentService_CanBeIEnumerableResolved()
        {
            // Arrange
            var collection = new TestServiceCollection();
            var provider = CreateServiceProvider(collection);

            // Act
            var services = provider.GetService<IEnumerable<INonexistentService>>();

            // Assert
            Assert.Empty(services);
        }
    }
}
