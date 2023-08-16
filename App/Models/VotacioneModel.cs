using System;
using System.Collections.Generic;



namespace App.Models
{
    public partial class VotacioneModel
    {
        public long VotId { get; set; }
        public long? UsuId { get; set; }
        public long? CanId { get; set; }
    }
}
