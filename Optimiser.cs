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
        public static Dictionary<string, SortedSet<string>>[] GlobalUse; // for GlobalUse
        public int ValsCounter = 0;// counter for PrintCommands
        public string StrCommands = ""; // Str for PrintCommands
        int[] ArrBlocks; // blocks of three address code
        SortedSet<int>[] GenArr;
        SortedSet<int>[] KillArr;
        SortedSet<string>[] DefArr;
        SortedSet<string>[] UseArr;
        SortedSet<int>[] IN;
        SortedSet<int>[] OUT;
        SortedSet<string>[] ActiveIN;
        SortedSet<string>[] ActiveOUT;
        SortedSet<int>[]Constants;
        SortedSet<string>[] DefCon;

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
                if (Commands[i].Tok == Toks.iff || Commands[i].Tok == Toks.got)
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
                Console.WriteLine("("+ i + ")"+Commands[i].Tok +  "->" + Commands[i].NumberOfCommand);
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
                                if (k - 1 == ind)
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

        public void AddGlobal(string s, string s2, int k)
        {
            if (DefCon[k] != null)
            {
                if (DefCon[k].Contains(s))
                {
                    if (!GlobalUse[k].ContainsKey(s))
                    {
                        var st = new SortedSet<string>();
                        st.Add(s2);
                        GlobalUse[k].Add(s, st);
                    }
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
                if (command.Tok == Toks.iff || command.Tok==Toks.got)
                {
                    BasicBlocks.Add(command.Goto + 1);
                    BasicBlocks.Add(i + 1);
                }
            }
            var Arr = new int[BasicBlocks.Count()];
            int k = 0;
            foreach (var item in BasicBlocks)
            {
                Arr[k++] = item;
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
                if (command.Count < 2)
                {
                    if (command.Tok == Toks.iff)
                    {
                        unsafe { AddV(Convert.ToString((ulong)command.pba), "t"); }
                    }
                    continue;
                }
                if (command.Assign != "aa")
                {
                    unsafe
                    {
                        switch (command.Types[0])
                        {
                            case 'i':
                                AddV(Convert.ToString((ulong)command.pia), "f");
                                AddV(Convert.ToString((ulong)command.pia), "t");
                                break;
                            case 'd':
                                AddV(Convert.ToString((ulong)command.pda), "f");
                                AddV(Convert.ToString((ulong)command.pda), "t");
                                break;
                            case 'b':
                                AddV(Convert.ToString((ulong)command.pba), "f");
                                AddV(Convert.ToString((ulong)command.pba), "t");
                                break;
                            default:
                                break;
                        }
                        switch (command.Types[1])
                        {
                            case 'i':
                                AddV(Convert.ToString((ulong)command.pib), "tib" + c);
                                break;
                            case 'd':
                                AddV(Convert.ToString((ulong)command.pdb), "tdb" + c);
                                break;
                            case 'b':
                                AddV(Convert.ToString((ulong)command.pbb), "tbb" + c);
                                break;
                            default:
                                break;
                        }

                    }

                }
                else
                {
                    unsafe
                    {
                        switch (command.Types[0])
                        {
                            case 'i':
                                AddV(Convert.ToString((ulong)command.pia), "f");
                                break;
                            case 'd':
                                AddV(Convert.ToString((ulong)command.pda), "f");
                                break;
                            case 'b':
                                AddV(Convert.ToString((ulong)command.pba), "f");
                                break;
                            default:
                                break;
                        }
                        switch (command.Types[1])
                        {
                            case 'i':
                                AddV(Convert.ToString((ulong)command.pib), "tib" + c);
                                break;
                            case 'd':
                                AddV(Convert.ToString((ulong)command.pdb), "tdb" + c);
                                break;
                            case 'b':
                                AddV(Convert.ToString((ulong)command.pbb), "tbb" + c);
                                break;
                            default:
                                break;
                        }
                    }
                }
                unsafe
                {
                    if (command.Count == 3)
                    {
                        switch (command.Types[2])
                        {
                            case 'i':
                                AddV(Convert.ToString((ulong)command.pic), "tic" + c);
                                break;
                            case 'd':
                                AddV(Convert.ToString((ulong)command.pdc), "tdc" + c);
                                break;
                            case 'b':
                                AddV(Convert.ToString((ulong)command.pbc), "tbc" + c);
                                break;
                            default:
                                break;
                        }
                    }
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
            Redundant.Clear();
            Temporary.Clear();
            UseFull.Clear();
            var Arr = ArrBlocks;

            int end = Arr[1] - 1;
            for (int i = 0; i < Arr.Length - 1; i++)
            {
                end = Arr[i + 1] - 1;
                CollectMarks(end, i, Arr);
                end = Arr[i + 1] - 2;
                int e = end;
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

                    if (ind < Arr[i] - 1 || ind > e+1)
                    {
                        continue;
                    }
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
                                        Commands[ind].intVal = command.intVal;
                                        Commands[ind].NumberOfCommand = 52;
                                        // Commands[ind].Tok = Toks.pbaapiolvi;
                                        //for (int j = 0; j < Size; j++)
                                        //{
                                        //    if (Commands[j].NumberOfCommand == 20)
                                        //    {
                                        //        Commands[j].intVal = command.intVal;
                                        //        Commands[j].NumberOfCommand = 52;
                                        //    }
                                        //}
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

                            if (Commands[ind].Assign != command.Assign)
                            {
                                if (command.Count <= 2)
                                {
                                    continue;
                                }
                                string tok = "";
                                switch (command.Types[0])
                                {
                                    case 'i':
                                        tok += "pi";
                                        break;
                                    case 'd':
                                        tok += "pd";
                                        break;
                                    case 'b':
                                        tok += "pb";
                                        break;
                                }
                                tok += Commands[ind].Assign;
                             switch (command.Types[1])
                                {
                                    case 'i':
                                        tok += "pi";
                                        Commands[ind].pib = command.pib;
                                        break;
                                    case 'd':
                                        tok += "pd";
                                        Commands[ind].pdb = command.pdb;
                                        break;
                                    case 'b':
                                        tok += "pb";
                                        Commands[ind].pbb = command.pbb;
                                        break;
                                }
                                tok += command.Operation;
                                switch (command.Types[2])
                                {
                                    case 'i':
                                        tok += "pi";
                                        Commands[ind].pic = command.pic;
                                        break;
                                    case 'd':
                                        tok += "pd";
                                        Commands[ind].pdc = command.pdc;
                                        break;
                                    case 'b':
                                        tok += "pb";
                                        Commands[ind].pbc = command.pbc;
                                        break;
                                }
                                Commands[ind].NumberOfCommand = 55;
                                Commands[ind].Count = 3;
                                var type = "";
                                type += tok[1];
                                type += tok[5];
                                type += tok[9];
                                Commands[ind].Types = type;
                                Toks tt = Toks.empty;
                                Enum.TryParse(tok, out tt);
                                Commands[ind].Tok = tt;
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
            var g = new Graph(ArrBlocks.Length + 2);
            g.AddEdge(-1, 0);
            int p = 0;
            for (int i = 1; i < Size; i++)
            {
                var command = Commands[i];
                if (command.Tok == Toks.iff)
                {
                    for (int j = 0; j < ArrBlocks.Length; j++)
                    {
                        if (Commands[ArrBlocks[j]].Tok == Toks.iff)
                        {
                            g.AddEdge(p, j);
                            g.AddEdge(j, j + 1);
                            g.AddEdge(j, j + 2);
                            p += 2;
                            break;
                        }
                    }
                }
                if (command.Tok == Toks.got)
                {
                    for (int j = 0; j < ArrBlocks.Length; j++)
                    {
                        if (ArrBlocks[j] == command.Goto + 1)
                        {
                            g.AddEdge(p, j);
                            p++;
                            break;
                        }
                    }
                }
                if (command.Tok == Toks.end)
                {
                    g.AddEdge(p, -2);
                    p++;
                }
            }
            return g;
        }

        public void GenKill()
        {
            SortedSet<int> Gen = new SortedSet<int>();
            SortedSet<int> Kill = new SortedSet<int>();
            GenArr = new SortedSet<int>[ArrBlocks.Length];
            KillArr = new SortedSet<int>[ArrBlocks.Length];
            int end = ArrBlocks[1] - 1;
            int k = 0;
            for (int i = 0; i < ArrBlocks.Length - 1; i++)
            {
                GenArr[i] = new SortedSet<int>();
                KillArr[i] = new SortedSet<int>();
                var Defs = new SortedSet<string>();
                k = ArrBlocks[i];
                end = ArrBlocks[i + 1] - 1;
                while (k != end + 1)
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
                                GenArr[i].Add(k);
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
                                if (s == s1 && !GenArr[i].Contains(j))
                                {
                                    KillArr[i].Add(j);
                                }
                            }
                        }


                    }
                    k++;
                }

            }
            GenArr[ArrBlocks.Length-1] = new SortedSet<int>();
            KillArr[ArrBlocks.Length-1] = new SortedSet<int>();

        }
        public void GlobalDefUse()
        {
            DefArr = new SortedSet<string>[ArrBlocks.Length];
            UseArr = new SortedSet<string>[ArrBlocks.Length];
            GlobalUse = new Dictionary<string, SortedSet<string>>[ArrBlocks.Length];
            int end = ArrBlocks[1] - 1;
            int k = 0;
            for (int i = 0; i < ArrBlocks.Length - 1; i++)
            {
                DefArr[i] = new SortedSet<string>();
                UseArr[i] = new SortedSet<string>();
                GlobalUse[i] = new Dictionary<string, SortedSet<string>>();
                k = ArrBlocks[i];
                end = ArrBlocks[i + 1] - 1;
                while (ArrBlocks[i] - 1 != end)
                {
                    k = end--;
                    if (Commands[k].Count >= 2)
                    {
                        unsafe
                        {
                            var s = "";
                            var t = "";
                            switch (Commands[k].Types[0])
                            {
                                case 'i':
                                    s = Convert.ToString((ulong)Commands[k].pia);
                                    t = "i";
                                    break;
                                case 'd':
                                    s = Convert.ToString((ulong)Commands[k].pda);
                                    t = "d";
                                    break;
                                case 'b':
                                    s = Convert.ToString((ulong)Commands[k].pba);
                                    t = "b";
                                    break;
                                default:
                                    break;
                            }
                                DefArr[i].Add(s);

                            if (Commands[k].Assign != "aa")
                            {
                                UseArr[i].Add(s);
                                AddGlobal(s,t+"b"+ k, i);
                            }
                            switch (Commands[k].Types[1])
                            {
                                case 'i':
                                    s = Convert.ToString((ulong)Commands[k].pib);
                                    t = "i";
                                    break;
                                case 'd':
                                    s = Convert.ToString((ulong)Commands[k].pdb);
                                    t = "d";
                                    break;
                                case 'b':
                                    s = Convert.ToString((ulong)Commands[k].pbb);
                                    t = "b";
                                    break;
                                default:
                                    break;
                            }
                            if (!DefArr[i].Contains(s))
                            {
                                UseArr[i].Add(s);
                                AddGlobal(s, t + "b"+ k, i);
                            }
                            if (Commands[k].Count == 3)
                            {
                                switch (Commands[k].Types[2])
                                {
                                    case 'i':
                                        s = Convert.ToString((ulong)Commands[k].pic);
                                        t = "i";
                                        break;
                                    case 'd':
                                        s = Convert.ToString((ulong)Commands[k].pdc);
                                        t = "i";
                                        break;
                                    case 'b':
                                        s = Convert.ToString((ulong)Commands[k].pbc);
                                        t = "i";
                                        break;
                                    default:
                                        break;
                                }
                                if (!DefArr[i].Contains(s))
                                {
                                    UseArr[i].Add(s);
                                    AddGlobal(s, t + "c"+ k, i);
                                }
                            }

                         
                        }


                    }
                    k++;
                }

            }
        }


        public SortedSet<int> SetUnion(SortedSet<int> s1, SortedSet<int> s2)
        {
            var s3 = new SortedSet<int> { };
            foreach (var item in s1)
            {
                s3.Add(item);
            }
            foreach (var item in s2)
            {
                if (!s3.Contains(item))
                {
                    s3.Add(item);
                }
            }
            return s3;
        }

        public SortedSet<string> SetUnion(SortedSet<string> s1, SortedSet<string> s2)
        {
            var s3 = new SortedSet<string> { };
            foreach (var item in s1)
            {
                s3.Add(item);
            }
            foreach (var item in s2)
            {
                if (!s3.Contains(item))
                {
                    s3.Add(item);
                }
            }
            return s3;
        }
        public SortedSet<int> DiffUnion(SortedSet<int> s1, SortedSet<int> s2)
        {
            var s3 = new SortedSet<int> { };
            foreach (var item in s1)
            {
              s3.Add(item);
            }
            foreach (var item in s2)
            {
                if (s3.Contains(item))
                {
                    s3.Remove(item);
                }
            }
            return s3;
        }
        public SortedSet<string> DiffUnion(SortedSet<string> s1, SortedSet<string> s2)
        {
            var s3 = new SortedSet<string> { };
            foreach (var item in s1)
            {
                s3.Add(item);
            }
            foreach (var item in s2)
            {
                if (s3.Contains(item))
                {
                    s3.Remove(item);
                }
            }
            return s3;
        }
        public void ReachingDefinitions(Graph g)
        {
             IN = new SortedSet<int>[ArrBlocks.Length+2];
             OUT = new SortedSet<int>[ArrBlocks.Length+2];
            SortedSet<int>[] TempOUT = new SortedSet<int>[ArrBlocks.Length + 2];
            for (int i = 0; i < ArrBlocks.Length+2; i++)
            {
                OUT[i] = new SortedSet<int> { };
            }
            while (TempOUT != OUT)
            {
                TempOUT = OUT;
                for (int i = 0; i < ArrBlocks.Length-1; i++)
                {
                    var s = new SortedSet<int> { };
                    IN[i] = new SortedSet<int> { };
                    OUT[i] = new SortedSet<int> { };
                    s = g.P(i);
                    foreach (var item in s)
                    {
                        if (item < 0)
                        {
                            continue;
                        }
                        IN[i] = SetUnion(IN[i], OUT[item]);
                    }
                    var t = DiffUnion(IN[i], KillArr[i]); 
                    OUT[i] = SetUnion(t, GenArr[i]);
                }
            }

        }

        public void LiveVariable(Graph g)
        {
            ActiveIN = new SortedSet<string>[ArrBlocks.Length + 2];
            ActiveOUT = new SortedSet<string>[ArrBlocks.Length + 2];
            SortedSet<string>[] TempOUT = new SortedSet<string>[ArrBlocks.Length + 2];
            for (int i = 0; i < ArrBlocks.Length + 2; i++)
            {
                ActiveIN[i] = new SortedSet<string> { };
            }
            while (TempOUT != ActiveIN)
            {
                TempOUT = ActiveIN;
                for (int i = ArrBlocks.Length - 2; i>=0 ; i--)
                {
                    var s = new SortedSet<int> { };
                    ActiveIN[i] = new SortedSet<string> { };
                    ActiveOUT[i] = new SortedSet<string> { };
                    s = g.S(i);
                    foreach (var item in s)
                    {
                        if (item < 0)
                        {
                            continue;
                        }
                        ActiveOUT[i] = SetUnion(ActiveOUT[i], ActiveIN[item]);
                    }
                    var t = DiffUnion(ActiveOUT[i], DefArr[i]);
                    ActiveIN[i] = SetUnion(t, UseArr[i]);
                }
            }

        }

        public void PrintGen()
        {
            Console.WriteLine("Gen");
            for (int i = 0; i < GenArr.Length; i++)
            {
                if (GenArr[i] == null)
                {
                    continue;
                }
                Console.Write(i + ": ");
                foreach (var x in GenArr[i])
                {
                    Console.Write(x + " ");
                }
                Console.WriteLine();
            }
        }
        public void PrintKill()
        {
            Console.WriteLine("Kill");
            for (int i = 0; i < KillArr.Length; i++)
            {
                if (KillArr[i] == null)
                {
                    continue;
                }
                Console.Write(i + ": ");
                foreach (var x in KillArr[i])
                {
                    Console.Write(x + " ");
                }
                Console.WriteLine();
            }
        }

        public void PrintDef()
        {
            Console.WriteLine("Def");
            for (int i = 0; i < DefArr.Length; i++)
            {
                if (DefArr[i] == null)
                {
                    continue;
                }
                Console.Write(i + ": ");
                foreach (var x in DefArr[i])
                {
                    Console.Write(x + " ");
                }
                Console.WriteLine();
            }
        }

        public void PrintUse()
        {
            Console.WriteLine("Use");
            for (int i = 0; i < UseArr.Length; i++)
            {
                if (UseArr[i] == null)
                {
                    continue;
                }
                Console.Write(i + ": ");
                foreach (var x in UseArr[i])
                {
                    Console.Write(x + " ");
                }
                Console.WriteLine();
            }
        }
        public void PrintIN()
        {
            Console.WriteLine("IN");
            for (int i = 0; i < IN.Length; i++)
            {
                if (IN[i] == null)
                {
                    continue;
                }
                Console.Write(i + ": ");
                foreach (var x in IN[i])
                {
                    Console.Write(x + " ");
                }
                Console.WriteLine();
            }
        }
        public void PrintOUT()
        {
            Console.WriteLine("OUT");
            for (int i = 0; i < OUT.Length; i++)
            {
                if (OUT[i] == null)
                {
                    continue;
                }
                Console.Write(i + ": ");
                foreach (var x in OUT[i])
                {
                    Console.Write(x + " ");
                }
                Console.WriteLine();
            }
        }

        public void PrintActiveIN()
        {
            Console.WriteLine("ActiveIN");
            for (int i = 0; i < ActiveIN.Length; i++)
            {
                if (ActiveIN[i] == null)
                {
                    continue;
                }
                Console.Write(i + ": ");
                foreach (var x in ActiveIN[i])
                {
                    Console.Write(x + " ");
                }
                Console.WriteLine();
            }
        }

        public void PrintActiveOUT()
        {
            Console.WriteLine("ActiveOUT");
            for (int i = 0; i < ActiveOUT.Length; i++)
            {
                if (ActiveOUT[i] == null)
                {
                    continue;
                }
                Console.Write(i + ": ");
                foreach (var x in ActiveOUT[i])
                {
                    Console.Write(x + " ");
                }
                Console.WriteLine();
            }
        }

        public void GetConstants()
        {
            Constants = new SortedSet<int>[ArrBlocks.Length];
            DefCon = new SortedSet<string>[ArrBlocks.Length];
            for (int i = 0; i < OUT.Length; i++)
            {
                if (OUT[i] != null)
                {
                    if (OUT[i].Count > 1)
                    {
                        Constants[i] = DiffUnion(OUT[i], GenArr[i]);
                        DefCon[i] = new SortedSet<string>();
                        var s = "";
                        foreach (var item in Constants[i])
                        {
                            unsafe 
                            {
                                switch (Commands[item].Types[0])
                                {
                                    case 'i':
                                        s = Convert.ToString((ulong)Commands[item].pia);
                                        break;
                                    case 'd':
                                        s = Convert.ToString((ulong)Commands[item].pda);
                                        break;
                                    case 'b':
                                        s = Convert.ToString((ulong)Commands[item].pba);
                                        break;
                                    default:
                                        break;
                                }
                            }
                            DefCon[i].Add(s);
                            Temporary.Add(item);

                        }
                    }

                }
            }
        }

        public void LiveGlobal()
        {
            for (int i = 0; i < GlobalUse.Length; i++)
            {
                if (ActiveIN[i] == null)
                {
                    continue;
                }
                var dict = GlobalUse[i];
                foreach (var x in ActiveIN[i])
                {
                    if (dict.ContainsKey(x))
                    {
                        foreach (var item in dict[x])
                        {
                            foreach (var num in Constants[i])
                            {
                                var s = "";
                                unsafe {
                                    switch (Commands[num].Types[0])
                                    {
                                        case 'i':
                                            s = Convert.ToString((ulong)Commands[num].pia);
                                            break;
                                        case 'd':
                                            s = Convert.ToString((ulong)Commands[num].pda);
                                            break;
                                        case 'b':
                                            s = Convert.ToString((ulong)Commands[num].pba);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                if (x == s)
                                {
                                    ReplaceGlobal(item,num);
                                }

                            }
     
                        }
                    }
                }
            }
        }

        public void ReplaceGlobal(string s, int n)
        {
            var command = Commands[n];
            c = n;
            int ind = int.Parse(s.Substring(2));
            unsafe
            {
                if (command.Count < 3)
                {
                    Redundant.Add(c);

                    if (command.Types[1] == '1' || command.Types[1] == '2' || command.Types[1] == '3')
                    {
                        if (s[0] == 'i')
                        {
                            if (s[1] == 'b')
                            {
                                *Commands[ind].pib = command.intVal;
                            }
                            if (s[1] == 'c')
                            {
                                Commands[ind].intVal = command.intVal;
                                Commands[ind].NumberOfCommand = 52;
                                //Commands[ind].Tok = Toks.pbaapiolvi;
                            }
                        }
                        if (s[0] == 'd')
                        {
                            if (s[1] == 'b')
                            {
                                *Commands[ind].pdb = command.doubleVal;
                            }
                            if (s[1] == 'c')
                            {
                                *Commands[ind].pdc = command.doubleVal;
                            }
                        }
                        if (s[0] == 'b')
                        {
                            if (s[1] == 'b')
                            {
                                *Commands[ind].pbb = command.boolVal;
                            }
                            if (s[1] == 'c')
                            {
                                *Commands[ind].pbc = command.boolVal;
                            }
                        }
                    }

                    else 
                    {
                        if (s[0] == 'i')
                        {
                            if (s[1] == 'b')
                            {
                                *Commands[ind].pib = *command.pib;
                            }
                            if (s[1] == 'c')
                            {
                                *Commands[ind].pic = *command.pib;
                            }
                        }
                        if (s[0] == 'd')
                        {
                            if (s[1] == 'b')
                            {
                                *Commands[ind].pdb = *command.pdb;
                            }
                            if (s[1] == 'c')
                            {
                                *Commands[ind].pdc = *command.pdb;
                            }
                        }
                        if (s[0] == 'b')
                        {
                            if (s[1] == 'b')
                            {
                                *Commands[ind].pbb = *command.pbb;
                            }
                            if (s[1] == 'c')
                            {
                                *Commands[ind].pbc = *command.pbb;
                            }
                        }
                    }
                }
            }
        }



        public void Preparing()
        {
            FindLeaders();
            if (ArrBlocks.Length <= 1)
            {
                Print();
                return;
            } 
            DefUse();
            int k = 0;
            while (Redundant.Count != 0)
            {
                foreach (var item in Redundant)
                {
                    DelCommand(item - k++);
                }
                Redundant.Clear();
                FindLeaders();
                DefUse();
            }
            ReplaceCopies();
            int temp = DelUseless();
            while (temp != 0)
            {
                FindLeaders();
                ReplaceCopies();
                temp = DelUseless();
            }
            Redundant.Clear();
            Temporary.Clear();
            UseFull.Clear();
            GenKill();
            var g = CreateGraph();

            ReachingDefinitions(g);

            GetConstants();

            GlobalDefUse();

            LiveVariable(g);

            LiveGlobal();
            temp = DelUseless();
            //Print();
            PrintOptimisedCommands();




        }
        public unsafe void PrintOptimisedCommands()
        {
            string StrOpt = "";
            for (int i = 0; i < Size; i++)
            {

                var command = Commands[i];
                switch (command.Tok)
                {
                    case Toks.printint:
                        break;
                    case Toks.printdouble:
                        StrOpt += "(" + i + ")" + "print \n";
                        break;
                    case Toks.printbool:
                        break;
                    case Toks.end:
                        StrOpt += "(" + i + ")"+ "end \n";
                        break;
                    case Toks.got:
                        StrOpt += "(" + i  + ")"+ "goto "+ command.Goto + "\n";
                        break;
                    case Toks.iff:
                        AddVal(Convert.ToString((ulong)command.pba));
                       
                        StrOpt += "(" + i + ")" + "if ("+ Values[Convert.ToString((ulong)command.pba)]  
                            + " == false) goto " + (command.Goto+1) + "\n";
                        break;
                    case Toks.empty:
                        StrOpt += "(" + i + ")" + "\n";
                        break;             
                    default:
                        StrOpt += "(" + i + ")";
                        switch (command.Types[0])
                        {
                            case 'i':
                                AddVal(Convert.ToString((ulong)command.pia));
                                StrOpt += Values[Convert.ToString((ulong)command.pia)];
                                break;
                            case 'd':
                                AddVal(Convert.ToString((ulong)command.pda));
                                StrOpt += Values[Convert.ToString((ulong)command.pda)];
                                break;
                            case 'b':
                                AddVal(Convert.ToString((ulong)command.pba));
                                StrOpt += Values[Convert.ToString((ulong)command.pba)];
                                break;
                            default:
                                break;
                        }
                        switch (command.Tok.ToString()[3])
                        {
                            case 'a':
                                StrOpt += " = ";
                                break;
                            case 'p':
                                StrOpt += " += ";
                                break;
                            case 'n':
                                StrOpt += " -=";
                                break;
                            case 'm':
                                StrOpt += " *= ";
                                break;
                            case 'd':
                                StrOpt += " /= ";
                                break;
                            default:
                                break;
                        }
                        switch (command.Types[1])
                        {
                            case 'i':
                                AddVal(Convert.ToString((ulong)command.pib));
                                StrOpt += Values[Convert.ToString((ulong)command.pib)];
                                break;
                            case 'd':
                                AddVal(Convert.ToString((ulong)command.pdb));
                                StrOpt += Values[Convert.ToString((ulong)command.pdb)];
                                break;
                            case 'b':
                                AddVal(Convert.ToString((ulong)command.pbb));
                                StrOpt += Values[Convert.ToString((ulong)command.pbb)];
                                break;
                            default:
                                break;
                        }
                        if (command.Count == 2)
                        {
                            StrOpt += "\n";
                            break;
                        }
                        switch (command.Tok.ToString()[7])
                        {
                            case 'p':
                                StrOpt += " + ";
                                break;
                            case 'n':
                                StrOpt += " -";
                                break;
                            case 'm':
                                StrOpt += " * ";
                                break;
                            case 'd':
                                StrOpt += " / ";
                                break;
                            case 'l':
                                StrOpt += " < ";
                                break;
                            case 'b':
                                StrOpt += " > ";
                                break;
                            case 'e':
                                StrOpt += " == ";
                                break;
                            case 'w':
                                StrOpt += " != ";
                                break;
                            default:
                                break;
                        }
                        switch (command.Types[2])
                        {
                            case 'i':
                                AddVal(Convert.ToString((ulong)command.pic));
                                StrOpt += Values[Convert.ToString((ulong)command.pic)];
                                break;
                            case 'd':
                                AddVal(Convert.ToString((ulong)command.pdc));
                                StrOpt += Values[Convert.ToString((ulong)command.pdc)];
                                break;
                            case 'b':
                                AddVal(Convert.ToString((ulong)command.pbc));
                                StrOpt += Values[Convert.ToString((ulong)command.pbc)];
                                break;
                            default:
                                break;
                        }
                        StrOpt += "\n";
                        break;
                }
            }
            Console.WriteLine(StrOpt);
        }

        public unsafe void RunCommands()
        {
            //Print();
            Preparing();
            Run r = new Run();
            r.Execute(Commands, Size);

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
                        unsafe { *command.pia = *command.pib + command.intVal;
                            AddVal(Convert.ToString((ulong)command.pia));
                            AddVal(Convert.ToString((ulong)command.pib));
                            StrCommands += Values[Convert.ToString((ulong)command.pia)] + " = "
                                + Values[Convert.ToString((ulong)command.pib)] + " + " +
                               command.intVal + "\n";
                        }

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
                        unsafe { *command.pba = *command.pib < command.intVal;
                            AddVal(Convert.ToString((ulong)command.pba));
                            AddVal(Convert.ToString((ulong)command.pib));
                            StrCommands += Values[Convert.ToString((ulong)command.pba)] + " = "
                                + Values[Convert.ToString((ulong)command.pib)] + " < " +
                                 command.intVal + "\n";
                        }
                        break; // bool = int < intVal
                    case 53:
                        unsafe { *command.pda += command.doubleVal / *command.pic;
                            AddVal(Convert.ToString((ulong)command.pda));
                            AddVal(Convert.ToString((ulong)command.pic));
                            StrCommands += Values[Convert.ToString((ulong)command.pda)] + " += "
                                + command.doubleVal + " / " 
                                + Values[Convert.ToString((ulong)command.pic)] + "\n";
                        }
                        break; // double += doubleVal / int
                    case 54:
                        unsafe { *command.pia += command.intVal;
                            AddVal(Convert.ToString((ulong)command.pia));
                            StrCommands += Values[Convert.ToString((ulong)command.pia)] + " += "+
                            command.intVal + "\n";
                        }
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
