using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoAutoCafeBar
{
    public partial class ElegirFecha : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ElegirFecha()
        {
            try
            {
                InitializeComponent();

                log.Debug("Se abrió la ventana Elegir Fechas");
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dtpFechaInicio.Value < dtpFechaFinal.Value)
                {
                    this.DialogResult = DialogResult.Yes;
                }
                else
                {
                    MessageBox.Show("La fecha final debe ser mayor que la fecha inicial", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();

            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ElegirFecha_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                log.Info("Se cerró la ventana Elegir Fechas");
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
