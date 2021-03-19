using DependencyInjectionSpec.Fakes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DependencyInjectionSpec
{
    public class LifetimeScopeSpec
    {
        private ServiceProvider CreateServiceProvider(IServiceCollection services)
        {
            return services.BuildServiceProvider();
        }

        //
        // Transient은 Scope 영역까지 요청별로 개별 인스턴스를 생성하여 의존성을 주입한다.
        //
        [Fact]
        public void TransientService_CanBeResolved_FromScope()
        {
            // Arrange
            var collection = new TestServiceCollection();
            collection.AddTransient(typeof(IFakeService), typeof(FakeService));
            var provider = CreateServiceProvider(collection);

            // Act
            var service1 = provider.GetService<IFakeService>();

            // TODO? : provider.CreateScope()
            using (var scope = provider.CreateScope())
            {
                var scopedService1 = scope.ServiceProvider.GetService<IFakeService>();
                var scopedService2 = scope.ServiceProvider.GetService<IFakeService>();

                // Assert
                Assert.NotSame(service1, scopedService1);
                Assert.NotSame(service1, scopedService2);
                Assert.NotSame(scopedService1, scopedService2);
            }
        }

        //
        // Singleton은 Scope 영역까지 요청과 상관없이 한번만 인스턴스를 생성하여 의존성을 주입한다.
        // Scope의 ServiceProvider은 다르다.
        //
        [Fact]
        public void SingletonService_CanBeResolved_FromScope()
        {
            // Arrange
            var collection = new TestServiceCollection();
            collection.AddSingleton<ClassWithServiceProvider>();
            var provider = CreateServiceProvider(collection);

            // Act
            IServiceProvider scopedSp1 = null;
            IServiceProvider scopedSp2 = null;
            ClassWithServiceProvider instance1 = null;
            ClassWithServiceProvider instance2 = null;

            using (var scope1 = provider.CreateScope())
            {
                scopedSp1 = scope1.ServiceProvider;
                instance1 = scope1.ServiceProvider.GetRequiredService<ClassWithServiceProvider>();
            }

            using (var scope2 = provider.CreateScope())
            {
                scopedSp2 = scope2.ServiceProvider;
                instance2 = scope2.ServiceProvider.GetRequiredService<ClassWithServiceProvider>();
            }

            // Assert
            Assert.Same(instance1.ServiceProvider, instance2.ServiceProvider);
            Assert.NotSame(instance1.ServiceProvider, scopedSp1);   // Scope의 ServiceProvider는 아니다.
            Assert.NotSame(instance2.ServiceProvider, scopedSp2);
        }
    }
}
