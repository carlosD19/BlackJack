using BlackJackDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Models;

namespace BlackJackBOL
{
    public class TwitterConfigBOL
    {
        private TwitterConfigDAL dal;

        public TwitterConfigBOL()
        {
            dal = new TwitterConfigDAL();
        }

        public string SolicitudCredenciales()
        {
            return dal.SolicitudCredenciales();
        }
        public IAuthenticatedUser AutenticarUsuario(string pin)
        {
            if (String.IsNullOrEmpty(pin))
            {
                throw new Exception("Ingresar el pin.");
            }
            return dal.AutenticarUsuario(pin);
        }
    }
}
