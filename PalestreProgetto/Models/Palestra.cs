using System;
using System.Collections.Generic;

namespace PalestreProgetto.Models;

public partial class Palestra
{
    public int Id { get; set; }

    public string Location { get; set; } = null!;

    public virtual ICollection<PalestraAttrezzo> PalestraAttrezzos { get; set; } = new List<PalestraAttrezzo>();
}
