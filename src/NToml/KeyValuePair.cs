﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NToml
{
    internal class KeyValuePair
    {
        private readonly string key;
        private readonly ITableValue value;

        public KeyValuePair(string key, ITableValue value)
        {
            this.key = key;
            this.value = value;
        }
    }
}