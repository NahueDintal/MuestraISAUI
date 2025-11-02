using System;
using System.Collections.Generic;
using MuestraISAUI.Clases.Entidades;
using MuestraISAUI.Clases.Repositorios;

namespace MuestraISAUI.Clases.Servicios
{
  public class ServicioZodiacal
  {
    private readonly RepositorioEmpanadas _repositorioEmpanadas;
    private readonly Dictionary<string, (string nombre, string lore, decimal precio, string ingredientes)> _saboresCosmicos;

    public ServicioZodiacal()
    {
      _repositorioEmpanadas = new RepositorioEmpanadas();
      _saboresCosmicos = InicializarSaboresCosmicos();
    }

    public (Empanada empanadaDestino, string lore) DescubrirDestinoEmpanaderil(string signoZodiacal)
    {
      var empanadaExistente = _repositorioEmpanadas.ObtenerPorSigno(signoZodiacal);
      
      if (empanadaExistente != null)
      {
          return (empanadaExistente, empanadaExistente.Descripcion);
      }

      return CrearEmpanadaTemporal(signoZodiacal);
    }

    public List<string> ObtenerSignosZodiacales()
    {
      return new List<string>
      {
          "Aries", "Tauro", "Géminis", "Cáncer", "Leo", "Virgo",
          "Libra", "Escorpio", "Sagitario", "Capricornio", "Acuario", "Piscis"
      };
    }

    public string GenerarLoreEpico(string nombreCliente, string signo, Empanada empanada)
    {
        return $@"🌟 **CRÓNICA ASTRAL PARA {nombreCliente.ToUpper()}** 🌟

**Signo Zodiacal:** {signo}
**Empanada del Destino:** {empanada.Nombre}
**Precio Cósmico:** {empanada.PrecioFormateado}

*{empanada.Descripcion}*

**Ingredientes Místicos:**
{empanada.Ingredientes}

¡Que los astros guíen tu paladar, {nombreCliente}!
✨🥟✨";
        }

        public string GenerarTicketEpico(Venta venta)
        {
            var ticket = $@"
╔══════════════════════════════════════════════╗
║           🌟 EMPANADAS ESTELARES 🌟         ║
║        *Donde los astros guían tu hambre*    ║
╠══════════════════════════════════════════════╣
║ Ticket N°: {venta.IdVenta,-30} 🌟║
║ Cliente: {venta.NombreCliente,-32} 🧙║
║ Signo: {venta.SignoZodiacal,-35} ♌║
║ Fecha: {venta.FechaVenta:dd/MM/yyyy HH:mm,-25} 📅║
╠══════════════════════════════════════════════╣";

            foreach (var detalle in venta.Detalles)
            {
                ticket += $@"
║ 🥟 {detalle.NombreProducto,-25} {detalle.Cantidad}x {detalle.PrecioFormateado} ║
║   Subtotal: {detalle.SubtotalFormateado,-33} ║";
            }

            ticket += $@"
╠══════════════════════════════════════════════╣
║ Total: {venta.TotalFormateado,-38} ║
╠══════════════════════════════════════════════╣
║                                              ║
║    {venta.LoreAstral,-40} ║
║                                              ║
║    ¡Que los Siete Dioses bendigan tu         ║
║        viaje culinario! ✨                   ║
║                                              ║
║    🏰✨ El Invierno Se Acerca ✨🏰          ║
╚══════════════════════════════════════════════╝";

      return ticket;
    }

    private (Empanada, string) CrearEmpanadaTemporal(string signo)
    {
      if (_saboresCosmicos.ContainsKey(signo))
      {
        var (nombre, lore, precio, ingredientes) = _saboresCosmicos[signo];
        var empanada = new Empanada(nombre, lore, precio, signo)
        {
            Ingredientes = ingredientes
        };
        return (empanada, lore);
      }

      // Empanada por defecto
      var empanadaDefault = new Empanada(
          "Clásica Misteriosa", 
          "Los astros aún deliberan sobre tu destino culinario", 
          250, 
          signo
      )
      {
        Ingredientes = "Carne, cebolla, huevo, aceitunas - los ingredientes básicos del cosmos"
      };
      
      return (empanadaDefault, empanadaDefault.Descripcion);
    }

    private Dictionary<string, (string, string, decimal, string)> InicializarSaboresCosmicos()
    {
      return new Dictionary<string, (string, string, decimal, string)>
      {
        ["Aries"] = ("Picante", "Tu espíritu guerrero necesita el fuego del ají para conquistar nuevos horizontes culinarios", 280, "Carne, ají molido, cebolla, huevo, aceitunas"),
        ["Tauro"] = ("Carne Premium", "Como amante de los placeres terrenales, solo la mejor carne satisface tu paladar exigente", 320, "Lomo, morrones, cebolla, especias seleccionadas"),
        ["Géminis"] = ("Doble Queso", "Tu naturaleza dual exige sabores que se complementen en perfecta armonía", 300, "Muzzarella, provolone, jamón, nuez moscada"),
        ["Cáncer"] = ("Pollo Cremoso", "Tu lado emocional y hogareño aprecia el comfort de sabores suaves y reconfortantes", 290, "Pollo, crema, cebolla, apio, zanahoria"),
        ["Leo"] = ("Jamón Crudo y Rúcula", "Como rey del zodiaco, tu empanada debe ser sofisticada y digna de tu grandeza", 350, "Jamón crudo, rúcula, queso parmesano, aceite de oliva"),
        ["Virgo"] = ("Verdura Perfecta", "Tu mente analítica valora el balance nutricional y la precisión en cada ingrediente", 270, "Espinaca, acelga, zanahoria, huevo, queso"),
        ["Libra"] = ("Capresse", "Buscas el equilibrio perfecto entre queso, tomate y albahaca, como la armonía del universo", 310, "Muzzarella, tomate, albahaca, aceite de oliva"),
        ["Escorpio"] = ("Camarón Picante", "Misteriosa e intensa, como las profundidades marinas que rigen tu signo", 380, "Camarones, ají, cebolla, pimiento, cilantro"),
        ["Sagitario"] = ("Humita con Especias", "Tu alma aventurera anhela sabores exóticos y viajes culinarios", 295, "Choclo, cebolla, pimiento, especias, crema"),
        ["Capricornio"] = ("Clásica de Carne", "Tradicional y confiable, como las montañas que simbolizan tu ambición", 260, "Carne, cebolla, huevo, aceitunas, pasas de uva"),
        ["Acuario"] = ("Ternera con Blue Cheese", "Innovadora y sorprendente, rompiendo paradigmas gastronómicos", 340, "Ternera, blue cheese, cebolla caramelizada, nueces"),
        ["Piscis"] = ("Espinaca y Ricotta", "Tu naturaleza soñadora se deleita con texturas suaves y sabores etéreos", 285, "Espinaca, ricotta, nuez moscada, pasas de uva")
      };
    }
  }
}
