using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLang
{
    internal class ThreeAddress
    {
        public int NumberOfCommand;
       unsafe public int* pia { get; set; }
        unsafe public int* pib { get; set; }
        unsafe public int* pic { get; set; }
        unsafe public double* pda { get; set; }
        unsafe public double* pdb { get; set; }
        unsafe public double* pdc { get; set; }
        unsafe public bool* pba { get; set; }
        unsafe public bool* pbb { get; set; }
        unsafe public bool* pbc { get; set; }

        public int intVal { get; set; }
        public double doubleVal { get; set; }
        public bool boolVal { get; set; }

        public int Goto;
        public ThreeAddress(int n)
        {
            NumberOfCommand = n;
        }

    }
}
