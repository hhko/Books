using DependencyInjectionSpec.Fakes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Xunit;

namespace DependencyInjectionSpec.GenericService
{
    public class ConstrainedGenericService
    {
        private ServiceProvider CreateServiceProvider(IServiceCollection services)
        {
            return services.BuildServiceProvider();
        }

        [Fact]
        public void ConstrainedOpenGenericServices_CanBeResolved()
        {
            // Arrange
            var collection = new TestServiceCollection();
            collection.AddTransient(typeof(IFakeOpenGenericService<>), typeof(FakeOpenGenericService<>));
            collection.AddTransient(typeof(IFakeOpenGenericService<>), typeof(ConstrainedFakeOpenGenericService<>));
            var poco = new PocoClass();
            collection.AddSingleton(poco);
            collection.AddSingleton<IFakeSingletonService, FakeService>();
            var provider = CreateServiceProvider(collection);

            // Act
            var allServices = provider.GetServices<IFakeOpenGenericService<PocoClass>>().ToList();
            var constrainedServices = provider.GetServices<IFakeOpenGenericService<IFakeSingletonService>>().ToList();
            var singletonService = provider.GetService<IFakeSingletonService>();

            // Assert
            Assert.Equal(2, allServices.Count);
            Assert.Same(poco, allServices[0].Value);
            Assert.Same(poco, allServices[1].Value);
            Assert.Single(constrainedServices);
            Assert.Same(singletonService, constrainedServices[0].Value);
        }

        [Fact]
        public void ConstrainedOpenGenericServices_Returns_EmptyWithNoMatches()
        {
            // Arrange
            var collection = new TestServiceCollection();
            collection.AddTransient(typeof(IFakeOpenGenericService<>), typeof(ConstrainedFakeOpenGenericService<>));
            collection.AddSingleton<IFakeSingletonService, FakeService>();
            var provider = CreateServiceProvider(collection);

            // Act
            var constrainedServices = provider.GetServices<IFakeOpenGenericService<IFakeSingletonService>>().ToList();

            // Assert
            Assert.Empty(constrainedServices);
        }

        [Fact]
        public void InterfaceConstrainedOpenGenericServices_CanBeResolved()
        {
            // Arrange
            var collection = new TestServiceCollection();
            collection.AddTransient(typeof(IFakeOpenGenericService<>), typeof(FakeOpenGenericService<>));
            collection.AddTransient(typeof(IFakeOpenGenericService<>), typeof(ClassWithInterfaceConstraint<>));
            var enumerableVal = new ClassImplementingIEnumerable();
            collection.AddSingleton(enumerableVal);
            collection.AddSingleton<IFakeSingletonService, FakeService>();
            var provider = CreateServiceProvider(collection);

            // Act
            var allServices = provider.GetServices<IFakeOpenGenericService<ClassImplementingIEnumerable>>().ToList();
            var constrainedServices = provider.GetServices<IFakeOpenGenericService<IFakeSingletonService>>().ToList();
            var singletonService = provider.GetService<IFakeSingletonService>();

            // Assert
            Assert.Equal(2, allServices.Count);
            Assert.Same(enumerableVal, allServices[0].Value);
            Assert.Same(enumerableVal, allServices[1].Value);
            Assert.Single(constrainedServices);
            Assert.Same(singletonService, constrainedServices[0].Value);
        }

        [Fact]
        public void AbstractClassConstrainedOpenGenericServices_CanBeResolved()
        {
            // Arrange
            var collection = new TestServiceCollection();
            collection.AddTransient(typeof(IFakeOpenGenericService<>), typeof(FakeOpenGenericService<>));
            collection.AddTransient(typeof(IFakeOpenGenericService<>), typeof(ClassWithAbstractClassConstraint<>));
            var poco = new PocoClass();
            collection.AddSingleton(poco);
            var classInheritingClassInheritingAbstractClass = new ClassInheritingClassInheritingAbstractClass();
            collection.AddSingleton(classInheritingClassInheritingAbstractClass);
            var provider = CreateServiceProvider(collection);
            
            // Act
            var allServices = provider.GetServices<IFakeOpenGenericService<ClassInheritingClassInheritingAbstractClass>>().ToList();
            var constrainedServices = provider.GetServices<IFakeOpenGenericService<PocoClass>>().ToList();
            
            // Assert
            Assert.Equal(2, allServices.Count);
            Assert.Same(classInheritingClassInheritingAbstractClass, allServices[0].Value);
            Assert.Same(classInheritingClassInheritingAbstractClass, allServices[1].Value);
            Assert.Single(constrainedServices);
            Assert.Same(poco, constrainedServices[0].Value);
        }

        [Fact]
        public void ClosedServicesPreferredOverOpenGenericServices()
        {
            // Arrange
            var collection = new TestServiceCollection();
            collection.AddTransient(typeof(IFakeOpenGenericService<PocoClass>), typeof(FakeService));
            collection.AddTransient(typeof(IFakeOpenGenericService<>), typeof(FakeOpenGenericService<>));
            collection.AddSingleton<PocoClass>();
            var provider = CreateServiceProvider(collection);

            // Act
            var service = provider.GetService<IFakeOpenGenericService<PocoClass>>();

            // Assert
            Assert.IsType<FakeService>(service);
        }

        
    }
}
