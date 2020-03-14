using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoLenguajesFormales
{
    class NodoArbol
    {
        public List<string> first = new List<string>();
        public List<string> last = new List<string>();
        public NodoArbol hijoIzq;
        public NodoArbol hijoDer;
        public string id;
        public string token;
        public bool nulleable = false;

    }
}
