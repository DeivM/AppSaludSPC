
using App.Helpers;
using System;
using System.Collections.Generic;


namespace App.Models
{
    public partial class CitaData
    {
        public CitaModel Data { get; set; }

        public List<ListModel> Medicos { get; set; }
        public List<ListModel> Pacientes { get; set; }
        public List<ListModel> MedicosEspecialidad { get; set; }
        public List<ListModel> Citas { get; set; }

    }
}
