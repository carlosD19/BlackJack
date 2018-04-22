using BlackJackENL;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Speech.Recognition;
using BlackJackBOL;
using System.Collections.Generic;

namespace BlackJack
{
    public partial class FrmJuego : Form
    {
        private int xJ1;
        private int yJ1;
        private int x;
        private int y;
        private int xRes;
        private int yRes;
        private bool bus;
        private Point point;
        private EUsuario usuario;
        private EMesa mesa;
        private MesaBOL mBOL;
        private List<EUsuario> usuJugando;
        private SpeechRecognitionEngine voz;

        public FrmJuego()
        {
            InitializeComponent();
            CenterToScreen();
        }

        public FrmJuego(EUsuario u, EMesa mesa)
        {
            InitializeComponent();
            CenterToScreen();
            this.mesa = mesa;
            usuario = u;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            yJ1 = 120;
            x = panel13.Location.X;
            y = panel13.Location.Y;
            mBOL = new MesaBOL();
            point = new Point(panel13.Location.X - 3, panel13.Location.Y);
            panel13.BringToFront();
            panel13.Visible = false;
            voz = new SpeechRecognitionEngine();
        }

        private void pb1_Click(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            switch (pb.Name)
            {
                case "pb1":
                    DibujarFicha(pb.Image, yJ1);
                    MessageBox.Show(pb.Image.ToString());
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
            fichas1.Controls.Add(pb);
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
            if (y + 50 > cartas2.Location.Y)
            {
                DibujarCarta(cartas7);
                panel13.Location = point;
                x = panel13.Location.X;
                y = panel13.Location.Y;
                timer1.Enabled = false;
                panel13.Visible = false;
            }
            else
            {
                panel13.Location = new Point(x, y);
            }
        }

        private void btnPedir_Click(object sender, EventArgs e)
        {
            mBOL.Plantarse(mesa);
            //ObtenerXY(7);
            //panel13.Visible = true;
            //timer1.Enabled = true;
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
                case 7:
                    xRes = 40;
                    yRes = 0;
                    break;
                default:
                    break;
            }
        }

        private void FrmJuego_FormClosing(object sender, FormClosingEventArgs e)
        {
            mBOL.Salir(usuario.Id);
            mBOL.Plantarse(mesa);
            Owner?.Show();
        }

        private void btnMicrofono_Click(object sender, EventArgs e)
        {
            if (!bus)
            {
                btnMicrofono.Image = Properties.Resources.micro;
                Escucha();
                bus = true;
            }
            else
            {
                btnMicrofono.Image = Properties.Resources.nomicro;
                Escucha();
                bus = false;
            }
        }

        private void Escucha()
        {
            try
            {
                if (bus)
                {
                    voz.RecognizeAsyncStop();
                }
                else
                {
                    voz.SetInputToDefaultAudioDevice(); // Usaremos el microfono predeterminado del sistema 
                    voz.LoadGrammar(new DictationGrammar()); //Carga la gramatica de Windows 
                    voz.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(LectorDeVoz); // Controlador de eventos. Se ejecutara al reconocer 
                    voz.RecognizeAsync(RecognizeMode.Multiple); //Iniciamos reconocimiento 
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error de audio.");
            }
        }

        private void LectorDeVoz(object sender, SpeechRecognizedEventArgs e)
        {
            foreach (RecognizedWordUnit palabra in e.Result.Words)
            {

                if (palabra.Text == "carta")
                {
                    MessageBox.Show("Pedir");
                }
                else if (palabra.Text == "plantarse")
                {
                    MessageBox.Show("Plantarse");
                }
            }
        }

        private void tmAct_Tick(object sender, EventArgs e)
        {
            Refrescar();
        }

        private void Refrescar()
        {
            mesa = mBOL.CargarPartida(mesa);
            lblEstado.Text = "Jugando .... " + mesa.JugadorAct;
            btnPedir.Enabled = mesa.JugadorAct == usuario.Id;
        }
    }
}
