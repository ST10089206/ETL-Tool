﻿using EthanETLTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EthanETLTool.Interfaces
{
    public interface IReader
    {
        IEnumerable<DataRecords> Read(string source);
    }
}
