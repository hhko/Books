using DependencyInjectionSpec.Fakes;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace DependencyInjectionSpec
{
    public class LifetimeDispose
    {
        private ServiceProvider CreateServiceProvider(IServiceCollection services)
        {
            // ServiceProvider BuildServiceProvider();
            // ServiceProvider BuildServiceProvider(bool validateScopes);
            // ServiceProvider BuildServiceProvider(ServiceProviderOptions options);
            //      public class ServiceProviderOptions
            //      {
            //          bool ValidateScopes { get; set; }
            //          bool ValidateOnBuild { get; set; }
            //      }
            return services.BuildServiceProvider();
        }

        [Fact]
        public void DisposingScopeDisposesService()
        {
            // Arrange
            var collection = new TestServiceCollection();
            collection.AddSingleton<IFakeSingletonService, FakeService>();
            collection.AddScoped<IFakeScopedService, FakeService>();
            collection.AddTransient<IFakeService, FakeService>();

            var provider = CreateServiceProvider(collection);
            FakeService disposableService;
            FakeService transient1;
            FakeService transient2;
            FakeService singleton;

            // Act and Assert
            var transient3 = Assert.IsType<FakeService>(provider.GetService<IFakeService>());
            using (var scope = provider.CreateScope())
            {
                disposableService = (FakeService)scope.ServiceProvider.GetService<IFakeScopedService>();
                transient1 = (FakeService)scope.ServiceProvider.GetService<IFakeService>();
                transient2 = (FakeService)scope.ServiceProvider.GetService<IFakeService>();
                singleton = (FakeService)scope.ServiceProvider.GetService<IFakeSingletonService>();

                Assert.False(disposableService.Disposed);
                Assert.False(transient1.Disposed);
                Assert.False(transient2.Disposed);
                Assert.False(singleton.Disposed);
            }

            Assert.True(disposableService.Disposed);
            Assert.True(transient1.Disposed);
            Assert.True(transient2.Disposed);
            Assert.False(singleton.Disposed);

            var disposableProvider = provider as IDisposable;
            if (disposableProvider != null)
            {
                disposableProvider.Dispose();
                Assert.True(singleton.Disposed);
                Assert.True(transient3.Disposed);
            }
        }

        //
        // cachedScopeFactory?
        // Scope은 인스턴스를 Dispose 시킨다.
        //
        [Fact]
        public void ScopedServices_FromCachedScopeFactory_CanBeResolvedAndDisposed()
        {
            // Arrange
            var collection = new TestServiceCollection();
            collection.AddScoped<IFakeScopedService, FakeService>();
            var provider = CreateServiceProvider(collection);
            var cachedScopeFactory = provider.GetService<IServiceScopeFactory>();

            // Act
            for (var i = 0; i < 3; i++)
            {
                FakeService outerScopedService;
                using (var outerScope = cachedScopeFactory.CreateScope())
                {
                    FakeService innerScopedService;
                    using (var innerScope = outerScope.ServiceProvider.CreateScope())
                    {
                        outerScopedService = outerScope.ServiceProvider.GetService<IFakeScopedService>() as FakeService;
                        innerScopedService = innerScope.ServiceProvider.GetService<IFakeScopedService>() as FakeService;

                        // Assert
                        Assert.NotNull(outerScopedService);
                        Assert.NotNull(innerScopedService);
                        Assert.NotSame(outerScopedService, innerScopedService);
                    }

                    Assert.False(outerScopedService.Disposed);
                    Assert.True(innerScopedService.Disposed);
                }

                Assert.True(outerScopedService.Disposed);
            }
        }
    }
}
