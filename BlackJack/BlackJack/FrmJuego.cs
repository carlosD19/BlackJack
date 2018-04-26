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
        private bool bus;
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
        private int temp;
        private int tempJug;
        private int ultimoJug;

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
            this.Text = u.Nombre + " " + u.Apellido;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            mBOL = new MesaBOL();
            jugadores = new List<EUsuario>();
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
                            if (ValidarApuesta(1))
                            {
                                apuesta += 1;
                                mBOL.AgregarFicha(pb.Tag.ToString(), usuario.Id);
                            }
                            break;
                        case "pb5":
                            if (ValidarApuesta(5))
                            {
                                apuesta += 5;
                                mBOL.AgregarFicha(pb.Tag.ToString(), usuario.Id);
                            }
                            break;
                        case "pb25":
                            if (ValidarApuesta(25))
                            {
                                apuesta += 25;
                                mBOL.AgregarFicha(pb.Tag.ToString(), usuario.Id);
                            }
                            break;
                        case "pb50":
                            if (ValidarApuesta(50))
                            {
                                apuesta += 50;
                                mBOL.AgregarFicha(pb.Tag.ToString(), usuario.Id);
                            }
                            break;
                        case "pb100":
                            if (ValidarApuesta(100))
                            {
                                apuesta += 100;
                                mBOL.AgregarFicha(pb.Tag.ToString(), usuario.Id);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private bool ValidarApuesta(int cant)
        {
            if (usuario.Dinero > apuesta + cant)
            {
                return true;
            }
            return false;
        }

        private void DibujarFicha(List<string> fichas)
        {
            int yF = 120;
            Image bitmap = null;

            for (int i = 0; i < fichas.Count; i++)
            {
                switch (fichas[i])
                {
                    case "_1":
                        bitmap = Properties.Resources._1;
                        break;
                    case "_5":
                        bitmap = Properties.Resources._5;
                        break;
                    case "_25":
                        bitmap = Properties.Resources._25;
                        break;
                    case "_50":
                        bitmap = Properties.Resources._50;
                        break;
                    case "_100":
                        bitmap = Properties.Resources._100;
                        break;
                    default:
                        break;
                }
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
                pb.Image = bitmap;
                pb.BringToFront();
                pb.BackColor = Color.Transparent;
                yF -= 8;
            }
        }
        private void DibujarCartaCrupier(List<Card> cartas)
        {
            int xJ1 = 0;
            for (int i = 0; i < cartas.Count; i++)
            {
                PictureBox pb = new PictureBox();
                pb.Location = new System.Drawing.Point(xJ1, 2);
                pb.Size = new System.Drawing.Size(80, 140);
                pb.SizeMode = PictureBoxSizeMode.StretchImage;
                pb.BorderStyle = BorderStyle.None;
                cartas7.Controls.Add(pb);
                if (i == 0)
                {
                    pb.Load(cartas[i].image);
                }
                else
                {
                    pb.Image = Properties.Resources.card;
                }
                pb.BringToFront();
                xJ1 += 25;
            }
        }

        private void DibujarCarta(List<Card> cartas, int usuId)
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
                if (usuario.Id == usuId)
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

        private void btnPedir_Click(object sender, EventArgs e)
        {
            if (mesa.JugadorAct == usuario.Id)
            {
                if (mesa.Jugando)
                {
                    mBOL.AgregarCarta(mesa.Deck_Id, usuario.Id);
                    Refrescar();
                    ObtenerSumaCartas();
                }
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
            if (mesa.JugadorAct == usuario.Id)
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
            jugadores = mBOL.CargarJug(mesa);
            int actual = jugadores.Count;
            int jugAct = mesa.JugadorAct;
            if (temp != actual)
            {
                LimpiarPanel();
            }
            if (tempJug != jugAct)
            {
                ObtenerSumaCartas();
            }
            for (int i = 0; i < jugadores.Count; i++)
            {
                if (jugadores[i].Id == usuario.Id)
                {
                    usuario = jugadores[i];
                }
                if (i == jugadores.Count - 1)
                {
                    ultimoJug = jugadores[i].Id;
                }
                AsignarPanel(jugadores[i].Turno, jugadores[i]);
                DibujarCarta(jugadores[i].Cartas, jugadores[i].Id);
                DibujarFicha(jugadores[i].Fichas);
                DibujarCartaCrupier(mesa.Cartas);
            }
            temp = jugadores.Count;
            tempJug = mesa.JugadorAct;
            lblJugando.Text = "Jugando .... " + mesa.JugadorAct + " / " + usuario.Id;
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
            cartas7.Controls.Clear();
        }

        private void btnApostrar_Click(object sender, EventArgs e)
        {
            if (apuesta > 0)
            {
                mBOL.JugadorApuesta(usuario.Id, apuesta, true);
                mBOL.Plantarse(mesa);
            }
            Refrescar();
            if (ultimoJug == usuario.Id)
            {
                bool bus2 = false;
                foreach (EUsuario us in jugadores)
                {
                    if (us.Aposto)
                    {
                        bus2 = true;
                        break;
                    }
                }
                if (bus2)
                {
                    mBOL.RepartirCartas(jugadores, mesa.Deck_Id, mesa.Id);
                }
            }
            Refrescar();
            ObtenerSumaCartas();
        }

        private void ObtenerSumaCartas()
        {
            foreach (EUsuario usu in jugadores)
            {
                if (usuario.Id == usu.Id)
                {
                    if (usuario.Cartas != null && usuario.Cartas.Count != 0)
                    {
                        int resultado = usuario.ContarCartas(usuario.Cartas);
                        if (resultado == 21)
                        {
                            lblSuma.Text = "BlackJack";
                        }
                        else
                        {
                            lblSuma.Text = resultado.ToString();
                        }
                        break;
                    }
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (mesa.JugadorAct == usuario.Id)
            {
                mBOL.Plantarse(mesa);
                mesa = mBOL.CargarPartida(mesa);
                foreach (EUsuario u in jugadores)
                {
                    if (u.Id == mesa.JugadorAct)
                    {
                        if (u.Aposto == false)
                        {
                            mBOL.Plantarse(mesa);
                            mesa = mBOL.CargarPartida(mesa);
                        }
                    }
                }
                Refrescar();
                if (usuario.Id == ultimoJug)
                {
                    mBOL.RepartirCartas(jugadores, mesa.Deck_Id, mesa.Id);
                }
            }
        }

        private void btnPlantarse_Click(object sender, EventArgs e)
        {
            if (mesa.JugadorAct == usuario.Id)
            {
                if (mesa.Jugando)
                {
                    mBOL.Plantarse(mesa);
                    mesa = mBOL.CargarPartida(mesa);
                    foreach (EUsuario u in jugadores)
                    {
                        if (u.Id == mesa.JugadorAct)
                        {
                            if (u.Aposto == false)
                            {
                                mBOL.Plantarse(mesa);
                                mesa = mBOL.CargarPartida(mesa);
                            }
                        }
                    }
                }
            }
        }
    }
}
