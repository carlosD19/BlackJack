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
    public partial class FrmLobby : Form
    {
        private int xP;
        private int yP;
        private int speed;
        private Point point;
        private EUsuario usuario;

        public FrmLobby()
        {
            InitializeComponent();
            CenterToScreen();
        }

        public FrmLobby(EUsuario usuario)
        {
            InitializeComponent();
            CenterToScreen();
            this.usuario = usuario;
        }

        private void FrmLobby_Load(object sender, EventArgs e)
        {
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            gp.AddEllipse(0, 0, pbImagen.Width - 3, pbImagen.Height - 3);
            Region rg = new Region(gp);
            pbImagen.Region = rg;
            speed = 3;
            point = new Point(panel1.Location.X - speed, panel1.Location.Y);
            CargarDatos();
        }

        private void CargarDatos()
        {
            if (usuario != null)
            {
                lblNom.Text = usuario.Nombre;
                lblApe.Text = usuario.Apellido;
                lblEmail.Text = usuario.Email;
                pbImagen.Load(usuario.Imagen);
            }
        }

        private void FrmLobby_LocationChanged(object sender, EventArgs e)
        {
            xP = pbImagen.Location.X + Location.X;
            yP = pbImagen.Location.Y + Location.Y;
        }

        private void FrmLobby_MouseMove(object sender, MouseEventArgs e)
        {
            int x = Cursor.Position.X;
            int y = Cursor.Position.Y;

            int limX = xP + panel1.Size.Width - 30;
            int limY = yP + panel1.Size.Height;

            if (x >= xP && x <= limX && y >= yP && y <= limY)
            {
                if (panel1.Location.X > xP - Location.X)
                {
                    panel1.Location = point;
                }
            }
            else
            {
                if (panel1.Location.X < xP - Location.X + 80)
                {
                    panel1.Location = new Point(17 + panel1.Location.X + speed, panel1.Location.Y);
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Ir a la cuenta.");
        }

        private void FrmLobby_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Owner != null)
            {
                Owner.Show();
            }
        }
    }
}
