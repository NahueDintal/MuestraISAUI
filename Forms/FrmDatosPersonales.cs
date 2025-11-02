using System;
using System.Drawing;
using System.Windows.Forms;
using MuestraISAUI.Clases.Servicios;

namespace MuestraISAUI.Forms
{
    public partial class FrmDatosPersonales : Form
    {
        private TextBox txtNombre;
        private TextBox txtApellido;
        private TextBox txtDocumento;
        private ComboBox cmbSigno;
        private Button btnAceptar;
        private Button btnCancelar;
        private Label lblNombre, lblApellido, lblDocumento, lblSigno;
        
        private readonly ServicioZodiacal _servicioZodiacal;
        private readonly ServicioVentas _servicioVentas;

        public FrmDatosPersonales()
        {
            InitializeComponent();
            _servicioZodiacal = new ServicioZodiacal();
            _servicioVentas = new ServicioVentas();
            CargarSignos();
        }

        private void CargarSignos()
        {
            cmbSigno.Items.Clear();
            foreach (var signo in _servicioZodiacal.ObtenerSignosZodiacales())
            {
                cmbSigno.Items.Add(signo);
            }
            if (cmbSigno.Items.Count > 0)
                cmbSigno.SelectedIndex = 0;
        }

        private void InitializeComponent()
        {
            // Configuración del formulario
            this.Text = "🌟 EMPANADAS ESTELARES - Tu Destino Cósmico 🌟";
            this.Size = new Size(500, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.MidnightBlue;
            this.ForeColor = Color.White;
            this.Font = new Font("Segoe UI", 10);

            // Título épico
            var lblTitulo = new Label
            {
                Text = "🎭 EL GRAN TORNEO CULINARIO DEL ZODIACO 🎭",
                Location = new Point(20, 20),
                Size = new Size(460, 30),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.Gold,
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Labels
            lblNombre = CrearLabel("Nombre del Aventurero:", 20, 70);
            lblApellido = CrearLabel("Apellido del Guerrero:", 20, 120);
            lblDocumento = CrearLabel("Documento de Identidad:", 20, 170);
            lblSigno = CrearLabel("Tu Signo Zodiacal:", 20, 220);

            // TextBoxes
            txtNombre = CrearTextBox(220, 70);
            txtApellido = CrearTextBox(220, 120);
            txtDocumento = CrearTextBox(220, 170);

            // ComboBox para Signos
            cmbSigno = new ComboBox();
            cmbSigno.Location = new Point(220, 220);
            cmbSigno.Size = new Size(200, 25);
            cmbSigno.Font = new Font("Segoe UI", 10);
            cmbSigno.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSigno.BackColor = Color.White;
            cmbSigno.ForeColor = Color.Black;

            // Botones
            btnAceptar = CrearBoton("✨ DESCUBRIR MI DESTINO ✨", 120, 280, Color.Gold, Color.Black);
            btnCancelar = CrearBoton("🚪 ABANDONAR EL TORNEO", 280, 280, Color.Red, Color.White);

            btnAceptar.Click += new EventHandler(btnAceptar_Click);
            btnCancelar.Click += new EventHandler(btnCancelar_Click);

            // Agregar controles
            this.Controls.AddRange(new Control[] {
                lblTitulo,
                lblNombre, lblApellido, lblDocumento, lblSigno,
                txtNombre, txtApellido, txtDocumento, cmbSigno,
                btnAceptar, btnCancelar
            });
        }

        private Label CrearLabel(string texto, int x, int y)
        {
            return new Label
            {
                Text = texto,
                Location = new Point(x, y),
                Size = new Size(190, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.LightSkyBlue
            };
        }

        private TextBox CrearTextBox(int x, int y)
        {
            return new TextBox
            {
                Location = new Point(x, y),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.White,
                ForeColor = Color.Black
            };
        }

        private Button CrearBoton(string texto, int x, int y, Color colorFondo, Color colorTexto)
        {
            return new Button
            {
                Text = texto,
                Location = new Point(x, y),
                Size = new Size(200, 40),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                BackColor = colorFondo,
                ForeColor = colorTexto,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                RevelarDestinoCosmico();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            var resultado = MessageBox.Show(
                "¿Estás seguro de abandonar el Gran Torneo Culinario?\n\nLos astros se entristecerán...",
                "⚠️ CONFIRMAR SALIDA",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private bool ValidarDatos()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MostrarError("Por favor, ingresa tu nombre de aventurero.");
                txtNombre.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                MostrarError("Todo guerrero necesita un apellido para la historia.");
                txtApellido.Focus();
                return false;
            }

            if (cmbSigno.SelectedItem == null)
            {
                MostrarError("Debes elegir tu signo zodiacal para conocer tu destino.");
                cmbSigno.Focus();
                return false;
            }

            return true;
        }

        private void MostrarError(string mensaje)
        {
            MessageBox.Show(mensaje, "❌ DATOS INCOMPLETOS", 
                          MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void RevelarDestinoCosmico()
        {
            try
            {
                string nombre = txtNombre.Text.Trim();
                string apellido = txtApellido.Text.Trim();
                string documento = txtDocumento.Text.Trim();
                string signo = cmbSigno.SelectedItem.ToString();

                // Descubrir destino usando el servicio
                var (empanadaDestino, lore) = _servicioZodiacal.DescubrirDestinoEmpanaderil(signo);
                
                // Generar mensaje épico
                string mensajeEpico = _servicioZodiacal.GenerarLoreEpico(nombre, signo, empanadaDestino);

                // Mostrar revelación
                var resultado = MessageBox.Show(mensajeEpico, 
                    "🌟 ¡REVELACIÓN CÓSMICA!", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    // Procesar venta épica
                    int idVenta = _servicioVentas.ProcesarVentaEpica(nombre, apellido, documento, signo);
                    
                    MessageBox.Show(
                        $"¡VENTA ÉPICA COMPLETADA!\n\nTu destino ha sido sellado con el ticket número: {idVenta}\n\n¡Que los astros bendigan tu paladar!",
                        "🎉 DESTINO CUMPLIDO",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ ERROR CÓSMICO: {ex.Message}", 
                              "FALLA EN EL DESTINO", 
                              MessageBoxButtons.OK, 
                              MessageBoxIcon.Error);
            }
        }
    }
}
