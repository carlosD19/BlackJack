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
        private Point p1;
        private Point p2;
        private Point p3;
        private int i;
        private List<String> lista = new List<string>();

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
            pbUno.BringToFront();
            i = 1;
            p1 = new Point(pbUno.Location.X, pbUno.Location.Y);
            p2 = new Point(pbDos.Location.X, pbDos.Location.Y);
            p3 = new Point(pbTres.Location.X, pbTres.Location.Y);
            lista.Add("Carlos");
            lista.Add("Jenny");
            lista.Add("Kevin");
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

        private void button2_Click(object sender, EventArgs e)
        {
            string temp = lista[lista.Count - 1];
            lista.RemoveAt(lista.Count - 1);
            lista.Insert(0, temp);
            pbUno.Location = p2;
            pbDos.Location = p3;
            pbTres.Location = p1;
            p1 = new Point(pbUno.Location.X, pbUno.Location.Y);
            p2 = new Point(pbDos.Location.X, pbDos.Location.Y);
            p3 = new Point(pbTres.Location.X, pbTres.Location.Y);
            switch (i)
            {
                case 1:
                    pbTres.BringToFront();
                    pbUno.SendToBack();
                    pbDos.SendToBack();
                    i = 3;
                    break;
                case 2:
                    pbUno.BringToFront();
                    pbDos.SendToBack();
                    pbTres.SendToBack();
                    i = 2;
                    break;
                case 3:
                    pbDos.BringToFront();
                    pbTres.SendToBack();
                    pbUno.SendToBack();
                    i = 1;
                    break;
                default:
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string temp = lista[0];
            lista.RemoveAt(0);
            lista.Add(temp);
            pbUno.Location = p3;
            pbDos.Location = p1;
            pbTres.Location = p2;

            p1 = new Point(pbUno.Location.X, pbUno.Location.Y);
            p2 = new Point(pbDos.Location.X, pbDos.Location.Y);
            p3 = new Point(pbTres.Location.X, pbTres.Location.Y);
            switch (i)
            {
                case 1:
                    pbDos.BringToFront();
                    pbTres.SendToBack();
                    pbUno.SendToBack();
                    i = 2;
                    break;
                case 2:
                    pbTres.BringToFront();
                    pbUno.SendToBack();
                    pbDos.SendToBack();
                    i = 3;
                    break;
                case 3:
                    pbUno.BringToFront();
                    pbDos.SendToBack();
                    pbTres.SendToBack();
                    i = 1;
                    break;
                default:
                    break;
            }
        }
    }
}
