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
                    else if (stackTokens.Count != 0 && stackTokens.Peek()!="(")
                    {
                        //Prueba extraer de T a op, sin extraccion y utilizando item
                        var nodoTemp = new NodoArbol();
                        nodoTemp.token = item;
                        if (stackArbol.Count<2)
                        {
                            Console.WriteLine("Error, faltan operandos");
                            break;
                        }
                        nodoTemp.hijoDer = stackArbol.Pop();
                        nodoTemp.hijoIzq = stackArbol.Pop();
                        stackArbol.Push(nodoTemp);
                    }
                    else if (item == "."|| item == "|")
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
            while (stackTokens.Count>0)
            {
                var nodoTemp = new NodoArbol();
                nodoTemp.token = stackTokens.Pop();
                if (nodoTemp.token=="(")
                {
                    Console.WriteLine("Error, faltan operandos");
                    return null;
                }
                if (stackArbol.Count<2)
                {
                    Console.WriteLine("Error, faltan operandos");
                    return null;
                }
                nodoTemp.hijoDer=stackArbol.Pop();
                nodoTemp.hijoIzq = stackArbol.Pop();
                stackArbol.Push(nodoTemp);
            }
            if (stackArbol.Count!=1)
            {
                Console.WriteLine("Error, faltan operandos");
                return null;
            }
            return stackArbol.Pop();
        }
    }
}
