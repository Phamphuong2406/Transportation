using System;
using System.Collections.Generic;

namespace DataAccess.Entity;

public partial class UserSession
{
    public int SessionId { get; set; }

    public int? UserId { get; set; }

    public string? Token { get; set; }

    public string? DeviceId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? Ip { get; set; }
}
