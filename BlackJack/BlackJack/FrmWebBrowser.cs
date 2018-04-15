using BlackJackBOL;
using Facebook;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tweetinvi.Models;

namespace BlackJack
{
    public partial class FrmWebBrowser : Form
    {
        private readonly Uri loginUrl;
        private bool tipo;
        private string url;
        protected readonly FacebookClient userFacebook;
        public FacebookOAuthResult facebookOAuthResult { get; private set; }
        public IAuthenticatedUser userTwitter { get; private set; }
        public FrmWebBrowser(string appId, string extendedPermissions, bool tipo)
            : this(new FacebookClient(), appId, extendedPermissions, tipo)
        {
        }
        public FrmWebBrowser(string url, bool tipo)
        {
            this.tipo = tipo;
            this.url = url;
            InitializeComponent();
            CenterToScreen();
        }
        public FrmWebBrowser()
        {
            InitializeComponent();
            CenterToScreen();
        }
        public FrmWebBrowser(FacebookClient fb, string appId, string extendedPermissions, bool tipo)
        {
            if (string.IsNullOrWhiteSpace(appId))
            {
                throw new ArgumentNullException("appId");
            }

            this.userFacebook = fb ?? throw new ArgumentNullException("fb");
            loginUrl = GenerateLoginUrl(appId, extendedPermissions);
            this.tipo = tipo;
            InitializeComponent();
            CenterToScreen();
        }

        private Uri GenerateLoginUrl(string appId, string extendedPermissions)
        {
            dynamic parameters = new ExpandoObject();
            parameters.client_id = appId;
            parameters.redirect_uri = "https://www.facebook.com/connect/login_success.html";

            parameters.response_type = "token";

            parameters.display = "popup";

            if (!string.IsNullOrWhiteSpace(extendedPermissions))
                parameters.scope = extendedPermissions;

            return userFacebook.GetLoginUrl(parameters);
        }

        private void FrmWebBrowser_Load(object sender, EventArgs e)
        {
            if (tipo)
            {
                webBrowser1.Navigate(loginUrl.AbsoluteUri);
            }
            else
            {
                webBrowser1.Navigate(url);
            }
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if (tipo)
            {
                FacebookOAuthResult oauthResult;
                if (userFacebook.TryParseOAuthCallbackUrl(e.Url, out oauthResult))
                {
                    facebookOAuthResult = oauthResult;
                    DialogResult = facebookOAuthResult.IsSuccess ? DialogResult.OK : DialogResult.No;
                }
                else
                {
                    facebookOAuthResult = null;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox1.Text))
            {
                TwitterConfigBOL t = new TwitterConfigBOL();
                userTwitter = t.AutenticarUsuario(textBox1.Text);
                this.Close();
            }
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (e.Url.AbsoluteUri.Equals("https://api.twitter.com/oauth/authorize"))
            {
                pnPIN.Visible = true;
            }
        }
    }
}
