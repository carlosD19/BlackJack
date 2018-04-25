using System.Collections.Generic;


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
        public List<Card> Cartas { get; set; }
        public List<string> Fichas { get; set; }
        public int Turno { get; set; }
    }
}
