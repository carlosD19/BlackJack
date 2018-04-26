using System;
using System.Collections.Generic;
using BlackJackDAL;
using BlackJackENL;

namespace BlackJackBOL
{
    public class MesaBOL
    {
        private MesaDAL dal;

        public MesaBOL()
        {
            dal = new MesaDAL();
        }

        public bool Agregar(EMesa mesa, string rePass)
        {
            if (String.IsNullOrEmpty(mesa.Nombre))
            {
                throw new Exception("Nombre requerido.");
            }
            if (String.IsNullOrEmpty(mesa.Pass))
            {
                throw new Exception("Contraseña requerida.");
            }
            if (String.IsNullOrEmpty(rePass))
            {
                throw new Exception("Re-Contraseña requerida.");
            }
            if (!mesa.Pass.Equals(rePass))
            {
                throw new Exception("Las contraseñas no coinciden.");
            }

            return dal.Insertar(mesa);
        }

        public bool Eliminar(EMesa mesa)
        {
            if (mesa.Id < 0)
            {
                throw new Exception("Mesa requerida.");
            }
            return dal.Eliminar(mesa);
        }

        public bool Modificar(EMesa mesa, string rePass)
        {
            if (mesa.Id < 0)
            {
                throw new Exception("Mesa requerida.");
            }
            if (String.IsNullOrEmpty(mesa.Nombre))
            {
                throw new Exception("Nombre requerido.");
            }
            if (String.IsNullOrEmpty(mesa.Pass))
            {
                throw new Exception("Contraseña requerida.");
            }
            if (String.IsNullOrEmpty(rePass))
            {
                throw new Exception("Re-Contraseña requerida.");
            }
            if (!mesa.Pass.Equals(rePass))
            {
                throw new Exception("Las contraseñas no coinciden.");
            }
            return dal.Modificar(mesa);
        }

        public List<EMesa> CargarTodo()
        {
            return dal.Cargar();
        }

        public EMesa BuscarMesa(EUsuario usuario, EMesa mesa, string pass)
        {
            return dal.Unirse(usuario, mesa, pass);
        }

        public void AgregarFicha(string ficha, int id)
        {
            dal.InsertarFicha(ficha, id);
        }

        public EMesa CargarPartida(EMesa mesa)
        {
            return dal.CargarPartidaID(mesa.Id);
        }

        public void Plantarse(EMesa mesa)
        {
            dal.Actualizar(mesa);
        }

        public void Salir(int id, int mesaId)
        {
            dal.SalirPartida(id, mesaId);
        }
        public void EliminarFicha(int id)
        {
            dal.EliminarFichas(id);
        }

        public bool VerificarPass(string pass)
        {
            return dal.VerificarP(pass);
        }

        public List<EUsuario> CargarJug(EMesa mesa)
        {
            return dal.CargarJugadores(mesa);
        }

        public void AgregarCarta(string deckID, int id)
        {
            dal.AgregarCarta(deckID, id);
        }

        public void JugadorApuesta(int id, int apuesta, bool tipo)
        {
            dal.Apostar(id, apuesta, tipo);
        }

        public void RepartirCartas(List<EUsuario> jugadores, string deck, int mesaId)
        {
            dal.Repartir(jugadores, deck, mesaId);
        }
    }
}
