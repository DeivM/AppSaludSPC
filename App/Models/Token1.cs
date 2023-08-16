using System;
using System.Collections.Generic;
using System.Text;

namespace App.Models
{
    public class Token1
    {
        public long UsuId { get; set; }
        public string UsuNombre { get; set; }
        public string UsuApellidos { get; set; }
        public string UsuEmail { get; set; }
        public string Access_token { get; set; }
        public DateTime expires { get; set; }
        public long? PerId { get; set; }
        //public List<MenuUsuario> Menu { get; set; }
        public List<string> Paths { get; set; }
    }
}
