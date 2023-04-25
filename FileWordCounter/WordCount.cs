﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWordCounter;

public record WordCount
{
    public string? Word { get; set; }
    public int Count { get; set; }
}
