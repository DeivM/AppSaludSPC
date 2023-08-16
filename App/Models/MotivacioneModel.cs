using System;
using System.Collections.Generic;


namespace App.Models
{
    public partial class MotivacioneModel
    {
        public long MotId { get; set; }
        public long? CanId { get; set; }
        public string MotDescripcion { get; set; }
    }
}
