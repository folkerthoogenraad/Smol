﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smol.Values
{
    public enum SmolType
    {
        String,
        Number,
        Boolean,

        Object,
        Array,

        Lambda,

        Native
    }
}