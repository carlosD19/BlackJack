﻿using BlackJackBOL;
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
    public partial class FrmMesa : Form
    {
        private List<EMesa> lista;
        private MesaBOL bol;
        private int funcion;
        private EMesa mesa;

        public FrmMesa()
        {
            InitializeComponent();
        }

        public FrmMesa(int fun)
        {
            funcion = fun;
            InitializeComponent();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            switch (funcion)
            {
                case 1:
                    Guardar();
                    break;
                case 2:
                    Modificar();
                    break;
                case 3:
                    Eliminar();
                    break;
                default:
                    break;
            }
        }

        private void FrmMesa_Load(object sender, EventArgs e)
        {
            lista = new List<EMesa>();
            mesa = new EMesa();
            bol = new MesaBOL();
            CargarTabla();
            CambiarTexto();
        }

        private void CambiarTexto()
        {
            switch (funcion)
            {
                case 1:
                    btnAceptar.Text = "Guardar";
                    break;
                case 2:
                    btnAceptar.Text = "Modificar";
                    break;
                case 3:
                    btnAceptar.Text = "Eliminar";
                    break;
                default:
                    break;
            }
        }

        private void Guardar()
        {
            mesa = new EMesa();
            try
            {
                string rePass = txtRePass.Text;
                mesa.Capacidad = (int)txtCapacidad.Value;
                mesa.Nombre = txtNombre.Text;
                mesa.Privada = true;
                mesa.Pass = txtPass.Text;
                if (bol.Agregar(mesa, rePass))
                {
                    lblError.Text = "Mesa registrada.";
                    CargarTabla();
                }
                else
                {
                    lblError.Text = "Error al agregar.";
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
        private void Modificar()
        {
            try
            {
                if (mesa != null)
                {
                    EMesa temp = new EMesa();
                    temp.Id = mesa.Id;
                    temp.Nombre = txtNombre.Text;
                    temp.Pass = txtPass.Text;
                    temp.Privada = true;
                    temp.Capacidad = (int)txtCapacidad.Value;
                    if (bol.Modificar(temp, txtRePass.Text))
                    {
                        lblError.Text = "Mesa modificada.";
                        CargarTabla();
                    }
                    else
                    {
                        lblError.Text = "Error al modificar.";
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
        private void Eliminar()
        {
            try
            {
                if (mesa != null)
                {
                    if (bol.Eliminar(mesa))
                    {
                        lblError.Text = "Mesa Eliminada.";
                        CargarTabla();
                    }
                    else
                    {
                        lblError.Text = "Error al eliminar.";
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
        private void CargarTabla()
        {
            lista = bol.CargarTodo();
            dgvMesa.DataSource = lista;
        }

        private void dgvMesa_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (funcion == 2 || funcion == 3)
                {
                    int row = e.RowIndex;

                    if (row >= 0)
                    {
                        mesa = dgvMesa.CurrentRow.DataBoundItem as EMesa;
                        CargarDatos();
                    }
                }
            }
            catch (Exception)
            {
                lblError.Text = "Error al seleccionar fila.";
            }
        }

        private void CargarDatos()
        {
            if (mesa.Activo)
            {
                if (mesa.Privada)
                {
                    txtNombre.Text = mesa.Nombre;
                    txtCapacidad.Value = mesa.Capacidad;
                    txtPass.Text = mesa.Pass;
                }
                else
                {
                    mesa = null;
                }
            }
        }

        private void FrmMesa_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Owner != null)
            {
                Owner.Show();
            }
        }
    }
}
