using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace ProyectoLenguajesFormales
{
    class ProyectoLF
    {
        static void Main(string[] args)
        {
            var listaSets = new Dictionary<string,char>();
            Console.WriteLine("Ingrese ruta de archivo:");
            var rutaArchivo=Console.ReadLine();
            var numeroLinea = 0;
            using (var reader = new StreamReader(new FileStream(rutaArchivo, FileMode.Open)))
            {
                var lineaActual = reader.ReadToEnd();
                if(Metodos.PoseePalabrasReservadas(lineaActual)==0)
                {
                    Console.WriteLine("El archivo no contiene todas las palabras reservadas requeridas o posee un error en ellas.");
                    Console.WriteLine("Las palabras reservadas deben de estar en MAYUSCULAS.");
                }
                else
                {
                    reader.BaseStream.Seek(0,SeekOrigin.Begin);
                    lineaActual = reader.ReadLine();
                    var funcionActual = string.Empty;
                    while (lineaActual != null)
                    {
                        if (lineaActual.Contains("SETS")&&funcionActual==string.Empty)
                        {
                            funcionActual = "SETS";
                        }
                        if (lineaActual.Contains("TOKENS") && funcionActual == "SETS")
                        {
                            funcionActual = "TOKENS";
                        }
                        if (lineaActual.Contains("ACTIONS") && funcionActual == "TOKENS")
                        {
                            funcionActual = "ACTIONS";
                        }
                        if (lineaActual.Contains("ERROR")&&funcionActual == "ACTIONS")
                        {
                            funcionActual = "ERROR";
                        }
                        if (funcionActual=="SETS")
                        {
                            if (!lineaActual.Contains("SETS"))
                            {
                                var lineaSet = lineaActual.Trim(' ');
                                var nombreSet = lineaSet.Split('=')[0];
                                var contenidoSet = lineaSet.Split('=')[1];
                                var separacionMas = contenidoSet.Split('+');
                                //foreach (var item in separacionMas)
                                //{

                                //}
                            }
                        }
                        lineaActual = reader.ReadLine();
                        numeroLinea++;
                    }
                }
            }
            Console.ReadLine();
        }
    }
}
