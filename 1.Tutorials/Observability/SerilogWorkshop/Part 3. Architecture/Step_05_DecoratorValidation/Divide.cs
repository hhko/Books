using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Step_05_DecoratorValidation
{
    public class Divide : IRequest<Unit>
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Divide(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
