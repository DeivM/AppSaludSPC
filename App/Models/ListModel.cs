
using App.Helpers;
using System.Collections.Generic;


namespace App.Models
{
    public partial class ListModel
    {
        public int id { get; set; }
        public string Nombre { get; set; }
        public string detalle { get; set; }
        public string detalleMensaje { get; set; }
        public string imagenUrl { get; set; }
        public int id1 { get; set; }
        public List<ListModel> ListaDetalle { get; set; }

    }
}
