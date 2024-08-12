using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class Organ
{
    public Guid FatherOrganId { get; set; }

    public Guid OrganId { get; set; }

    public string Name { get; set; }

    public string Src { get; set; }
}
