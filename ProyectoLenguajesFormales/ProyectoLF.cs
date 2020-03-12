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
            var dictionarySets = new Dictionary<string, List<int>>();
            var dictionaryActions = new Dictionary<string, string>();
            var listError = new List<string>();
            var listNombreSets = new List<string>();
            var dictionaryTokens = new Dictionary<string, string>();
            Console.WriteLine("Ingrese ruta de archivo:");
            var rutaArchivo = Console.ReadLine();
            var numeroLinea = 1;
            var error = false;
            using (var reader = new StreamReader(new FileStream(rutaArchivo, FileMode.Open)))
            {
                try
                {
                    var lineaActual = reader.ReadToEnd();
                    if (Metodos.PoseePalabrasReservadas(lineaActual) == 0)
                    {
                        Console.WriteLine("El archivo no contiene todas las palabras reservadas requeridas o posee un error en ellas.");
                        Console.WriteLine("Las palabras reservadas deben de estar en MAYUSCULAS.");
                    }
                    else
                    {
                        lineaActual = string.Empty;
                        reader.BaseStream.Seek(0, SeekOrigin.Begin);
                        lineaActual = reader.ReadLine();
                        lineaActual = lineaActual.Replace("\t", " ");
                        lineaActual = lineaActual.Trim();

                        var funcionActual = string.Empty;
                        while (lineaActual != null && !error)
                        {
                            lineaActual = lineaActual.Replace("\t", " ");
                            lineaActual = lineaActual.Trim();
                            if (lineaActual.Contains("SETS") && funcionActual == string.Empty)
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
                            if (lineaActual.Contains("ERROR") && funcionActual == "ACTIONS")
                            {
                                funcionActual = "ERROR";
                            }
                            if (funcionActual == "SETS")
                            {
                                if (!lineaActual.Contains("SETS"))
                                {

                                    var lineaSet = lineaActual.Replace("\t", " ");
                                    lineaSet = lineaSet.Trim();
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
                                        nombreSet = nombreSet.Trim();
                                        dictionarySets.Add(nombreSet, listValuesDict);
                                        listNombreSets.Add(nombreSet);

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
                                                var contSet = Enumerable.Range(valorRInicio, conteo).Select(x => (Int32)x).ToList();
                                                listValuesDict.AddRange(contSet);
                                            }
                                        }
                                        nombreSet = nombreSet.Trim();
                                        dictionarySets.Add(nombreSet, listValuesDict);
                                        listNombreSets.Add(nombreSet);
                                    }

                                }
                            }
                            if (funcionActual == "TOKENS")
                            {
                                if (!lineaActual.Contains("TOKENS"))
                                {
                                    if (lineaActual.Contains("'"))
                                    {
                                        var arregloLinea = lineaActual.ToCharArray();
                                        var nombreToken = string.Empty;
                                        var contieneSet = false;
                                        var token = string.Empty;
                                        foreach (var item in listNombreSets)
                                        {
                                            if (lineaActual.Contains(item))
                                            {
                                                contieneSet = true;
                                            }
                                        }
                                        if (contieneSet)
                                        {
                                            var dobleComilla = ('"').ToString();
                                            var comilla = ("'").ToString();
                                            if (lineaActual.Contains("'''"))
                                            {
                                                lineaActual = lineaActual.Replace("'''", "'");
                                            }
                                            if (lineaActual.Contains(comilla + dobleComilla + comilla))
                                            {
                                                lineaActual = lineaActual.Replace((comilla + dobleComilla + comilla).ToString(), dobleComilla);
                                            }
                                            foreach (var item in lineaActual)
                                            {
                                                if (nombreToken.Contains("="))
                                                {
                                                    token = token + (item);
                                                }
                                                else
                                                {
                                                    nombreToken = nombreToken + (item);
                                                }
                                            }
                                            token = token.TrimStart();
                                            token = token.TrimEnd();
                                            token = token.Replace(" ", ".");
                                            var tokenParentesis = "(";
                                            foreach (var item in token)
                                            {
                                                if (item=='|')
                                                {
                                                    tokenParentesis = tokenParentesis + ")" + item+"(";
                                                }
                                                else
                                                {
                                                    tokenParentesis += item;
                                                }                                                
                                            }
                                            tokenParentesis += ")";
                                            dictionaryTokens.Add(nombreToken.Replace('=', ' ').Trim(), tokenParentesis);
                                        }
                                        else
                                        {
                                            foreach (var item in arregloLinea)
                                            {
                                                if (nombreToken.Contains("="))
                                                {
                                                    token = token + (item);
                                                }
                                                else
                                                {
                                                    nombreToken = nombreToken + (item);
                                                }
                                            }
                                            if (token.Contains("'''"))
                                            {
                                                var comilla = ("'").ToCharArray();
                                                var separarEspacios = token.Split(comilla[0]);
                                            }
                                            else
                                            {
                                                token = token.Replace("'", "");
                                                var tokenFinal = string.Empty;
                                                var contador = 0;
                                                token = token.TrimStart();
                                                token = token.TrimEnd();
                                                foreach (var item in token)
                                                {
                                                    if (contador == token.Length - 1)
                                                    {
                                                        tokenFinal = tokenFinal + item;
                                                    }
                                                    else
                                                    {
                                                        tokenFinal = tokenFinal + item + ".";
                                                    }
                                                    contador++;
                                                }
                                                dictionaryTokens.Add(nombreToken.Replace('=', ' ').Trim(), tokenFinal);
                                            }
                                        }

                                    }
                                    else
                                    {
                                        var contieneSet = false;

                                        foreach (var item in listNombreSets)
                                        {
                                            if (lineaActual.Contains(item))
                                            {
                                                contieneSet = true;
                                            }
                                        }
                                        if (contieneSet)
                                        {

                                            var separadaIgual = lineaActual.Split('=');
                                            if (separadaIgual[1].Contains("  "))
                                            {
                                                separadaIgual[1] = separadaIgual[1].Replace(" *", "*");
                                                separadaIgual[1] = separadaIgual[1].Replace("  ", ".");
                                                separadaIgual[1] = separadaIgual[1].TrimStart();
                                                separadaIgual[1] = separadaIgual[1].TrimEnd();
                                                var sustituidoEspacios = separadaIgual[1].Trim();
                                                dictionaryTokens.Add(separadaIgual[0].Replace('=', ' ').Trim(), sustituidoEspacios);
                                            }
                                            else
                                            {
                                                separadaIgual[1] = separadaIgual[1].TrimStart();
                                                separadaIgual[1] = separadaIgual[1].TrimEnd();
                                                separadaIgual[1] = separadaIgual[1].Replace(" *", "*");
                                                var tokenFin = string.Empty;
                                                if (separadaIgual[1].Contains(" ("))
                                                {
                                                    foreach (var item in separadaIgual[1])
                                                    {
                                                        if (item == '(')
                                                        {
                                                            tokenFin = tokenFin + '.' + item;
                                                        }
                                                        else if (item == ' ')
                                                        {
                                                            //skip
                                                        }
                                                        else
                                                        {
                                                            tokenFin = tokenFin + item;
                                                        }
                                                    }
                                                    dictionaryTokens.Add(separadaIgual[0].Replace('=', ' ').Trim(), tokenFin);
                                                }
                                                else
                                                {
                                                    var sustituidoEspacio = separadaIgual[1].Replace(" ", ".");
                                                    dictionaryTokens.Add(separadaIgual[0].Replace('=', ' ').Trim(), sustituidoEspacio);
                                                }

                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("Hay un error en la linea: " + numeroLinea);
                                        }

                                    }
                                }
                            }
                            if (funcionActual == "ACTIONS")
                            {
                                if (!lineaActual.Contains("ACTIONS"))
                                {
                                    if (lineaActual.Contains("{") || lineaActual.Contains("}"))
                                    {
                                        //skip
                                    }
                                    else
                                    {
                                        if (lineaActual.Contains("="))
                                        {
                                            var formatoFinal = lineaActual.Replace("\t", string.Empty);
                                            formatoFinal = formatoFinal.Replace("'", string.Empty);
                                            formatoFinal = formatoFinal.Trim();
                                            var separacionIgual = formatoFinal.Split('=');
                                            dictionaryActions.Add(separacionIgual[0], separacionIgual[1]);
                                        }
                                    }
                                }
                            }
                            if (funcionActual == "ERROR")
                            {
                                if (lineaActual.Contains("ERROR"))
                                {
                                    var separacionIgual = lineaActual.Split('=');
                                    listError.Add(separacionIgual[1].Trim());
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
                catch (Exception)
                {

                    Console.WriteLine("Posee un error en la linea" + numeroLinea);
                }
            }
            var listaExpresionRegular = new List<string>();
            var expresionRegular = string.Empty;
            var conteo2 = 0;
            foreach (var item in dictionaryTokens)
            {
                var tokenActual = "(" + item.Value + ")";
                if (conteo2 == dictionaryTokens.Count - 1)
                {
                    expresionRegular += tokenActual;
                }
                else
                {
                    expresionRegular += tokenActual + "|";
                }
                conteo2++;
            }
            expresionRegular = "(" + expresionRegular + ")" + ".#";
            var tokenConcatenacion = string.Empty;
            foreach (var item in expresionRegular)
            {
                if (item == '(' || item == ')' || item == '.' || item == '*' || item == '?' || item == '+' || item == '|' || item == '#')
                {
                    if (tokenConcatenacion.Length != 0)
                    {
                        listaExpresionRegular.Add(tokenConcatenacion.ToString());
                        tokenConcatenacion = string.Empty;
                        listaExpresionRegular.Add(item.ToString());
                    }
                    else
                    {
                        listaExpresionRegular.Add(item.ToString());
                    }
                }
                else
                {
                    tokenConcatenacion = tokenConcatenacion + item;
                }
            }
            //Crear Arbol
            var stackArboles = new Stack<NodoArbol>();
            var stackTokens = new Stack<string>();
            var ArbolExpresion = Metodos.CreacionArbol(listaExpresionRegular, stackTokens, stackArboles);
            Console.ReadLine();

        }
    }
}
