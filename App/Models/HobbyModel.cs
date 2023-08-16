using System;
using System.Collections.Generic;



namespace App.Models
{
    public partial class HobbyModel
    {
        public long HobId { get; set; }
        public long? CanId { get; set; }
        public string HobDescripcion { get; set; }
    }
}
