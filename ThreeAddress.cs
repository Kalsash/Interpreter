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
        unsafe public ThreeAddress(int n, int *pia, int *pib, int* pic)
        {
            NumberOfCommand = n;
            this.pia = pia;
            this.pib = pib;
            this.pic = pic;
        }
        unsafe public ThreeAddress(int n, int* pia, int* pib)
        {
            NumberOfCommand = n;
            this.pia = pia;
            this.pib = pib;
        }
        unsafe public ThreeAddress(int n, int* pia)
        {
            NumberOfCommand = n;
            this.pia = pia;
        }
        unsafe public ThreeAddress(int n, double* pda, double* pdb, double* pdc)
        {
            NumberOfCommand = n;
            this.pda = pda;
            this.pdb = pdb;
            this.pdc = pdc;
        }
        unsafe public ThreeAddress(int n, double* pda, double* pdb)
        {
            NumberOfCommand = n;
            this.pda = pda;
            this.pdb = pdb;
        }
        unsafe public ThreeAddress(int n, double* pda)
        {
            NumberOfCommand = n;
            this.pda = pda;
        }
       unsafe public ThreeAddress(int n, bool* pba, bool* pbb, bool* pbc)
        {
            NumberOfCommand = n;
            this.pba = pba;
            this.pbb = pbb;
            this.pbc = pbc;
        }
        unsafe public ThreeAddress(int n, bool* pba, bool* pbb)
        {
            NumberOfCommand = n;
            this.pba = pba;
            this.pbb = pbb;
        }
        unsafe public ThreeAddress(int n, bool* pba)
        {
            NumberOfCommand = n;
            this.pba = pba;
        }
        unsafe public ThreeAddress(int n, bool* pba, int* pib, int* pic)
        {
            NumberOfCommand = n;
            this.pba = pba;
            this.pib = pib;
            this.pic = pic;
        }
        unsafe public ThreeAddress(int n, double* pda, double* pdb, int* pic)
        {
            NumberOfCommand = n;
            this.pda = pda;
            this.pdb = pdb;
            this.pic = pic;
        }
    }
}
