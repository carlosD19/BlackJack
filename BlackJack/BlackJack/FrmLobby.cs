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
            cBOL = new CorreoBOL();
            uBOL = new UsuarioBOL();
            copia = new List<string>();
            listaM = new List<EMesa>();
            listaU = new List<EUsuario>();
            richTextBox1.Text += "Destinatarios:" + System.Environment.NewLine;
            panel2.Visible = false;
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
                pbImagen.Load(usuario.Imagen);
                //Image img;
                //WebRequest request = WebRequest.Create(usuario.Imagen);
                //using (var response = request.GetResponse())
                //{
                //    using (var str = response.GetResponseStream())
                //    {
                //        img = Bitmap.FromStream(str);
                //        pbImagen.Image = img;
                //    }
                //}

            }
            dgvMesa.DataSource = listaM;
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
                richTextBox1.Text += dgvUsuario.Rows[e.RowIndex].Cells[1].Value.ToString() + System.Environment.NewLine;
            }
        }

        private void dgvMesa_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                int row = e.RowIndex;

                if (row >= 0)
                {
                    mesa = dgvMesa.CurrentRow.DataBoundItem as EMesa;
                    if (mesa.Privada)
                    {
                        panel2.Visible = true;
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
                panel2.Visible = false;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }

        }

        private void dgvMesa_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int row = e.RowIndex;

                if (row >= 0)
                {
                    mesa = dgvMesa.CurrentRow.DataBoundItem as EMesa;
                    if (mesa.Privada)
                    {
                        panel2.Visible = true;
                    }
                    else
                    {
                        FrmJuego frm = new FrmJuego(usuario, mesa);
                        frm.Show();
                        frm.Owner = this;
                        Hide();
                    }
                }
            }
            catch (Exception)
            {
                lblError.Text = "Error al seleccionar mesa.";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmJuego frm = new FrmJuego(usuario, mesa);
            frm.Show();
            frm.Owner = this;
            Hide();
        }
    }
}
