using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoLenguajesFormales
{
    class Metodos
    {
        public static int numeroHojaNumeral = 0;
        public static bool bandera = true;
        public static int contadorListaEstados = 0;
        public static int enumeracion = 0;
        public static Dictionary<int, List<int>> dictionaryFollows = new Dictionary<int, List<int>>();
        public static List<NodoArbol> listaHojas = new List<NodoArbol>();

        public static void BuscarNumeral(NodoArbol parent)
        {
            if (parent != null)
            {
                BuscarNumeral(parent.hijoIzq);
                BuscarNumeral(parent.hijoDer);
                if (parent.token=="#")
                {
                    numeroHojaNumeral = parent.id;
                }
            }
        }
        public static int PoseePalabrasReservadas(string archivo)
        {
            var resultado = 1;
            if (!archivo.Contains("SETS"))
            {
                resultado = 0;
            }
            if (!archivo.Contains("TOKENS"))
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
        public static NodoArbol CreacionArbol(List<string> listaToken, Stack<string> stackTokens, Stack<NodoArbol> stackArbol)
        {
            foreach (var item in listaToken)
            {
                if (item == "(")
                {
                    stackTokens.Push(item);
                }
                else if (item == ")")
                {
                    while (stackTokens.Count > 0 && stackTokens.Peek() != "(")
                    {
                        if (stackTokens.Count == 0)
                        {
                            Console.WriteLine("Error, faltan operandos");
                            break;
                        }
                        if (stackArbol.Count < 2)
                        {
                            Console.WriteLine("Error, faltan operandos");
                            break;
                        }
                        var nodoTemp = new NodoArbol();
                        nodoTemp.token = stackTokens.Pop();
                        nodoTemp.hijoDer = stackArbol.Pop();
                        nodoTemp.hijoIzq = stackArbol.Pop();
                        stackArbol.Push(nodoTemp);
                    }
                    stackTokens.Pop();
                }
                else if (item == "." || item == "*" || item == "?" || item == "+" || item == "|")
                {
                    if (item == "*" || item == "?" || item == "+")
                    {
                        var nodoTemp = new NodoArbol();
                        nodoTemp.token = item;
                        if (stackArbol.Count < 0)
                        {
                            Console.WriteLine("Error, faltan operandos");
                            break;
                        }
                        nodoTemp.hijoIzq = stackArbol.Pop();
                        stackArbol.Push(nodoTemp);
                    }
                    //Prueba sin precedencia 
                    else if (stackTokens.Count != 0 && stackTokens.Peek() != "(")
                    {
                        //Prueba extraer de T a op, sin extraccion y utilizando item
                        var nodoTemp = new NodoArbol();
                        nodoTemp.token = item;
                        if (stackArbol.Count < 2)
                        {
                            Console.WriteLine("Error, faltan operandos");
                            break;
                        }
                        nodoTemp.hijoDer = stackArbol.Pop();
                        nodoTemp.hijoIzq = stackArbol.Pop();
                        stackArbol.Push(nodoTemp);
                    }
                    else if (item == "." || item == "|")
                    {
                        stackTokens.Push(item);
                    }
                }
                else
                {
                    //es caracter
                    var nodoTemp = new NodoArbol();
                    nodoTemp.token = item;
                    stackArbol.Push(nodoTemp);
                }
            }
            while (stackTokens.Count > 0)
            {
                var nodoTemp = new NodoArbol();
                nodoTemp.token = stackTokens.Pop();
                if (nodoTemp.token == "(")
                {
                    Console.WriteLine("Error, faltan operandos");
                    return null;
                }
                if (stackArbol.Count < 2)
                {
                    Console.WriteLine("Error, faltan operandos");
                    return null;
                }
                nodoTemp.hijoDer = stackArbol.Pop();
                nodoTemp.hijoIzq = stackArbol.Pop();
                stackArbol.Push(nodoTemp);
            }
            if (stackArbol.Count != 1)
            {
                Console.WriteLine("Error, faltan operandos");
                return null;
            }
            return stackArbol.Pop();
        }
        public static int EnumerarHojas(NodoArbol parent)
        {
            if (parent != null)
            {
                EnumerarHojas(parent.hijoIzq);
                EnumerarHojas(parent.hijoDer);
                if (parent.hijoIzq == null && parent.hijoDer == null)
                {
                    enumeracion++;
                    parent.id = enumeracion;
                }
            }
            return enumeracion;
        }
        public static void RegresarHojas(NodoArbol parent)
        {
            if (parent != null)
            {
                RegresarHojas(parent.hijoIzq);
                RegresarHojas(parent.hijoDer);
                if (parent.hijoIzq == null && parent.hijoDer == null)
                {
                    listaHojas.Add(parent);
                }
            }
        }
        public static void IdentificarNulos(NodoArbol parent)
        {
            if (parent != null)
            {
                IdentificarNulos(parent.hijoIzq);
                IdentificarNulos(parent.hijoDer);
                if (parent.hijoIzq == null && parent.hijoDer == null)
                {
                    parent.nulleable = false;
                }
                if (parent.token == "*" || parent.token == "?")
                {
                    parent.nulleable = true;
                }
                else if (parent.token == "|" || parent.token == "." || parent.token == "+")
                {
                    if (parent.token == "|")
                    {
                        if (parent.hijoIzq.nulleable == true || parent.hijoDer.nulleable == true)
                        {
                            parent.nulleable = true;
                        }
                        else
                        {
                            parent.nulleable = false;
                        }
                    }
                    else if (parent.token == ".")
                    {
                        if (parent.hijoIzq.nulleable == true && parent.hijoDer.nulleable == true)
                        {
                            parent.nulleable = true;
                        }
                        else
                        {
                            parent.nulleable = false;
                        }
                    }
                    else if (parent.token == "+")
                    {
                        parent.nulleable = false;
                    }
                }
            }
        }
        public static void IdentificarFirst(NodoArbol parent)
        {
            if (parent != null)
            {
                IdentificarFirst(parent.hijoIzq);
                IdentificarFirst(parent.hijoDer);
                if (parent.hijoIzq == null && parent.hijoDer == null)
                {
                    parent.first.Add(parent.id);
                }
                if (parent.token == ".")
                {
                    if (parent.hijoIzq.nulleable == true)
                    {
                        parent.first.AddRange(parent.hijoIzq.first);
                        parent.first.AddRange(parent.hijoDer.first);
                    }
                    else
                    {
                        parent.first.AddRange(parent.hijoIzq.first);
                    }
                }
                else if (parent.token == "|")
                {
                    parent.first.AddRange(parent.hijoIzq.first);
                    parent.first.AddRange(parent.hijoDer.first);
                }
                else if (parent.token == "*" || parent.token == "+" || parent.token == "?")
                {
                    parent.first.AddRange(parent.hijoIzq.first);
                }
            }
        }
        public static void IdentificarLast(NodoArbol parent)
        {
            if (parent != null)
            {
                IdentificarLast(parent.hijoIzq);
                IdentificarLast(parent.hijoDer);
                if (parent.hijoIzq == null && parent.hijoDer == null)
                {
                    parent.last.Add(parent.id);
                }
                if (parent.token == ".")
                {
                    if (parent.hijoDer.nulleable == true)
                    {
                        parent.last.AddRange(parent.hijoIzq.last);
                        parent.last.AddRange(parent.hijoDer.last);
                    }
                    else
                    {
                        parent.last.AddRange(parent.hijoDer.last);
                    }
                }
                else if (parent.token == "|")
                {
                    parent.last.AddRange(parent.hijoIzq.last);
                    parent.last.AddRange(parent.hijoDer.last);
                }
                else if (parent.token == "*" || parent.token == "+" || parent.token == "?")
                {
                    parent.last.AddRange(parent.hijoIzq.last);
                }
            }
        }
        public static Dictionary<int, List<int>> IdentificarFollows(NodoArbol parent, int cantidadHojas)
        {
            if (parent != null)
            {
                IdentificarFollows(parent.hijoIzq, cantidadHojas);
                IdentificarFollows(parent.hijoDer, cantidadHojas);
                if (parent.token == "." || parent.token == "*" || parent.token == "+")
                {
                    if (parent.token == ".")
                    {
                        foreach (var item in parent.hijoIzq.last)
                        {
                            if (dictionaryFollows.ContainsKey(item))
                            {
                                var listaParcial = dictionaryFollows[item];
                                listaParcial.AddRange(parent.hijoDer.first);
                                List<int> uniqueList = listaParcial.Distinct().ToList();
                                dictionaryFollows[item] = uniqueList;
                            }
                            else
                            {
                                dictionaryFollows.Add(item, parent.hijoDer.first);
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in parent.hijoIzq.last)
                        {
                            if (dictionaryFollows.ContainsKey(item))
                            {
                                var listaParcial = dictionaryFollows[item];
                                listaParcial.AddRange(parent.hijoIzq.first);
                                List<int> uniqueList = listaParcial.Distinct().ToList();
                                dictionaryFollows[item] = uniqueList;
                            }
                            else
                            {
                                dictionaryFollows.Add(item, parent.hijoIzq.first);
                            }
                        }
                    }
                }
            }
            return dictionaryFollows;
        }
        public static void MostrarFirstLast(NodoArbol parent)
        {
            if (parent != null)
            {
                MostrarFirstLast(parent.hijoIzq);
                MostrarFirstLast(parent.hijoDer);

                var first = string.Empty;
                foreach (var item in parent.first)
                {
                    first = first + item + ",";
                }
                var last = string.Empty;
                foreach (var item in parent.last)
                {
                    last = last + item + ",";
                }
                Console.WriteLine(parent.token + "|" + first + "|" + last);

            }
        }
        public static List<string> Transiciones(Dictionary<int, List<int>> Follows, NodoArbol ArbolExpresion)
        {
            var diccionarioEstados = new List<string>();
            var ListaEstados = new List<List<int>>(); 
            var primerEstadoInicial = ArbolExpresion.first;
            var estadoInicial = ArbolExpresion.first;
            RegresarHojas(ArbolExpresion);
            var listaCaracteresSinRepetir = new List<string>();
            foreach (var item in listaHojas)
            {
                var caracter = item.token;
                if (!listaCaracteresSinRepetir.Contains(caracter) && caracter != "#")
                {
                    listaCaracteresSinRepetir.Add(caracter);
                }
            }
            ListaEstados.Add(estadoInicial);
            estadoInicial = ListaEstados[contadorListaEstados];
            do
            {
                var estadoCompleto = string.Empty;
                Console.Write("Estado   |");
                foreach (var item in listaCaracteresSinRepetir)
                {
                    Console.Write(item + "    |");
                }

                var estadoEscribir = string.Empty;
                foreach (var item in estadoInicial)
                {
                    estadoEscribir += (item + ",");
                }
                Console.WriteLine();
                Console.Write(estadoEscribir + "     |");
                foreach (var item in listaCaracteresSinRepetir)
                {
                    var siguienteEstado = new List<int>();
                    foreach (var estados in estadoInicial)
                    {
                        if (listaHojas[estados - 1].token == item)
                        {
                            siguienteEstado.AddRange(Follows[estados]);
                        }
                    }
                    var comparacion = false;
                    foreach (var listaNumeros in ListaEstados)
                    {
                        comparacion = listaNumeros.SequenceEqual(siguienteEstado);
                        if (comparacion)
                        {
                            break;
                        }
                    }
                    if (!comparacion)
                    {
                        ListaEstados.Add(siguienteEstado);
                    }
                    var escribirEstadoSiguiente = string.Empty;
                    foreach (var numero in siguienteEstado)
                    {
                        escribirEstadoSiguiente += (numero + ",");                        

                    }
                    Console.Write(escribirEstadoSiguiente + "   |");
                    estadoCompleto = estadoCompleto + escribirEstadoSiguiente + "|";
                }
                try
                {
                    contadorListaEstados += 1;
                    estadoInicial = ListaEstados[contadorListaEstados];
                }
                catch (Exception)
                {
                    bandera = false;
                }
                diccionarioEstados.Add(estadoCompleto);
                Console.WriteLine();
                Console.WriteLine();

            } while (bandera);
            var escribirPrimerEstado = string.Empty;
            foreach (var item in primerEstadoInicial)
            {
                escribirPrimerEstado += (item + ",");
            }
            Console.WriteLine("Estado Inicial: " + escribirPrimerEstado);
            BuscarNumeral(ArbolExpresion);
            Console.WriteLine("Estados de Aceptacion: ");
            foreach (var item in ListaEstados)
            {
                var escribirEstadoAcpetacion = string.Empty;
                if (item.Contains(numeroHojaNumeral))
                {
                    foreach (var item2 in item)
                    {
                        escribirEstadoAcpetacion = escribirEstadoAcpetacion + item2 + ",";
                    }
                    Console.WriteLine(escribirEstadoAcpetacion);
                }
            }
            var nuevaListaEstados = new Dictionary<int,string>();
            var i = 1;
            foreach (var item in ListaEstados)
            {
                var concatenacion = string.Empty;
                foreach (var item2 in item)
                {
                    concatenacion = concatenacion + item2 + ",";
                }
                nuevaListaEstados.Add(i, concatenacion);
                i++;
            }
            nuevaListaEstados.Remove(nuevaListaEstados.Count());
            var listaEstadosNumeros = new List < string > ();
            var contadorAux = 1;
            var lineaAux = string.Empty;
            foreach (var item in listaCaracteresSinRepetir)
            {
                lineaAux = lineaAux + item + "~";
            }
            listaEstadosNumeros.Add("Caracteres|"+lineaAux);
            diccionarioEstados.RemoveAt(diccionarioEstados.Count() - 1);
            foreach (var item in diccionarioEstados)
            {
                var bandera = true;
                var linea = contadorAux.ToString()+"|";   
                var separacion = item.Split('|');
                foreach (var item2 in separacion)
                {
                    foreach (var item3 in nuevaListaEstados)
                    {
                        if (item3.Value==item2)
                        {
                            linea = linea + item3.Key.ToString()+"~";
                            bandera = false;
                        }
                    }
                    if (bandera)
                    {
                        linea = linea+ "~";
                    }
                }
                listaEstadosNumeros.Add(linea);
                contadorAux++;
            }
            return listaEstadosNumeros;
        }
    }
}
