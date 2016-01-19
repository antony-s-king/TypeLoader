using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maverick.TypeLoader.Tests.TestCase
{
    public class AdderMath : IMath
    {
        public int Add(int x, int y)
        {
            return x + y;
        }
    }
}
