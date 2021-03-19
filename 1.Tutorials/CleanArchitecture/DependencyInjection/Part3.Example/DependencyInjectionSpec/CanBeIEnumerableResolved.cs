using DependencyInjectionSpec.Fakes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DependencyInjectionSpec
{
    public class CanBeIEnumerableResolved
    {
        private ServiceProvider CreateServiceProvider(IServiceCollection services)
        {
            return services.BuildServiceProvider();
        }

        //
        // 복수개 의존성을 등록하여 복수개 의존성을 꺼낸다.
        //
        [Fact]
        public void MultipleServiceCanBeIEnumerableResolved()
        {
            // Arrange
            var collection = new TestServiceCollection();
            collection.AddTransient(typeof(IFakeMultipleService), typeof(FakeOneMultipleService));
            collection.AddTransient(typeof(IFakeMultipleService), typeof(FakeTwoMultipleService));
            var provider = CreateServiceProvider(collection);

            // Act
            var services = provider.GetService<IEnumerable<IFakeMultipleService>>();

            // Assert
            Assert.Collection(services.OrderBy(s => s.GetType().FullName),
                service => Assert.IsType<FakeOneMultipleService>(service),
                service => Assert.IsType<FakeTwoMultipleService>(service));
        }

        //
        // 단/복수 구분 없이 복수개 의존성을 꺼낸다.
        //
        [Fact]
        public void SingleService_CanBeIEnumerableResolved()
        {
            // Arrange
            var collection = new TestServiceCollection();
            collection.AddTransient(typeof(IFakeService), typeof(FakeService));
            var provider = CreateServiceProvider(collection);

            // Act
            var services = provider.GetService<IEnumerable<IFakeService>>();

            // Assert
            Assert.NotNull(services);
            var service = Assert.Single(services);
            Assert.IsType<FakeService>(service);
        }

        //
        // 복수개 의존성 등록 순서를 역순으로 변경한다.
        //
        [Fact]
        public void RegistrationOrder_IsPreserved_WhenServices_AreIEnumerableResolved()
        {
            // Arrange
            var collection = new TestServiceCollection();
            collection.AddTransient(typeof(IFakeMultipleService), typeof(FakeOneMultipleService));
            collection.AddTransient(typeof(IFakeMultipleService), typeof(FakeTwoMultipleService));

            var provider = CreateServiceProvider(collection);

            collection.Reverse();
            var providerReversed = CreateServiceProvider(collection);

            // Act
            var services = provider.GetService<IEnumerable<IFakeMultipleService>>();
            var servicesReversed = providerReversed.GetService<IEnumerable<IFakeMultipleService>>();

            // Assert
            Assert.Collection(services,
                service => Assert.IsType<FakeOneMultipleService>(service),
                service => Assert.IsType<FakeTwoMultipleService>(service));

            Assert.Collection(servicesReversed,
                service => Assert.IsType<FakeTwoMultipleService>(service),
                service => Assert.IsType<FakeOneMultipleService>(service));
        }

        //
        // 마지막으로 등록한 의존성을 꺼낸다.
        //
        [Fact]
        public void LastService_Replaces_PreviousServices()
        {
            // Arrange
            var collection = new TestServiceCollection();
            collection.AddTransient<IFakeMultipleService, FakeOneMultipleService>();
            collection.AddTransient<IFakeMultipleService, FakeTwoMultipleService>();
            var provider = CreateServiceProvider(collection);

            // Act
            var service = provider.GetService<IFakeMultipleService>();

            // Assert
            Assert.IsType<FakeTwoMultipleService>(service);
        }
    }
}
