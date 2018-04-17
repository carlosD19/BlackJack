using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using BlackJackENL;

namespace BlackJackDAL
{
    public class CorreoDAL
    {
        public void Enviar(string correo, List<String> copias, EMesa mesa)
        {
            using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
            {
                string des = "Id Sala: " + mesa.Id + "\nContraseña: " + mesa.Pass;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("game21blackjack@gmail.com", "Blackjack21");
                MailMessage message = new MailMessage("game21blackjack@gmail.com", correo, "Solicitud de sala privada", des);
                message.BodyEncoding = System.Text.Encoding.UTF8;
                for (int i = 0; i < copias.Count; i++)
                {
                    message.Bcc.Add(copias[i]);
                }
                try
                {
                    client.Send(message);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al enviar correo.");
                }
            }
        }
    }
}
