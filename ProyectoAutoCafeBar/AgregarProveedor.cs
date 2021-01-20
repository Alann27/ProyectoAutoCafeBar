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
    public partial class AgregarProveedor : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        bool proveedorAgregado = false;
        public AgregarProveedor()
        {
            try
            {
                InitializeComponent();

                log.Debug("Se abrió la ventana Agregar Proveedor");
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtRNC.MaskFull && txtNombre.Text.Trim() != "")
                {
                    tblProveedorTableAdapter proveedorAdapter = new tblProveedorTableAdapter();

                    string telefono = txtTelefono.MaskFull == true ? txtTelefono.Text.Remove(3, 1).Remove(6, 1) : "";

                    proveedorAdapter.InsertarProveedor(txtRNC.Text, txtNombre.Text.Trim(), telefono);

                    proveedorAgregado = true;

                    MessageBox.Show($"{txtNombre.Text.Trim()} agregado como proveedor", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    log.Info($"Se agregó al proveedor con el RNC {txtRNC.Text} y el nombre {txtNombre.Text}");

                    txtNombre.Text = "";
                    txtRNC.Text = "";
                    txtTelefono.Text = "";
                    
                }
                else
                {
                    string mensaje = "Falta por completar los siguientes datos:\n";

                    if (txtRNC.MaskFull == false)
                    {
                        mensaje += "\nRNC";
                    }
                    if (txtNombre.Text.Trim() == "")
                    {
                        mensaje += "\nNombre";
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

        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                Metodos metodos = new Metodos();
                metodos.SoloLetras(sender, e);
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void AgregarProveedor_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                log.Debug("Se cerró la ventana Agregar Proveedor");

                if (proveedorAgregado ==true)
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
