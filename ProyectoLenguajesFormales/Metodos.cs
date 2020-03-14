using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoLenguajesFormales
{
    class Metodos
    {
        public static int enumeracion = 1;
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

        public static void EnumerarHojas (NodoArbol parent)
        {
            if (parent != null)
            {
                EnumerarHojas(parent.hijoIzq);
                EnumerarHojas(parent.hijoDer);
                if (parent.hijoIzq == null && parent.hijoDer == null)
                {
                    parent.id = enumeracion.ToString();
                    enumeracion++;
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
                if (parent.token==".")
                {
                    if (parent.hijoIzq.nulleable==true)
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
                else if (parent.token == "*"|| parent.token == "+"|| parent.token == "?")
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
    }
}
