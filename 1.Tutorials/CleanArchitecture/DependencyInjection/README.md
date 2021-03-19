# Microsoft.Extensions.DependencyInjection Tutorial

## 목차
1. 의존성 주입 개요
1. 의존성 주입 주요 API 구성
1. 의존성 주입 기본 예제
1. 의존성 주입 응용 예제
1. 의존성 주입 패키지 통합
1. 참고 자료

<br/>

## 의존성 주입 개요
1. 의존성
1. 의존성 주입 방법
   - 생성자
   - Action Injection
   - Middleware Injection?

## 의존성 주입 주요 API 구성
1. Lifetime
   - Transient
   - Scoped
   - Singleton
1. namespace `Microsoft.Extensions.DependencyInjection`
   - Service
     - IServiceCollection AddTransient`<TService>`()                   `// 인터페이스가 없는 구체 클래스`
			where TService : class;
     - IServiceCollection AddTransient(`Type serviceType`);            `// 인터페이스가 없는 Generic 구체 클래스  `
   - Service, Implementation
     - IServiceCollection AddTransient`<TService, TImplementation>`()  `// 인터페이스가 있는 구체 클래스`
            where TService : class  
            where TImplementation : class, TService;  
     - IServiceCollection AddTransient(`Type serviceType, Type implementationType`);   `//인터페이스가 있는 Generic 구체 클래스`
   - Service, Factory Implementation
     - IServiceCollection AddTransient<TService>(`Func<IServiceProvider, TService> implementationFactory`)   `// 팩토리 패턴`
			where TService : class;
     - IServiceCollection AddTransient<TService, TImplementation>(`Func<IServiceProvider, TImplementation> implementationFactory`)  
            where TService : class  
            where TImplementation : class, TService;  
     - IServiceCollection AddTransient(Type serviceType, `Func<IServiceProvider, object> implementationFactory`);
   - ServiceDescriptor 인스턴스 만들기   
     - Service, Implementation
       - static ServiceDescriptor Transient<TService, TImplementation>()  
            where TService : class  
            where TImplementation : class, TService;
       - static ServiceDescriptor Transient(Type service, Type implementationType);
     - Service, Factory Implementation
       - static ServiceDescriptor Transient<TService>(Func<IServiceProvider, TService> implementationFactory)  
            where TService : class;
       - static ServiceDescriptor Transient<TService, TImplementation>(Func<IServiceProvider, TImplementation> implementationFactory)  
            where TService : class  
            where TImplementation : class, TService;
       - static ServiceDescriptor Transient(Type service, Func<IServiceProvider, object> implementationFactory);
1. namespace `Microsoft.Extensions.DependencyInjection.Extensions` : 복수개(ServiceDescriptor), 중복(Try)
   - Service
     - void TryAddTransient`<TService>`()   
	        where TService : class;
     - void TryAddTransient(`Type service`);
   - Service, Implementation
     - void TryAddTransient`<TService, TImplementation>`()
            where TService : class
            where TImplementation : class, TService;
     - void TryAddTransient(`Type service, Type implementationType`);
   - Service, Factory Implementation
     - void TryAddTransient<TService>(Func<IServiceProvider, TService> implementationFactory) 
	        where TService : class;
     - void TryAddTransient(Type service, Func<IServiceProvider, object> implementationFactory);
   - ServiceDescriptor 추가
     - Add
       - IServiceCollection Add(ServiceDescriptor descriptor);
       - IServiceCollection Add(IEnumerable<ServiceDescriptor> descriptors);
     - TryAdd
       - void TryAdd(ServiceDescriptor descriptor);
       - void TryAdd(IEnumerable<ServiceDescriptor> descriptors);
       - void TryAddEnumerable(ServiceDescriptor descriptor);
       - void TryAddEnumerable(IEnumerable<ServiceDescriptor> descriptors);

<br/>

## 의존성 주입 기본 예제
1. 인테페이스가 없는 구체 클래스  
   IServiceCollection AddTransient`<TService>`()  
       where TService : class;
1. 인터페이스와 있는 구체 클래스  
   IServiceCollection AddTransient`<TService, TImplementation>`()   
       where TService : class   
       where TImplementation : class, TService;  
1. 인터페이스가 없는 Generic 구체 클래스  
   IServiceCollection AddTransient(`Type serviceType`);
1. 인터페이스가 있는 Generic 구체 클래스  
   IServiceCollection AddTransient(`Type serviceType, Type implementationType`);
1. Factory  
   IServiceCollection AddTransient<TService>(`Func<IServiceProvider, TService> implementationFactory`)   
       where TService : class;
1. 중복 허용(복수개)
1. 중복 차단
1. 의존성 제거 Remove
1. 의존성 교체 Replasce

<br/>

## 의존성 주입 응용 예제
1. 복수 인터페이스 구현 단일 객체
1. Builder 패턴
   ```cs
   services.TryAddTransient<IMembershipAdvertBuilder, MembershipAdvertBuilder>();
   services.TryAddScoped<IMembershipAdvert>(sp =>
   {
       var builder = sp.GetService<IMembershipAdvertBuilder>();
   
       builder.WithDiscount(10m);
   
       return builder.Build();
   });

   public interface IMembershipAdvertBuilder
   {
       MembershipAdvert Build();
       MembershipAdvertBuilder WithDiscount(decimal discount);
   }
   
   public class MembershipAdvert : IMembershipAdvert
   {
       public MembershipAdvert(decimal offerPrice, decimal discount)
       {
           OfferPrice = offerPrice;
           Saving = discount;
       }
   
       public decimal OfferPrice { get; }
   
       public decimal Saving { get; }
   }
   ```
1. IOption 통합
1. 코드 구조화
   - `namespace Microsoft.Extensions.DependencyInjection`
1. Main -> Singleton 방지?
1. Lifetime Validation?

<br/>

## 의존성 주입 패키지 통합
### Scrutor
### Autofac

## 참고 자료
- [Microsoft GitHub | runtime | DependencyInjectionSpecificationTests.cs](https://github.com/dotnet/runtime/blob/main/src/libraries/Microsoft.Extensions.DependencyInjection.Specification.Tests/src/DependencyInjectionSpecificationTests.cs)