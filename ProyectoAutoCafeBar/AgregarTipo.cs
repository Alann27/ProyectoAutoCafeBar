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
    public partial class AgregarTipo : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        bool agregadoTipo = false;
        Metodos metodos = new Metodos();
        public AgregarTipo(string Tipo)
        {
            try
            {
                InitializeComponent();
                if (Tipo == "Empleado")
                {
                    lblTipo.Text = "Agregar Tipo de Empleado";
                    this.Text = "Agregar Tipo de Empleado";
                }
                else if (Tipo == "Egreso")
                {
                    lblTipo.Text = "Agregar Tipo de Egreso";
                    this.Text = "Agregar Tipo de Egreso";
                }
                else if (Tipo == "Ingreso")
                {
                    lblTipo.Text = "Agregar Tipo de Ingreso";
                    this.Text = "Agregar Tipo de Ingreso";
                }

                log.Info($"Se abrió la ventana {Text}");
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
                if (txtNombre.Text != "")
                {
                    if (lblTipo.Text == "Agregar Tipo de Empleado")
                    {
                        TiposDeEmpleadosTableAdapter tipoEmpleadoAdapter = new TiposDeEmpleadosTableAdapter();

                        tipoEmpleadoAdapter.InsertarTipoEmpleado(txtNombre.Text.Trim());

                        MessageBox.Show($"{txtNombre.Text.Trim()} ha sido agregado como tipo de empleado", "Agregar Tipo de Empleado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        log.Info($"Se insertó un tipo de empleado con el nombre {txtNombre.Text.Trim()}");
                    }
                    else if (lblTipo.Text == "Agregar Tipo de Egreso")
                    {
                        tblTipoEgresoTableAdapter tipoEgresoAdapter = new tblTipoEgresoTableAdapter();

                        tipoEgresoAdapter.InsertarTipoDeEgreso(txtNombre.Text.Trim());

                        MessageBox.Show($"{txtNombre.Text} ha sido agregado como tipo de egreso", "Agregar Tipo de Egreso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        log.Info($"Se insertó un tipo de egreso con el nombre {txtNombre.Text.Trim()}");
                    }
                    else if (lblTipo.Text == "Agregar Tipo de Ingreso")
                    {
                        tblTipoIngresoTableAdapter tipoIngresoAdapter = new tblTipoIngresoTableAdapter();

                        tipoIngresoAdapter.InsertarTipoDeIngreso(txtNombre.Text.Trim());

                        MessageBox.Show($"{txtNombre.Text} ha sido agregado como tipo de Ingreso", "Agregar Tipo de Ingreso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        log.Info($"Se insertó un tipo de ingreso con el nombre {txtNombre.Text.Trim()}");
                    }
                    txtNombre.Text = "";
                    agregadoTipo = true;
                }
                else
                {
                    MessageBox.Show("Debe ingresar el nombre del tipo de empleado a agregar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void AgregarTipoEmpleado_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                log.Debug($"Se cerró la ventana {Text}");
                if (agregadoTipo == true)
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

        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                metodos.SoloLetras(sender, e);

            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
