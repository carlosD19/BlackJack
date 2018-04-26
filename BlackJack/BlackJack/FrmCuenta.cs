using BlackJackBOL;
using BlackJackENL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlackJack
{
    public partial class FrmCuenta : Form
    {
        private EUsuario usuario;
        private UsuarioBOL bol;
        public FrmCuenta()
        {
            InitializeComponent();
        }
        public FrmCuenta(EUsuario usu)
        {
            InitializeComponent();
            CenterToScreen();
            usuario = usu;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (bol.Depositar(usuario.Id, (int)txtValor.Value))
                {
                    lblError.Text = "Deposito realizado.";
                }
                else
                {
                    lblError.Text = "Fallo en el deposito.";
                }
            }
            catch (Exception)
            {
                lblError.Text = "Error en el deposito.";
            }
        }

        private void FrmCuenta_FormClosing(object sender, FormClosingEventArgs e)
        {
            Owner?.Show();
        }

        private void FrmCuenta_Load(object sender, EventArgs e)
        {
            bol = new UsuarioBOL();
        }
    }
}
