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
            var dictionaryFollows = new Dictionary<int, List<int>>();
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
                                                if (item == '|')
                                                {
                                                    tokenParentesis = tokenParentesis + ")" + item + "(";
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
                                            dictionaryActions.Add(separacionIgual[1], separacionIgual[0]);
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
            //Enumerar Hojas
            var cantidadHojas = Metodos.EnumerarHojas(ArbolExpresion);
            //Detectar Nullables
            Metodos.IdentificarNulos(ArbolExpresion);
            //First y Last
            Metodos.IdentificarFirst(ArbolExpresion);
            Metodos.IdentificarLast(ArbolExpresion);
            //Follows
            dictionaryFollows = Metodos.IdentificarFollows(ArbolExpresion, cantidadHojas);

            Console.WriteLine("SETS");
            foreach (var item in dictionarySets)
            {
                var first = string.Empty;
                foreach (var item2 in item.Value)
                {
                    first = first + Convert.ToChar(item2) + ",";
                }
                Console.WriteLine(item.Key + "|" + first);
            }
            Console.ReadLine();
            Console.Clear();

            //Transiciones
            Console.WriteLine("TOKEN|FIRST|LAST-----CON RECORRIDO POSTORDEN");
            Metodos.MostrarFirstLast(ArbolExpresion);
            Console.ReadLine();
            Console.Clear();

            Console.WriteLine("----------------FOLLOW------CON RECORRIDO POSTORDEN");
            Console.WriteLine("ID HOJA|FOLLOW");

            foreach (var item in dictionaryFollows)
            {
                var first = string.Empty;
                foreach (var item2 in item.Value)
                {
                    first = first + item2 + ",";
                }
                Console.WriteLine(item.Key + "|" + first);
            }

            Console.ReadLine();
            Console.Clear();

            var lista = Metodos.Transiciones(dictionaryFollows, ArbolExpresion);
            //Construir sintaxis
            var diccionarioIf = new Dictionary<int, string>();
            var listCase = new List<string>();
            var i = 1;
            foreach (var item in lista)
            {
                var dividir = item.Split('|');
                if (dividir[0] == "Caracteres" && dividir.Length == 2)
                {
                    var dividirCaracteres = dividir[1].Split('~');
                    foreach (var item2 in dividirCaracteres)
                    {
                        if (dictionarySets.ContainsKey(item2))
                        {
                            var listadoCaracteres = dictionarySets[item2];
                            var inferiorLimite = listadoCaracteres[0];
                            var superiorLimite = listadoCaracteres.Max();
                            var sentenciaIf = "Convert.ToByte(Convert.ToChar(caracter[i])) >= " + inferiorLimite + " && Convert.ToByte(Convert.ToChar(caracter[i])) <=" + superiorLimite;
                            diccionarioIf.Add(i, sentenciaIf);
                            i++;
                        }
                        else
                        {
                            var caracterAscii = Convert.ToByte(Convert.ToChar(item2));
                            var sentenciaIf = "Convert.ToByte(Convert.ToChar(caracter[i]))==" + caracterAscii;
                            diccionarioIf.Add(i, sentenciaIf);
                            i++;
                        }
                    }
                }
                else if (dividir.Length == 3)
                {
                    var sentenciaCase = "case " + dividir[0] + " : ";
                    var separarTransiciones = dividir[1].Split(' ');
                    var estadoNoValidos = string.Empty;
                    var contador = 1;
                    var bandera = false;
                    foreach (var item3 in separarTransiciones)
                    {
                        var sentenciaIf = string.Empty;

                        if (item3.Length > 1 && item3 != " ")
                        {
                            foreach (var item4 in item3)
                            {
                                if (item4 == '~')
                                {
                                    contador++;
                                }
                                else
                                {
                                    sentenciaIf = "if (" + diccionarioIf[contador] + ") \n";
                                    sentenciaIf = sentenciaIf + " { Estado = " + item4 + ";\n salir=true; }\n";
                                    contador++;
                                }
                            }
                        }
                        else if (item3.Length == 1 && item3 != " ")
                        {
                            sentenciaIf = "if (" + diccionarioIf[contador] + ")\n";
                            sentenciaIf = sentenciaIf + " { Estado = " + item3 + ";\n salir=true; }\n";
                        }
                        if (sentenciaIf == "")
                        {
                            bandera = true;
                        }
                        sentenciaCase = sentenciaCase + sentenciaIf;
                        contador++;
                    }
                    if (bandera)
                    {
                        sentenciaCase = sentenciaCase + " salir=true;\n break;\n";
                        listCase.Add(sentenciaCase);
                    }
                    else
                    {
                        sentenciaCase = sentenciaCase + "else { salir = true;} break;\n";
                        listCase.Add(sentenciaCase);
                    }


                }
                else if (dividir.Length == 2)
                {
                    var sentenciaCase = "case " + dividir[0] + " : ";
                    var separarTransiciones = dividir[1].Split(' ');
                    var estadoNoValidos = string.Empty;
                    var contador = 1;
                    var sentenciaIf = string.Empty;
                    var bandera = false;
                    foreach (var item3 in separarTransiciones)
                    {
                        if (item3.Length > 1 && item3 != " ")
                        {
                            foreach (var item4 in item3)
                            {
                                if (item4 == '~')
                                {
                                    contador++;
                                }
                                else
                                {
                                    sentenciaIf = "if (" + diccionarioIf[contador] + ") \n";
                                    sentenciaIf = sentenciaIf + " { Estado = " + item4 + "; }\n";
                                    contador++;
                                }
                            }
                        }
                        else if (item3.Length == 1 && item3 != " ")
                        {
                            sentenciaIf = "if (" + diccionarioIf[contador] + ")\n";
                            sentenciaIf = sentenciaIf + " { Estado = " + item3 + "; }\n";
                        }
                        if (sentenciaIf == "")
                        {
                            bandera = true;
                        }
                        sentenciaCase = sentenciaCase + sentenciaIf;
                        sentenciaIf = "";
                        contador++;
                    }
                    sentenciaCase = sentenciaCase + "salir = true;\n break;\n";
                    listCase.Add(sentenciaCase);

                }
            }
            var sentenciaSwitch = "while(!salir) { \n switch(Estado) { \n";
            foreach (var item in listCase)
            {
                sentenciaSwitch = sentenciaSwitch + item;
            }
            sentenciaSwitch = sentenciaSwitch + " }; }";
            var sentenciaSwitchTokens = "  switch (Estado) { \n";
            foreach (var item in lista)
            {
                var dividirPipe = item.Split('|');
                if (dividirPipe[0] == "Caracteres" || dividirPipe[0] == "1")
                {
                    //do nothing
                }
                else
                {
                    sentenciaSwitchTokens = sentenciaSwitchTokens + " case " + dividirPipe[0] + ": NumToken = " + (Convert.ToInt32(dividirPipe[0]) - 1) + "; break; \n";
                }
            }
            sentenciaSwitchTokens = sentenciaSwitchTokens + "default: NumToken = " + listError[0] + "; break; \n";
            sentenciaSwitchTokens = sentenciaSwitchTokens + " };";

            Console.ReadLine();
            Console.ReadLine();
            var infoArchivo = new FileInfo(rutaArchivo);
            var nombreArchivo = infoArchivo.Name.Trim().Split('.')[0];
            nombreArchivo = nombreArchivo.Replace(" ", "");
            var encabezado = "using System; \n using System.Collections.Generic;\n using System.Linq;\nusing System.IO;\n public class " + nombreArchivo + "{\n" + "public static void Main()\n{\n";
            
            var cuerpo = "var diccionarioActions= new Dictionary<string,int>();";
            
            var comillas = Convert.ToChar(34);
            foreach (var item in dictionaryActions)
            {
                var key = item.Key.TrimStart();
                cuerpo = cuerpo + "\n  diccionarioActions.Add(" + comillas + key + comillas + "," + item.Value + ");";
            }
            cuerpo += "\n";
            cuerpo += "var error = false;\n var salir=false;\nvar NumToken = 0;\nConsole.WriteLine(" + comillas + "Ingrese ruta de archivo a analizar" + comillas + ");";
            cuerpo += "var ruta = Console.ReadLine();\n";
            
            var cuerpoStreamReader = "using (var reader = new StreamReader(new FileStream(ruta, FileMode.Open)))\ntry\n{\n";
            cuerpoStreamReader += "var lineaActual = reader.ReadToEnd();\nvar separacion=lineaActual.Split(' ');\n";
            
            var cuerpoForeach = "foreach( var caracter in separacion)\n{\n salir=false;\nvar Estado=1;\n";
            cuerpoForeach += "if(caracter.Length>1)\n{\n//Es reservada\n if(diccionarioActions.ContainsKey(caracter))\n{\n";
            cuerpoForeach +="NumToken=diccionarioActions[caracter];\n}\nelse\n{var arregloTokens=new int[caracter.Length];\n";
            cuerpoForeach += "for(int i =0;i<caracter.Length;i++)\n{\n"+sentenciaSwitch+"\n"+sentenciaSwitchTokens+"\n";
            cuerpoForeach += "arregloTokens[i] = NumToken; \n }\n}\n Console.WriteLine(caracter+"+comillas+"="+comillas+"+NumToken);\n}\n";
            cuerpoForeach += "else\n{\n for(int i =0;i<caracter.Length;i++)\n{\n" +sentenciaSwitch+"\n"+sentenciaSwitchTokens+"\n"+"Console.WriteLine(Convert.ToChar(caracter).ToString()+"+comillas+"="+comillas+"+ NumToken);\n}";
           
            var pieForeach = "}";
            
            var pieStreamReader = "}\n}\ncatch (Exception)\n{\nthrow;\n}\n";
            
            var pie = "\nConsole.ReadLine();\n} \n }";
           
            using (StreamWriter escritor  =File.CreateText("D:\\analisis"+nombreArchivo+".cs"))
            {
                escritor.Write(encabezado);
                escritor.Write(cuerpo);
                escritor.Write(cuerpoStreamReader);
                escritor.Write(cuerpoForeach);
                escritor.Write(pieForeach);
                escritor.Write(pieStreamReader);
                escritor.Write(pie);
            }
        }
    }
}
