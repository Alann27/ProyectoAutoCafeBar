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
using static ProyectoAutoCafeBar.Data;

namespace ProyectoAutoCafeBar
{
    public partial class AgregarEmpleado : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        bool agregoEmpleados = false;
        int idEmpleado = 0, idTipoEmpleado = 0;
        Metodos metodos = new Metodos();
        public AgregarEmpleado()
        {
            try
            {
                InitializeComponent();

                cboEstado.SelectedIndex = 0;

                log.Debug("Se abrió la ventana Agregar Empleado");
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        public AgregarEmpleado(int IdEmpleado)
        {
            try
            {
                InitializeComponent();
                idEmpleado = IdEmpleado;
                lblEmpleado.Text = "Modificar Empleado";
                btnAgregar.Text = "Modificar";

                tblEmpleadoTableAdapter empleadoAdapter = new tblEmpleadoTableAdapter();
                tblEmpleadoRow empleadoFila = empleadoAdapter.EmpleadoPorId(idEmpleado)[0];

                txtCedula.Text = empleadoFila.Cedula;
                txtNombre.Text = empleadoFila.Nombre;
                txtApellidos.Text = empleadoFila.Apellidos;
                txtSalario.Text = empleadoFila.Salario.ToString();
                txtTelefono.Text = empleadoFila.Telefono;
                idTipoEmpleado = empleadoFila.idTipoEmpleado;
                txtDireccion.Text = empleadoFila.Direccion;
                cboEstado.SelectedIndex = empleadoFila.Activo == true ? 0 : 1;

                txtCedula.Enabled = false;
                cboEstado.Enabled = false;

                log.Debug($"Se abrió la ventana Modificar Empleado para modificar al empleado con el ID: {idEmpleado}");
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void AgregarEmpleado_Load(object sender, EventArgs e)
        {
            try
            {
                // TODO: This line of code loads data into the 'data.TiposDeEmpleados' table. You can move, or remove it, as needed.
                this.tiposDeEmpleadosTableAdapter.FTipoEmpleados(this.data.TiposDeEmpleados);
                // TODO: This line of code loads data into the 'data.Dispensadores' table. You can move, or remove it, as needed.
                this.dispensadoresTableAdapter.Fill(this.data.Dispensadores);

                if (idTipoEmpleado != 0)
                {
                    cboTipoEmpleado.SelectedValue = idTipoEmpleado;
                }
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
                if (txtCedula.MaskFull && txtNombre.Text != "" && txtApellidos.Text != "" && txtSalario.Text != "")
                {
                    string cedula = txtCedula.Text.Remove(3, 1).Remove(10, 1);
                    string telefono = txtTelefono.MaskFull == true ? txtTelefono.Text.Remove(3, 1).Remove(6, 1) : "";
                    decimal salario = Convert.ToDecimal(txtSalario.Text);
                    bool estado = cboEstado.SelectedIndex == 0 ? true : false;
                    int idTipoEmpleado = Convert.ToInt32(cboTipoEmpleado.SelectedValue);
                
                    tblEmpleadoTableAdapter empleadoAdapter = new tblEmpleadoTableAdapter();
                    if (btnAgregar.Text == "Agregar")
                    {
                        empleadoAdapter.InsertarEmpleado(idTipoEmpleado, cedula, txtNombre.Text.Trim(), txtApellidos.Text.Trim(),
                        salario, estado, telefono, txtDireccion.Text.Trim());

                        MessageBox.Show($"{txtNombre.Text} {txtApellidos.Text} ha sido agregado como empleado", "Empleado agregado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        log.Info($"Se agregó un nuevo empleado con la cédula {cedula} y el nombre {txtNombre.Text} {txtApellidos.Text}");
                        LimpiarAgregar();
                    }
                    else
                    {
                        empleadoAdapter.ActualizarEmpleado(idEmpleado, idTipoEmpleado, txtNombre.Text.Trim(), txtApellidos.Text.Trim(), 
                        salario, telefono, txtDireccion.Text.Trim());
                        log.Info($"Se modificó al empleado con el ID: {idEmpleado}");
                        MessageBox.Show($"{txtNombre.Text} {txtApellidos.Text} ha sido actualizado", "Empleado actualizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtNombre.Focus();
                    }

                    agregoEmpleados = true;
                }
                else
                {
                    string mensaje = "Falta por completar los siguientes datos:";
                
                    if (!txtCedula.MaskFull)
                    {
                        mensaje += "\nCédula";
                    }
                    if (txtNombre.Text == "")
                    {
                        mensaje += "\nNombre";
                    }
                    if (txtApellidos.Text == "")
                    {
                        mensaje += "\nApellidos";
                    }
                    if (txtSalario.Text == "")
                    {
                        mensaje += "\nSalario";
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

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnAgregar.Text == "Agregar")
                {
                    LimpiarAgregar();
                }
                else
                {
                    LimpiarModificar();
                    txtNombre.Focus();
                }
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void LimpiarModificar()
        {
            try
            {
                txtNombre.Text = "";
                txtApellidos.Text = "";
                txtSalario.Text = "";
                txtTelefono.Text = "";
                txtDireccion.Text = "";
                cboTipoEmpleado.SelectedValue = 3;
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void LimpiarAgregar()
        {
            try
            {
                LimpiarModificar();
                txtCedula.Text = "";
                cboEstado.SelectedIndex = 0;
                txtCedula.Focus();
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

        private void txtSalario_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtDireccion_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                metodos.SoloLetrasyNum(sender, e);

            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AgregarEmpleado_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                log.Info($"Se cerró la ventana {btnAgregar.Text} Empleado");
                if (agregoEmpleados == true)
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
    }
}
