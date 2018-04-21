using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            if (mesa.Privada)
            {
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

        public bool Modificar(EMesa mesa)
        {
            if (mesa.Id < 0)
            {
                throw new Exception("Mesa requerida.");
            }
            return dal.Modificar(mesa);
        }

        public List<EMesa> CargarTodo()
        {
            return dal.Cargar();
        }

        public EMesa BuscarPublica(EUsuario usuario, EMesa mesa, string pass)
        {
            return dal.Unirse(usuario, mesa, pass);
        }

        public EMesa CargarPartida(EMesa mesa)
        {
            return dal.CargarPartidaID(mesa.Id);
        }

        public void Plantarse(EMesa mesa)
        {
            dal.Actualizar(mesa);
        }

        public void Salir(int id)
        {
            dal.SalirPartida(id);
        }
    }
}
