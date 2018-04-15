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
    public partial class FrmJuego : Form
    {
        private int yJ1;
        private int x;
        private int y;
        private int xRes;
        private int yRes;
        private Point point;
        private int xJ1, xJ2, xJ3, xJ4, xJ5, xJ6;
        public FrmJuego()
        {
            InitializeComponent();
            CenterToScreen();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            yJ1 = 120;
            x = panel13.Location.X;
            y = panel13.Location.Y;
            point = new Point(panel13.Location.X - 3, panel13.Location.Y);
            panel13.BringToFront();
        }

        private void pb1_Click(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            switch (pb.Name)
            {
                case "pb1":
                    DibujarFicha(pb.Image, yJ1);
                    yJ1 -= 8;
                    break;
                case "pb5":
                    DibujarFicha(pb.Image, yJ1);
                    yJ1 -= 8;
                    break;
                case "pb25":
                    DibujarFicha(pb.Image, yJ1);
                    yJ1 -= 8;
                    break;
                case "pb50":
                    DibujarFicha(pb.Image, yJ1);
                    yJ1 -= 8;
                    break;
                case "pb100":
                    DibujarFicha(pb.Image, yJ1);
                    yJ1 -= 8;
                    break;
                default:
                    break;
            }
        }

        private void DibujarFicha(Image img, int yF)
        {
            PictureBox pb = new PictureBox();
            pb.Location = new System.Drawing.Point(2, yF);
            pb.Size = new System.Drawing.Size(50, 50);
            pb.SizeMode = PictureBoxSizeMode.Normal;
            pb.BorderStyle = BorderStyle.None;
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            gp.AddEllipse(0, 0, pb.Width - 3, pb.Height - 3);
            Region rg = new Region(gp);
            pb.Region = rg;
            panel10.Controls.Add(pb);
            pb.Image = img;
            pb.BringToFront();
            pb.BackColor = Color.Transparent;
        }

        private void DibujarCarta(Panel panel)
        {
            PictureBox pb = new PictureBox();
            pb.Location = new System.Drawing.Point(xJ1, 2);
            pb.Size = new System.Drawing.Size(80, 140);
            pb.SizeMode = PictureBoxSizeMode.StretchImage;
            pb.BorderStyle = BorderStyle.None;
            panel.Controls.Add(pb);
            pb.Image = Properties.Resources.card;
            pb.BringToFront();
            xJ1 += 25;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            y += yRes;
            x -= xRes;
            if (y + 50 > panel2.Location.Y)
            {
                DibujarCarta(panel4);
                panel13.Location = point;
                x = panel13.Location.X;
                y = panel13.Location.Y;
                timer1.Enabled = false;
            }
            else
            {
                panel13.Location = new Point(x, y);
            }
        }

        private void btnPedir_Click(object sender, EventArgs e)
        {
            ObtenerXY(4);
            timer1.Enabled = true;
        }

        private void ObtenerXY(int p)
        {
            switch (p)
            {
                case 1:
                    xRes = 60;
                    yRes = 22;
                    break;
                case 2:
                    xRes = 70;
                    yRes = 35;
                    break;
                case 3:
                    xRes = 70;
                    yRes = 50;
                    break;
                case 4:
                    xRes = 40;
                    yRes = 55;
                    break;
                case 5:
                    xRes = 10;
                    yRes = 60;
                    break;
                case 6:
                    xRes = -25;
                    yRes = 44;
                    break;
                default:
                    break;
            }
        }
    }
}
