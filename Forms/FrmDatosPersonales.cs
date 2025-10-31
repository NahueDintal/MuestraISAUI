using System;
using System.Drawing;
using System.Windows.Forms;
using MuestraISAUI.Clases;

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
        
        private Zodiacal _zodiacal;

        public FrmDatosPersonales()
        {
            InitializeComponent();
            _zodiacal = new Zodiacal();
            CargarSignos();
        }

        private void CargarSignos()
        {
            cmbSigno.Items.Clear();
            foreach (var signo in _zodiacal.ObtenerSignosDisponibles())
            {
                cmbSigno.Items.Add(signo);
            }
            cmbSigno.SelectedIndex = 0;
        }

        private void InitializeComponent()
        {
            // Configuración del formulario
            this.Text = "🌟 Empandas Estelares - Tu Destino Cósmico 🌟";
            this.Size = new Size(450, 350);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.Navy;
            this.ForeColor = Color.White;

            // Labels
            lblNombre = CrearLabel("Nombre:", 20, 30);
            lblApellido = CrearLabel("Apellido:", 20, 80);
            lblDocumento = CrearLabel("Documento:", 20, 130);
            lblSigno = CrearLabel("Tu Signo Zodiacal:", 20, 180);

            // TextBoxes
            txtNombre = CrearTextBox(150, 30);
            txtApellido = CrearTextBox(150, 80);
            txtDocumento = CrearTextBox(150, 130);

            // ComboBox para Signos
            cmbSigno = new ComboBox();
            cmbSigno.Location = new Point(150, 180);
            cmbSigno.Size = new Size(200, 25);
            cmbSigno.Font = new Font("Arial", 10);
            cmbSigno.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSigno.BackColor = Color.White;
            cmbSigno.ForeColor = Color.Black;

            // Botones
            btnAceptar = CrearBoton("✨ Descubre tu Destino", 100, 230, Color.Gold);
            btnCancelar = CrearBoton("🚪 Salir", 250, 230, Color.Silver);

            btnAceptar.Click += new EventHandler(btnAceptar_Click);
            btnCancelar.Click += new EventHandler(btnCancelar_Click);

            // Agregar controles
            this.Controls.AddRange(new Control[] {
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
                Size = new Size(120, 20),
                Font = new Font("Arial", 10, FontStyle.Bold),
                ForeColor = Color.Gold
            };
        }

        private TextBox CrearTextBox(int x, int y)
        {
            return new TextBox
            {
                Location = new Point(x, y),
                Size = new Size(200, 25),
                Font = new Font("Arial", 10)
            };
        }

        private Button CrearBoton(string texto, int x, int y, Color colorFondo)
        {
            return new Button
            {
                Text = texto,
                Location = new Point(x, y),
                Size = new Size(140, 35),
                Font = new Font("Arial", 9, FontStyle.Bold),
                BackColor = colorFondo,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat
            };
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                //RevelarDestinoCosmico();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool ValidarDatos()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || 
                string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                MessageBox.Show("Por favor, completa tu nombre y apellido.", 
                              "Datos Incompletos", 
                              MessageBoxButtons.OK, 
                              MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

    //     private void RevelarDestinoCosmico()
    //     {
    //         string nombre = txtNombre.Text.Trim();
    //         string apellido = txtApellido.Text.Trim();
    //         string documento = txtDocumento.Text.Trim();
    //         string signo = cmbSigno.SelectedItem.ToString();
    //
    //         var (sabor, lore, precio) = _servicioAstral.ObtenerEmpanadaDestino(signo);
    //
    //         string mensajeCosmico = $"""
    //         🌟 **REVELACIÓN ASTRAL GASTRONÓMICA** 🌟
    //
    //         **{nombre} {apellido}**, eres del signo **{signo}**
    //
    //         ✨ *El universo ha hablado:* ✨
    //         Tu empanada del destino es:
    //         🥟 **{sabor}** 🥟
    //
    //         💰 **Precio Cósmico:** ${precio}
    //
    //         📜 *Sabiduría astral:*
    //         {lore}
    //
    //         ¿Aceptas tu destino empanaderil?
    //         """;
    //
    //         var resultado = MessageBox.Show(mensajeCosmico, 
    //                                      "¡REVELACIÓN CÓSMICA!", 
    //                                      MessageBoxButtons.YesNo, 
    //                                      MessageBoxIcon.Question);
    //
    //         if (resultado == DialogResult.Yes)
    //         {
    //             // Guardar cliente y proceder con la venta
    //             GuardarClienteYVenta(nombre, apellido, documento, signo, sabor, precio, lore);
    //         }
    //     }
    //
    //     private void GuardarClienteYVenta(string nombre, string apellido, string documento, 
    //                                     string signo, string sabor, decimal precio, string lore)
    //     {
    //         try
    //         {
    //             // Guardar cliente
    //             var cliente = new Cliente();
    //             cliente.Insertar(nombre, apellido, documento, signo, sabor);
    //
    //             // Registrar venta
    //             var venta = new Venta();
    //             venta.RegistrarVenta(0, $"{nombre} {apellido}", signo, sabor, precio, lore);
    //
    //             // Mostrar ticket
    //             MostrarTicketCosmico(nombre, apellido, signo, sabor, precio, lore);
    //
    //             this.Close();
    //         }
    //         catch (Exception ex)
    //         {
    //             MessageBox.Show($"Error cósmico: {ex.Message}", "Error", 
    //                           MessageBoxButtons.OK, MessageBoxIcon.Error);
    //         }
    //     }
    //
    //     private void MostrarTicketCosmico(string nombre, string apellido, string signo, 
    //                                     string sabor, decimal precio, string lore)
    //     {
    //         string ticket = $"""
    //         ╔═══════════════════════════════════════╗
    //         ║        🌟 EMPANADAS ESTELARES 🌟     ║
    //         ║     *Donde los astros saben de sabores*║
    //         ╠═══════════════════════════════════════╣
    //         ║ Cliente: {nombre} {apellido,-15} 🌟║
    //         ║ Signo Zodiacal: {signo,-12} ♈║
    //         ╠═══════════════════════════════════════╣
    //         ║         🥟 EMPANADA DEL DESTINO 🥟   ║
    //         ║           **{sabor}**            ║
    //         ╠═══════════════════════════════════════╣
    //         ║ Precio Cósmico: ${precio}            ║
    //         ╠═══════════════════════════════════════╣
    //         ║           📜 SABIDURÍA CÓSMICA 📜    ║
    //         ║ {lore,-37} ║
    //         ╠═══════════════════════════════════════╣
    //         ║                                       ║
    //         ║ ¡Que los astros bendigan tu paladar!  ║
    //         ║     ✨🍽️✨                           ║
    //         ╚═══════════════════════════════════════╝
    //         """;
    //
    //         MessageBox.Show(ticket, "🎉 ¡DESTINO CUMPLIDO!", 
    //                       MessageBoxButtons.OK, MessageBoxIcon.Information);
    //     }
    }
}
