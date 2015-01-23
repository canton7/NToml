using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NToml
{
    public class DuplicateTableKeyException : Exception
    {
        public string TableName { get; private set; }
        public string KeyName { get; private set; }

        public DuplicateTableKeyException(string tableName, string keyName)
            : base(String.Format("Duplicate key '{0}' in table {1}", keyName, tableName))
        {
            this.TableName = tableName;
            this.KeyName = keyName;
        }
    }
}
