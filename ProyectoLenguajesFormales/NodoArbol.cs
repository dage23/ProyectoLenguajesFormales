using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoLenguajesFormales
{
    class NodoArbol
    {
        public List<int> first = new List<int>();
        public List<int> last = new List<int>();
        public NodoArbol hijoIzq;
        public NodoArbol hijoDer;
        public int id;
        public string token;
        public bool nulleable = false;

    }
}
