using BlackJackBOL;
using BlackJackENL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlackJack
{
    public partial class FrmLobby : Form
    {
        private EUsuario usuario;
        private EMesa mesa;
        private CorreoBOL cBOL;
        private UsuarioBOL uBOL;
        private List<string> copia;
        private List<EUsuario> listaU;
        private List<EMesa> listaM;
        private MesaBOL mBOL;

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
            mesa = new EMesa();
            mBOL = new MesaBOL();
            cBOL = new CorreoBOL();
            uBOL = new UsuarioBOL();
            copia = new List<string>();
            listaM = new List<EMesa>();
            listaU = new List<EUsuario>();
            listaM = mBOL.CargarTodo();
            panel2.Visible = false;
            panel3.Visible = false;
            btnSolicitar.Visible = false;
            CargarDatos();
        }

        private void CargarDatos()
        {
            listaU = uBOL.CargarTodo();
            for (int i = 0; i < listaU.Count; i++)
            {
                dgvUsuario.Rows.Add(listaU[i].Nombre, listaU[i].Email, listaU[i].Id);
            }
            if (usuario != null)
            {
                lblNom.Text = usuario.Nombre;
                lblApe.Text = usuario.Apellido;
                lblEmail.Text = usuario.Email;
                pbImagen.ImageLocation = usuario.Imagen;
            }
            foreach (EMesa m in listaM)
            {
                bool privada = m.Privada;
                dgvMesa.Rows.Add(m.Id, m.Nombre, m.Turno + "/" + m.Capacidad, privada);
            }
        }

        private void FrmLobby_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Owner != null)
            {
                Owner.Show();
            }
        }

        private void pbImagen_MouseEnter(object sender, EventArgs e)
        {
            panel1.Visible = true;
        }

        private void pbImagen_MouseLeave(object sender, EventArgs e)
        {
            panel1.Visible = false;
        }

        private void depositarToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void EnviarGmail()
        {
            try
            {
                cBOL.EnviarCorreo(usuario.Email, copia, mesa);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string c = dgvUsuario.Rows[e.RowIndex].Cells[1].Value.ToString();
            bool bus = true;
            for (int i = 0; i < copia.Count; i++)
            {
                if (c.Equals(copia[i]))
                {
                    bus = false;
                }
            }
            if (bus)
            {
                copia.Add(c);
                txtCorreo.Text += dgvUsuario.Rows[e.RowIndex].Cells[1].Value.ToString() + System.Environment.NewLine;
            }
        }

        private void dgvMesa_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                int row = e.RowIndex;

                if (row >= 0)
                {
                    int id = Int32.Parse(dgvMesa.Rows[e.RowIndex].Cells[0].Value.ToString());
                    for (int i = 0; i < listaM.Count; i++)
                    {
                        if (listaM[i].Id == id)
                        {
                            mesa = listaM[i];
                            if (mesa.Privada)
                            {
                                btnSolicitar.Visible = true;
                            }
                            else
                            {
                                btnSolicitar.Visible = false;
                                panel2.Visible = false;
                                copia = new List<string>();
                                txtCorreo.Text = "";
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                lblError.Text = "Error al seleccionar mesa.";
            }
        }

        private void crearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmMesa frm = new FrmMesa(1);
            frm.Show(this);
            Hide();
        }

        private void modificarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmMesa frm = new FrmMesa(2);
            frm.Show(this);
            Hide();
        }

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmMesa frm = new FrmMesa(3);
            frm.Show(this);
            Hide();
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                EnviarGmail();
                lblError.Text = "Correo Enviado.";
                txtCorreo.Text = "";
                btnSolicitar.Visible = false;
                panel2.Visible = false;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmJuego frm = new FrmJuego(usuario, mesa);
            frm.Show();
            frm.Owner = this;
            Hide();
        }

        private void dgvMesa_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                int row = e.RowIndex;

                if (row >= 0)
                {
                    int id = Int32.Parse(dgvMesa.Rows[e.RowIndex].Cells[0].Value.ToString());
                    for (int i = 0; i < listaM.Count; i++)
                    {
                        if (listaM[i].Id == id)
                        {
                            mesa = listaM[i];
                            if (mesa.Privada)
                            {
                                panel3.Visible = true;
                            }
                            else
                            {
                                EMesa m = new EMesa();
                                m = mBOL.BuscarMesa(usuario, mesa, "");
                                if (m != null)
                                {
                                    FrmJuego frm = new FrmJuego(usuario, mesa);
                                    frm.Show();
                                    frm.Owner = this;
                                    Hide();
                                }
                                else
                                {
                                    lblError.Text = "Mesa llena.";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                lblError.Text = "Error al seleccionar mesa.";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (mesa != null)
            {
                if (mBOL.VerificarPass(txtPass.Text))
                {
                    panel3.Visible = false;
                    EMesa m = new EMesa();
                    m = mBOL.BuscarMesa(usuario, mesa, txtPass.Text);
                    txtPass.Text = "";
                    if (m != null)
                    {
                        FrmJuego frm = new FrmJuego(usuario, m);
                        frm.Show();
                        frm.Owner = this;
                        Hide();
                    }
                    else
                    {
                        lblError.Text = "Mesa llena.";
                    }
                }
                else
                {
                    lblError.Text = "Contraseña Incorrecta.";
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (mesa != null)
            {
                if (mesa.Privada)
                {
                    panel2.Visible = true;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            listaM = mBOL.CargarTodo();
            usuario = uBOL.VerificarUsuario(usuario);
            dgvMesa.Rows.Clear();
            foreach (EMesa m in listaM)
            {
                bool privada = m.Privada;
                dgvMesa.Rows.Add(m.Id, m.Nombre, m.Turno + "/" + m.Capacidad, privada);
            }
        }

        private void estadisticaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmEstadistica frm = new FrmEstadistica(usuario);
            frm.Show(this);
            Hide();
        }
    }
}
