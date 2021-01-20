using ProyectoAutoCafeBar.DataTableAdapters;
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
    public partial class Ingreso_Egreso : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        bool aplicado = false;

        tblTipoEgresoTableAdapter tipoEgresoAdapter = new tblTipoEgresoTableAdapter();
        tblTipoIngresoTableAdapter tipoIngresoAdapter = new tblTipoIngresoTableAdapter();

        public Ingreso_Egreso(string Tipo)
        {
            try
            {
                InitializeComponent();

                if (Tipo == "Egreso")
                {
                    this.Text = "Agregar Egreso";
                    lblMovimiento.Text = "Agregar Egreso";

                    lblFecha.Text += "Egreso";

                    cboTipo.DisplayMember = "TipoEgreso";
                    cboTipo.ValueMember = "IdTipoEgreso";

                    cboTipo.DataSource = tipoEgresoAdapter.TiposEgreso();
                }
                else if (Tipo == "Ingreso")
                {
                    this.Text = "Agregar Ingreso";
                    lblMovimiento.Text = "Agregar Ingreso";

                    lblFecha.Text += "Ingreso";

                    cboTipo.DisplayMember = "TipoIngreso";
                    cboTipo.ValueMember = "IdTipoIngreso";

                    cboTipo.DataSource = tipoIngresoAdapter.TiposIngreso();
                }

                log.Debug($"Se abrió la ventana {this.Text}");
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void Limpiar()
        {
            txtConcepto.Text = "";
            txtMonto.Text = "";
            dtpFecha.Value = DateTime.Now;
            cboTipo.SelectedIndex = 0;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtConcepto.Text.Trim() != "" && txtMonto.Text != "")
                {
                    int idTipo = Convert.ToInt32(cboTipo.SelectedValue);
                    decimal monto = Convert.ToDecimal(txtMonto.Text);

                    if (lblMovimiento.Text == "Agregar Egreso")
                    {
                        EgresosConTipoTableAdapter egresoAdapter = new EgresosConTipoTableAdapter();

                        egresoAdapter.InsertarEgreso(idTipo, 0, txtConcepto.Text.Trim(), monto, dtpFecha.Value);

                        MessageBox.Show($"Se insertó un nuevo egreso con un monto de RD${monto}", "Egreso insertado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        log.Info($"Se insertó un nuevo egreso con un monto de RD${txtMonto.Text} dentro de la categoría {cboTipo.Text} con el concepto: {txtConcepto.Text}");
                    }
                    else
                    {
                        IngresosConTipoTableAdapter ingresoAdapter = new IngresosConTipoTableAdapter();

                        ingresoAdapter.InsertarIngreso(idTipo, 0, txtConcepto.Text.Trim(), monto, dtpFecha.Value);

                        MessageBox.Show($"Se insertó un nuevo ingreso con un monto de RD${monto}", "Ingreso insertado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        log.Info($"Se insertó un nuevo ingreso con un monto de RD${txtMonto.Text} dentro de la categoría {cboTipo.Text} con el concepto: {txtConcepto.Text}");

                    }

                    Limpiar();
                    aplicado = true;
                }
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void Ingreso_Egreso_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                log.Debug($"Se cerró la ventana {this.Text}");
                if (aplicado == true)
                {
                    this.DialogResult = DialogResult.Yes;
                }
                else
                {
                    this.DialogResult = DialogResult.No;
                }

            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnRegresar_Click(object sender, EventArgs e)
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
    }
}
