using BlackJackDAL;
using BlackJackENL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackBOL
{
    public class UsuarioBOL
    {
        UsuarioDAL dal;

        public UsuarioBOL()
        {
            dal = new UsuarioDAL();
        }

        public EUsuario VerificarUsuario(EUsuario usuario)
        {
            if (usuario == null)
            {
                throw new Exception("Usuario requerido.");
            }
            return dal.Verificar(usuario);
        }

        public List<EUsuario> CargarTodo()
        {
            return dal.Cargar();
        }

        public bool Depositar(int id, int value)
        {
            return dal.AgregarDinero(id, value);
        }
    }
}