using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackENL
{
    public class EMesa
    {
        public int Id { get; set; }
        public bool Activo { get; set; }
        public string Nombre { get; set; }
        public int Capacidad { get; set; }
        public string Pass { get; set; }
        public bool Privada { get; set; }
        public int JugadorAct { get; set; }
        public int ContadorJug { get; set; }

    }
}
