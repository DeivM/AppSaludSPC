
using App.Helpers;
using System;
using System.Collections.Generic;


namespace App.Models
{
    public partial class CitaModel
    {
        public long CitId { get; set; }
        public long? MesId { get; set; }
        public long? EspId { get; set; }
        public DateTime? CitFechaAtencion { get; set; }
        public TimeSpan? CitInicioAtencion { get; set; }
        public TimeSpan? CitFinAtencion { get; set; }
        public string CitEstadoPaciente { get; set; }
        public string CitObservaciones { get; set; }
        public short? CitEstado { get; set; }
        public long? UsuId { get; set; }
        public string MedNombres { get; set; }
        public string UsuNombres { get; set; }
        public string EspNombre { get; set; }
        public long PerId { get; set; }

    }
}
