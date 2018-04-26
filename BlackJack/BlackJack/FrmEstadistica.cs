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
using System.Windows.Forms.DataVisualization.Charting;

namespace BlackJack
{
    public partial class FrmEstadistica : Form
    {
        private EUsuario usu;
        public FrmEstadistica()
        {
            InitializeComponent();
        }
        public FrmEstadistica(EUsuario usuario)
        {
            InitializeComponent();
            CenterToScreen();
            usu = usuario;
        }

        private void FrmEstadistica_Load(object sender, EventArgs e)
        {
            string[] series = { "Dinero", "Ganado", "Apostado" };
            double[] cantidad = { usu.Dinero, usu.Ganado, usu.Apostado };

            chartEstadistica.Titles.Add("Estadísticas");
            for (int i = 0; i < series.Length; i++)
            {
                Series serie = chartEstadistica.Series.Add(series[i]);
                serie.Label = cantidad[i].ToString();
                serie.Points.Add(cantidad[i]);
            }
        }

        private void FrmEstadistica_FormClosing(object sender, FormClosingEventArgs e)
        {
            Owner?.Show();
        }
    }
}
