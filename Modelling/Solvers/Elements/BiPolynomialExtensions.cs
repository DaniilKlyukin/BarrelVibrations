using System.Collections.Generic;
using System.Linq;

namespace Modelling.Solvers.Elements
{
    public static class BiPolynomialExtensions
    {
        public static BiPolynomial Sum(this IEnumerable<BiPolynomial> biPolynomials)
        {
            var array = biPolynomials.ToArray();

            var sum = array.First();

            for (var i = 1; i < array.Length; i++)
            {
                sum += array[i];
            }

            return sum;
        }
    }
}
