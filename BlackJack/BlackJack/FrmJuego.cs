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
        private List<EUsuario> jugadores;
        private SpeechRecognitionEngine voz;
        private PictureBox foto;
        private Panel cartaPanel;
        private Panel fichaPanel;
        private Region rg;
        private int apuesta;
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
            jugadores = new List<EUsuario>();
            point = new Point(panel13.Location.X - 3, panel13.Location.Y);
            panel13.BringToFront();
            panel13.Visible = false;
            voz = new SpeechRecognitionEngine();
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            gp.AddEllipse(0, 0, picJ1.Width - 3, picJ1.Height - 3);
            rg = new Region(gp);
        }

        private void pb1_Click(object sender, EventArgs e)
        {
            if (!mesa.Jugando)
            {
                if (mesa.JugadorAct == usuario.Id)
                {
                    PictureBox pb = (PictureBox)sender;
                    switch (pb.Name)
                    {
                        case "pb1":
                            apuesta += 1;
                            mBOL.AgregarFicha(pb.Tag.ToString(), usuario.Id);
                            break;
                        case "pb5":
                            apuesta += 5;
                            mBOL.AgregarFicha(pb.Tag.ToString(), usuario.Id);
                            break;
                        case "pb25":
                            apuesta += 25;
                            mBOL.AgregarFicha(pb.Tag.ToString(), usuario.Id);
                            break;
                        case "pb50":
                            apuesta += 50;
                            mBOL.AgregarFicha(pb.Tag.ToString(), usuario.Id);
                            break;
                        case "pb100":
                            apuesta += 100;
                            mBOL.AgregarFicha(pb.Tag.ToString(), usuario.Id);
                            break;
                        default:
                            break;
                    }
                }
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
            fichaPanel.Controls.Add(pb);
            pb.Image = img;
            pb.BringToFront();
            pb.BackColor = Color.Transparent;
        }

        private void DibujarCarta(List<Card> cartas)
        {
            int xJ1 = 0;
            for (int i = 0; i < cartas.Count; i++)
            {
                PictureBox pb = new PictureBox();
                pb.Location = new System.Drawing.Point(xJ1, 2);
                pb.Size = new System.Drawing.Size(80, 140);
                pb.SizeMode = PictureBoxSizeMode.StretchImage;
                pb.BorderStyle = BorderStyle.None;
                cartaPanel.Controls.Add(pb);
                if (usuario.Id == mesa.JugadorAct)
                {
                    pb.Load(cartas[i].image);
                }
                else
                {
                    if (i == 0)
                    {
                        pb.Load(cartas[i].image);
                    }
                    else
                    {
                        pb.Image = Properties.Resources.card;
                    }
                }
                pb.BringToFront();
                xJ1 += 25;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //y += yRes;
            //x -= xRes;
            //if (y + 50 > cartas2.Location.Y)
            //{
            //    DibujarCarta();
            //    panel13.Location = point;
            //    x = panel13.Location.X;
            //    y = panel13.Location.Y;
            //    timer1.Enabled = false;
            //    panel13.Visible = false;
            //}
            //else
            //{
            //    panel13.Location = new Point(x, y);
            //}
        }

        private void btnPedir_Click(object sender, EventArgs e)
        {
            ObtenerXY(7);
            panel13.Visible = true;
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
            mBOL.Salir(usuario.Id, mesa.Id);
            mBOL.Plantarse(mesa);
            Owner?.Show();
        }

        private void btnMicrofono_Click(object sender, EventArgs e)
        {
            if (!mesa.Jugando)
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
            LimpiarPanel();
            mesa = mBOL.CargarPartida(mesa);
            jugadores = mBOL.CargarJug(mesa);
            foreach (EUsuario u in jugadores)
            {
                if (u.Id == usuario.Id)
                {
                    usuario = u;
                }
                AsignarPanel(u.Turno, u);
            }
            lblEstado.Text = "Jugando .... " + mesa.JugadorAct;
            btnPlantarse.Enabled = mesa.JugadorAct == usuario.Id;
            btnPedir.Enabled = mesa.JugadorAct == usuario.Id;
            btnMicrofono.Enabled = !mesa.Jugando;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mBOL.Plantarse(mesa);
        }

        private void AsignarPanel(int turno, EUsuario usuario)
        {
            switch (turno)
            {
                case 1:
                    foto = picJ1;
                    cartaPanel = cartas1;
                    fichaPanel = fichas1;
                    foto.Load(usuario.Imagen);
                    foto.Region = rg;
                    break;
                case 2:
                    foto = picJ2;
                    cartaPanel = cartas2;
                    fichaPanel = fichas2;
                    foto.Load(usuario.Imagen);
                    foto.Region = rg;
                    break;
                case 3:
                    foto = picJ3;
                    cartaPanel = cartas3;
                    fichaPanel = fichas3;
                    foto.Load(usuario.Imagen);
                    foto.Region = rg;
                    break;
                case 4:
                    foto = picJ4;
                    cartaPanel = cartas4;
                    fichaPanel = fichas4;
                    foto.Load(usuario.Imagen);
                    foto.Region = rg;
                    break;
                case 5:
                    foto = picJ5;
                    cartaPanel = cartas5;
                    fichaPanel = fichas5;
                    foto.Load(usuario.Imagen);
                    foto.Region = rg;
                    break;
                case 6:
                    foto = picJ6;
                    cartaPanel = cartas6;
                    fichaPanel = fichas6;
                    foto.Load(usuario.Imagen);
                    foto.Region = rg;
                    break;
                default:
                    break;
            }
        }

        private void LimpiarPanel()
        {
            cartas1.Controls.Clear();
            fichas1.Controls.Clear();
            picJ1.Image = null;
            cartas2.Controls.Clear();
            fichas2.Controls.Clear();
            picJ2.Image = null;
            cartas3.Controls.Clear();
            fichas3.Controls.Clear();
            picJ3.Image = null;
            cartas4.Controls.Clear();
            fichas4.Controls.Clear();
            picJ4.Image = null;
            cartas5.Controls.Clear();
            fichas5.Controls.Clear();
            picJ5.Image = null;
            cartas6.Controls.Clear();
            fichas6.Controls.Clear();
            picJ6.Image = null;
        }
    }
}
