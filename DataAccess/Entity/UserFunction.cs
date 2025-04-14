using System;
using System.Collections.Generic;

namespace DataAccess.Entity;

public partial class UserFunction
{
    public int UserFunctionId { get; set; }

    public int? UserId { get; set; }

    public int? FunctionId { get; set; }

    public int? IsView { get; set; }

    public int? IsUpdate { get; set; }

    public int? IsCreate { get; set; }

    public int? IsDelete { get; set; }
}
