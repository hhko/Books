using System;
using System.Collections.Generic;
using System.Text;

namespace DependencyInjectionSpec.Fakes
{
    public class FakeService : IFakeEveryService, IDisposable
    {
        //public PocoClass Value { get; set; }

        public bool Disposed { get; private set; }

        public void Dispose()
        {
            if (Disposed)
            {
                throw new ObjectDisposedException(nameof(FakeService));
            }

            Disposed = true;
        }
    }
}
