﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NToml
{
    public interface IValue
    {
        TomlValueType Type { get; }
        void Visit(IValueVisitor visitor);
    }
}
