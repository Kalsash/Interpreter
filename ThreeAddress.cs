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
        public int Count = 0;
        public string Types = "";
        public ThreeAddress(int n)
        {
            NumberOfCommand = n;
        }
        public ThreeAddress(int n, int Goto)
        {
            NumberOfCommand = n;
            this.Goto = Goto;
        }

        unsafe public ThreeAddress(int n, int *pia, int *pib, int* pic)
        {
            NumberOfCommand = n;
            this.pia = pia;
            this.pib = pib;
            this.pic = pic;
            Count = 3;
            Types = "iii";
        }
        unsafe public ThreeAddress(int n, int* pia, int* pib)
        {
            NumberOfCommand = n;
            this.pia = pia;
            this.pib = pib;
            Count = 2;
            Types = "ii";
        }
        unsafe public ThreeAddress(int n, int* pia)
        {
            NumberOfCommand = n;
            this.pia = pia;
            Count = 1;
            Types = "i";
        }
        unsafe public ThreeAddress(int n, int* pia, int intVal)
        {
            NumberOfCommand = n;
            this.pia = pia;
            this.intVal = intVal;
            Count = 2;
            Types = "i1";
        }
        unsafe public ThreeAddress(int n, int* pia, int* pib,int intVal)
        {
            NumberOfCommand = n;
            this.pia = pia;
            this.pib = pib;
            this.intVal = intVal;
            Count = 3;
            Types = "ii1";
        }
        unsafe public ThreeAddress(int n, double* pda, double* pdb, double* pdc)
        {
            NumberOfCommand = n;
            this.pda = pda;
            this.pdb = pdb;
            this.pdc = pdc;
            Count = 3;
            Types = "ddd";
        }
        unsafe public ThreeAddress(int n, double* pda, int* pib)
        {
            NumberOfCommand = n;
            this.pda = pda;
            this.pib = pib;
            Count = 2;
            Types = "di";
        }
        unsafe public ThreeAddress(int n, double* pda, double* pdb)
        {
            NumberOfCommand = n;
            this.pda = pda;
            this.pdb = pdb;
            Count = 2;
            Types = "dd";
        }
        unsafe public ThreeAddress(int n, double* pda)
        {
            NumberOfCommand = n;
            this.pda = pda;
            Count = 1;
            Types = "d";
        }
       unsafe public ThreeAddress(int n, bool* pba, bool* pbb, bool* pbc)
        {
            NumberOfCommand = n;
            this.pba = pba;
            this.pbb = pbb;
            this.pbc = pbc;
            Count = 3;
            Types = "bbb";
        }
        unsafe public ThreeAddress(int n, bool* pba, bool* pbb)
        {
            NumberOfCommand = n;
            this.pba = pba;
            this.pbb = pbb;
            Count = 2;
            Types = "bb";
        }
        unsafe public ThreeAddress(int n, bool* pba)
        {
            NumberOfCommand = n;
            this.pba = pba;
            Count = 1;
            Types = "b";
        }
        unsafe public ThreeAddress(int n, bool* pba, bool boolVal)
        {
            NumberOfCommand = n;
            this.pba = pba;
            this.boolVal = boolVal;
            Count = 2;
            Types = "b";
        }
        unsafe public ThreeAddress(int n, bool* pba, int* pib, int* pic)
        {
            NumberOfCommand = n;
            this.pba = pba;
            this.pib = pib;
            this.pic = pic;
            Count = 3;
            Types = "bii";
        }
        unsafe public ThreeAddress(int n, double* pda, double* pdb, int* pic)
        {
            NumberOfCommand = n;
            this.pda = pda;
            this.pdb = pdb;
            this.pic = pic;
            Count = 3;
            Types = "ddi";
        }
        unsafe public ThreeAddress(int n, bool* pba, double* pdb, double* pdc)
        {
            NumberOfCommand = n;
            this.pba = pba;
            this.pdb = pdb;
            this.pdc = pdc;
            Count = 3;
            Types = "bdd";
        }
        unsafe public ThreeAddress(int n, bool* pba, double* pdb, int* pic)
        {
            NumberOfCommand = n;
            this.pba = pba;
            this.pdb = pdb;
            this.pic = pic;
            Count = 3;
            Types = "bdi";
        }
        unsafe public ThreeAddress(int n, double* pda, int* pib, double* pdc)
        {
            NumberOfCommand = n;
            this.pda = pda;
            this.pib = pib;
            this.pdc = pdc;
            Count = 3;
            Types = "did";
        }
        unsafe public ThreeAddress(int n, bool* pba, int* pib, double* pdc)
        {
            NumberOfCommand = n;
            this.pba = pba;
            this.pib = pib;
            this.pdc = pdc;
            Count = 3;
            Types = "bid";
        }
        unsafe public ThreeAddress(int n, double* pda, double doubleVal)
        {
            NumberOfCommand = n;
            this.pda = pda;
            this.doubleVal = doubleVal;
            Count = 2;
            Types = "d2";
        }
        unsafe public ThreeAddress(int n, bool* pba, int* pib, int intVal)
        {
            NumberOfCommand = n;
            this.pba = pba;
            this.pib = pib;
            this.intVal = intVal;
            Count = 3;
            Types = "bi1";
        }
        unsafe public ThreeAddress(int n, bool* pba, int Goto)
        {
            NumberOfCommand = n;
            this.pba = pba;
            this.Goto = Goto;
            Count = 1;
            Types = "b";
        }
        unsafe public ThreeAddress(int n, double* pda, double doubleVal, int* pic)
        {
            NumberOfCommand = n;
            this.pda = pda;
            this.doubleVal = doubleVal;
            this.pic = pic;
            Count = 3;
            Types = "d2i";
        }

    }
}
