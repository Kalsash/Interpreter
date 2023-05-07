using SimpleParser;
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

        public Toks Tok = Toks.empty;

        public string Assign = "aa";

        public string Operation = "__";
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

        public Toks CreateToken()
        {
            if (Count < 2)
            {
                if (NumberOfCommand == 0)
                {
                    return Toks.end;
                }
                if (NumberOfCommand == 22)
                {
                    return Toks.iff;
                }
                if (NumberOfCommand == 23)
                {
                    return Toks.got;
                }
                return Toks.empty;
            }
            Toks t = Toks.empty;
            string s = "p";
            s += Types[0];
            s += Assign;
                switch (Types[1])
                {
                 case 'i':
                    s += "pi";
                     break;
                 case 'd':
                    s += "pd";
                     break;
                case 'b':
                    s += "pb";
                    break;
                case '1':
                    s += "vi";
                    break;
                case '2':
                    s += "vd";
                    break;
                case '3':
                    s += "vb";
                    break;
                default:
                        break;
                }
            if (Count == 2)
            {

                Enum.TryParse(s, out t);

                return t;
            }
            s += Operation;
            switch (Types[2])
            {
                case 'i':
                    s += "pi";
                    break;
                case 'd':
                    s += "pd";
                    break;
                case 'b':
                    s += "pb";
                    break;
                case '1':
                    s += "vi";
                    break;
                case '2':
                    s += "vd";
                    break;
                case '3':
                    s += "vb";
                    break;
                default:
                    break;
            }
            if (Count == 3)
            {
                Enum.TryParse(s, out t);
                return t;
            }
            return Toks.empty;
        }
        public ThreeAddress(int n)
        {
            NumberOfCommand = n;
            Tok = CreateToken();
        }
        public ThreeAddress(int n, int Goto)
        {
            NumberOfCommand = n;
            this.Goto = Goto;
            Tok = CreateToken();
        }

        unsafe public ThreeAddress(int n, int *pia, int *pib, int* pic, string Assign, string Operation)
        {
            NumberOfCommand = n;
            this.pia = pia;
            this.pib = pib;
            this.pic = pic;
            Count = 3;
            Types = "iii";
            this.Assign = Assign;
            this.Operation = Operation;
            Tok = CreateToken();
        }
        unsafe public ThreeAddress(int n, int* pia, int* pib, string Assign, string Operation)
        {
            NumberOfCommand = n;
            this.pia = pia;
            this.pib = pib;
            Count = 2;
            Types = "ii";
            this.Assign = Assign;
            this.Operation = Operation;
            Tok = CreateToken();
        }
        unsafe public ThreeAddress(int n, int* pia)
        {
            NumberOfCommand = n;
            this.pia = pia;
            Count = 1;
            Types = "i";
            Tok = CreateToken();
        }
        unsafe public ThreeAddress(int n, int* pia, int intVal, string Assign, string Operation)
        {
            NumberOfCommand = n;
            this.pia = pia;
            this.intVal = intVal;
            Count = 2;
            Types = "i1";
            this.Assign = Assign;
            this.Operation = Operation;
            Tok = CreateToken();
        }
        unsafe public ThreeAddress(int n, int* pia, int* pib,int intVal, string Assign, string Operation)
        {
            NumberOfCommand = n;
            this.pia = pia;
            this.pib = pib;
            this.intVal = intVal;
            Count = 3;
            Types = "ii1";
            this.Assign = Assign;
            this.Operation = Operation;
            Tok = CreateToken();
        }
        unsafe public ThreeAddress(int n, double* pda, double* pdb, double* pdc, string Assign, string Operation)
        {
            NumberOfCommand = n;
            this.pda = pda;
            this.pdb = pdb;
            this.pdc = pdc;
            Count = 3;
            Types = "ddd";
            this.Assign = Assign;
            this.Operation = Operation;
            Tok = CreateToken();
        }
        unsafe public ThreeAddress(int n, double* pda, int* pib, string Assign, string Operation)
        {
            NumberOfCommand = n;
            this.pda = pda;
            this.pib = pib;
            Count = 2;
            Types = "di";
            this.Assign = Assign;
            this.Operation = Operation;
            Tok = CreateToken();
        }
        unsafe public ThreeAddress(int n, double* pda, double* pdb, string Assign, string Operation)
        {
            NumberOfCommand = n;
            this.pda = pda;
            this.pdb = pdb;
            Count = 2;
            Types = "dd";
            this.Assign = Assign;
            this.Operation = Operation;
            Tok = CreateToken();
        }
        unsafe public ThreeAddress(int n, double* pda)
        {
            NumberOfCommand = n;
            this.pda = pda;
            Count = 1;
            Types = "d";
            Tok = CreateToken();
        }
       unsafe public ThreeAddress(int n, bool* pba, bool* pbb, bool* pbc, string Assign, string Operation)
        {
            NumberOfCommand = n;
            this.pba = pba;
            this.pbb = pbb;
            this.pbc = pbc;
            Count = 3;
            Types = "bbb";
            this.Assign = Assign;
            this.Operation = Operation;
            Tok = CreateToken();
        }
        unsafe public ThreeAddress(int n, bool* pba, bool* pbb, string Assign, string Operation)
        {
            NumberOfCommand = n;
            this.pba = pba;
            this.pbb = pbb;
            Count = 2;
            Types = "bb";
            this.Assign = Assign;
            this.Operation = Operation;
            Tok = CreateToken();
        }
        unsafe public ThreeAddress(int n, bool* pba)
        {
            NumberOfCommand = n;
            this.pba = pba;
            Count = 1;
            Types = "b";
            Tok = CreateToken();
        }
        unsafe public ThreeAddress(int n, bool* pba, bool boolVal, string Assign, string Operation)
        {
            NumberOfCommand = n;
            this.pba = pba;
            this.boolVal = boolVal;
            Count = 2;
            Types = "b";
            this.Assign = Assign;
            this.Operation = Operation;
            Tok = CreateToken();
        }
        unsafe public ThreeAddress(int n, bool* pba, int* pib, int* pic, string Assign, string Operation)
        {
            NumberOfCommand = n;
            this.pba = pba;
            this.pib = pib;
            this.pic = pic;
            Count = 3;
            Types = "bii";
            this.Assign = Assign;
            this.Operation = Operation;
            Tok = CreateToken();
        }
        unsafe public ThreeAddress(int n, double* pda, double* pdb, int* pic, string Assign, string Operation)
        {
            NumberOfCommand = n;
            this.pda = pda;
            this.pdb = pdb;
            this.pic = pic;
            Count = 3;
            Types = "ddi";
            this.Assign = Assign;
            this.Operation = Operation;
            Tok = CreateToken();
        }
        unsafe public ThreeAddress(int n, bool* pba, double* pdb, double* pdc, string Assign, string Operation)
        {
            NumberOfCommand = n;
            this.pba = pba;
            this.pdb = pdb;
            this.pdc = pdc;
            Count = 3;
            Types = "bdd";
            this.Assign = Assign;
            this.Operation = Operation;
            Tok = CreateToken();
        }
        unsafe public ThreeAddress(int n, bool* pba, double* pdb, int* pic, string Assign, string Operation)
        {
            NumberOfCommand = n;
            this.pba = pba;
            this.pdb = pdb;
            this.pic = pic;
            Count = 3;
            Types = "bdi";
            this.Assign = Assign;
            this.Operation = Operation;
            Tok = CreateToken();
        }
        unsafe public ThreeAddress(int n, double* pda, int* pib, double* pdc, string Assign, string Operation)
        {
            NumberOfCommand = n;
            this.pda = pda;
            this.pib = pib;
            this.pdc = pdc;
            Count = 3;
            Types = "did";
            this.Assign = Assign;
            this.Operation = Operation;
            Tok = CreateToken();
        }
        unsafe public ThreeAddress(int n, bool* pba, int* pib, double* pdc, string Assign, string Operation)
        {
            NumberOfCommand = n;
            this.pba = pba;
            this.pib = pib;
            this.pdc = pdc;
            Count = 3;
            Types = "bid";
            this.Assign = Assign;
            this.Operation = Operation;
            Tok = CreateToken();
        }
        unsafe public ThreeAddress(int n, double* pda, double doubleVal, string Assign, string Operation)
        {
            NumberOfCommand = n;
            this.pda = pda;
            this.doubleVal = doubleVal;
            Count = 2;
            Types = "d2";
            this.Assign = Assign;
            this.Operation = Operation;
            Tok = CreateToken();
        }
        unsafe public ThreeAddress(int n, bool* pba, int* pib, int intVal, string Assign, string Operation)
        {
            NumberOfCommand = n;
            this.pba = pba;
            this.pib = pib;
            this.intVal = intVal;
            Count = 3;
            Types = "bi1";
            this.Assign = Assign;
            this.Operation = Operation;
            Tok = CreateToken();
        }
        unsafe public ThreeAddress(int n, bool* pba, int Goto)
        {
            NumberOfCommand = n;
            this.pba = pba;
            this.Goto = Goto;
            Count = 1;
            Types = "b";
            Tok = CreateToken();
        }
        unsafe public ThreeAddress(int n, double* pda, double doubleVal, int* pic, string Assign, string Operation)
        {
            NumberOfCommand = n;
            this.pda = pda;
            this.doubleVal = doubleVal;
            this.pic = pic;
            Count = 3;
            Types = "d2i";
            this.Assign = Assign;
            this.Operation = Operation;
            Tok = CreateToken();
        }

    }
}
