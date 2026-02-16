using System;
using System.Collections.Generic;

namespace ucpract1.DB;

public partial class Agent
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string MiddleName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int? DealShare { get; set; }
}
