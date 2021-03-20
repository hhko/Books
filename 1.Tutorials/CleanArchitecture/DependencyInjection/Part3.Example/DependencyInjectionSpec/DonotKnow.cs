using DependencyInjectionSpec.Fakes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Xunit;

namespace DependencyInjectionSpec
{
    public class DonotKnow
    {
        private ServiceProvider CreateServiceProvider(IServiceCollection services)
        {
            return services.BuildServiceProvider();
        }

        public static TheoryData ServiceContainerPicksConstructorWithLongestMatchesData
        {
            get
            {
                var fakeService = new FakeService();
                var multipleService = new FakeService();
                var factoryService = new TransientFactoryService();
                var scopedService = new FakeService();

                return new TheoryData<IServiceCollection, TypeWithSupersetConstructors>
                {
                    {
                        new TestServiceCollection()
                            .AddSingleton<IFakeService>(fakeService),
                        new TypeWithSupersetConstructors(fakeService)
                    },
                    {
                        new TestServiceCollection()
                            .AddSingleton<IFactoryService>(factoryService),
                        new TypeWithSupersetConstructors(factoryService)
                    },
                    {
                        new TestServiceCollection()
                            .AddSingleton<IFakeService>(fakeService)
                            .AddSingleton<IFactoryService>(factoryService),
                       new TypeWithSupersetConstructors(fakeService, factoryService)
                    },
                    {
                        new TestServiceCollection()
                            .AddSingleton<IFakeService>(fakeService)
                            .AddSingleton<IFakeMultipleService>(multipleService)
                            .AddSingleton<IFactoryService>(factoryService),
                       new TypeWithSupersetConstructors(fakeService, multipleService, factoryService)
                    },
                    {
                        new TestServiceCollection()
                            .AddSingleton<IFakeService>(fakeService)
                            .AddSingleton<IFakeMultipleService>(multipleService)
                            .AddSingleton<IFakeScopedService>(scopedService)
                            .AddSingleton<IFactoryService>(factoryService),
                       new TypeWithSupersetConstructors(multipleService, factoryService, fakeService, scopedService)
                    }
                };
            }
        }

        [Theory]
        [MemberData(nameof(ServiceContainerPicksConstructorWithLongestMatchesData))]
        public void ServiceContainerPicksConstructorWithLongestMatches(
            IServiceCollection serviceCollection,
            TypeWithSupersetConstructors expected)
        {
            // Arrange
            serviceCollection.AddTransient<TypeWithSupersetConstructors>();
            var serviceProvider = CreateServiceProvider(serviceCollection);

            // Act
            var actual = serviceProvider.GetService<TypeWithSupersetConstructors>();

            // Assert
            Assert.NotNull(actual);
            Assert.Same(expected.Service, actual.Service);
            Assert.Same(expected.FactoryService, actual.FactoryService);
            Assert.Same(expected.MultipleService, actual.MultipleService);
            Assert.Same(expected.ScopedService, actual.ScopedService);
        }
    }
}
