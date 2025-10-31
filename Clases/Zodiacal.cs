using System;
using System.Collections.Generic;
using System.Data;
using MuestraISAUI.Clases;

namespace MuestraISAUI.Clases
{
    public class Zodiacal
    {
        private Dictionary<string, (string sabor, string lore, decimal precio, string ingredientes)> _saboresPorSigno;

        public Zodiacal()
        {
            InicializarSaboresCosmicos();
        }

        private void InicializarSaboresCosmicos()
        {
            _saboresPorSigno = new Dictionary<string, (string, string, decimal, string)>
            {
                ["Aries"] = ("Picante", "Tu espíritu guerrero necesita el fuego del ají", 280, "Carne, ají, cebolla, huevo"),
                ["Tauro"] = ("Carne Premium", "Como amante de los placeres terrenales", 320, "Lomo, morrones, cebolla"),
                ["Géminis"] = ("Doble Queso", "Tu naturaleza dual exige sabores complementarios", 300, "Muzzarella, provolone, jamón"),
                ["Cáncer"] = ("Pollo Cremoso", "Tu lado emocional aprecia el comfort", 290, "Pollo, crema, cebolla, apio"),
                ["Leo"] = ("Jamón Crudo y Rúcula", "Como rey del zodiaco, tu empanada debe ser sofisticada", 350, "Jamón crudo, rúcula, queso parmesano"),
                ["Virgo"] = ("Verdura Perfecta", "Tu mente analítica valora el balance", 270, "Espinaca, acelga, zanahoria, huevo"),
                ["Libra"] = ("Capresse", "Buscas el equilibrio perfecto", 310, "Muzzarella, tomate, albahaca"),
                ["Escorpio"] = ("Camarón Picante", "Misteriosa e intensa como las profundidades", 380, "Camarones, ají, cebolla, pimiento"),
                ["Sagitario"] = ("Humita con Especias", "Tu alma aventurera anhela sabores exóticos", 295, "Choclo, cebolla, pimiento, especias"),
                ["Capricornio"] = ("Clásica de Carne", "Tradicional y confiable como las montañas", 260, "Carne, cebolla, huevo, aceitunas"),
                ["Acuario"] = ("Ternera con Blue Cheese", "Innovadora y sorprendente", 340, "Ternera, blue cheese, cebolla caramelizada"),
                ["Piscis"] = ("Espinaca y Ricotta", "Tu naturaleza soñadora se deleita con texturas suaves", 285, "Espinaca, ricotta, nuez moscada")
            };
        }

        public List<string> ObtenerSignosDisponibles()
        {
            return new List<string>(_saboresPorSigno.Keys);
        }

        public (string sabor, string lore, decimal precio, string ingredientes) ObtenerEmpanadaDestino(string signo)
        {
            if (_saboresPorSigno.ContainsKey(signo))
                return _saboresPorSigno[signo];
            
            return ("Clásica de Carne", "El universo aún no ha decidido tu sabor perfecto", 250, "Carne, cebolla, huevo");
        }
    }
}
