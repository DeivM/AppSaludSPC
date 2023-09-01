
namespace App.Models
{
    public partial class SeguimientoPacienteModel
    {
        public long SepId { get; set; }
        public long? CasId { get; set; }
        public string SepObservacion { get; set; }
        public bool SepFinalizar { get; set; }
        public long? UsuId { get; set; }

        public long CitId { get; set; }
        public string CasNombre { get; set; }

    }
}
