using HtmlAgilityPack;
using ProyectoAutoCafeBar.DataTableAdapters;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Transactions;
using System.Windows.Forms;
using ProyectoAutoCafeBar.InformesTableAdapters;
using static ProyectoAutoCafeBar.Data;
using Microsoft.Reporting.WinForms;
using log4net;

namespace ProyectoAutoCafeBar
{
    public partial class formulario : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        Metodos Metodos = new Metodos();
        public formulario()
        {
            try
            {
                InitializeComponent();

                log.Debug("Se inició el sistema");
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}");
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                // TODO: This line of code loads data into the 'data8.UltimasCompras' table. You can move, or remove it, as needed.
                this.ultimasComprasTableAdapter.FUltimasCompras(this.data8.UltimasCompras);
                // TODO: This line of code loads data into the 'data7.TotalTipoIngreso15Dias' table. You can move, or remove it, as needed.
                this.totalTipoIngreso15DiasTableAdapter.FTotalTipoIngresos15Dias(this.data7.TotalTipoIngreso15Dias);
                // TODO: This line of code loads data into the 'data6.IngresosConTipo' table. You can move, or remove it, as needed.
                this.ingresosConTipoTableAdapter.FIngresosConTipo(this.data6.IngresosConTipo);
                // TODO: This line of code loads data into the 'data2.TotalTipoEgreso15Dias' table. You can move, or remove it, as needed.
                this.totalTipoEgreso15DiasTableAdapter.FTotalTipoEgreso15Dias(this.data2.TotalTipoEgreso15Dias);
                // TODO: This line of code loads data into the 'data2.EgresosConTipo' table. You can move, or remove it, as needed.
                this.egresosConTipoTableAdapter.FEgresosConTipo(this.data2.EgresosConTipo);
                // TODO: This line of code loads data into the 'data5.tblCombustible' table. You can move, or remove it, as needed.
                this.tblCombustibleTableAdapter.Fill(this.data5.tblCombustible);
                // TODO: This line of code loads data into the 'data2.tblProveedor' table. You can move, or remove it, as needed.
                this.tblProveedorTableAdapter.Fill(this.data2.tblProveedor);
                // TODO: This line of code loads data into the 'data2.tblCombustible' table. You can move, or remove it, as needed.
                this.tblCombustibleTableAdapter.Fill(this.data2.tblCombustible);
                // TODO: This line of code loads data into the 'data2.tblCombustible' table. You can move, or remove it, as needed.
                this.tblCombustibleTableAdapter.Fill(this.data2.tblCombustible);
                // TODO: This line of code loads data into the 'data4.TiposDeEmpleados' table. You can move, or remove it, as needed.
                this.tiposDeEmpleadosTableAdapter.FTipoEmpleados(this.data4.TiposDeEmpleados);
                // TODO: This line of code loads data into the 'data3.EmpleadosConTipo' table. You can move, or remove it, as needed.
                this.empleadosConTipoTableAdapter.FEmpleados(this.data3.EmpleadosConTipo);
                // TODO: This line of code loads data into the 'data2.Dispensadores' table. You can move, or remove it, as needed.
                this.dispensadoresTableAdapter.Fill(this.data2.Dispensadores);
                // TODO: This line of code loads data into the 'data1.tblEmpleado' table. You can move, or remove it, as needed.
                this.tblEmpleadoTableAdapter.Fill(this.data1.tblEmpleado);
                // TODO: This line of code loads data into the 'data.Dispensadores' table. You can move, or remove it, as needed.
                this.dispensadoresTableAdapter.Fill(this.data.Dispensadores);
                // TODO: This line of code loads data into the 'data.Dispensadores' table. You can move, or remove it, as needed.
                this.dispensadoresTableAdapter.Fill(this.data.Dispensadores);
                pictLogin.Controls.Add(panelAuxiliar);

                CargarDashboard();
                CargarReporteMovimientos(DateTime.Today, DateTime.Now, $"Reporte de hoy - {DateTime.Now.ToString()}");
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtUsuario.Text != "" && txtContrasena.Text != "")
                {
                    tblPerfilTableAdapter perfilAdapter = new tblPerfilTableAdapter();
                    tblPerfilDataTable perfilData = perfilAdapter.GetPerfilPorUsuario(txtUsuario.Text);

                    if (perfilData.Rows.Count > 0)
                    {
                        tblPerfilRow filaPerfil = perfilData[0];

                        if (txtContrasena.Text == filaPerfil.Contraseña)
                        {
                            MessageBox.Show($"{filaPerfil.Nombre} ingresaste al sistema", "Entrada al sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            pnlMenu.Visible = true;

                            lblNombreUsuario.Text = filaPerfil.Nombre;

                            log.Debug($"{filaPerfil.Nombre} inició sesión en el sistema");

                            pnlLogin.Visible = false;
                            pictFondoLogin.Visible = false;
                            pnlDashboard.Visible = true;
                        }
                        else
                        {
                            log.Warn($"Intentaron iniciar sesion con la cuenta de {filaPerfil.Usuario} pero ingresaron una contraseña incorrecta");
                            MessageBox.Show("Usuario o contraseña incorrectos, favor intente de nuevo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }
                    else
                    {
                        log.Warn("Intentaron iniciar sesión en el sistema pero el nombre de usuario ingresado no pertenece a ninguna cuenta");
                        MessageBox.Show("Usuario o contraseña incorrectos, favor intente de nuevo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    string mensaje = "Faltan los siguientes datos: ";
                    if (txtUsuario.Text == "")
                    {
                        mensaje += "\nNombre de usuario";
                    }
                    if (txtContrasena.Text == "")
                    {
                        mensaje += "\nContraseña";
                    }
                    MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"{error.Message}");
            }
        }

        private void txtUsuario_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {

                if (!(char.IsLetterOrDigit(e.KeyChar)) && (e.KeyChar != (char)Keys.Back) && (e.KeyChar != '-') && (e.KeyChar != '_'))
                {
                    e.Handled = true;
                    return;
                }
                
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}");
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
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

        private void CargarDashboard()
        {
            try
            {
                FuncionesTableAdapter funcionesTableAdapter = new FuncionesTableAdapter();


                decimal pagoDeNomina = Convert.ToDecimal(funcionesTableAdapter.PagoDeNomina()[0].Valor.ToString()),
                        sobranteFaltante = Convert.ToDecimal(funcionesTableAdapter.SobranteAcumulado()[0].Valor.ToString()),
                        faltanteAcumulado = Convert.ToDecimal(funcionesTableAdapter.FaltanteAcumulado()[0].Valor.ToString());

                lblEmpleadosActivos.Text = funcionesTableAdapter.EmpleadosActivos()[0].Valor.ToString();
                lblPagoDeNomina.Text = string.Format("{0:0,0.00}", pagoDeNomina);
                lblSobrantesAcumulados.Text = string.Format("{0:0,0.00}",sobranteFaltante);
                lblFaltantesAcumulado.Text = string.Format("{0:0,0.00}",faltanteAcumulado);

                lblEntrada.Text = string.Format("{0:0,0.00}", funcionesTableAdapter.EntradaHoy()[0].Valor);
                lblSalida.Text = string.Format("{0:0,0.00}", funcionesTableAdapter.SalidaHoy()[0].Valor);
                lblNeto.Text = string.Format("{0:0,0.00}", funcionesTableAdapter.NetoHoy()[0].Valor);

                pictEmpleadosActivos.Controls.Add(lblEmpleadosActivos);
                pictPagoDeNomina.Controls.Add(lblPagoDeNomina);
                pictSobrantesAcumulados.Controls.Add(lblSobrantesAcumulados);
                pictFaltanteAcumulado.Controls.Add(lblFaltantesAcumulado);

                pictEntrada.Controls.Add(lblEntrada);
                pictSalida.Controls.Add(lblSalida);
                pictNeto.Controls.Add(lblNeto);

                int ventaDia = Convert.ToInt32(funcionesTableAdapter.HayVentaHoy()[0].Valor);
            
                GananciaPorCombustibleTableAdapter ventasAdapter = new GananciaPorCombustibleTableAdapter();
                GananciaPorCombustibleDataTable ventasData = ventasAdapter.GetData(ventaDia);

                lblVentasHoYer.Text = ventaDia == 0 ? "Ventas\nde\nHoy" : "Ventas\nde\nAyer";

                chartVentas.Series[0].Points.DataBindXY(ventasData.Rows, "Combustible", ventasData.Rows, "Generado");

            

                int ventaDisp1 = Convert.ToInt32(funcionesTableAdapter.HayNumeracionHoy(1)[0].Valor);
                int ventaDisp2 = Convert.ToInt32(funcionesTableAdapter.HayNumeracionHoy(2)[0].Valor);
                int ventaDisp3 = Convert.ToInt32(funcionesTableAdapter.HayNumeracionHoy(3)[0].Valor);

                lblChartDisp1.Text = ventaDisp1 == 1 ? "Dispensador #1 - Hoy" : "Dispensador #1 - Ayer";
                lblChartDisp2.Text = ventaDisp2 == 1 ? "Dispensador #2 - Hoy" : "Dispensador #2 - Ayer";
                lblChartDisp3.Text = ventaDisp3 == 1 ? "Dispensador #3 - Hoy" : "Dispensador #3 - Ayer";

                ventaDisp1 = ventaDisp1 == 1 ? 0 : 1;
                ventaDisp2 = ventaDisp2 == 1 ? 0 : 1;
                ventaDisp3 = ventaDisp3 == 1 ? 0 : 1;

                GalonesDeCombustibleTableAdapter galonesAdapter = new GalonesDeCombustibleTableAdapter();
                GalonesDeCombustibleDataTable galonesData1 = galonesAdapter.Dispensador(ventaDisp1, 1);
                GalonesDeCombustibleDataTable galonesData2 = galonesAdapter.Dispensador(ventaDisp2, 2);
                GalonesDeCombustibleDataTable galonesData3 = galonesAdapter.Dispensador(ventaDisp3, 3);
                GalonesDeCombustibleDataTable galonesRestantesData = galonesAdapter.CombustibleRestante();

                chartDispensador1.Series[0].Points.DataBindXY(galonesData1.Rows, "Combustible", galonesData1.Rows, "Galones");
                chartDispensador2.Series[0].Points.DataBindXY(galonesData2.Rows, "Combustible", galonesData2.Rows, "Galones");
                chartDispensador3.Series[0].Points.DataBindXY(galonesData3.Rows, "Combustible", galonesData3.Rows, "Galones");

                chartCombustibleRestante.Series[0].Points.DataBindXY(galonesRestantesData.Rows, "Combustible", galonesRestantesData.Rows, "Galones");

                log.Debug("Se abrió el apartado Dashboard");
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }





        }

        private void txtContrasena_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblContrasena_Click(object sender, EventArgs e)
        {

        }


        private void pictColapseMenu_Click(object sender, EventArgs e)
        {
            try
            {
                if (pnlMenu.Width != 88)
                {
                    pnlMenu.Size = new Size(88, pnlMenu.Height);
                    pictColapseMenu.Location = new Point(12, 0);
                    lblBienvenido.Visible = false;
                    lblNombreUsuario.Visible = false;
                    lblDesarrollo.Visible = false;
                }
                else
                {
                    pnlMenu.Size = new Size(249, pnlMenu.Height);
                    pictColapseMenu.Location = new Point(184, 0);
                    lblBienvenido.Visible = true;
                    lblNombreUsuario.Visible = true;
                    lblDesarrollo.Visible = true;
                }
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}");
            }
        }

        private void btnDispensadores_Click(object sender, EventArgs e)
        {
            try
            {
                if (pnlDispensadores.Visible == false)
                {
                    log.Debug("Se abrió el apartado Dispensadores");

                    LimpiarTextBoxDispensadores();

                    ActualizarDispensadores();

                    pnlReportes.Visible = false;
                    pnlDashboard.Visible = false;
                    pnlMenu.Dock = DockStyle.Left;
                    pnlEmpleados.Visible = false;
                    pnlCombustibles.Visible = false;
                    pnlEgresos.Visible = false;
                    pnlIngresos.Visible = false;
                    pnlDispensadores.Visible = true;
                    pnlElegirDispensadores.Visible = true;
                }

            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        private void pictDispensador1_Click(object sender, EventArgs e)
        {
            try
            {
                PictureBox dispensador = (PictureBox)sender;

                foreach (PictureBox item in pnlElegirDispensadores.Controls)
                {
                    if (item == dispensador)
                    {
                        dispensador.BackColor = Color.LightBlue;
                        dispensador.BorderStyle = BorderStyle.None;
                    }
                    else
                    {
                        item.BackColor = Color.White;
                        item.BorderStyle = BorderStyle.FixedSingle;
                    }
                }
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pnlDispensadores_SizeChanged(object sender, EventArgs e)
        {

        }

        private void pnlDispensador1LadoA_SizeChanged(object sender, EventArgs e)
        {


        }

        private void pnlDispensador1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void pictDispensador1_BackColorChanged(object sender, EventArgs e)
        {
            try
            {
                if (pictDispensador1.BackColor == Color.LightBlue)
                {
                    pnlDispensador1.Visible = true;
                    pnlDispensador2.Visible = false;
                    pnlDispensador3.Visible = false;
                }
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void pictDispensador2_BackColorChanged(object sender, EventArgs e)
        {
            try
            {
                if (pictDispensador2.BackColor == Color.LightBlue)
                {
                    pnlDispensador2.Visible = true;
                    pnlDispensador1.Visible = false;
                    pnlDispensador3.Visible = false;
                }
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void pictDispensador3_BackColorChanged(object sender, EventArgs e)
        {
            try
            {
                if (pictDispensador3.BackColor == Color.LightBlue)
                {
                    pnlDispensador3.Visible = true;
                    pnlDispensador2.Visible = false;
                    pnlDispensador1.Visible = false;
                }
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void pnlDispensadores_VisibleChanged(object sender, EventArgs e)
        {
            try
            {
                if (pnlDispensadores.Visible == true)
                {
                    ActualizarDispensadores();
                }
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void ActualizarDispensadores()
        {
            try
            {
                tblEmpleadoTableAdapter empleadoAdapter = new tblEmpleadoTableAdapter();

                NumeracionesTableAdapter numeracionesTableAdapter = new NumeracionesTableAdapter();

                cboBomberos1.DataSource = empleadoAdapter.Bomberos();
                cboBombero2.DataSource = empleadoAdapter.Bomberos();
                cboBombero3.DataSource = empleadoAdapter.Bomberos();

                dgvLadoA1.DataSource = numeracionesTableAdapter.Numeraciones(1,"A");
                dgvLadoB1.DataSource = numeracionesTableAdapter.Numeraciones(1, "B");
                dgvLadoA2.DataSource = numeracionesTableAdapter.Numeraciones(2, "A");
                dgvLadoB2.DataSource = numeracionesTableAdapter.Numeraciones(2, "B");
                dgvLadoA3.DataSource = numeracionesTableAdapter.Numeraciones(3, "A");
                dgvLadoB3.DataSource = numeracionesTableAdapter.Numeraciones(3, "B");

                FuncionesTableAdapter funcionesAdapter = new FuncionesTableAdapter();

                int entregaHoy = Convert.ToInt32(funcionesAdapter.HayNumeracionHoy(1)[0].Valor.ToString());

                if (entregaHoy == 1)
                {
                    UltimosIngresosEntregadosTableAdapter ingresosAdapter = new UltimosIngresosEntregadosTableAdapter();
                    UltimosIngresosEntregadosRow ingresosData = ingresosAdapter.GetData(1)[0];

                    txtGeneradoHoy1.Text = ingresosData.TotalGenerado.ToString();
                    txtEfectivo1.Text = ingresosData.EfectivoEntregado.ToString();
                    txtVales1.Text = ingresosData.Vales.ToString();
                    txtTarjeta1.Text = ingresosData.Credito.ToString();
                    cboBomberos1.SelectedValue = ingresosData.IdEmpleado;
                    cboBomberos1.Enabled = false;

                    bloquearTextBox(txtEfectivo1);
                    bloquearTextBox(txtVales1);
                    bloquearTextBox(txtTarjeta1);

                    btnAplicar1.Enabled = false;
                    btnCalcular1.Enabled = false;
                    dgvLadoA1.ReadOnly = true;
                    dgvLadoB1.ReadOnly = true;
                }

                entregaHoy = Convert.ToInt32(funcionesAdapter.HayNumeracionHoy(2)[0].Valor.ToString());

                if (entregaHoy == 1)
                {
                    UltimosIngresosEntregadosTableAdapter ingresosAdapter = new UltimosIngresosEntregadosTableAdapter();
                    UltimosIngresosEntregadosRow ingresosData = ingresosAdapter.GetData(2)[0];

                    txtGeneradoHoy2.Text = ingresosData.TotalGenerado.ToString();
                    txtEfectivo2.Text = ingresosData.EfectivoEntregado.ToString();
                    txtVales2.Text = ingresosData.Vales.ToString();
                    txtTarjeta2.Text = ingresosData.Credito.ToString();
                    cboBombero2.SelectedValue = ingresosData.IdEmpleado;
                    cboBombero2.Enabled = false;

                    bloquearTextBox(txtEfectivo2);
                    bloquearTextBox(txtVales2);
                    bloquearTextBox(txtTarjeta2);

                    btnAplicar2.Enabled = false;
                    btnCalcular2.Enabled = false;
                    dgvLadoA2.ReadOnly = true;
                    dgvLadoB2.ReadOnly = true;
                }

                entregaHoy = Convert.ToInt32(funcionesAdapter.HayNumeracionHoy(3)[0].Valor.ToString());

                if (entregaHoy == 1)
                {
                    UltimosIngresosEntregadosTableAdapter ingresosAdapter = new UltimosIngresosEntregadosTableAdapter();
                    UltimosIngresosEntregadosRow ingresosData = ingresosAdapter.GetData(3)[0];

                    txtGeneradoHoy3.Text = ingresosData.TotalGenerado.ToString();
                    txtEfectivo3.Text = ingresosData.EfectivoEntregado.ToString();
                    txtVales3.Text = ingresosData.Vales.ToString();
                    txtTarjeta3.Text = ingresosData.Credito.ToString();
                    cboBombero3.SelectedValue = ingresosData.IdEmpleado;
                    cboBombero3.Enabled = false;

                    bloquearTextBox(txtEfectivo3);
                    bloquearTextBox(txtVales3);
                    bloquearTextBox(txtTarjeta3);

                    btnAplicar3.Enabled = false;
                    btnCalcular3.Enabled = false;
                    dgvLadoA3.ReadOnly = true;
                    dgvLadoB3.ReadOnly = true;
                }
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void dgv_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                e.Control.KeyPress -= new KeyPressEventHandler(textBox_KeyPress);
                DataGridView dgv = (DataGridView)sender;
                if (dgv.CurrentCell.ColumnIndex == 3) //Desired Column
                {
                    TextBox tb = e.Control as TextBox;
                    if (tb != null)
                    {
                        tb.KeyPress += new KeyPressEventHandler(textBox_KeyPress);
                    }
                }
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
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

        private bool TodoCorrecto(DataGridView dgv1, DataGridView dgv2)
        {
            try
            {
                bool todoCorrecto = true;

                foreach (DataGridViewRow fila in dgv1.Rows)
                {
                    if (fila.Cells[3].Value != null && fila.Cells[3].Value.ToString() != "")
                    {
                        decimal valorHoy = Convert.ToDecimal(fila.Cells[3].Value.ToString());
                        decimal valorAyer = Convert.ToDecimal(fila.Cells[2].Value.ToString());

                        if (valorHoy < valorAyer)
                        {
                            return todoCorrecto = false;
                        }

                    }
                    else
                    {
                        return todoCorrecto = false;
                    }

                }

                foreach (DataGridViewRow fila in dgv2.Rows)
                {
                    if (fila.Cells[3].Value != null && fila.Cells[3].Value.ToString() != "")
                    {
                        decimal valorHoy = Convert.ToDecimal(fila.Cells[3].Value.ToString());
                        decimal valorAyer = Convert.ToDecimal(fila.Cells[2].Value.ToString());

                        if (valorHoy < valorAyer)
                        {
                            return todoCorrecto = false;
                        }
                    }
                    else
                    {
                        return todoCorrecto = false;
                    }

                }

                return todoCorrecto;
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }


        private void CalcularDGV(DataGridView dgv)
        {
            try
            {
                decimal numeracionAyer = 0, numeracionHoy = 0, galonesHoy = 0, precioGalon = 0, generadoHoy = 0;


                foreach (DataGridViewRow fila in dgv.Rows)
                {
                    if (fila.Cells[3].Value != null)
                    {
                        numeracionAyer = Convert.ToDecimal(fila.Cells[2].Value.ToString());
                        numeracionHoy = Convert.ToDecimal(fila.Cells[3].Value.ToString());
                        galonesHoy = numeracionHoy - numeracionAyer;
                        fila.Cells[4].Value = galonesHoy;
                        precioGalon = Convert.ToDecimal(fila.Cells[5].Value.ToString());
                        generadoHoy = galonesHoy * precioGalon;
                        fila.Cells[6].Value = Math.Round(generadoHoy, 2);
                    }
                    else
                    {
                        fila.Cells[4].Value = "0";
                        fila.Cells[6].Value = "0";
                    }

                }
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void CalcularTodo(DataGridView dgvLadoA, DataGridView dgvLadoB, TextBox txtGeneradoHoy)
        {
            try
            {
                CalcularDGV(dgvLadoA);
                CalcularDGV(dgvLadoB);

                decimal total = 0;

                foreach (DataGridViewRow fila in dgvLadoA.Rows)
                {
                    if (fila.Cells[6].Value != null)
                    {
                        total += Convert.ToDecimal(fila.Cells[6].Value.ToString());

                    }
                }

                foreach (DataGridViewRow fila in dgvLadoB.Rows)
                {
                    if (fila.Cells[6].Value != null)
                    {
                        total += Convert.ToDecimal(fila.Cells[6].Value.ToString());

                    }
                }

                txtGeneradoHoy.Text = total.ToString();
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void InsertarNumeraciones(DataGridView dgv1, DataGridView dgv2, int idEntrega)
        {
            try
            {
                foreach (DataGridViewRow fila in dgv1.Rows)
                {
                
                    decimal numeracionHoy = Convert.ToDecimal(fila.Cells[3].Value.ToString());
                    decimal numeracionAyer = Convert.ToDecimal(fila.Cells[2].Value.ToString());

                    if (numeracionHoy != numeracionAyer)
                    {
                        int idManguera = Convert.ToInt32(fila.Cells[7].Value.ToString());
                        decimal precioGalon = Convert.ToDecimal(fila.Cells[5].Value.ToString());
                        dispensadoresTableAdapter.InsertarNumeracion(idManguera, numeracionHoy, precioGalon, idEntrega);

                        log.Info($"La numeración de hoy de la manguera {fila.Cells[0].Value} (ID: {idManguera}) es {numeracionHoy} con un precio de {precioGalon} en la entrega con el ID: {idEntrega}");
                    }

                }

                foreach (DataGridViewRow fila in dgv2.Rows)
                {
                    decimal numeracionHoy = Convert.ToDecimal(fila.Cells[3].Value.ToString());
                    decimal numeracionAyer = Convert.ToDecimal(fila.Cells[2].Value.ToString());

                    if (numeracionHoy != numeracionAyer)
                    {
                        int idManguera = Convert.ToInt32(fila.Cells[7].Value.ToString());
                        decimal precioGalon = Convert.ToDecimal(fila.Cells[5].Value.ToString());
                        dispensadoresTableAdapter.InsertarNumeracion(idManguera, numeracionHoy, precioGalon, idEntrega);
                        log.Info($"La numeración de hoy de la manguera {fila.Cells[0].Value} (ID: {idManguera}) es {numeracionHoy} con un precio de {precioGalon} en la entrega con el ID: {idEntrega}");
                    }
                }
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnCalcular3_Click(object sender, EventArgs e)
        {
            try
            {
                if (TodoCorrecto(dgvLadoA3, dgvLadoB3) == true)
                {
                    CalcularTodo(dgvLadoA3, dgvLadoB3, txtGeneradoHoy3);
                    if (txtGeneradoHoy3.Text != "0")
                    {
                        txtEfectivo3.Enabled = true;
                        txtTarjeta3.Enabled = true;
                        txtVales3.Enabled = true;
                        btnAplicar3.Enabled = true;
                    }
                }
                else
                {
                    MessageBox.Show("Debe introducir correctamente las numeraciones de hoy", "Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnCalcular1_Click(object sender, EventArgs e)
        {
            try
            {
                if (TodoCorrecto(dgvLadoA1,dgvLadoB1) == true)
                {
                    CalcularTodo(dgvLadoA1, dgvLadoB1, txtGeneradoHoy1);
                    if (txtGeneradoHoy1.Text != "0")
                    {
                        txtEfectivo1.Enabled = true;
                        txtTarjeta1.Enabled = true;
                        txtVales1.Enabled = true;
                        btnAplicar1.Enabled = true;
                    }
                }
                else
                {
                    MessageBox.Show("Debe introducir correctamente las numeraciones de hoy", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnCalcular2_Click(object sender, EventArgs e)
        {
            try
            {
                if (TodoCorrecto(dgvLadoA2,dgvLadoB2) == true)
                {
                    CalcularTodo(dgvLadoA2, dgvLadoB2, txtGeneradoHoy2);
                    if (txtGeneradoHoy2.Text != "0")
                    {
                        txtEfectivo2.Enabled = true;
                        txtTarjeta2.Enabled = true;
                        txtVales2.Enabled = true;
                        btnAplicar2.Enabled = true;
                    }

                }
                else
                {
                    MessageBox.Show("Debe introducir correctamente las numeraciones de hoy", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnEmpleados_Click(object sender, EventArgs e)
        {
            try
            {
                if (pnlEmpleados.Visible == false)
                {
                    log.Debug("Se abrió el apartado Empleados");

                    dgvEmpleados.DataSource = empleadosConTipoAdapter.EmpleadosConTipo();
                    dgvTipoEmpleados.DataSource = tiposDeEmpleadosTableAdapter.TipoDeEmpleados();

                    txtBuscarEmpNombre.Text = "";
                    txtBuscarEmpNombre.Focus();

                    pnlReportes.Visible = false;
                    pnlDashboard.Visible = false;
                    pnlDispensadores.Visible = false;
                    pnlCombustibles.Visible = false;
                    pnlEgresos.Visible = false;
                    pnlIngresos.Visible = false;
                    pnlEmpleados.Visible = true;
                }
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnAgregarEmpleado_Click(object sender, EventArgs e)
        {
            try
            {
                AgregarEmpleado agregarEmpleado = new AgregarEmpleado();
                DialogResult resultado = agregarEmpleado.ShowDialog();

                if (resultado == DialogResult.Yes)
                {
                    EmpleadosConTipoTableAdapter empleadosAdapter = new EmpleadosConTipoTableAdapter();
                    dgvEmpleados.DataSource = empleadosAdapter.EmpleadosConTipo();
                }
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        EmpleadosConTipoTableAdapter empleadosConTipoAdapter = new EmpleadosConTipoTableAdapter();
        private void btnModEmpleado_Click(object sender, EventArgs e)
        {
            try
            {
                int idEmpleado = Convert.ToInt32(dgvEmpleados.SelectedRows[0].Cells[10].Value.ToString());

                AgregarEmpleado agregarEmpleado = new AgregarEmpleado(idEmpleado);

                DialogResult resultado = agregarEmpleado.ShowDialog();

                if (resultado == DialogResult.Yes)
                {
                    dgvEmpleados.DataSource = empleadosConTipoAdapter.EmpleadosConTipo();
                    dgvEmpleados.Rows[FilaModificada(idEmpleado,dgvEmpleados)].Selected = true;
                }
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        tblEmpleadoTableAdapter empleadoAdapter = new tblEmpleadoTableAdapter();
        private void btnActEmpleado_Click(object sender, EventArgs e)
        {
            try
            {
                string Nombre = dgvEmpleados.SelectedRows[0].Cells[2].Value.ToString(), Apellidos = dgvEmpleados.SelectedRows[0].Cells[3].Value.ToString();
                if (dgvEmpleados.SelectedRows[0].Cells[6].Value.ToString() == "Desactivado")
                {
                    int idEmpleado = Convert.ToInt32(dgvEmpleados.SelectedRows[0].Cells[10].Value.ToString());

                    empleadoAdapter.CambiarEstadoEmpleado(idEmpleado);
                    dgvEmpleados.DataSource = empleadosConTipoAdapter.EmpleadosConTipo();
                    dgvEmpleados.Rows[FilaModificada(idEmpleado, dgvEmpleados)].Selected = true;

                    MessageBox.Show($"{Nombre} {Apellidos} ha sido activado","Empleado activado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    log.Info($"Se activó al empleado con el ID: {idEmpleado} y nombre {Nombre} {Apellidos}");
                }
                else
                {
                    MessageBox.Show($"{Nombre} {Apellidos} ya se encuentra activado en el sistema","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    log.Warn($"Se intentó activar al empleado {Nombre} {Apellidos} pero este ya esta activado");
                }
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnDesacEmpleado_Click(object sender, EventArgs e)
        {
            try
            {
                string Nombre = dgvEmpleados.SelectedRows[0].Cells[2].Value.ToString(), Apellidos = dgvEmpleados.SelectedRows[0].Cells[3].Value.ToString();
                if (dgvEmpleados.SelectedRows[0].Cells[6].Value.ToString() == "Activado")
                {
                    int idEmpleado = Convert.ToInt32(dgvEmpleados.SelectedRows[0].Cells[10].Value.ToString());

                    empleadoAdapter.CambiarEstadoEmpleado(idEmpleado);
                    dgvEmpleados.DataSource = empleadosConTipoAdapter.EmpleadosConTipo();
                    dgvEmpleados.Rows[FilaModificada(idEmpleado, dgvEmpleados)].Selected = true;
                    MessageBox.Show($"{Nombre} {Apellidos} ha sido desactivado", "Empleado desactivado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    log.Info($"Se desactivó al empleado con el ID: {idEmpleado} y nombre {Nombre} {Apellidos}");
                }
                else
                {
                    MessageBox.Show($"{Nombre} {Apellidos} ya se encuentra desactivado en el sistema", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    log.Warn($"Se intentó desactivar al empleado {Nombre} {Apellidos} pero este ya esta desactivado");
                }
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private int FilaModificada(int Id, DataGridView dgv)
        {
            try
            {
                int fila = 0;
                foreach (DataGridViewRow filas in dgv.Rows)
                {
                    if (filas.Cells[10].Value.ToString() == Id.ToString())
                    {
                        fila = filas.Index;
                    }
                }
                return fila;
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }

        }

        private void btnAgregarTipoEmpleado_Click(object sender, EventArgs e)
        {
            try
            {
                AgregarTipo agregarTipoEmpleado = new AgregarTipo("Empleado");
                DialogResult resultado = agregarTipoEmpleado.ShowDialog();

                if (resultado == DialogResult.Yes)
                {
                    TiposDeEmpleadosTableAdapter tipoEmpleadoAdapter = new TiposDeEmpleadosTableAdapter();
                    dgvTipoEmpleados.DataSource = tipoEmpleadoAdapter.TipoDeEmpleados();
                }
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void txtEntregado2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox txt = (TextBox)sender;

                decimal efectivo = 0, vales = 0, credito = 0 ,total = 0, devuelta = 0;

                if (txt.Text != "" && txt.Text != ".")
                {
                    if (txt == txtEfectivo1 || txt == txtVales1 || txt == txtTarjeta1 || txt == txtGeneradoHoy1)
                    {

                        total = Convert.ToDecimal(txtGeneradoHoy1.Text);
                        efectivo = Convert.ToDecimal(txtEfectivo1.Text);
                        vales = Convert.ToDecimal(txtVales1.Text);
                        credito = Convert.ToDecimal(txtTarjeta1.Text);
                        devuelta = efectivo + vales + credito - total;
                        txtSoFa1.Text = devuelta.ToString();
                    }
                    else if (txt == txtEfectivo2 || txt == txtVales2 || txt == txtTarjeta2 || txt == txtGeneradoHoy2)
                    {
                        total = Convert.ToDecimal(txtGeneradoHoy2.Text);
                        efectivo = Convert.ToDecimal(txtEfectivo2.Text);
                        vales = Convert.ToDecimal(txtVales2.Text);
                        credito = Convert.ToDecimal(txtTarjeta2.Text);
                        devuelta = efectivo + vales + credito - total;
                        txtSoFa2.Text = devuelta.ToString();
                    }
                    else if (txt == txtEfectivo3 || txt == txtVales3 || txt == txtTarjeta3 || txt == txtGeneradoHoy3)
                    {
                        total = Convert.ToDecimal(txtGeneradoHoy3.Text);
                        efectivo = Convert.ToDecimal(txtEfectivo3.Text);
                        vales = Convert.ToDecimal(txtVales3.Text);
                        credito = Convert.ToDecimal(txtTarjeta3.Text);
                        devuelta = efectivo + vales + credito - total;
                        txtSoFa3.Text = devuelta.ToString();
                    }
                }


            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnCombustibles_Click(object sender, EventArgs e)
        {
            try
            {
                if (pnlCombustibles.Visible == false)
                {
                    log.Debug("Se abrió el apartado Combustibles");

                    dgvCombustibles.DataSource = tblCombustibleTableAdapter.Combustibles();
                    dgvProveedores.DataSource = tblProveedorTableAdapter.Proveedores();

                    pnlReportes.Visible = false;
                    pnlDashboard.Visible = false;
                    pnlDispensadores.Visible = false;
                    pnlEmpleados.Visible = false;
                    pnlIngresos.Visible = false;
                    pnlEgresos.Visible = false;
                    pnlCombustibles.Visible = true;
                }
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }




        }
        public delegate void MyDelegate();//para poder ejecutar el metodo creardocumento en otro hilo
        private void pnlCombustibles_VisibleChanged(object sender, EventArgs e)
        {
            try
            {
                if (pnlCombustibles.Visible == true)
                {
                    dgvCombustibles.DataSource = tblCombustibleTableAdapter.Combustibles();


                    MyDelegate instance = new MyDelegate(CambiarPrecioCombustiblePorInternet);//corre el metodo CrearDocumento en otro hilo
                    instance.BeginInvoke(null, null);//aqui lo corre
                    //CambiarPrecioCombustiblePorInternet();
                }

            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



        }

        private void CambiarPrecioCombustiblePorInternet()
        {
            try
            {
                bool conexionInternet = IsConnectedToInternet();
                if (conexionInternet)
                {
                    WebClient webClient = new WebClient();
                    string page = webClient.DownloadString("https://micm.gob.do/");

                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(page);
                    var headers = doc.DocumentNode.SelectNodes("//tr/th");
                    DataTable table = new DataTable();
                    foreach (HtmlNode header in headers)
                        table.Columns.Add(header.InnerText); // create columns from th
                                                             // select rows with td elements 
                    foreach (var row in doc.DocumentNode.SelectNodes("//tr[td]"))
                        table.Rows.Add(row.SelectNodes("td").Select(td => td.InnerText).ToArray());

                    table.Rows[2][0] = "Gasoil Óptimo";

                    bool precioCambiado = false;

                    foreach (DataGridViewRow filaDGV in dgvCombustibles.Rows)
                    {
                        foreach (DataRow filaDT in table.Rows)
                        {
                            if (filaDGV.Cells[0].Value.ToString() == filaDT[0].ToString())
                            {
                                if (Convert.ToDecimal(filaDGV.Cells[2].Value.ToString()) != Convert.ToDecimal(filaDT[1].ToString().Remove(0,4)))
                                {
                                    precioCambiado = true;
                                    break;
                                }
                            }

                        }
                    }

                    if (precioCambiado == true)
                    {
                        DialogResult resultado = MessageBox.Show("Hay una actualización en los precios de los combustibles, ¿Desea actualizarlos?", "Actualización de precios", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                        if (resultado == DialogResult.Yes)
                        {
                            foreach (DataGridViewRow filaDGV in dgvCombustibles.Rows)
                            {
                                foreach (DataRow filaDT in table.Rows)
                                {
                                    if (filaDGV.Cells[0].Value.ToString() == filaDT[0].ToString())
                                    {
                                        int idCombustible = Convert.ToInt32(filaDGV.Cells[3].Value.ToString());
                                        decimal precioCombustible = Convert.ToDecimal(filaDT[1].ToString().Remove(0, 4));

                                        tblCombustibleTableAdapter.CambiarPrecioCombustible(idCombustible, precioCombustible);
                                    }

                                }
                            }
                            btnCambiarPrecio.BeginInvoke(new Action (()=> { btnCambiarPrecio.Enabled = false; }));
                            dgvCombustibles.BeginInvoke(new Action(() =>
                            {
                                dgvCombustibles.DataSource = tblCombustibleTableAdapter.Combustibles();
                                dgvCombustibles.Columns[2].ReadOnly = true;
                            }));

                            MessageBox.Show("Precios de los combustible actualizado correctamente", "Actualización de precios", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            log.Info("Se actualizarón los precioes de los combustibles obtenidos de la página web del Ministerio de Industria y Comercio");
                        }
                        else
                        {
                            log.Info("Hubo una actualización de los precios de los combustibles disponible, pero no se actualizó");
                        }

                    }


                }
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        public bool IsConnectedToInternet()
        {
            try
            {
                string host = "micm.gob.do";  
                bool result = false;
                Ping p = new Ping();
                try
                {
                    PingReply reply = p.Send(host, 3000);
                    if (reply.Status == IPStatus.Success)
                        return true;
                }
                catch { }
                return result;
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }

        private void btnCambiarPrecio_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult resultado = MessageBox.Show("¿Está seguro que desea cambiar los precios de los combustibles por los introducidos en la tabla?", "Cambiar precio", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resultado == DialogResult.Yes)
                {
                    tblCombustibleTableAdapter combustibleAdapter = new tblCombustibleTableAdapter();

                    foreach (DataGridViewRow fila in dgvCombustibles.Rows)
                    {
                        int idCombustible = Convert.ToInt32(fila.Cells[3].Value.ToString());
                        decimal precioCombustible = Convert.ToDecimal(fila.Cells[2].Value.ToString());

                        combustibleAdapter.CambiarPrecioCombustible(idCombustible, precioCombustible);

                    }

                    MessageBox.Show("Precio de los combustibles actualizado correctamente", "Precios actualizados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnAgregarProveedor_Click(object sender, EventArgs e)
        {
            try
            {
                AgregarProveedor agregarProveedor = new AgregarProveedor();
                DialogResult resultado = agregarProveedor.ShowDialog();

                if (resultado == DialogResult.Yes)
                {
                    dgvProveedores.DataSource = tblProveedorTableAdapter.Proveedores();
                }

            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnComprarCombustible_Click(object sender, EventArgs e)
        {
            try
            {
                ComprarCombustible comprarCombustible = new ComprarCombustible();
                DialogResult resultado = comprarCombustible.ShowDialog();

                if (resultado == DialogResult.Yes)
                {
                    dgvCombustibles.DataSource = tblCombustibleTableAdapter.Combustibles();
                    dgvUltimasCompras.DataSource = ultimasComprasTableAdapter.UltimasCompras();
                }
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnEgresos_Click(object sender, EventArgs e)
        {
            try
            {
                if (pnlEgresos.Visible == false)
                {
                    log.Debug("Se abrió el apartado Egresos");

                    pnlReportes.Visible = false;
                    pnlDashboard.Visible = false;
                    pnlCombustibles.Visible = false;
                    pnlDispensadores.Visible = false;
                    pnlEmpleados.Visible = false;
                    pnlIngresos.Visible = false;

                    FuncionesTableAdapter funcionesAdapter = new FuncionesTableAdapter();
                    FuncionesRow funcionesRow = funcionesAdapter.TotalEgresos15Dias()[0];

                    txtBuscarEgPorConcepto.Text = "";
                    txtBuscarEgPorConcepto.Focus();
                    txtTotalEgresos15Dias.Text = "RD$ " + string.Format("{0:0,0.00}", funcionesRow.Valor);

                    dgvEgresos.DataSource = egresosConTipoTableAdapter.EgresosConTipo();
                    dgvTotalTipoEgreso.DataSource = totalTipoEgreso15DiasTableAdapter.TotalTipoEgreso15Dias();

                    pnlEgresos.Visible = true;
                }
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void btnAgregarEgreso_Click(object sender, EventArgs e)
        {
            try
            {
                Ingreso_Egreso ingreso_Egreso = new Ingreso_Egreso("Egreso");

                DialogResult resultado = ingreso_Egreso.ShowDialog();

                if (resultado == DialogResult.Yes)
                {
                    dgvEgresos.DataSource = egresosConTipoTableAdapter.EgresosConTipo();
                    dgvTotalTipoEgreso.DataSource = totalTipoEgreso15DiasTableAdapter.TotalTipoEgreso15Dias();

                    FuncionesTableAdapter funcionesAdapter = new FuncionesTableAdapter();
                    FuncionesRow funcionesRow = funcionesAdapter.TotalEgresos15Dias()[0];

                    txtTotalEgresos15Dias.Text = "RD$ " + string.Format("{0:0,0.00}", funcionesRow.Valor);
                }
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnAgregarTipoEgreso_Click(object sender, EventArgs e)
        {
            try
            {
                AgregarTipo agregarTipo = new AgregarTipo("Egreso");
                agregarTipo.ShowDialog();
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
 

        }

        private void btnAplicar2_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtGeneradoHoy2.Text != "0")
                {
                    using (TransactionScope transaction = new TransactionScope())
                    {
                        int idEmpleado = Convert.ToInt32(cboBombero2.SelectedValue);

                        decimal efectivoEntregado = Convert.ToDecimal(txtEfectivo2.Text),
                                vales = Convert.ToDecimal(txtVales2.Text),
                                credito = Convert.ToDecimal(txtTarjeta2.Text);

                        dispensadoresTableAdapter.InsertarDispensadorEntrega(2, idEmpleado, efectivoEntregado,vales,credito);

                        log.Info($"Se realizó una entrega de efectivo para el dispensador #2 por el empleado {cboBombero2.Text} (ID: {idEmpleado}) entregando RD${efectivoEntregado}");

                        FuncionesTableAdapter funcionesAdapter = new FuncionesTableAdapter();

                        int idEntrega = Convert.ToInt32(funcionesAdapter.UltimaEntrega()[0].Valor);

                        InsertarNumeraciones(dgvLadoA2, dgvLadoB2, idEntrega);

                        tblEmpleadoTableAdapter.ActualizarArqueo(idEntrega);

                        log.Info($"Se actualizó el arqueo del empleado {cboBombero2.Text} (ID: {idEmpleado}) {(Convert.ToDecimal(txtSoFa2.Text) >= 0 ? "añadiendole" : "restándole")} {txtSoFa2.Text}");

                        MessageBox.Show("Numeración aplicada correctamente", "Numeración", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        dgvLadoA2.DataSource = numeracionesTableAdapter.Numeraciones(2, "A");
                        dgvLadoB2.DataSource = numeracionesTableAdapter.Numeraciones(2, "B");

                        btnCalcular2.Enabled = false;
                        btnAplicar2.Enabled = false;

                        bloquearTextBox(txtEfectivo2);
                        bloquearTextBox(txtVales2);
                        bloquearTextBox(txtTarjeta2);
                        cboBombero2.Enabled = false;

                        transaction.Complete();
                    }
                }
                else
                {
                    MessageBox.Show("Deben haber numeraciones nuevas de hoy para poder insertarlas", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnAplicar1_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtGeneradoHoy1.Text != "0")
                {
                    using (TransactionScope transaction = new TransactionScope())
                    {
                        int idEmpleado = Convert.ToInt32(cboBomberos1.SelectedValue);

                        decimal efectivoEntregado = Convert.ToDecimal(txtEfectivo1.Text),
                                vales = Convert.ToDecimal(txtVales1.Text),
                                credito = Convert.ToDecimal(txtTarjeta1.Text);

                        dispensadoresTableAdapter.InsertarDispensadorEntrega(1, idEmpleado, efectivoEntregado, vales, credito);

                        log.Info($"Se realizó una entrega de efectivo para el dispensador #1 por el empleado {cboBomberos1.Text} (ID: {idEmpleado}) entregando RD${efectivoEntregado}");

                        FuncionesTableAdapter funcionesAdapter = new FuncionesTableAdapter();

                        int idEntrega = Convert.ToInt32(funcionesAdapter.UltimaEntrega()[0].Valor);

                        InsertarNumeraciones(dgvLadoA1, dgvLadoB1, idEntrega);

                        tblEmpleadoTableAdapter.ActualizarArqueo(idEntrega);

                        log.Info($"Se actualizó el arqueo del empleado {cboBomberos1.Text} (ID: {idEmpleado}) {(Convert.ToDecimal(txtSoFa1.Text) >= 0 ? "añadiendole" : "restándole")} {txtSoFa1.Text}");

                        MessageBox.Show("Numeración aplicada correctamente", "Numeración", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        dgvLadoA1.DataSource = numeracionesTableAdapter.Numeraciones(1, "A");
                        dgvLadoB1.DataSource = numeracionesTableAdapter.Numeraciones(1, "B");

                        btnCalcular1.Enabled = false;
                        btnAplicar1.Enabled = false;

                        bloquearTextBox(txtEfectivo1);
                        bloquearTextBox(txtVales1);
                        bloquearTextBox(txtTarjeta1);
                        cboBomberos1.Enabled = false;

                        transaction.Complete();
                    }
                }
                else
                {
                    MessageBox.Show("Deben haber numeraciones nuevas de hoy para poder insertarlas", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void bloquearTextBox(TextBox txt)
        {
            txt.Enabled = true;
            txt.ReadOnly = true;
            txt.TabStop = false;
            txt.BackColor = SystemColors.ControlLight;
        }
        private void btnAplicar3_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtGeneradoHoy3.Text != "0")
                {
                    
                    using (TransactionScope transaction = new TransactionScope())
                    {
                        int idEmpleado = Convert.ToInt32(cboBombero3.SelectedValue);

                        decimal efectivoEntregado = Convert.ToDecimal(txtEfectivo3.Text),
                                vales = Convert.ToDecimal(txtVales3.Text),
                                credito = Convert.ToDecimal(txtTarjeta3.Text);

                        dispensadoresTableAdapter.InsertarDispensadorEntrega(3, idEmpleado, efectivoEntregado, vales, credito);

                        log.Info($"Se realizó una entrega de efectivo para el dispensador #3 por el empleado {cboBombero3.Text} (ID: {idEmpleado}) entregando RD${efectivoEntregado}");

                        FuncionesTableAdapter funcionesAdapter = new FuncionesTableAdapter();

                        int idEntrega = Convert.ToInt32(funcionesAdapter.UltimaEntrega()[0].Valor);

                        InsertarNumeraciones(dgvLadoA3, dgvLadoB3, idEntrega);

                        tblEmpleadoTableAdapter.ActualizarArqueo(idEntrega);

                        log.Info($"Se actualizó el arqueo del empleado {cboBombero3.Text} (ID: {idEmpleado}) {(Convert.ToDecimal(txtSoFa3.Text) >= 0 ? "añadiendole" : "restándole")} {txtSoFa3.Text}");

                        MessageBox.Show("Numeración aplicada correctamente", "Numeración", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        dgvLadoA3.DataSource = numeracionesTableAdapter.Numeraciones(3, "A");
                        dgvLadoB3.DataSource = numeracionesTableAdapter.Numeraciones(3, "B");

                        btnCalcular3.Enabled = false;
                        btnAplicar3.Enabled = false;

                        bloquearTextBox(txtEfectivo3);
                        bloquearTextBox(txtVales3);
                        bloquearTextBox(txtTarjeta3);
                        cboBombero3.Enabled = false;

                        transaction.Complete();
                    }

                }
                else
                {
                    MessageBox.Show("Deben haber numeraciones nuevas de hoy para poder insertarlas", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarTextBoxDispensadores()
        {
            txtGeneradoHoy1.Text = "0";
            txtGeneradoHoy2.Text = "0";
            txtGeneradoHoy3.Text = "0";

            txtVales1.Text = "0";
            txtVales2.Text = "0";
            txtVales3.Text = "0";

            txtEfectivo1.Text = "0";
            txtEfectivo2.Text = "0";
            txtEfectivo3.Text = "0";

            txtTarjeta1.Text = "0";
            txtTarjeta2.Text = "0";
            txtTarjeta3.Text = "0";

            txtSoFa1.Text = "0";
            txtSoFa2.Text = "0";
            txtSoFa3.Text = "0";
        }

        private void btnIngresos_Click(object sender, EventArgs e)
        {
            try
            {
                if (pnlIngresos.Visible == false)
                {
                    log.Debug("Se abrió el apartado Ingresos");

                    FuncionesTableAdapter funcionesAdapter = new FuncionesTableAdapter();
                    FuncionesRow funcionesRow = funcionesAdapter.TotalIngresos15Dias()[0];

                    txtTotalIngreso15Dias.Text = "RD$ " + string.Format("{0:0,0.00}", funcionesRow.Valor);

                    dgvIngresos.DataSource = ingresosConTipoTableAdapter.IngresosConTipo();
                    dgvTotalTipoIngreso15Dias.DataSource = totalTipoIngreso15DiasTableAdapter.TotalTipoIngresos15Dias();

                    txtBuscarIngConcepto.Text = "";
                    txtBuscarIngConcepto.Focus();

                    pnlReportes.Visible = false;
                    pnlDashboard.Visible = false;
                    pnlEmpleados.Visible = false;
                    pnlCombustibles.Visible = false;
                    pnlDispensadores.Visible = false;
                    pnlEgresos.Visible = false;
                    pnlIngresos.Visible = true;
                }

            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAgregarIngreso_Click(object sender, EventArgs e)
        {
            try
            {
                Ingreso_Egreso ingreso_Egreso = new Ingreso_Egreso("Ingreso");

                DialogResult resultado = ingreso_Egreso.ShowDialog();

                if (resultado == DialogResult.Yes)
                {
                    dgvIngresos.DataSource = ingresosConTipoTableAdapter.IngresosConTipo();
                    dgvTotalTipoIngreso15Dias.DataSource = totalTipoIngreso15DiasTableAdapter.TotalTipoIngresos15Dias();

                    FuncionesTableAdapter funcionesAdapter = new FuncionesTableAdapter();
                    FuncionesRow funcionesRow = funcionesAdapter.TotalIngresos15Dias()[0];

                    txtTotalIngreso15Dias.Text = "RD$ " + string.Format("{0:0,0.00}", funcionesRow.Valor);
                }

            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAgregarTipoIngreso_Click(object sender, EventArgs e)
        {
            try
            {
                AgregarTipo agregarTipo = new AgregarTipo("Ingreso");
                agregarTipo.ShowDialog();

            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnInicio_Click(object sender, EventArgs e)
        {
            try
            {
                if (pnlDashboard.Visible == false)
                {

                    CargarDashboard();

                    pnlReportes.Visible = false;
                    pnlCombustibles.Visible = false;
                    pnlDispensadores.Visible = false;
                    pnlEmpleados.Visible = false;
                    pnlIngresos.Visible = false;
                    pnlEgresos.Visible = false;
                    pnlDashboard.Visible = true;
                }

            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReportes_Click(object sender, EventArgs e)
        {
            try
            {
                if (pnlReportes.Visible == false)
                {
                    log.Debug("Se abrió el apartado Reportes");

                    CargarReporteMovimientos(DateTime.Today, DateTime.Now, $"Reporte de hoy: {DateTime.Now.ToString()}");
                
                    pictMovimientos.BackColor = Color.LightBlue;

                    pnlNomina.Visible = false;
                    pnlCombustibles.Visible = false;
                    pnlDispensadores.Visible = false;
                    pnlEmpleados.Visible = false;
                    pnlIngresos.Visible = false;
                    pnlEgresos.Visible = false;
                    pnlDashboard.Visible = false;
                    pnlNumeraciones.Visible = false;
                    pnlNomina.Visible = false;
                    pnlReportes.Visible = true; 
                    pnlMovimientos.Visible = true;
                }


            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void CargarReporteMovimientos(DateTime fechaInicio, DateTime fechaFin, string fechaReporte)
        {
            try
            {
                Movimientos movimientosAdapter = new Movimientos();
                IngresosPorFechaTableAdapter ingresosPorFechaAdapter = new IngresosPorFechaTableAdapter();
                EgresosPorFechaTableAdapter egresosPorFechaAdapter = new EgresosPorFechaTableAdapter();
                FuncionesTableAdapter funcionesAdapter = new FuncionesTableAdapter();

                ReportDataSource sourceMovimiento = new ReportDataSource("Movimientos", (DataTable)movimientosAdapter.MovimientosPorFecha(fechaInicio, fechaFin));
                ReportDataSource sourceIngreso = new ReportDataSource("Ingresos", (DataTable)ingresosPorFechaAdapter.GetData(fechaInicio, fechaFin));
                ReportDataSource sourceEgreso = new ReportDataSource("Egresos", (DataTable)egresosPorFechaAdapter.GetData(fechaInicio, fechaFin));
                ReportDataSource sourceVentaCombustible = new ReportDataSource("VentaCombustible", (DataTable)funcionesAdapter.VentaCombustibleEntreFechas(fechaInicio, fechaFin));

                rpvMovimientos.LocalReport.DataSources.Clear();

                ReportParameter[] fechaParametro = new ReportParameter[1];

                fechaParametro[0] = new ReportParameter("FechaReporte", fechaReporte);

                rpvMovimientos.LocalReport.SetParameters(fechaParametro);

                rpvMovimientos.LocalReport.DataSources.Add(sourceMovimiento);
                rpvMovimientos.LocalReport.DataSources.Add(sourceIngreso);
                rpvMovimientos.LocalReport.DataSources.Add(sourceEgreso);
                rpvMovimientos.LocalReport.DataSources.Add(sourceVentaCombustible);

                log.Info($"Se buscó el reporte de movimientos desde {fechaInicio} al {fechaFin}");

                rpvMovimientos.RefreshReport();

            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarNomina()
        {
            try
            {
                SelectNominaBindingSource.DataSource = SelectNominaTableAdapter.Nomina();

                log.Info("Se refrescó el reporte de la nómina");

                rpvNomina.RefreshReport();

            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarNumeracionesPorFecha(DateTime fecha)
        {
            try
            {
                NumeracionPorFechaTableAdapter numeracionAdapter = new NumeracionPorFechaTableAdapter();
                DatosNumeracionPorFechaTableAdapter datosAdapter = new DatosNumeracionPorFechaTableAdapter();

                ReportDataSource sourceNumeraciones = new ReportDataSource("Numeraciones", (DataTable) numeracionAdapter.NumeracionPorFecha(fecha));
                ReportDataSource sourceDatos = new ReportDataSource("Datos", (DataTable) datosAdapter.GetData(fecha));

                rpvNumeraciones.LocalReport.DataSources.Clear();

                rpvNumeraciones.LocalReport.DataSources.Add(sourceNumeraciones);
                rpvNumeraciones.LocalReport.DataSources.Add(sourceDatos);

                ReportParameter[] numeracionesParametros = new ReportParameter[2];

                numeracionesParametros[0] = new ReportParameter("FechaReporte", $"Cuadre del {fecha.ToString("dd/MM/yyyy")}");
                numeracionesParametros[1] = new ReportParameter("Nota", "");

                rpvNumeraciones.LocalReport.SetParameters(numeracionesParametros);

                log.Info($"Se buscó el cuadre de la {fecha}");

                rpvNumeraciones.RefreshReport();

            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CargarNumeracionesEntreFechas(DateTime fechaInicio, DateTime fechaFinal)
        {
            try
            {
                NumeracionPorFechaTableAdapter numeracionAdapter = new NumeracionPorFechaTableAdapter();
                DatosNumeracionPorFechaTableAdapter datosAdapter = new DatosNumeracionPorFechaTableAdapter();

                ReportDataSource sourceNumeraciones = new ReportDataSource("Numeraciones", (DataTable)numeracionAdapter.NumeracionesEntreFechas(fechaInicio, fechaFinal));
                ReportDataSource sourceDatos = new ReportDataSource("Datos", (DataTable)datosAdapter.EntreFechas(fechaInicio, fechaFinal));

                rpvNumeraciones.LocalReport.DataSources.Clear();

                rpvNumeraciones.LocalReport.DataSources.Add(sourceNumeraciones);
                rpvNumeraciones.LocalReport.DataSources.Add(sourceDatos);

                ReportParameter[] numeracionesParametros = new ReportParameter[2];

                numeracionesParametros[0] = new ReportParameter("FechaReporte", $"Cuadre desde {fechaInicio.ToString("dd/MM/yyyy")} hasta {DateTime.Now.ToString("dd/MM/yyyy")}");
                numeracionesParametros[1] = new ReportParameter("Nota", "Nota: el precio por galón de los combustibles es un promedio de los precios de combustibles que hubieron entre las fechas del cuadre, por ello el valor generado es una aproximación");

                rpvNumeraciones.LocalReport.SetParameters(numeracionesParametros);

                log.Info($"Se buscó el cuadre entre las fechas {fechaInicio} al {fechaFinal}");

                rpvNumeraciones.RefreshReport();

            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCustomM_Click(object sender, EventArgs e)
        {
            try
            {
                ElegirFecha elegirFecha = new ElegirFecha();

                DialogResult resultado = elegirFecha.ShowDialog();

                if (resultado == DialogResult.Yes)
                {
                    CargarReporteMovimientos(elegirFecha.dtpFechaInicio.Value, elegirFecha.dtpFechaFinal.Value, $"Cuadre desde {elegirFecha.dtpFechaInicio.Value.ToString("dd/MM/yyyy")} hasta {elegirFecha.dtpFechaFinal.Value.ToString("dd/MM/yyyy")}");
                }

            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHoyM_Click(object sender, EventArgs e)
        {
            try
            {
                CargarReporteMovimientos(DateTime.Today, DateTime.Now, $"Reporte de hoy - {DateTime.Now.ToString()}");

            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn7DiasM_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime fechaInicio = DateTime.Today.AddDays(-7);
                CargarReporteMovimientos(fechaInicio, DateTime.Now, $"Reporte desde {fechaInicio.ToString("dd/MM/yyyy")} hasta {DateTime.Now.ToString("dd/MM/yyyy")}");

            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btn15DiasM_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime fechaInicio = DateTime.Today.AddDays(-15);
                CargarReporteMovimientos(fechaInicio, DateTime.Now, $"Reporte desde {fechaInicio.ToString("dd/MM/yyyy")} hasta {DateTime.Now.ToString("dd/MM/yyyy")}");

            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnMesM_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime fechaInicio = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                CargarReporteMovimientos(fechaInicio, DateTime.Now, $"Reporte desde {fechaInicio.ToString("dd/MM/yyyy")} hasta {DateTime.Now.ToString("dd/MM/yyyy")}");

            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn30DiasM_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime fechaInicio = DateTime.Today.AddDays(-30);
                CargarReporteMovimientos(fechaInicio, DateTime.Now, $"Reporte desde {fechaInicio.ToString("dd/MM/yyyy")} hasta {DateTime.Now.ToString("dd/MM/yyyy")}");

            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAnoM_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime fechaInicio = new DateTime(DateTime.Today.Year, 1, 1);
                CargarReporteMovimientos(fechaInicio, DateTime.Now, $"Reporte desde {fechaInicio.ToString("dd/MM/yyyy")} hasta {DateTime.Now.ToString("dd/MM/yyyy")}");

            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictNomina_Click(object sender, EventArgs e)
        {
            try
            {
                if (pnlNomina.Visible == false)
                {
                    CargarNomina();
                    pnlMovimientos.Visible = false;
                    pnlNumeraciones.Visible = false;

                    pictNomina.BackColor = Color.LightBlue;
                    pictNomina.BorderStyle = BorderStyle.None;

                    pnlNomina.Visible = true;
                }
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnPagoNomina_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult resultado = MessageBox.Show("¿Está seguro que desea pagar la nómina a cada empleado?", "Pago de Nómina", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    FuncionesTableAdapter funcionesAdapter = new FuncionesTableAdapter();
                    int dias = Convert.ToInt32(funcionesAdapter.DiasDeNomina()[0].Valor);

                    if (dias < 12)
                    {
                        DialogResult result = MessageBox.Show($"Han pasado solo {dias} días, ¿Desea continuar?", "Pago de Nómina", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            SelectNominaTableAdapter.PagarNomina();
                            MessageBox.Show("Pago de nómina realizado correctamente", "Pago de Nómina", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                    }
                }
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void btnRefrescarNomina_Click(object sender, EventArgs e)
        {
            try
            {
                CargarNomina();
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void txtBuscarEmpNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                Metodos.SoloLetras(sender, e);
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void bnBuscarEmpleado_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtBuscarEmpNombre.Text != "")
                {
                    dgvEmpleados.DataSource = empleadosConTipoAdapter.EmpleadosPorNombre(txtBuscarEmpNombre.Text.Trim());
                    txtBuscarEmpNombre.Focus();
                    log.Info($"Se buscó al empleado con el nombre {txtBuscarEmpNombre.Text.Trim()}");
                }
                else
                {
                    MessageBox.Show("Debe ingresar el nombre", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnLimpiarEmp_Click(object sender, EventArgs e)
        {
            try
            {
                dgvEmpleados.DataSource = empleadosConTipoAdapter.EmpleadosConTipo();
                txtBuscarEmpNombre.Text = "";
                txtBuscarEmpNombre.Focus();
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnBuscarIngreso_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtBuscarIngConcepto.Text != "")
                {
                    dgvIngresos.DataSource = ingresosConTipoTableAdapter.IngresosPorConcepto(txtBuscarIngConcepto.Text.Trim());
                    txtBuscarIngConcepto.Focus();
                    log.Info($"Se buscaron los ingresos con por el concepto {txtBuscarIngConcepto.Text.Trim()}");
                }
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnLimpiarIngreso_Click(object sender, EventArgs e)
        {
            try
            {
                dgvIngresos.DataSource = ingresosConTipoTableAdapter.IngresosConTipo();
                txtBuscarIngConcepto.Text = "";
                txtBuscarIngConcepto.Focus();
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
  
        }

        private void btnBuscarEgreso_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtBuscarEgPorConcepto.Text != "")
                {
                    dgvEgresos.DataSource = egresosConTipoTableAdapter.EgresosPorConcepto(txtBuscarEgPorConcepto.Text.Trim());
                    txtBuscarEgPorConcepto.Focus();
                    log.Info($"Se buscaron los egresos por el concepto {txtBuscarEgPorConcepto.Text.Trim()}");
                }
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnLimpiarEgreso_Click(object sender, EventArgs e)
        {
            try
            {
                dgvEgresos.DataSource = egresosConTipoTableAdapter.EgresosConTipo();
                txtBuscarEgPorConcepto.Text = "";
                txtBuscarEgPorConcepto.Focus();
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void pictNumeraciones_Click(object sender, EventArgs e)
        {
            try
            {
                if (pnlNumeraciones.Visible == false)
                {
                    pnlNomina.Visible = false;
                    pnlMovimientos.Visible = false;
                    CargarNumeracionesPorFecha(DateTime.Today);

                    pictNumeraciones.BackColor = Color.LightBlue;
                    pictNumeraciones.BorderStyle = BorderStyle.None;

                    pnlNumeraciones.Visible = true;
                }

            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnHoyN_Click(object sender, EventArgs e)
        {
            try
            {
                CargarNumeracionesPorFecha(DateTime.Today);

            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btn7DiasN_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime fechaInicio = DateTime.Today.AddDays(-7);
                CargarNumeracionesEntreFechas(fechaInicio, DateTime.Now);

            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn15DiasN_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime fechaInicio = DateTime.Today.AddDays(-15);
                CargarNumeracionesEntreFechas(fechaInicio, DateTime.Now);

            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnMesN_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime fechaInicio = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                CargarNumeracionesEntreFechas(fechaInicio, DateTime.Now);

            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn30DiasN_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime fechaInicio = DateTime.Today.AddDays(-30);
                CargarNumeracionesEntreFechas(fechaInicio, DateTime.Now);

            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAnoN_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime fechaInicio = new DateTime(DateTime.Today.Year, 1, 1);
                CargarNumeracionesEntreFechas(fechaInicio, DateTime.Now);
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnElegirFechasN_Click(object sender, EventArgs e)
        {
            try
            {
                ElegirFecha elegirFecha = new ElegirFecha();

                DialogResult resultado = elegirFecha.ShowDialog();

                if (resultado == DialogResult.Yes)
                {
                    CargarNumeracionesEntreFechas(elegirFecha.dtpFechaInicio.Value, elegirFecha.dtpFechaFinal.Value);
                }
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void pictMovimientos_Click(object sender, EventArgs e)
        {
            try
            {
                if (pnlMovimientos.Visible == false)
                {
                    CargarReporteMovimientos(DateTime.Today, DateTime.Now, $"Reporte de hoy: {DateTime.Now.ToString()}");

                    pictMovimientos.BackColor = Color.LightBlue;
                    pictMovimientos.BorderStyle = BorderStyle.None;

                    pnlNomina.Visible = false;
                    pnlNumeraciones.Visible = false;
                    pnlMovimientos.Visible = true;

                }
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void pictMovimientos_BackColorChanged(object sender, EventArgs e)
        {
            try
            {
                if (pictMovimientos.BackColor == Color.LightBlue)
                {
                    pictNumeraciones.BackColor = Color.White;
                    pictNumeraciones.BorderStyle = BorderStyle.FixedSingle;
                    pictNomina.BackColor = Color.White;
                    pictNomina.BorderStyle = BorderStyle.FixedSingle;
                }
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void pictNomina_BackColorChanged(object sender, EventArgs e)
        {
            try
            {
                if (pictNomina.BackColor == Color.LightBlue)
                {
                    pictMovimientos.BackColor = Color.White;
                    pictMovimientos.BorderStyle = BorderStyle.FixedSingle;
                    pictNumeraciones.BackColor = Color.White;
                    pictNumeraciones.BorderStyle = BorderStyle.FixedSingle;
                }
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void pictNumeraciones_BackColorChanged(object sender, EventArgs e)
        {
            try
            {
                if (pictNumeraciones.BackColor == Color.LightBlue)
                {
                    pictNomina.BackColor = Color.White;
                    pictNomina.BorderStyle = BorderStyle.FixedSingle;
                    pictMovimientos.BackColor = Color.White;
                    pictMovimientos.BorderStyle = BorderStyle.FixedSingle;
                }
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnFechaEspN_Click(object sender, EventArgs e)
        {
            try
            {
                FechaEspecifica fechaEspecifica = new FechaEspecifica();

                DialogResult resultado = fechaEspecifica.ShowDialog();

                if (resultado == DialogResult.Yes)
                {
                    CargarNumeracionesPorFecha(fechaEspecifica.dtpFechaDia.Value.Date);
                }
            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void txt_Enter(object sender, EventArgs e)
        {
            try
            {
                TextBox txt = (TextBox)sender;

                if (txt.Text == "0")
                {
                    txt.Text = "";
                }

            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txt_Leave(object sender, EventArgs e)
        {
            try
            {
                TextBox txt = (TextBox)sender;

                if (txt.Text == "")
                {
                    txt.Text = "0";
                }

            }
            catch (Exception error)
            {
                log.Error($"Error: {error.Message}", error);
                MessageBox.Show($"Error: {error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnElegirDiaM_Click(object sender, EventArgs e)
        {
            try
            {
                FechaEspecifica fechaEspecifica = new FechaEspecifica();

                DialogResult resultado = fechaEspecifica.ShowDialog();

                if (resultado == DialogResult.Yes)
                {
                    DateTime fechaDTP = fechaEspecifica.dtpFechaDia.Value;
                    DateTime fechaInicio = fechaDTP.Date;
                    DateTime fechaFinal = new DateTime(fechaDTP.Year, fechaDTP.Month, fechaDTP.Day, 23, 59, 59);
                    CargarReporteMovimientos(fechaInicio, fechaFinal, $"Reporte del día: {fechaDTP.ToString("dddd dd, MMMM yyyy")}");
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
