using System;
using System.Collections.Generic;

namespace PalestreProgetto.Models;

public partial class PalestraAttrezzo
{
    public int Id { get; set; }

    public int PalestraId { get; set; }

    public int AttrezzoId { get; set; }

    public virtual Attrezzo Attrezzo { get; set; } = null!;

    public virtual Palestra Palestra { get; set; } = null!;
}
