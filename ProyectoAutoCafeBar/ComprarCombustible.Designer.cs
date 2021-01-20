namespace ProyectoAutoCafeBar
{
    partial class ComprarCombustible
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblProveedor = new System.Windows.Forms.Label();
            this.cboProveedor = new System.Windows.Forms.ComboBox();
            this.tblProveedorBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.data = new ProyectoAutoCafeBar.Data();
            this.cboCombustible = new System.Windows.Forms.ComboBox();
            this.tblCombustibleBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tblProveedorTableAdapter = new ProyectoAutoCafeBar.DataTableAdapters.tblProveedorTableAdapter();
            this.tblCombustibleTableAdapter = new ProyectoAutoCafeBar.DataTableAdapters.tblCombustibleTableAdapter();
            this.txtPrecio = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCantidad = new System.Windows.Forms.TextBox();
            this.btnRegresar = new System.Windows.Forms.Button();
            this.btnRealizarCompra = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.tblProveedorBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.data)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblCombustibleBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // lblProveedor
            // 
            this.lblProveedor.AutoSize = true;
            this.lblProveedor.Font = new System.Drawing.Font("Calibri", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProveedor.Location = new System.Drawing.Point(97, 45);
            this.lblProveedor.Name = "lblProveedor";
            this.lblProveedor.Size = new System.Drawing.Size(293, 35);
            this.lblProveedor.TabIndex = 27;
            this.lblProveedor.Text = "Compra de Combustible";
            // 
            // cboProveedor
            // 
            this.cboProveedor.DataSource = this.tblProveedorBindingSource;
            this.cboProveedor.DisplayMember = "Nombre";
            this.cboProveedor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProveedor.FormattingEnabled = true;
            this.cboProveedor.Location = new System.Drawing.Point(36, 162);
            this.cboProveedor.Name = "cboProveedor";
            this.cboProveedor.Size = new System.Drawing.Size(415, 29);
            this.cboProveedor.TabIndex = 0;
            this.cboProveedor.ValueMember = "idProveedor";
            // 
            // tblProveedorBindingSource
            // 
            this.tblProveedorBindingSource.DataMember = "tblProveedor";
            this.tblProveedorBindingSource.DataSource = this.data;
            // 
            // data
            // 
            this.data.DataSetName = "Data";
            this.data.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // cboCombustible
            // 
            this.cboCombustible.DataSource = this.tblCombustibleBindingSource;
            this.cboCombustible.DisplayMember = "Nombre";
            this.cboCombustible.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCombustible.FormattingEnabled = true;
            this.cboCombustible.Location = new System.Drawing.Point(36, 245);
            this.cboCombustible.Name = "cboCombustible";
            this.cboCombustible.Size = new System.Drawing.Size(415, 29);
            this.cboCombustible.TabIndex = 1;
            this.cboCombustible.ValueMember = "idCombustible";
            // 
            // tblCombustibleBindingSource
            // 
            this.tblCombustibleBindingSource.DataMember = "tblCombustible";
            this.tblCombustibleBindingSource.DataSource = this.data;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(32, 128);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 21);
            this.label1.TabIndex = 31;
            this.label1.Text = "Proveedor";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(32, 209);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 21);
            this.label2.TabIndex = 32;
            this.label2.Text = "Combustible";
            // 
            // tblProveedorTableAdapter
            // 
            this.tblProveedorTableAdapter.ClearBeforeFill = true;
            // 
            // tblCombustibleTableAdapter
            // 
            this.tblCombustibleTableAdapter.ClearBeforeFill = true;
            // 
            // txtPrecio
            // 
            this.txtPrecio.Location = new System.Drawing.Point(267, 327);
            this.txtPrecio.Name = "txtPrecio";
            this.txtPrecio.Size = new System.Drawing.Size(184, 28);
            this.txtPrecio.TabIndex = 3;
            this.txtPrecio.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPrecio_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(263, 293);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(127, 21);
            this.label3.TabIndex = 34;
            this.label3.Text = "Precio por Galón";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(32, 293);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(155, 21);
            this.label4.TabIndex = 36;
            this.label4.Text = "Cantidad de Galones";
            // 
            // txtCantidad
            // 
            this.txtCantidad.Location = new System.Drawing.Point(36, 327);
            this.txtCantidad.Name = "txtCantidad";
            this.txtCantidad.Size = new System.Drawing.Size(184, 28);
            this.txtCantidad.TabIndex = 2;
            this.txtCantidad.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPrecio_KeyPress);
            // 
            // btnRegresar
            // 
            this.btnRegresar.Location = new System.Drawing.Point(293, 378);
            this.btnRegresar.Name = "btnRegresar";
            this.btnRegresar.Size = new System.Drawing.Size(108, 50);
            this.btnRegresar.TabIndex = 5;
            this.btnRegresar.Text = "Regresar";
            this.btnRegresar.UseVisualStyleBackColor = true;
            this.btnRegresar.Click += new System.EventHandler(this.btnRegresar_Click);
            // 
            // btnRealizarCompra
            // 
            this.btnRealizarCompra.Location = new System.Drawing.Point(90, 378);
            this.btnRealizarCompra.Name = "btnRealizarCompra";
            this.btnRealizarCompra.Size = new System.Drawing.Size(108, 50);
            this.btnRealizarCompra.TabIndex = 4;
            this.btnRealizarCompra.Text = "Realizar compra";
            this.btnRealizarCompra.UseVisualStyleBackColor = true;
            this.btnRealizarCompra.Click += new System.EventHandler(this.btnRealizarCompra_Click);
            // 
            // ComprarCombustible
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.LightGreen;
            this.ClientSize = new System.Drawing.Size(483, 446);
            this.Controls.Add(this.btnRegresar);
            this.Controls.Add(this.btnRealizarCompra);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtCantidad);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtPrecio);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboCombustible);
            this.Controls.Add(this.cboProveedor);
            this.Controls.Add(this.lblProveedor);
            this.Font = new System.Drawing.Font("Calibri", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ComprarCombustible";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ComprarCombustible";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ComprarCombustible_FormClosing);
            this.Load += new System.EventHandler(this.ComprarCombustible_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tblProveedorBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.data)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblCombustibleBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblProveedor;
        private System.Windows.Forms.ComboBox cboProveedor;
        private System.Windows.Forms.ComboBox cboCombustible;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Data data;
        private System.Windows.Forms.BindingSource tblProveedorBindingSource;
        private DataTableAdapters.tblProveedorTableAdapter tblProveedorTableAdapter;
        private System.Windows.Forms.BindingSource tblCombustibleBindingSource;
        private DataTableAdapters.tblCombustibleTableAdapter tblCombustibleTableAdapter;
        private System.Windows.Forms.TextBox txtPrecio;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCantidad;
        private System.Windows.Forms.Button btnRegresar;
        private System.Windows.Forms.Button btnRealizarCompra;
    }
}