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
    public partial class ComprarCombustible : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        bool compraRealizada = false;
        public ComprarCombustible()
        {
            try
            {
                InitializeComponent();

                log.Debug("Se abrió la ventana Comprar Combustible");
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ComprarCombustible_Load(object sender, EventArgs e)
        {
            try
            {
                // TODO: This line of code loads data into the 'data.tblCombustible' table. You can move, or remove it, as needed.
                this.tblCombustibleTableAdapter.Fill(this.data.tblCombustible);
                // TODO: This line of code loads data into the 'data.tblProveedor' table. You can move, or remove it, as needed.
                this.tblProveedorTableAdapter.Fill(this.data.tblProveedor);

            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void txtPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                Metodos.SoloNumConPunto(sender, e);

            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRealizarCompra_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtCantidad.Text != "" && txtPrecio.Text != "")
                {
                    decimal precio = Convert.ToDecimal(txtPrecio.Text),
                            cantidad = Convert.ToDecimal(txtCantidad.Text);

                    int idProveedor = Convert.ToInt32(cboProveedor.SelectedValue),
                        idCombustible = Convert.ToInt32(cboCombustible.SelectedValue);

                    tblProveedorCombustibleTableAdapter compraAdapter = new tblProveedorCombustibleTableAdapter();

                    compraAdapter.InsertarCompraCombustible(idProveedor, idCombustible, precio, cantidad);

                    compraRealizada = true;

                    MessageBox.Show($"Se realizó la compra de {txtCantidad.Text} de {cboCombustible.Text}", "Compra realizada", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    log.Info($"Se compraron {txtCantidad.Text} galones de {cboCombustible.Text} al proveedor {cboProveedor.Text} al precio de {txtPrecio.Text}");

                    txtCantidad.Text = "";
                    txtPrecio.Text = "";
                }
                else
                {
                    string mensaje = "Falta por completar los siguientes datos:\n";

                    if (txtPrecio.Text == "")
                    {
                        mensaje += "\nPrecio por galón";
                    }
                    if (txtCantidad.Text == "")
                    {
                        mensaje += "\nCantidad de galones";
                    }

                    MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



        }

        private void ComprarCombustible_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                log.Debug("Se cerró la ventana Comprar Combustible");
                if (compraRealizada == true)
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
