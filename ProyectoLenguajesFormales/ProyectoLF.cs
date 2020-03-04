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
            var dictionarySets = new Dictionary<string,List<int>>();
            var dictionaryActions = new Dictionary<string, string>();
            var listError = new List<string>();
            Console.WriteLine("Ingrese ruta de archivo:");
            var rutaArchivo=Console.ReadLine();
            var numeroLinea = 0;
            var error = false;
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
                    lineaActual.Replace('\t',' ');
                    lineaActual.Trim();
                    var funcionActual = string.Empty;
                    while (lineaActual != null && !error)
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

                                var lineaSet = lineaActual.Replace("\t"," ");
                                lineaSet=lineaSet.Trim();
                                var nombreSet = lineaSet.Split('=')[0];
                                var contenidoSet = lineaSet.Split('=')[1]; 
                                var listValuesDict = new List<int>();
                                if (contenidoSet.Contains("CHR"))
                                {
                                    var contenidoCHR = contenidoSet.Replace("CHR", string.Empty);
                                    contenidoCHR = contenidoCHR.Replace("..", "|");
                                    contenidoCHR = contenidoCHR.Replace("(", string.Empty);
                                    contenidoCHR = contenidoCHR.Replace(")", string.Empty);
                                    var rango = contenidoCHR.Split('|');
                                    var conteo = Convert.ToInt32(rango[1]) - Convert.ToInt32(rango[0]);
                                    var contSet = Enumerable.Range(Convert.ToInt32(rango[0]), conteo).Select(x => (Int32)x).ToList();
                                    listValuesDict = contSet;
                                    dictionarySets.Add(nombreSet, listValuesDict);
                                }
                                else
                                {
                                    var separacionMas = contenidoSet.Split('+');
                                    foreach (var item in separacionMas)
                                    {
                                        var sustitucionPuntos = item.Replace("..", ".");
                                        sustitucionPuntos = sustitucionPuntos.Trim();
                                        var rango = sustitucionPuntos.Split('.');
                                        if (rango.Length == 1)
                                        {
                                            rango[0] = rango[0].Replace("'", string.Empty);
                                            var valorRInicio = Convert.ToInt32(rango[0].ToCharArray()[0]);
                                            listValuesDict.Add(valorRInicio);
                                        }
                                        else
                                        {
                                            rango[0] = rango[0].Replace("'", string.Empty);
                                            rango[1] = rango[1].Replace("'", string.Empty);
                                            var valorRInicio = Convert.ToInt32(rango[0].ToCharArray()[0]);
                                            var valorRFin = Convert.ToInt32(rango[1].ToCharArray()[0]);
                                            var conteo = valorRFin - valorRInicio;
                                            var contSet=Enumerable.Range(valorRInicio,conteo).Select(x=>(Int32)x).ToList();
                                            listValuesDict = contSet;
                                        }
                                    }
                                    dictionarySets.Add(nombreSet, listValuesDict);
                                }
                                
                            }
                        }
                        if (funcionActual=="TOKENS")
                        {
                            if (!lineaActual.Contains("TOKENS"))
                            {
                                if (lineaActual.Contains("'='"))
                                {

                                }
                                else
                                {

                                }
                            }
                        }
                        if (funcionActual=="ACTIONS")
                        {
                            if (!lineaActual.Contains("ACTIONS"))
                            {
                                if (lineaActual==("{")||lineaActual==("}"))
                                {
                                    //skip
                                }
                                else
                                {
                                    if (lineaActual.Contains("="))
                                    {
                                        var codigoAction = string.Empty;
                                        var valueAction = string.Empty;
                                        var separacionIgual = lineaActual.Split('=');
                                        codigoAction = separacionIgual[0];
                                        valueAction = separacionIgual[1];
                                        valueAction.Replace("'", string.Empty);
                                        valueAction.Trim();
                                        dictionaryActions.Add(codigoAction, valueAction);
                                    }
                                    else if (!lineaActual.Contains("RESERVADAS"))
                                    {
                                        Console.WriteLine("En ACTIONS no posee la palabra RESERVADAS(), linea: " + numeroLinea); 
                                    }
                                }
                            }
                        }
                        if (funcionActual=="ERROR")
                        {
                            if (lineaActual.Contains("ERROR"))
                            {
                                var separacionIgual = lineaActual.Split('=');
                                listError.Add(separacionIgual[1]);
                            }
                            else
                            {
                                Console.WriteLine("No posee la palabra clave ERROR en la linea: " + numeroLinea);
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
