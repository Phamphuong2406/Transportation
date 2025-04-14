using System;
using System.Collections.Generic;

namespace DataAccess.Entity;

public partial class Function
{
    public int FunctionId { get; set; }

    public string? FunctionName { get; set; }

    public string? FunctionCode { get; set; }
}
