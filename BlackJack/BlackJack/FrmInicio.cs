using BlackJackBOL;
using BlackJackDAL;
using BlackJackENL;
using Facebook;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tweetinvi.Models;

namespace BlackJack
{
    public partial class FrmInicio : Form
    {
        private const string AppId = "114165376102953";
        private const string ExtendedPermissions = "public_profile,email";
        private string accessToken;
        private MiniJsonBOL bol;
        private EUsuario usuario;
        private TwitterConfigBOL tw;
        private UsuarioBOL uBOL;
        public FrmInicio()
        {
            InitializeComponent();
            CenterToScreen();
        }

        private void btnFacebook_Click(object sender, EventArgs e)
        {
            var frmWEB = new FrmWebBrowser(AppId, ExtendedPermissions, true);
            frmWEB.ShowDialog();

            ObtenerUsuario(frmWEB.facebookOAuthResult);
        }

        private void ObtenerUsuario(FacebookOAuthResult facebookOAuthResult)
        {
            if (facebookOAuthResult != null)
            {
                if (facebookOAuthResult.IsSuccess)
                {
                    accessToken = facebookOAuthResult.AccessToken;
                    var fb = new FacebookClient(facebookOAuthResult.AccessToken);
                    CargarUsuarioFB(fb);
                    Cerrar();
                }
                else
                {
                    MessageBox.Show(facebookOAuthResult.ErrorDescription);
                }
            }
        }

        private void Cerrar()
        {
            var webBrowser = new WebBrowser();
            var fb = new FacebookClient();
            var logouUrl = fb.GetLogoutUrl(new { access_token = accessToken, next = "https://www.facebook.com/connect/login_success.html" });
            webBrowser.Navigate(logouUrl);
        }

        private void CargarUsuarioFB(FacebookClient facebookClient)
        {
            usuario = new EUsuario();
            var MyData = facebookClient.Get("me", new { fields = new[] { "id", "email", "first_name", "last_name" } });
            var dict = bol.Deserialize(MyData.ToString()) as Dictionary<string, object>;
            usuario.Nombre = dict["first_name"].ToString();
            usuario.Apellido = dict["last_name"].ToString();
            usuario.Email = dict["email"].ToString();
            string id_Usu = dict["id"].ToString();
            string img = String.Format("https://graph.facebook.com/{0}/picture", id_Usu);
            usuario.Imagen = img;
            usuario.IdApp = long.Parse(id_Usu);
            EUsuario u = new EUsuario();
            u = uBOL.VerificarUsuario(usuario);
            if (u != null)
            {
                FrmLobby frm = new FrmLobby(u);
                frm.Show();
            }
        }

        private void FrmInicio_Load(object sender, EventArgs e)
        {
            bol = new MiniJsonBOL();
            uBOL = new UsuarioBOL();
        }

        private void btnTwitter_Click(object sender, EventArgs e)
        {
            TwitterConfigBOL t = new TwitterConfigBOL();
            tw = new TwitterConfigBOL();
            var frm = new FrmWebBrowser(tw.SolicitudCredenciales(), false);
            frm.ShowDialog();
            CargarUsuarioTW(frm.userTwitter);
        }

        private void CargarUsuarioTW(IAuthenticatedUser userTwitter)
        {
            if (userTwitter != null)
            {
                usuario = new EUsuario();
                usuario.Nombre = userTwitter.Name;
                usuario.Email = userTwitter.Email;
                usuario.Imagen = userTwitter.ProfileImageUrl;
                usuario.Apellido = "";
                usuario.IdApp = userTwitter.Id;
                EUsuario u = new EUsuario();
                u = uBOL.VerificarUsuario(usuario);
                if (u != null)
                {
                    FrmLobby frm = new FrmLobby(u);
                    frm.Show();
                }
            }
        }
    }
}
