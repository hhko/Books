using System;

namespace Step_01_ExceptionFilter
{
    class Program
    {
        static void Main(string[] args)
        {
            var throwException =  new ThrowException();

            try
            {
                throwException.A();
            }
            catch when (False())    // Call Stack 확인하기
            {

            }
            catch 
            {

            }
        }

        public static bool False()
        {
            return false;
        }
    }

    class ThrowException
    {
        public void A()
        {
            B();
        }

        private void B()
        {
            C();
        }

        private void C()
        {
            D();
        }

        private void D()
        {
            int localVariable1 = 1;
            int localVariable2 = 2;
            int localVariable3 = 3;
            int localVariable4 = 4;
            int localVariable5 = 5;

            throw new DivideByZeroException();
        }
    }
}
