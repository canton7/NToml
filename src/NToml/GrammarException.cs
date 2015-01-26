using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NToml
{
    public class GrammarException : ParseException
    {
        public GrammarException(string message) : base(message)
        {
        }
    }
}
