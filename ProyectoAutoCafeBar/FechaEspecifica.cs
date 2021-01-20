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
    public partial class FechaEspecifica : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public FechaEspecifica()
        {
            try
            {
                InitializeComponent();

                log.Debug("Se abrió la ventana Elegir Día Específico");
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
                this.DialogResult = DialogResult.Yes;

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

        private void FechaEspecifica_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                log.Debug("Se cerró la ventana Elegir Día Específico");

                if (this.DialogResult != DialogResult.Yes)
                {
                    this.DialogResult = DialogResult.Cancel;
                }


            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
