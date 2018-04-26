using System;
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
        public bool BlackJack { get; set; }
        public bool Aposto { get; set; }
        public List<Card> Cartas { get; set; }
        public List<string> Fichas { get; set; }
        public int Turno { get; set; }
        public int ApuestaTemp { get; set; }

        /// <summary>
        /// Metodo que suma las cartas del jugador
        /// </summary>
        /// <param name="cartasJug">lista de cartas</param>
        /// <returns>la suma de las cartas</returns>
        public int ContarCartas(List<Card> cartasJug)
        {
            int total = 0;
            int total2 = 0;
            for (int i = 0; i < cartasJug.Count; i++)
            {
                switch (cartasJug[i].value)
                {
                    case "KING":
                        total += 10;
                        total2 += 10;
                        break;
                    case "ACE":
                        total += 11;
                        total2 += 1;
                        break;
                    case "JACK":
                        total += 10;
                        total2 += 10;
                        break;
                    case "QUEEN":
                        total += 10;
                        total2 += 10;
                        break;
                    default:
                        total += Int32.Parse(cartasJug[i].value);
                        total2 += Int32.Parse(cartasJug[i].value);
                        break;
                }
            }
            if (total > 21)
            {
                return total2;
            }
            else
            {
                return total;
            }
        }
        /// <summary>
        /// Metodo que compara la suma de las cartas de los jugadores con la del crupier
        /// </summary>
        /// <param name="totalJug">suma de las cartas del jugador</param>
        /// <param name="cartasMesa">lista de cartas del crupier</param>
        /// <returns>true si gano el jugador y false sino</returns>
        public bool ContarCartasCrupier(int totalJug, List<Card> cartasMesa)
        {
            int total = 0;
            int total2 = 0;
            for (int i = 0; i < cartasMesa.Count; i++)
            {
                switch (cartasMesa[i].value)
                {
                    case "KING":
                        total += 10;
                        total2 += 10;
                        break;
                    case "ACE":
                        total += 11;
                        total2 += 1;
                        break;
                    case "JACK":
                        total += 10;
                        total2 += 10;
                        break;
                    case "QUEEN":
                        total += 10;
                        total2 += 10;
                        break;
                    default:
                        total += Int32.Parse(cartasMesa[i].value);
                        total2 += Int32.Parse(cartasMesa[i].value);
                        break;
                }
            }
            if (total > 21)
            {
                total = total2;
            }
            else
            {
                total2 = total;
            }
            if(total >= totalJug && total <= 21)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
