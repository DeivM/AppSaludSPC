using System;
using System.Collections.Generic;


namespace App.Models
{
    public partial class UsuarioModel
    {
        public long UsuId { get; set; }
        public string UsuNombres { get; set; }
        public string UsuApellidos { get; set; }
        public string UsuEmail { get; set; }
        public string UsuPassword { get; set; }
        public short? UsuEstado { get; set; }
        public string UsuCedula { get; set; }
        public string UsuDireccion { get; set; }
        public string UsuTelefono { get; set; }
        public string UsuSexo { get; set; }
        public DateTime? UsuFechaNacimiento { get; set; }
        public long? PerId { get; set; }

    }
}
