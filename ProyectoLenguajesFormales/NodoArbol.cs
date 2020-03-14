using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoLenguajesFormales
{
    class NodoArbol
    {
        public string first;
        public string last;
        public NodoArbol hijoIzq;
        public NodoArbol hijoDer;
        public int id;
        public string token;
        public bool nulleable=false;
    }
}
