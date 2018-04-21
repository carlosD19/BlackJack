using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BlackJackENL
{
    public class EUsuario
    {
        public int Id { get; set; }
        public long IdApp { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Imagen { get; set; }
        public double Dinero { get; set; }
        public double Apostado { get; set; }
        public double Ganado { get; set; }
        public bool Aposto { get; set; }
        public Card[] Cartas { get; set; }
    }
}
