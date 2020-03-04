using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoLenguajesFormales
{
    class Metodos
    {
        public static int PoseePalabrasReservadas(string archivo)
        {
            var resultado = 1;
            if (!archivo.Contains("SETS"))
            {
                resultado = 0;
            }
            if(!archivo.Contains("TOKENS"))
            { 
                resultado = 0;
            }
            if (!archivo.Contains("ACTIONS"))
            {
                resultado = 0;
            }
            if (!archivo.Contains("ERROR"))
            {
                resultado = 0;
            }
            return resultado;
        }
        
    }
}
