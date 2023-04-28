using SimpleParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLang
{
    internal class Optimiser
    {
        public int Size; // size of array
        public int c = 0; // counter
        public ThreeAddress[] Commands; // array of commands
        public SortedSet<int> Redundant = new SortedSet<int>(); // useless commands
        public SortedSet<int> Temporary = new SortedSet<int>(); // temporary commands
        public Dictionary<string, int> UseFull = new Dictionary<string, int>(); // values in use
        public static Dictionary<string, string> Values = new Dictionary<string, string>(); // for DefUse
        public static Dictionary<string, string> Vals = new Dictionary<string, string>(); //for PrintCommands
        public int ValsCounter = 0;// counter for PrintCommands
        public string StrCommands = ""; // Str for PrintCommands
        int[] ArrBlocks; // blocks of three address code
        SortedSet<int>[] DefArr;
        SortedSet<int>[] KillArr;
        public Optimiser(int size)
        {
            Size = size;
            Commands = new ThreeAddress[Size];
        }
        public void AddCommands(ThreeAddress t)
        {
            Commands[c] = t;       
            c++;
        }
        public void DelCommand(int ind)
        {
     
            Commands = Commands.Where((val, idx) => idx != ind).ToArray();
            Size--;
            for (int i = 0; i < Size; i++)
            {
                if (Commands[i].NumberOfCommand == 22 || Commands[i].NumberOfCommand == 23)
                {
                    if (Commands[i].Goto < ind)
                    {
                        break;
                    }
                    Commands[i].Goto--;
                }
            }
        }

        public void Print()
        {
            for (int i = 0; i < Size; i++)
            {
                Console.WriteLine(Commands[i].NumberOfCommand);
            }                  
        }
        public void AddVal(string s)
        {
            int k = 0;
            int ind = 0;
            if (!Values.ContainsKey(s))
            {
                foreach (var item in SymbolTable.mem)
                {
                    unsafe
                    {
                        if (k++ == SymbolTable.MemSize)
                        {
                            break;
                        }
                        if (Convert.ToString((ulong)item.pi) == s || Convert.ToString((ulong)item.pd) == s
                            || Convert.ToString((ulong)item.pb) == s)
                        {
                            foreach (var x in SymbolTable.Vars)
                            {
                                ind = x.Value.Index;
                                if (k-1 == ind)
                                {
                                    Values.Add(s, x.Key);
                                }
                            }
                            return;
                        }
                    }
                }
                Values.Add(s, "temp" + ++ValsCounter);
            }
        }
        public void AddV(string s, string flag)
        {
            if (flag[0] == 't')
            {
                if (!UseFull.ContainsKey(s))
                {
                    UseFull.Add(s, c);
                }
                else
                    UseFull[s] = c;
            }
            if (!Vals.ContainsKey(s))
            {
                Vals.Add(s, flag);
            }
            else
            {
                if (Vals[s][0] == 'f' && flag[0] == 'f')
                {
                    Redundant.Add(c);
                }
                else
                {
                    if (flag[0] == 'f')
                    {
                        var t = Vals[s];
                        Vals[s] = "f" + t.Substring(1);
                    }
                    else
                    Vals[s] = flag;
                }
            
            }
    
        }
        public void FindLeaders() 
        {
            SortedSet<int> BasicBlocks = new SortedSet<int>();
            BasicBlocks.Add(0);
            for (int i = 1; i < Size; i++)
            {
                var command = Commands[i];
                if (command.NumberOfCommand == 22 || command.NumberOfCommand == 23)
                {
                    BasicBlocks.Add(command.Goto+1);
                    BasicBlocks.Add(i + 1);
                }
            }
            var Arr = new int[BasicBlocks.Count()];
           int k = 0;
            foreach (var item in BasicBlocks)
            {
                Arr[k++] = item;
                //Console.WriteLine(item);
            }
            ArrBlocks = Arr;

        }
        public void CollectMarks(int end, int i, int[] Arr)
        {
            end = Arr[i + 1] - 1;
            while (Arr[i] - 1 != end)
            {

                var command = Commands[end];

                c = end--;
                //Console.WriteLine(c);

                switch (command.NumberOfCommand)
                {
                    case 0:
                        i = c;
                        break; // stop
                    case 1:
                        unsafe { AddV(Convert.ToString((ulong)command.pia), "f"); }
                        break; // int = intVal
                    case 2:
                        unsafe { AddV(Convert.ToString((ulong)command.pda), "f"); }
                        break; // double = doubleVal
                    case 3:
                        unsafe { AddV(Convert.ToString((ulong)command.pba), "f"); }
                        break; // bool = boolVal
                    case 4:
                        unsafe
                        {
                            AddV(Convert.ToString((ulong)command.pia), "f");
                            AddV(Convert.ToString((ulong)command.pib), "tib" + c);
                        }
                        break; // int = int 
                    case 5:
                        unsafe
                        {
                            AddV(Convert.ToString((ulong)command.pda), "f");
                            AddV(Convert.ToString((ulong)command.pdb), "tdb" + c);
                        }
                        break; // double = double
                    case 6:
                        unsafe
                        {
                            AddV(Convert.ToString((ulong)command.pba), "f");
                            AddV(Convert.ToString((ulong)command.pbb), "tbb" + c);
                        }
                        break; // bool = bool
                    case 7:
                        unsafe { 
                            AddV(Convert.ToString((ulong)command.pda), "f");
                            AddV(Convert.ToString((ulong)command.pib), "tib" + c);
                        }
                        break; // double = int
                    case 8:
                        unsafe
                        {
                            AddV(Convert.ToString((ulong)command.pia), "f");
                            AddV(Convert.ToString((ulong)command.pib), "tib" + c);
                            AddV(Convert.ToString((ulong)command.pic), "tic" + c);
                        }

                        break; // int = int + int
                    case 9:
                        unsafe
                        {
                            AddV(Convert.ToString((ulong)command.pda), "f");
                            AddV(Convert.ToString((ulong)command.pdb), "tdb" + c);
                            AddV(Convert.ToString((ulong)command.pdc), "tdc" + c);
                        }
                        break; // double = double + double
                    case 10:
                        unsafe { 
                            AddV(Convert.ToString((ulong)command.pda), "f");
                            AddV(Convert.ToString((ulong)command.pdb), "tdb" + c);
                            AddV(Convert.ToString((ulong)command.pdc), "tic" + c);
                        }
                        break; // double = double + int
                    case 11:
                        unsafe { 
                            *command.pda = *command.pib + *command.pic;
                            AddV(Convert.ToString((ulong)command.pda), "f");
                            AddV(Convert.ToString((ulong)command.pdb), "tib" + c);
                            AddV(Convert.ToString((ulong)command.pdc), "tic" + c);
                        }
                        break; // double = int + int
                    case 12:
                        break; // goto
                    case 13:
                        unsafe { AddV(Convert.ToString((ulong)command.pba), "f");}
                        break; // if
                    case 14:
                        unsafe {
                            AddV(Convert.ToString((ulong)command.pda), "f");
                            AddV(Convert.ToString((ulong)command.pic), "tic" + c);
                        }
                        break; // double = doubleVal / int 
                    case 15:
                        unsafe { 
                            AddV(Convert.ToString((ulong)command.pia), "f");
                            AddV(Convert.ToString((ulong)command.pib), "tib" + c);
                        }
                        break; // int = int + intVal
                    case 16:
                        unsafe { 
                            *command.pba = *command.pib >= *command.pic;
                            AddV(Convert.ToString((ulong)command.pba), "f");
                            AddV(Convert.ToString((ulong)command.pib), "tib" + c);
                            AddV(Convert.ToString((ulong)command.pic), "tic" + c);
                        }
                        break; // bool = int >= int
                    case 17:
                        break; // print(int)
                    case 18:
                        break; // print(double)
                    case 19:
                        break; // print(int)
                    case 20:
                        unsafe
                        {
                            AddV(Convert.ToString((ulong)command.pba), "f");
                            AddV(Convert.ToString((ulong)command.pib), "tib" + c);
                            AddV(Convert.ToString((ulong)command.pic), "tic" + c);
                        }
                        break; // bool = int < int
                    case 21:
                        unsafe
                        {
                            AddV(Convert.ToString((ulong)command.pda), "f");
                            AddV(Convert.ToString((ulong)command.pdb), "tdb" + c);
                            AddV(Convert.ToString((ulong)command.pic), "tic" + c);
                        }
                        break; // double = doubleVal / int 
                    case 22:
                        unsafe
                        {
                            AddV(Convert.ToString((ulong)command.pba), "t");
                        }
                        break; // if
                    case 23:
                        break;
                    case 24:
                        unsafe {
                            AddV(Convert.ToString((ulong)command.pia), "f");
                            AddV(Convert.ToString((ulong)command.pib), "tib" + c);
                            AddV(Convert.ToString((ulong)command.pic), "tic" + c);
                        }
                        break; // int = int - int
                    case 25:
                        unsafe {
                            AddV(Convert.ToString((ulong)command.pia), "f");
                            AddV(Convert.ToString((ulong)command.pib), "tib" + c);
                            AddV(Convert.ToString((ulong)command.pic), "tic" + c);
                        }
                        break; // int = int * int
                    case 26:
                        unsafe {
                            AddV(Convert.ToString((ulong)command.pia), "f");
                            AddV(Convert.ToString((ulong)command.pib), "tib" + c);
                            AddV(Convert.ToString((ulong)command.pic), "tic" + c);
                        }
                        break; // int = int / int
                    case 27:
                        unsafe {
                            AddV(Convert.ToString((ulong)command.pba), "f");
                            AddV(Convert.ToString((ulong)command.pib), "tib" + c);
                            AddV(Convert.ToString((ulong)command.pic), "tic" + c);
                        }
                        break; // bool = int > int
                    case 28:
                        unsafe
                        {
                            AddV(Convert.ToString((ulong)command.pba), "f");
                            AddV(Convert.ToString((ulong)command.pib), "tib" + c);
                            AddV(Convert.ToString((ulong)command.pic), "tic" + c);
                        }
                        break; // bool = int == int
                    case 29:
                        unsafe {
                            AddV(Convert.ToString((ulong)command.pba), "f");
                            AddV(Convert.ToString((ulong)command.pib), "tib" + c);
                            AddV(Convert.ToString((ulong)command.pic), "tic" + c);
                        }
                        break; // bool = int != int
                    case 30:
                        unsafe { 
                            AddV(Convert.ToString((ulong)command.pda), "f");
                            AddV(Convert.ToString((ulong)command.pdb), "tdb" + c);
                            AddV(Convert.ToString((ulong)command.pdc), "tdc" + c);
                        }
                        break; // double = double - double
                    case 31:
                        unsafe
                        {
                            AddV(Convert.ToString((ulong)command.pda), "f");
                            AddV(Convert.ToString((ulong)command.pdb), "tdb" + c);
                            AddV(Convert.ToString((ulong)command.pdc), "tdc" + c);
                        }
                        break; // double = double * double
                    case 32:
                        unsafe
                        {
                            AddV(Convert.ToString((ulong)command.pda), "f");
                            AddV(Convert.ToString((ulong)command.pdb), "tdb" + c);
                            AddV(Convert.ToString((ulong)command.pdc), "tdc" + c);
                        }
                        break; // double = double / double
                    case 33:
                        unsafe
                        {
                            AddV(Convert.ToString((ulong)command.pba), "f");
                            AddV(Convert.ToString((ulong)command.pdb), "tdb" + c);
                            AddV(Convert.ToString((ulong)command.pdc), "tdc" + c);
                        }
                        break; // bool = double > double
                    case 34:
                        unsafe
                        {
                            AddV(Convert.ToString((ulong)command.pba), "f");
                            AddV(Convert.ToString((ulong)command.pdb), "tdb" + c);
                            AddV(Convert.ToString((ulong)command.pdc), "tdc" + c);
                        }
                        break; // bool = double < double
                    case 35:
                        unsafe
                        {
                            AddV(Convert.ToString((ulong)command.pba), "f");
                            AddV(Convert.ToString((ulong)command.pdb), "tdb" + c);
                            AddV(Convert.ToString((ulong)command.pdc), "tdc" + c);
                        }
                        break; // bool = double == double
                    case 36:
                        unsafe
                        {
                            AddV(Convert.ToString((ulong)command.pba), "f");
                            AddV(Convert.ToString((ulong)command.pdb), "tdb" + c);
                            AddV(Convert.ToString((ulong)command.pdc), "tdc" + c);
                        }
                        break; // bool = double != double
                    case 37:
                        unsafe
                        {
                            AddV(Convert.ToString((ulong)command.pda), "f");
                            AddV(Convert.ToString((ulong)command.pdb), "tdb" + c);
                            AddV(Convert.ToString((ulong)command.pic), "tic" + c);
                        }
                        break; // double = double - int 
                    case 38:
                        unsafe
                        {
                            AddV(Convert.ToString((ulong)command.pda), "f");
                            AddV(Convert.ToString((ulong)command.pdb), "tdb" + c);
                            AddV(Convert.ToString((ulong)command.pic), "tic" + c);
                        }
                        break; // double = double * int 
                    case 39:
                        unsafe
                        {
                            AddV(Convert.ToString((ulong)command.pba), "f");
                            AddV(Convert.ToString((ulong)command.pdb), "tdb" + c);
                            AddV(Convert.ToString((ulong)command.pic), "tic" + c);
                        }
                        break; // bool = double > int
                    case 40:
                        unsafe
                        {
                            AddV(Convert.ToString((ulong)command.pba), "f");
                            AddV(Convert.ToString((ulong)command.pdb), "tdb" + c);
                            AddV(Convert.ToString((ulong)command.pic), "tic" + c);
                        }
                        break; // bool = double < int
                    case 41:
                        unsafe
                        {
                            AddV(Convert.ToString((ulong)command.pba), "f");
                            AddV(Convert.ToString((ulong)command.pdb), "tdb" + c);
                            AddV(Convert.ToString((ulong)command.pic), "tic" + c);
                        }
                        break; // bool = double == int
                    case 42:
                        unsafe
                        {
                            AddV(Convert.ToString((ulong)command.pba), "f");
                            AddV(Convert.ToString((ulong)command.pdb), "tdb" + c);
                            AddV(Convert.ToString((ulong)command.pic), "tic" + c);
                        }
                        break; // bool = double != int
                    case 43:
                        unsafe
                        {
                            AddV(Convert.ToString((ulong)command.pda), "f");
                            AddV(Convert.ToString((ulong)command.pib), "tib" + c);
                            AddV(Convert.ToString((ulong)command.pdc), "tdc" + c);
                        }
                        break; // double = int - double
                    case 44:
                        unsafe
                        {
                            AddV(Convert.ToString((ulong)command.pda), "f");
                            AddV(Convert.ToString((ulong)command.pib), "tib" + c);
                            AddV(Convert.ToString((ulong)command.pdc), "tdc" + c);
                        }
                        break; // double = int * double
                    case 45:
                        unsafe
                        {
                            AddV(Convert.ToString((ulong)command.pda), "f");
                            AddV(Convert.ToString((ulong)command.pib), "tib" + c);
                            AddV(Convert.ToString((ulong)command.pdc), "tdc" + c);
                        }
                        break; // double = int / double
                    case 46:
                        unsafe
                        {
                            AddV(Convert.ToString((ulong)command.pba), "f");
                            AddV(Convert.ToString((ulong)command.pib), "tib" + c);
                            AddV(Convert.ToString((ulong)command.pdc), "tdc" + c);
                        }
                        break; // bool = int > double
                    case 47:
                        unsafe
                        {
                            AddV(Convert.ToString((ulong)command.pba), "f");
                            AddV(Convert.ToString((ulong)command.pib), "tib" + c);
                            AddV(Convert.ToString((ulong)command.pdc), "tdc" + c);
                        }
                        break; // bool = int < double
                    case 48:
                        unsafe
                        {
                            AddV(Convert.ToString((ulong)command.pba), "f");
                            AddV(Convert.ToString((ulong)command.pib), "tib" + c);
                            AddV(Convert.ToString((ulong)command.pdc), "tdc" + c);
                        }
                        break; // bool = int == double
                    case 49:
                        unsafe
                        {
                            AddV(Convert.ToString((ulong)command.pba), "f");
                            AddV(Convert.ToString((ulong)command.pib), "tib" + c);
                            AddV(Convert.ToString((ulong)command.pdc), "tdc" + c);
                        }
                        break; // bool = int != double
                    case 50:
                        unsafe
                        {
                            AddV(Convert.ToString((ulong)command.pia), "f");
                            AddV(Convert.ToString((ulong)command.pia), "t");
                            AddV(Convert.ToString((ulong)command.pib), "tib" + c);
                        }
                        break; // int += int
                    case 51:
                        unsafe
                        {
                            AddV(Convert.ToString((ulong)command.pda), "f");
                            AddV(Convert.ToString((ulong)command.pda), "t");
                            AddV(Convert.ToString((ulong)command.pdb), "tdb" + c);
                        }
                        break; // double += double
                    case 52:
                        unsafe
                        {
                            AddV(Convert.ToString((ulong)command.pba), "f");
                            AddV(Convert.ToString((ulong)command.pib), "tib" + c);
                        }
                        break; // bool = int < intVal
                    case 53:
                        unsafe
                        {
                            AddV(Convert.ToString((ulong)command.pda), "f");
                            AddV(Convert.ToString((ulong)command.pic), "tic" + c);
                        }
                        break; // double += doubleVal / int
                    case 54:
                        unsafe { AddV(Convert.ToString((ulong)command.pia), "f"); }
                        break; // int += intVal
                    case 55:
                        unsafe
                        {
                            AddV(Convert.ToString((ulong)command.pda), "f");
                            AddV(Convert.ToString((ulong)command.pda), "t");
                            AddV(Convert.ToString((ulong)command.pdb), "tdb" + c);
                            AddV(Convert.ToString((ulong)command.pic), "tic" + c);
                        }
                        break; // double += double / int
                    case 56:
                        unsafe { AddV(Convert.ToString((ulong)command.pda), "f"); }
                        break; // double = intVal
                    case 57:
                        unsafe { *command.pba = *command.pbb && *command.pbc;
                            AddV(Convert.ToString((ulong)command.pba), "f");
                            AddV(Convert.ToString((ulong)command.pbb), "tbb");
                            AddV(Convert.ToString((ulong)command.pbc), "tbc" + c);
                        }
                        break; // bool = bool && bool
                    case 58:
                        unsafe
                        {
                            *command.pba = *command.pbb && *command.pbc;
                            AddV(Convert.ToString((ulong)command.pba), "f");
                            AddV(Convert.ToString((ulong)command.pbb), "tbb");
                            AddV(Convert.ToString((ulong)command.pbc), "tbc" + c);
                        }
                        break; // bool = bool || bool
                    case 59:
                        unsafe
                        {
                            AddV(Convert.ToString((ulong)command.pia), "f");
                            AddV(Convert.ToString((ulong)command.pia), "t");
                            AddV(Convert.ToString((ulong)command.pib), "tib" + c);
                        }
                        break; // int -= int
                    case 60:
                        unsafe
                        {
                            AddV(Convert.ToString((ulong)command.pia), "f");
                            AddV(Convert.ToString((ulong)command.pia), "t");
                            AddV(Convert.ToString((ulong)command.pib), "tib" + c);
                        }
                        break; // int *= int
                    case 61:
                        unsafe
                        {
                            AddV(Convert.ToString((ulong)command.pia), "f");
                            AddV(Convert.ToString((ulong)command.pia), "t");
                            AddV(Convert.ToString((ulong)command.pib), "tib" + c);
                        }
                        break; // int /= int
                    case 62:
                        unsafe
                        {
                            AddV(Convert.ToString((ulong)command.pda), "f");
                            AddV(Convert.ToString((ulong)command.pda), "t");
                            AddV(Convert.ToString((ulong)command.pdb), "tdb" + c);
                        }
                        break; // double -= double
                    case 63:
                        unsafe
                        {
                            AddV(Convert.ToString((ulong)command.pda), "f");
                            AddV(Convert.ToString((ulong)command.pda), "t");
                            AddV(Convert.ToString((ulong)command.pdb), "tdb" + c);
                        }
                        break; // double *= double
                    case 64:
                        unsafe
                        {
                            AddV(Convert.ToString((ulong)command.pda), "f");
                            AddV(Convert.ToString((ulong)command.pda), "t");
                            AddV(Convert.ToString((ulong)command.pdb), "tdb" + c);
                        }
                        break; // double /= double
                    case 65:
                        unsafe
                        {
                            AddV(Convert.ToString((ulong)command.pda), "f");
                            AddV(Convert.ToString((ulong)command.pda), "t");
                            AddV(Convert.ToString((ulong)command.pib), "tib" + c);
                        }
                        break; // double += int
                    case 66:
                        unsafe
                        {
                            AddV(Convert.ToString((ulong)command.pda), "f");
                            AddV(Convert.ToString((ulong)command.pda), "t");
                            AddV(Convert.ToString((ulong)command.pib), "tib" + c);
                        }
                        break; // double -= int
                    case 67:
                        unsafe
                        {
                            AddV(Convert.ToString((ulong)command.pda), "f");
                            AddV(Convert.ToString((ulong)command.pda), "t");
                            AddV(Convert.ToString((ulong)command.pib), "tib" + c);
                        }
                        break; // double *= int
                    case 68:
                        unsafe
                        {
                            AddV(Convert.ToString((ulong)command.pda), "f");
                            AddV(Convert.ToString((ulong)command.pda), "t");
                            AddV(Convert.ToString((ulong)command.pib), "tib" + c);
                        }
                        break; // double /= int
                    case 69:
                        unsafe { *command.pia += *command.pib + *command.pic; }
                        break; // int += int + int 
                    case 70:
                        unsafe { *command.pia += *command.pib - *command.pic; }
                        break; // int += int - int
                    case 71:
                        unsafe { *command.pia += *command.pib * *command.pic; }
                        break; // int += int * int 
                    case 72:
                        unsafe { *command.pia += *command.pib + *command.pic; }
                        break; // int += int / int
                    case 73:
                        unsafe { *command.pda += *command.pdb + *command.pdc; }
                        break; // double += double + double
                    case 74:
                        unsafe { *command.pda += *command.pdb - *command.pdc; }
                        break; // double += double - double 
                    case 75:
                        unsafe { *command.pda += *command.pdb * *command.pdc; }
                        break; // double += double * double 
                    case 76:
                        unsafe { *command.pda += *command.pdb / *command.pdc; }
                        break; // double += double / double
                    case 77:
                        unsafe { *command.pda += *command.pib + *command.pdc; }
                        break; // double += int + double
                    case 78:
                        unsafe { *command.pda += *command.pib - *command.pdc; }
                        break; // double += int - double
                    case 79:
                        unsafe { *command.pda += *command.pib * *command.pdc; }
                        break; // double += int * double
                    case 80:
                        unsafe { *command.pda += *command.pib / *command.pdc; }
                        break; // double += int / double
                    case 81:
                        unsafe { *command.pda += *command.pdb + *command.pic; }
                        break; // double += double + int 
                    case 82:
                        unsafe { *command.pda += *command.pdb - *command.pic; }
                        break; // double += double - int 
                    case 83:
                        unsafe { *command.pda += *command.pdb * *command.pic; }
                        break; // double += double * int 

                    default:
                        break;
                }

            }
        }
        public void DefUse()
        {
            var Arr = ArrBlocks;
            int end = Arr[1] - 1;
            for (int i = 0; i < Arr.Length - 1; i++)
            {
                CollectMarks(end, i, Arr);
                Vals.Clear();
            }
        }

        public void ReplaceCopies()
        {
            var Arr = ArrBlocks;
            UseFull.Clear();
            int end = Arr[1] - 1;
            for (int i = 0; i < Arr.Length - 1; i++)
            {
                end = Arr[i + 1] - 1;
                CollectMarks(end, i, Arr);
                end = Arr[i + 1] - 2;
                while (Arr[i] - 1 != end)
                {
                    var command = Commands[end];

                    c = end--;
                    string s = " ";
                    int ind = 0;
                    unsafe
                    {
                        if (command.Count <= 1)
                        {
                            continue;
                        }
                        switch (command.Types[0])
                        {
                            case 'i':
                                s = Vals[Convert.ToString((ulong)command.pia)];
                                break;
                            case 'd':
                                if (Vals.ContainsKey(Convert.ToString((ulong)command.pda)))
                                {
                                    s = Vals[Convert.ToString((ulong)command.pda)];
                                }
                                break;
                            case 'b':
                                s = Vals[Convert.ToString((ulong)command.pba)];
                                break;
                            default:
                                break;
                        }        
                    }
                    if (s[0] == 't' || s.Length <= 1)
                    {
                        if (s[0] == 'f')
                        {
                            Redundant.Add(c);
                        }
                        continue;
                    }
                    ind = int.Parse(s.Substring(3));
                  
                    unsafe
                    {
                        if (command.Count < 3)
                        {
                            Redundant.Add(c);
                            if (command.Types[1] == '1' || command.Types[1] == '2' || command.Types[1] == '3')
                            {
                                if (s[1] == 'i')
                                {
                                    if (s[2] == 'b')
                                    {
                                        *Commands[ind].pib = command.intVal;
                                    }
                                    if (s[2] == 'c')
                                    {
                                        *Commands[ind].pic = command.intVal;
                                    }
                                }
                                if (s[1] == 'd')
                                {
                                    if (s[2] == 'b')
                                    {
                                        *Commands[ind].pdb = command.doubleVal;
                                    }
                                    if (s[2] == 'c')
                                    {
                                        *Commands[ind].pdc = command.doubleVal;
                                    }
                                }
                                if (s[1] == 'b')
                                {
                                    if (s[2] == 'b')
                                    {
                                        *Commands[ind].pbb = command.boolVal;
                                    }
                                    if (s[2] == 'c')
                                    {
                                        *Commands[ind].pbc = command.boolVal;
                                    }
                                }
                            }
                            else
                            {
                                if (s[1] == 'i')
                                {
                                    UseFull[Convert.ToString((ulong)command.pib)] = -1;
                                    if (s[2] == 'b')
                                    {
                                        *Commands[ind].pib = *command.pib;
                                    }
                                    if (s[2] == 'c')
                                    {
                                        *Commands[ind].pic = *command.pib;
                                    }
                                }
                                if (s[1] == 'd')
                                {
                                    UseFull[Convert.ToString((ulong)command.pdb)] = -1;
                                    if (s[2] == 'b')
                                    {
                                        //Console.WriteLine(s);
                                        *Commands[ind].pdb = *command.pdb;
                                    }
                                    if (s[2] == 'c')
                                    {
                                        *Commands[ind].pdc = *command.pdb;
                                    }
                                }
                                if (s[1] == 'b')
                                {
                                    UseFull[Convert.ToString((ulong)command.pbb)] = -1;
                                    if (s[2] == 'b')
                                    {
                                        *Commands[ind].pbb = *command.pbb;
                                    }
                                    if (s[2] == 'c')
                                    {
                                        *Commands[ind].pbc = *command.pbb;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (Commands[ind].Count != 2)
                            {
                                continue;
                            }
                            if (Commands[ind].NumberOfCommand >= 59 && Commands[ind].NumberOfCommand <=68 
                                || Commands[ind].NumberOfCommand == 51 || Commands[ind].NumberOfCommand == 50)
                            {
                                if (command.Count <= 2)
                                {
                                    continue;
                                }
                                switch (Commands[ind].NumberOfCommand)
                                {
                                    case 50:
                                    case 59-61:

                                        break;
                                    case 51:
                                        if (command.NumberOfCommand == 21)
                                        {
                                            Commands[ind].NumberOfCommand = 55;
                                            Commands[ind].pdb = command.pdb;
                                            Commands[ind].pic = command.pic;
                                        }
                                        break;
                                    default:
                                        break;
                                }
                                Temporary.Add(c);
                                Redundant.Add(c);
                                continue;
                            }
                             
                            Temporary.Add(ind);
                            Redundant.Add(ind);
                            unsafe
                            { 
                            switch (command.Types[0])
                            {
                                case 'i':
                                        command.pia = Commands[ind].pia;
                                        break;
                                case 'd':
                                        command.pda = Commands[ind].pda;
                                        break;
                                case 'b':
                                        command.pba = Commands[ind].pba;
                                        break;
                                default:
                                    break;
                            }

                            }
                        }
                       
                    }             
                }
                Vals.Clear();

            }
        }
        public int DelUseless()
        {
            int k = 0;
            foreach (var x in Redundant)
            {
                if (Temporary.Contains(x))
                {
                    DelCommand(x - k++);
                    continue;
                }
                var command = Commands[x - k];
                int t = -1;
                unsafe
                {
                    switch (command.Types[0])
                    {
                        case 'i':
                            if (UseFull.ContainsKey(Convert.ToString((ulong)command.pia)))
                            {
                                t = UseFull[Convert.ToString((ulong)command.pia)];
                            }
                            break;
                        case 'd':
                            if (UseFull.ContainsKey(Convert.ToString((ulong)command.pda)))
                            {
                                t = UseFull[Convert.ToString((ulong)command.pda)];
                            }
                            break;
                        case 'b':
                            if (UseFull.ContainsKey(Convert.ToString((ulong)command.pba)))
                            {
                                t = UseFull[Convert.ToString((ulong)command.pba)];
                            }
                            break;
                        default:
                            break;
                    }
                }

                if (t == -1)
                {
                    DelCommand(x - k++);
                }
            }
            return k;
        }

        public Graph CreateGraph()
        {
            var g = new Graph(ArrBlocks.Length);
            for (int i = 0; i < ArrBlocks.Length; i++)
            {
                //g.AddEdge(ArrBlocks[i], )
            }
            g.AddEdge(2, 3);
           // g.PrintEdges();
           // Console.WriteLine(g.VertexSize);
            return g;
        }

        public void KillDef()
        {
          SortedSet<int> Def = new SortedSet<int>();
            SortedSet<int> Kill = new SortedSet<int>();
           DefArr = new SortedSet<int>[ArrBlocks.Length];
           KillArr = new SortedSet<int>[ArrBlocks.Length];
            int end = ArrBlocks[1] - 1;
            int k = 0;
            for (int i = 0; i < ArrBlocks.Length - 1; i++)
            {
                DefArr[i] = new SortedSet<int>();
                KillArr[i] = new SortedSet<int>();
                var Defs = new SortedSet<String>();
                k = ArrBlocks[i];
                end = ArrBlocks[i + 1] - 1;
                while (k != end+1)
                {
                    if (Commands[k].Count >= 2)
                    {
                        unsafe 
                        {
                            var s = "";
                            switch (Commands[k].Types[0])
                            {
                                case 'i':
                                    s = Convert.ToString((ulong)Commands[k].pia);
                                    break;
                                case 'd':
                                    s = Convert.ToString((ulong)Commands[k].pda);
                                    break;
                                case 'b':
                                    s = Convert.ToString((ulong)Commands[k].pba);
                                    break;
                                default:
                                    break;
                            }
                            if (!Defs.Contains(s))
                            {
                                DefArr[i].Add(k);
                                Defs.Add(s);
                            }

                            for (int j = 0; j < Size; j++)
                            {
                                var s1 = "";
                                if (Commands[j].Count <= 1)
                                {
                                    continue;
                                }
                                switch (Commands[j].Types[0])
                                {
                                    case 'i':
                                        s1 = Convert.ToString((ulong)Commands[j].pia);
                                        break;
                                    case 'd':
                                        s1 = Convert.ToString((ulong)Commands[j].pda);
                                        break;
                                    case 'b':
                                        s1 = Convert.ToString((ulong)Commands[j].pba);
                                        break;
                                    default:
                                        break;
                                }
                                if (s == s1 && !DefArr[i].Contains(j))
                                {
                                    KillArr[i].Add(j);
                                }
                            }
                        }
                    
                    
                    }
                    k++;
                }

            }

        }

        public void Preparing()
        {
            FindLeaders();
            if (ArrBlocks.Length <= 1)
            {
                return;
            }          
            DefUse();
            int k = 0;
            while (Redundant.Count != 0)
            {
                foreach (var item in Redundant)
                {
                    DelCommand(item-k++);
                }
                Redundant.Clear();
                FindLeaders();
                DefUse();
            }
            ReplaceCopies();
            int temp = DelUseless();
            while (temp != 0)
            {
                Redundant.Clear();
                UseFull.Clear();
                Temporary.Clear();
                FindLeaders();            
                ReplaceCopies();
                temp = DelUseless();
            }
            // KillDef();
            //foreach (var item in KillArr[2])
            //{
            //    Console.WriteLine(item);
            //}


        }
        public unsafe void RunCommands()
        {
           // Print();
             Preparing();
            Console.WriteLine("Выполнение программы");
            for (int i = 0; i < Size; i++)
            {
                var command = Commands[i];
                switch (command.NumberOfCommand)
                {            
                    case 0:
                        i = Size;
                        break; // stop
                    case 1:
                        unsafe {
                            *command.pia = command.intVal;
                        }
                        break; // int = intVal
                    case 2:
                        unsafe
                        {
                            *command.pda = command.doubleVal;
                        }
                        break; // double = doubleVal
                    case 3:
                        unsafe { *command.pba = command.boolVal; }
                        break; // bool = boolVal
                    case 4:
                        unsafe { *command.pia = *command.pib;                       
                        }
                        break; // int = int 
                    case 5:
                        unsafe { *command.pda = *command.pdb; }
                        break; // double = double
                    case 6:
                        unsafe { *command.pba = *command.pbb; }
                        break; // bool = bool
                    case 7:
                        unsafe { *command.pda = *command.pib; }
                        break; // double = int
                    case 8:     
                        unsafe { *command.pia = *command.pib + *command.pic;}

                        break; // int = int + int
                    case 9:
                        unsafe { *command.pda = *command.pdb + *command.pdc; }
                        break; // double = double + double
                    case 10:
                        unsafe { *command.pda = *command.pdb + *command.pic; }
                        break; // double = double + int
                    case 11:
                        unsafe { *command.pda = *command.pib + *command.pic; }
                        break; // double = int + int
                    case 12:
                        i = command.Goto-2;
                        break; // goto
                    case 13:
                        unsafe { if (*command.pba == true) i = command.Goto - 2;}
                        break; // if
                    case 14:
                        unsafe { *command.pda = command.doubleVal / *command.pib; }
                        break; // double = doubleVal / int 
                    case 15:
                        unsafe { *command.pia = *command.pib + command.intVal;}
                        
                        break; // int = int + intVal
                    case 16:
                        unsafe { *command.pba = *command.pib >= *command.pic; }
                        break; // bool = int >= int
                    case 17:
                        unsafe { Console.WriteLine(*command.pia); }
                        break; // print(int)
                    case 18:
                        unsafe { Console.WriteLine(*command.pda); }
                        break; // print(double)
                    case 19:
                        unsafe { Console.WriteLine(*command.pba); }
                        break; // print(int)
                    case 20:
                        unsafe
                        {
                            *command.pba = *command.pib < *command.pic;}
                        break; // bool = int < int
                    case 21:
                        unsafe {
                          
                            *command.pda = *command.pdb / *command.pic; }
                        break; // double = double / int 
                    case 22:
                        unsafe
                        {
                          
                            if (*command.pba == false) i = command.Goto; }
                        break; // if
                    case 23:
                        i = command.Goto;
                        break;
                    case 24:
                        unsafe { *command.pia = *command.pib - *command.pic; }
                        break; // int = int - int
                    case 25:
                        unsafe { *command.pia = *command.pib * *command.pic; }
                        break; // int = int * int
                    case 26:
                        unsafe { *command.pia = *command.pib / *command.pic;}
                        break; // int = int / int
                    case 27:
                        unsafe { *command.pba = *command.pib > *command.pic; }
                        break; // bool = int > int
                    case 28:
                        unsafe { *command.pba = *command.pib == *command.pic; }
                        break; // bool = int == int
                    case 29:
                        unsafe { *command.pba = *command.pib != *command.pic; }
                        break; // bool = int != int
                    case 30:
                        unsafe { *command.pda = *command.pdb - *command.pdc; }
                        break; // double = double - double
                    case 31:
                        unsafe { *command.pda = *command.pdb * *command.pdc; }
                        break; // double = double * double
                    case 32:
                        unsafe { *command.pda = *command.pdb / *command.pdc; }
                        break; // double = double / double
                    case 33:
                        unsafe { *command.pba = *command.pdb > *command.pdc; }
                        break; // bool = double > double
                    case 34:
                        unsafe { *command.pba = *command.pdb < *command.pdc; }
                        break; // bool = double < double
                    case 35:
                        unsafe { *command.pba = *command.pdb == *command.pdc; }
                        break; // bool = double == double
                    case 36:
                        unsafe { *command.pba = *command.pdb != *command.pdc; }
                        break; // bool = double != double
                    case 37:
                        unsafe { *command.pda = *command.pdb - *command.pic; }
                        break; // double = doubleVal - int 
                    case 38:
                        unsafe { *command.pda = *command.pdb * *command.pic; }
                        break; // double = doubleVal * int 
                    case 39:
                        unsafe { *command.pba = *command.pdb > *command.pic; }
                        break; // bool = double > int
                    case 40:
                        unsafe { *command.pba = *command.pdb < *command.pic; }
                        break; // bool = double < int
                    case 41:
                        unsafe { *command.pba = *command.pdb == *command.pic; }
                        break; // bool = double == int
                    case 42:
                        unsafe { *command.pba = *command.pdb != *command.pic; }
                        break; // bool = double != int
                    case 43:
                        unsafe { *command.pda = *command.pib - *command.pdc; }
                        break; // double = int - double
                    case 44:
                        unsafe { *command.pda = *command.pib * *command.pdc; }
                        break; // double = int * double
                    case 45:
                        unsafe { *command.pda = *command.pib / *command.pdc; }
                        break; // double = int / double
                    case 46:
                        unsafe { *command.pba = *command.pib > *command.pdc; }
                        break; // bool = int > double
                    case 47:
                        unsafe { *command.pba = *command.pib < *command.pdc; }
                        break; // bool = int < double
                    case 48:
                        unsafe { *command.pba = *command.pib == *command.pdc; }
                        break; // bool = int == double
                    case 49:
                        unsafe { *command.pba = *command.pib != *command.pdc; }
                        break; // bool = int != double
                    case 50:
                        unsafe { *command.pia += *command.pib; }
                        break; // int += int
                    case 51:
                        unsafe { *command.pda += *command.pdb; }
                        break; // double += double
                    case 52:
                        unsafe { *command.pba = *command.pib < command.intVal; }
                        break; // bool = int < intVal
                    case 53:
                        unsafe { *command.pda += command.doubleVal / *command.pic; }
                        break; // double += doubleVal / int
                    case 54:
                        unsafe { *command.pia += command.intVal; }
                        break; // int += intVal
                    case 55:
                        unsafe{*command.pda += *command.pdb / *command.pic;}
                        break; // double += double / int 
                    case 56:
                        unsafe { *command.pda = command.intVal; }
                        break; // double = intVal
                    case 57:
                        unsafe { *command.pba = *command.pbb && *command.pbc; }
                        break; // bool = bool && bool
                    case 58:
                        unsafe { *command.pba = *command.pbb || *command.pbc; }
                        break; // bool = bool || bool
                    case 59:
                        unsafe { *command.pia -= *command.pib; }
                        break; // int -= int
                    case 60:
                        unsafe { *command.pia *= *command.pib; }
                        break; // int *= int
                    case 61:
                        unsafe { *command.pia /= *command.pib; }
                        break; // int /= int
                    case 62:
                        unsafe { *command.pda -= *command.pdb; }
                        break; // double -= double
                    case 63:
                        unsafe { *command.pda *= *command.pdb; }
                        break; // double *= double
                    case 64:
                        unsafe { *command.pda /= *command.pdb; }
                        break; // double /= double
                    case 65:
                        unsafe { *command.pda += *command.pib; }
                        break; // double += int
                    case 66:
                        unsafe { *command.pda -= *command.pib; }
                        break; // double -= int
                    case 67:
                        unsafe { *command.pda *= *command.pib; }
                        break; // double *= int
                    case 68:
                        unsafe { *command.pda /= *command.pib; }
                        break; // double /= int
                    case 69:
                        unsafe { *command.pia += *command.pib + *command.pic; }
                        break; // int += int + int 
                    case 70:
                        unsafe { *command.pia += *command.pib - *command.pic; }
                        break; // int += int - int
                    case 71:
                        unsafe { *command.pia += *command.pib * *command.pic; }
                        break; // int += int * int 
                    case 72:
                        unsafe { *command.pia += *command.pib + *command.pic; }
                        break; // int += int / int
                    case 73:
                        unsafe { *command.pda += *command.pdb + *command.pdc; }
                        break; // double += double + double
                    case 74:
                        unsafe { *command.pda += *command.pdb - *command.pdc; }
                        break; // double += double - double 
                    case 75:
                        unsafe { *command.pda += *command.pdb * *command.pdc; }
                        break; // double += double * double 
                    case 76:
                        unsafe { *command.pda += *command.pdb / *command.pdc; }
                        break; // double += double / double
                    case 77:
                        unsafe { *command.pda += *command.pib + *command.pdc; }
                        break; // double += int + double
                    case 78:
                        unsafe { *command.pda += *command.pib - *command.pdc; }
                        break; // double += int - double
                    case 79:
                        unsafe { *command.pda += *command.pib * *command.pdc; }
                        break; // double += int * double
                    case 80:
                        unsafe { *command.pda += *command.pib / *command.pdc; }
                        break; // double += int / double
                    case 81:
                        unsafe { *command.pda += *command.pdb + *command.pic; }
                        break; // double += double + int 
                    case 82:
                        unsafe { *command.pda += *command.pdb - *command.pic; }
                        break; // double += double - int 
                    case 83:
                        unsafe { *command.pda += *command.pdb * *command.pic; }
                        break; // double += double * int 
                    default:
                        break;
                }
            }

        }

        public unsafe void PrintCommands()
        {
            Preparing();
           // Print();
            for (int i = 0; i < Size; i++)
            {
                var command = Commands[i];
                StrCommands += command.NumberOfCommand + ": "+ "(" + i + ") ";
                switch (command.NumberOfCommand)
                {
                    case 0:
                        i = Size;
                        Console.WriteLine(StrCommands);
                        break; // stop
                    case 1:
                        unsafe {
                            *command.pia = command.intVal;
                            AddVal(Convert.ToString((ulong)command.pia));
                            StrCommands += Values[Convert.ToString((ulong)command.pia)] + " = "
                                + command.intVal + "\n";
                        }
                        break; // int = intVal
                    case 2:
                        unsafe { *command.pda = command.doubleVal;
                            AddVal(Convert.ToString((ulong)command.pda));
                            StrCommands += Values[Convert.ToString((ulong)command.pda)] + " = "
                                + command.doubleVal + "\n";
                        }
                        break; // double = doubleVal
                    case 3:
                        unsafe { *command.pba = command.boolVal;
                            AddVal(Convert.ToString((ulong)command.pba));
                            StrCommands += Values[Convert.ToString((ulong)command.pba)] + " = "
                                + command.boolVal + "\n";
                        }
                        break; // bool = boolVal
                    case 4:
                        unsafe {
                            *command.pia = *command.pib;
                            AddVal(Convert.ToString((ulong)command.pia));
                            AddVal(Convert.ToString((ulong)command.pib));
                            StrCommands += Values[Convert.ToString((ulong)command.pia)] + " = " 
                                + Values[Convert.ToString((ulong)command.pib)] + "\n";
                        }
                        break; // int = int 
                    case 5:
                        unsafe { 
                            *command.pda = *command.pdb;
                            AddVal(Convert.ToString((ulong)command.pda));
                            AddVal(Convert.ToString((ulong)command.pdb));
                            StrCommands += Values[Convert.ToString((ulong)command.pda)] + " = "
                                + Values[Convert.ToString((ulong)command.pdb)] + "\n";
                        }
                        break; // double = double
                    case 6:
                        unsafe { *command.pba = *command.pbb;
                            AddVal(Convert.ToString((ulong)command.pba));
                            AddVal(Convert.ToString((ulong)command.pbb));
                            StrCommands += Values[Convert.ToString((ulong)command.pba)] + " = "
                                + Values[Convert.ToString((ulong)command.pbb)] + "\n";
                        }
                        break; // bool = bool
                    case 7:
                        unsafe { *command.pda = *command.pib; }
                        break; // double = int
                    case 8:
                        unsafe { 
                            *command.pia = *command.pib + *command.pic;
                            AddVal(Convert.ToString((ulong)command.pia));
                            AddVal(Convert.ToString((ulong)command.pib));
                            AddVal(Convert.ToString((ulong)command.pic));
                            StrCommands += Values[Convert.ToString((ulong)command.pia)] + " = "
                                + Values[Convert.ToString((ulong)command.pib)] + " + " +
                                 Values[Convert.ToString((ulong)command.pic)] + "\n";
                        }

                        break; // int = int + int
                    case 9:
                        unsafe { 
                            *command.pda = *command.pdb + *command.pdc;
                            AddVal(Convert.ToString((ulong)command.pda));
                            AddVal(Convert.ToString((ulong)command.pdb));
                            AddVal(Convert.ToString((ulong)command.pdc));
                            StrCommands += Values[Convert.ToString((ulong)command.pda)] + " = "
                                + Values[Convert.ToString((ulong)command.pdb)] + " + " +
                                 Values[Convert.ToString((ulong)command.pdc)] + "\n";
                        }
                        break; // double = double + double
                    case 10:
                        unsafe { *command.pda = *command.pdb + *command.pic; }
                        break; // double = double + int
                    case 11:
                        unsafe { *command.pda = *command.pib + *command.pic; }
                        break; // double = int + int
                    case 12:
                        i = command.Goto - 2;
                        break; // goto
                    case 13:
                        unsafe { if (*command.pba == true) i = command.Goto - 2; }
                        break; // if
                    case 14:
                        unsafe {*command.pda = command.doubleVal / *command.pic;
                            AddVal(Convert.ToString((ulong)command.pda));
                            AddVal(Convert.ToString((ulong)command.pic));
                            StrCommands += Values[Convert.ToString((ulong)command.pda)] + " = "
                                + command.doubleVal + " / " +
                                 Values[Convert.ToString((ulong)command.pic)] + "\n";
                        }
                        break; // double = doubleVal / int 
                    case 15:
                        unsafe { *command.pia = *command.pib + command.intVal; }

                        break; // int = int + intVal
                    case 16:
                        unsafe { *command.pba = *command.pib >= *command.pic; }
                        break; // bool = int >= int
                    case 17:
                        unsafe { 
                            Console.WriteLine(*command.pia);
                            AddVal(Convert.ToString((ulong)command.pia));
                            StrCommands += "print(" +Values[Convert.ToString((ulong)command.pia)] + ")" + "\n";
                        }
                        break; // print(int)
                    case 18:
                        unsafe { Console.WriteLine(*command.pda); }
                        break; // print(double)
                    case 19:
                        unsafe { Console.WriteLine(*command.pba); }
                        break; // print(int)
                    case 20:
                        unsafe { 
                            *command.pba = *command.pib < *command.pic;
                            AddVal(Convert.ToString((ulong)command.pba));
                            AddVal(Convert.ToString((ulong)command.pib));
                            AddVal(Convert.ToString((ulong)command.pic));
                            StrCommands += Values[Convert.ToString((ulong)command.pba)] + " = "
                                + Values[Convert.ToString((ulong)command.pib)] + " < " +
                                 Values[Convert.ToString((ulong)command.pic)] + "\n";
                        }
                        break; // bool = int < int
                    case 21:
                        unsafe { 
                            *command.pda = *command.pdb / *command.pic;
                            AddVal(Convert.ToString((ulong)command.pda));
                            AddVal(Convert.ToString((ulong)command.pdb));
                            AddVal(Convert.ToString((ulong)command.pic));
                            StrCommands += Values[Convert.ToString((ulong)command.pda)] + " = "
                                + Values[Convert.ToString((ulong)command.pdb)] + " / " +
                                 Values[Convert.ToString((ulong)command.pic)] + "\n";
                        }
                        break; // double = doubleVal / int 
                    case 22:
                        unsafe {
                            if (*command.pba == false) i = command.Goto;
                            AddVal(Convert.ToString((ulong)command.pba));
                            StrCommands += "if (" + Values[Convert.ToString((ulong)command.pba)] +
                                " == false) goto " + (command.Goto + 1) + "\n";
                        }
                        break; // if
                    case 23:
                        StrCommands += "goto " + (command.Goto+1) + "\n";
                        i = command.Goto;
                        break;
                    case 24:
                        unsafe { *command.pia = *command.pib - *command.pic; }
                        break; // int = int - int
                    case 25:
                        unsafe { *command.pia = *command.pib * *command.pic; }
                        break; // int = int * int
                    case 26:
                        unsafe { *command.pia = *command.pib / *command.pic; }
                        break; // int = int / int
                    case 27:
                        unsafe { *command.pba = *command.pib > *command.pic; }
                        break; // bool = int > int
                    case 28:
                        unsafe { *command.pba = *command.pib == *command.pic; }
                        break; // bool = int == int
                    case 29:
                        unsafe { *command.pba = *command.pib != *command.pic; }
                        break; // bool = int != int
                    case 30:
                        unsafe { *command.pda = *command.pdb - *command.pdc; }
                        break; // double = double - double
                    case 31:
                        unsafe { *command.pda = *command.pdb * *command.pdc; }
                        break; // double = double * double
                    case 32:
                        unsafe { *command.pda = *command.pdb / *command.pdc; }
                        break; // double = double / double
                    case 33:
                        unsafe { *command.pba = *command.pdb > *command.pdc; }
                        break; // bool = double > double
                    case 34:
                        unsafe { *command.pba = *command.pdb < *command.pdc; }
                        break; // bool = double < double
                    case 35:
                        unsafe { *command.pba = *command.pdb == *command.pdc; }
                        break; // bool = double == double
                    case 36:
                        unsafe { *command.pba = *command.pdb != *command.pdc; }
                        break; // bool = double != double
                    case 37:
                        unsafe { *command.pda = *command.pdb - *command.pic; }
                        break; // double = doubleVal - int 
                    case 38:
                        unsafe { *command.pda = *command.pdb * *command.pic; }
                        break; // double = doubleVal * int 
                    case 39:
                        unsafe { *command.pba = *command.pdb > *command.pic; }
                        break; // bool = double > int
                    case 40:
                        unsafe { *command.pba = *command.pdb < *command.pic; }
                        break; // bool = double < int
                    case 41:
                        unsafe { *command.pba = *command.pdb == *command.pic; }
                        break; // bool = double == int
                    case 42:
                        unsafe { *command.pba = *command.pdb != *command.pic; }
                        break; // bool = double != int
                    case 43:
                        unsafe { *command.pda = *command.pib - *command.pdc; }
                        break; // double = int - double
                    case 44:
                        unsafe { *command.pda = *command.pib * *command.pdc; }
                        break; // double = int * double
                    case 45:
                        unsafe { *command.pda = *command.pib / *command.pdc; }
                        break; // double = int / double
                    case 46:
                        unsafe { *command.pba = *command.pib > *command.pdc; }
                        break; // bool = int > double
                    case 47:
                        unsafe { *command.pba = *command.pib < *command.pdc; }
                        break; // bool = int < double
                    case 48:
                        unsafe { *command.pba = *command.pib == *command.pdc; }
                        break; // bool = int == double
                    case 49:
                        unsafe { *command.pba = *command.pib != *command.pdc; }
                        break; // bool = int != double
                    case 50:
                        unsafe { 
                            *command.pia += *command.pib;
                            AddVal(Convert.ToString((ulong)command.pia));
                            AddVal(Convert.ToString((ulong)command.pib));
                            StrCommands += Values[Convert.ToString((ulong)command.pia)] + " += "
                                + Values[Convert.ToString((ulong)command.pib)] + "\n";
                        }
                        break; // int += int
                    case 51:
                        unsafe { 
                            *command.pda += *command.pdb;
                            AddVal(Convert.ToString((ulong)command.pda));
                            AddVal(Convert.ToString((ulong)command.pdb));
                            StrCommands += Values[Convert.ToString((ulong)command.pda)] + " += "
                                + Values[Convert.ToString((ulong)command.pdb)] + "\n";
                        }
                        break; // double += double
                    case 52:
                        unsafe { *command.pba = *command.pib < command.intVal; }
                        break; // bool = int < intVal
                    case 53:
                        unsafe { *command.pda += 1.0 / *command.pic; }
                        break; // double += doubleVal / int
                    case 54:
                        unsafe { *command.pia += command.intVal; }
                        break; // int += intVal
                    case 55:
                        unsafe { 
                            *command.pda += *command.pdb / *command.pic;
                            AddVal(Convert.ToString((ulong)command.pda));
                            AddVal(Convert.ToString((ulong)command.pdb));
                            AddVal(Convert.ToString((ulong)command.pic));
                            StrCommands += Values[Convert.ToString((ulong)command.pda)] + " += "
                            + Values[Convert.ToString((ulong)command.pdb)] + " / " +
                             Values[Convert.ToString((ulong)command.pic)] + "\n";
                        }
                        break; // double += double / int
                    case 56:
                        unsafe { *command.pda = command.intVal;
                            AddVal(Convert.ToString((ulong)command.pda));
                            StrCommands += Values[Convert.ToString((ulong)command.pda)] + " = "
                                + command.intVal + "\n";
                        }
                        break; // double = intVal
                    case 57:
                        unsafe { *command.pba = *command.pbb && *command.pbc; }
                        break; // bool = bool && bool
                    case 58:
                        unsafe { *command.pba = *command.pbb || *command.pbc; }
                        break; // bool = bool || bool
                    case 59:
                        unsafe { *command.pia -= *command.pib; }
                        break; // int -= int
                    case 60:
                        unsafe { *command.pia *= *command.pib; }
                        break; // int *= int
                    case 61:
                        unsafe { *command.pia /= *command.pib; }
                        break; // int /= int
                    case 62:
                        unsafe { *command.pda -= *command.pdb; }
                        break; // double -= double
                    case 63:
                        unsafe { *command.pda *= *command.pdb; }
                        break; // double *= double
                    case 64:
                        unsafe { *command.pda /= *command.pdb; }
                        break; // double /= double
                    case 65:
                        unsafe { *command.pda += *command.pib; }
                        break; // double += int
                    case 66:
                        unsafe { *command.pda -= *command.pib; }
                        break; // double -= int
                    case 67:
                        unsafe { *command.pda *= *command.pib; }
                        break; // double *= int
                    case 68:
                        unsafe { *command.pda /= *command.pib; }
                        break; // double /= int
                    case 69:
                        unsafe { *command.pia += *command.pib + *command.pic; }
                        break; // int += int + int 
                    case 70:
                        unsafe { *command.pia += *command.pib - *command.pic; }
                        break; // int += int - int
                    case 71:
                        unsafe { *command.pia += *command.pib * *command.pic; }
                        break; // int += int * int 
                    case 72:
                        unsafe { *command.pia += *command.pib + *command.pic; }
                        break; // int += int / int
                    case 73:
                        unsafe { *command.pda += *command.pdb + *command.pdc; }
                        break; // double += double + double
                    case 74:
                        unsafe { *command.pda += *command.pdb - *command.pdc; }
                        break; // double += double - double 
                    case 75:
                        unsafe { *command.pda += *command.pdb * *command.pdc; }
                        break; // double += double * double 
                    case 76:
                        unsafe { *command.pda += *command.pdb / *command.pdc; }
                        break; // double += double / double
                    case 77:
                        unsafe { *command.pda += *command.pib + *command.pdc; }
                        break; // double += int + double
                    case 78:
                        unsafe { *command.pda += *command.pib - *command.pdc; }
                        break; // double += int - double
                    case 79:
                        unsafe { *command.pda += *command.pib * *command.pdc; }
                        break; // double += int * double
                    case 80:
                        unsafe { *command.pda += *command.pib / *command.pdc; }
                        break; // double += int / double
                    case 81:
                        unsafe { *command.pda += *command.pdb + *command.pic; }
                        break; // double += double + int 
                    case 82:
                        unsafe { *command.pda += *command.pdb - *command.pic; }
                        break; // double += double - int 
                    case 83:
                        unsafe { *command.pda += *command.pdb * *command.pic; }
                        break; // double += double * int 

                    default:
                        break;
                }
            }

        }
    }
}
