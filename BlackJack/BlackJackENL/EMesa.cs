using System.Collections.Generic;

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
        public int Turno { get; set; }
        public string Deck_Id { get; set; }
        public bool Jugando { get; set; }
        public List<Card> Cartas { get; set; }
    }
}
