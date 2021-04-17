using System;
using System.Collections.Generic;

#nullable disable

namespace SignalRDbChartServerExample.Models
{
    public partial class Sale
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public int Price { get; set; }
    }
}
