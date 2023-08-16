
using App.Helpers;
using System.Collections.Generic;


namespace App.Models
{
    public partial class ComidaModel
    {
        public long ComId { get; set; }
        public long PlaId { get; set; }
        public long AliId { get; set; }
        public short? ComDiaNr { get; set; }
        public string ComComida { get; set; }
        public string ComAlimentoNombre { get; set; }
        public bool? Calorias { get; set; }

        public List<ComidaModel> detalle { get; set; }


    }
}
