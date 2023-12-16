using System;
using System.Collections.Generic;

namespace PalestreProgetto.Models;

public partial class Attrezzo
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<PalestraAttrezzo> PalestraAttrezzos { get; set; } = new List<PalestraAttrezzo>();
}
