using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class News
{
    public Guid NewsId { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }

    public DateTime StartDateTime { get; set; }

    public DateTime EndDateTime { get; set; }

    public int Click { get; set; }

    public DateTime UpdateDateTime { get; set; }

    public int UpdateEmployeeId { get; set; }

    public DateTime InsertDateTime { get; set; }

    public int InsertEmployeeId { get; set; }

    public bool Enable { get; set; }

    public int? Orders { get; set; }
}
