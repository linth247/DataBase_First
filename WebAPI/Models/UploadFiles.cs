using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class UploadFiles
{
    public Guid UploadFileId { get; set; }

    public string Name { get; set; }

    public string Src { get; set; }

    public Guid TodoId { get; set; }

    public virtual TodoList Todo { get; set; }
}
