using BlackJackDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackBOL
{
    public class MiniJsonBOL
    {
        private Json dal;

        public MiniJsonBOL()
        {
            dal = new Json();
        }

        public object Deserialize(string json)
        {
            if (String.IsNullOrEmpty(json))
            {
                throw new Exception("Ingrese un Json.");
            }
            return dal.Deserialize(json);
        }
    }
}
