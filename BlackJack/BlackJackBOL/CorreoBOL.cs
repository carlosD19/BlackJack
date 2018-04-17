using BlackJackDAL;
using BlackJackENL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackBOL
{
    public class CorreoBOL
    {
        private CorreoDAL dal;

        public CorreoBOL()
        {
            dal = new CorreoDAL();
        }
        public void EnviarCorreo(string correo, List<String> copias, EMesa mesa)
        {
            if (String.IsNullOrEmpty(correo))
            {
                throw new Exception("Correo requerido.");
            }
            dal.Enviar(correo, copias, mesa);
        }
    }
}
