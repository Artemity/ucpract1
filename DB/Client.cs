using System;
using System.Collections.Generic;

namespace ucpract1.DB;

public partial class Client
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string? LastName { get; set; }

    public string Phone { get; set; } = null!;

    public string? Email { get; set; }
}
