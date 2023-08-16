

namespace App.Models
{
    public partial class ImagenFichaInscripcionModel
    {
        public long ImcId { get; set; }
        public long? CanId { get; set; }
        public string ImcUrl { get; set; }
        public int? ImcTipo { get; set; }
        public bool? ImcPrincipal { get; set; }
    }
}
