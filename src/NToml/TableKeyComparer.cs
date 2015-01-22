using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NToml
{
    internal class TableKeyComparer : IEqualityComparer<string[]>
    {
        public bool Equals(string[] x, string[] y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;
            return x.SequenceEqual(y);
        }

        public int GetHashCode(string[] obj)
        {
            return obj.Aggregate(17, (acc, item) => unchecked(acc * 23 + item.GetHashCode()));
        }
    }
}
