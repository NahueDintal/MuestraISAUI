using System;
using System.Drawing;
using System.Windows.Forms;

namespace MuestraISAUI.Forms
{
    public partial class FrmDatosPersonales : Form
    {
        private TextBox txtNombre;
        private DateTimePicker dtpFechaNacimiento;
        private Button btnAceptar;
        private Button btnCancelar;
        private Label lblNombre;
        private Label lblFechaNacimiento;
        private Label lblResultado;

        public FrmDatosPersonales()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Configuración del formulario
            this.Text = "Ingreso de Datos Personales";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Label para Nombre
            lblNombre = new Label();
            lblNombre.Text = "Nombre Completo:";
            lblNombre.Location = new Point(20, 30);
            lblNombre.Size = new Size(150, 20);
            lblNombre.Font = new Font("Arial", 10, FontStyle.Bold);

            // TextBox para Nombre
            txtNombre = new TextBox();
            txtNombre.Location = new Point(180, 30);
            txtNombre.Size = new Size(180, 25);
            txtNombre.Font = new Font("Arial", 10);
            txtNombre.MaxLength = 100;
            txtNombre.TabIndex = 0;

            // Label para Fecha de Nacimiento
            lblFechaNacimiento = new Label();
            lblFechaNacimiento.Text = "Fecha de Nacimiento:";
            lblFechaNacimiento.Location = new Point(20, 80);
            lblFechaNacimiento.Size = new Size(150, 20);
            lblFechaNacimiento.Font = new Font("Arial", 10, FontStyle.Bold);

            // DateTimePicker para Fecha de Nacimiento
            dtpFechaNacimiento = new DateTimePicker();
            dtpFechaNacimiento.Location = new Point(180, 80);
            dtpFechaNacimiento.Size = new Size(180, 25);
            dtpFechaNacimiento.Font = new Font("Arial", 10);
            dtpFechaNacimiento.Format = DateTimePickerFormat.Short;
            dtpFechaNacimiento.MaxDate = DateTime.Today;
            dtpFechaNacimiento.TabIndex = 1;

            // Botón Aceptar
            btnAceptar = new Button();
            btnAceptar.Text = "Aceptar";
            btnAceptar.Location = new Point(100, 150);
            btnAceptar.Size = new Size(80, 30);
            btnAceptar.Font = new Font("Arial", 10);
            btnAceptar.TabIndex = 2;
            btnAceptar.Click += new EventHandler(btnAceptar_Click);

            // Botón Cancelar
            btnCancelar = new Button();
            btnCancelar.Text = "Cancelar";
            btnCancelar.Location = new Point(200, 150);
            btnCancelar.Size = new Size(80, 30);
            btnCancelar.Font = new Font("Arial", 10);
            btnCancelar.TabIndex = 3;
            btnCancelar.Click += new EventHandler(btnCancelar_Click);

            // Label para mostrar resultados
            lblResultado = new Label();
            lblResultado.Text = "";
            lblResultado.Location = new Point(20, 200);
            lblResultado.Size = new Size(340, 40);
            lblResultado.Font = new Font("Arial", 9);
            lblResultado.ForeColor = Color.Blue;

            // Agregar controles al formulario
            this.Controls.Add(lblNombre);
            this.Controls.Add(txtNombre);
            this.Controls.Add(lblFechaNacimiento);
            this.Controls.Add(dtpFechaNacimiento);
            this.Controls.Add(btnAceptar);
            this.Controls.Add(btnCancelar);
            this.Controls.Add(lblResultado);
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                MostrarResultado();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool ValidarDatos()
        {
            // Validar que el nombre no esté vacío
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Por favor, ingrese un nombre válido.", 
                              "Error de Validación", 
                              MessageBoxButtons.OK, 
                              MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }

            // Validar que la fecha no sea futura (ya está controlado por MaxDate)
            // Validar que la persona tenga al menos 1 año
            DateTime fechaMinima = DateTime.Today.AddYears(-100);
            if (dtpFechaNacimiento.Value < fechaMinima)
            {
                MessageBox.Show("Por favor, ingrese una fecha de nacimiento válida.", 
                              "Error de Validación", 
                              MessageBoxButtons.OK, 
                              MessageBoxIcon.Warning);
                dtpFechaNacimiento.Focus();
                return false;
            }

            return true;
        }

        private void MostrarResultado()
        {
            string nombre = txtNombre.Text.Trim();
            DateTime fechaNacimiento = dtpFechaNacimiento.Value;
            int edad = CalcularEdad(fechaNacimiento);

            string resultado = $"Nombre: {nombre}\n" +
                             $"Fecha de Nacimiento: {fechaNacimiento:dd/MM/yyyy}\n" +
                             $"Edad: {edad} años";

            lblResultado.Text = resultado;

            // También mostrar en MessageBox
            MessageBox.Show(resultado, "Datos Ingresados", 
                          MessageBoxButtons.OK, 
                          MessageBoxIcon.Information);
        }

        private int CalcularEdad(DateTime fechaNacimiento)
        {
            DateTime hoy = DateTime.Today;
            int edad = hoy.Year - fechaNacimiento.Year;
            
            // Ajustar si aún no ha pasado el cumpleaños este año
            if (fechaNacimiento.Date > hoy.AddYears(-edad))
                edad--;

            return edad;
        }
    }
}
