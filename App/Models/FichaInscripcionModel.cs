
using App.Helpers;
using System.Collections.Generic;


namespace App.Models
{
    public partial class FichaInscripcionModel
    {

        public long FiiId { get; set; }
        public string FiiNombres { get; set; }
        public string FiiApellidos { get; set; }
        public string FiiIdentificacion { get; set; }
        public string FiiTelefono { get; set; }
        public string FiiEmail { get; set; }
        public long? CarId { get; set; }
        public string CarNombre { get; set; }
        public string IfiUrl { get; set; }
        public virtual List<HobbyModel> Hobbies { get; set; }
        public virtual List<ImagenFichaInscripcionModel> ImagenFichaInscripcion { get; set; }
        public virtual List<MotivacioneModel> Motivaciones { get; set; }
        public virtual List<VotacioneModel> Votaciones { get; set; }
        public int TotalVotaciones { get; set; }
        public string FullImageUrl => IfiUrl;
        //  public string FullImageUrl => VariablesGlobales.URL+ VariablesGlobales.URLIMAGE + IfiUrl;



    }
}
