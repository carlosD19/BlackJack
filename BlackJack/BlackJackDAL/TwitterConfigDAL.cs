﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;

namespace BlackJackDAL
{
    public class TwitterConfigDAL
    {
        public static IAuthenticationContext authenticationContext;

        /// <summary>
        /// Metodo que solicita las credenciales de la app
        /// </summary>
        /// <returns>la url</returns>
        public string SolicitudCredenciales()
        {
            var appCredentials = new TwitterCredentials("f5jqvOWyeIOsnngNAQgaq6pj7", "2TFXpGd624BlzExpVvdTm2mKCMZk5HBVkeYsbqaU3hLxnN0mlR");

            authenticationContext = AuthFlow.InitAuthentication(appCredentials);

            return authenticationContext.AuthorizationURL;
        }
        /// <summary>
        /// Metodo que verificar el pin y devuelve un usuario
        /// </summary>
        /// <param name="pin">pin del usuario twitter</param>
        /// <returns>un usuario de twitter</returns>
        public IAuthenticatedUser AutenticarUsuario(String pin)
        {
            try
            {
                var userCredentials = AuthFlow.CreateCredentialsFromVerifierCode(pin, authenticationContext);

                Auth.SetCredentials(userCredentials);

                var user = User.GetAuthenticatedUser();
                return user;
            }
            catch (Exception e)
            {
                throw new ApplicationException("Problema de autenticación.", e);
            }
        }
    }
}
