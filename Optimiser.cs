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
        SortedSet<int> BasicBlocks = new SortedSet<int>();
        SortedSet<int> Redundant = new SortedSet<int>();





        public static Dictionary<string, string> Values = new Dictionary<string, string>();
        public static Dictionary<string, string> Vals = new Dictionary<string, string>();
        public int ValsCounter = 0;
        public string StrCommands = ""; 

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
            foreach (var command in Commands)
            {
                if (command.NumberOfCommand == 22 || command.NumberOfCommand == 23)
                {
                    command.Goto--;
                }
            }

               
          
        }
        public void AddVal(string s)
        {
            if (!Values.ContainsKey(s))
            {
                Values.Add(s, "val" + ++ValsCounter);
            }
        }
        public void AddV(string s, string flag)
        {
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
                        unsafe { *command.pia = command.intVal; }
                        break; // int = intVal
                    case 2:
                        unsafe { *command.pda = command.doubleVal; }
                        break; // double = doubleVal
                    case 3:
                        unsafe { *command.pba = command.boolVal; }
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
                        unsafe { *command.pda = *command.pib; }
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
                        unsafe { *command.pda = command.doubleVal / *command.pib; }
                        break; // double = doubleVal / int 
                    case 15:
                        unsafe { *command.pia = *command.pib + command.intVal; }

                        break; // int = int + intVal
                    case 16:
                        unsafe { *command.pba = *command.pib >= *command.pic; }
                        break; // bool = int >= int
                    case 17:
                        unsafe
                        {
                            Console.WriteLine(*command.pia);
                            AddVal(Convert.ToString((ulong)command.pia));
                            StrCommands += "print(" + Values[Convert.ToString((ulong)command.pia)] + ")" + "\n";
                        }
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
                            AddV(Convert.ToString((ulong)command.pba), "f");
                        }
                        break; // if
                    case 23:
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
                        unsafe
                        {
                            AddV(Convert.ToString((ulong)command.pia), "f");
                            AddV(Convert.ToString((ulong)command.pib), "tib" + c);
                        }
                        break; // int += int
                    case 51:
                        unsafe
                        {
                            AddV(Convert.ToString((ulong)command.pda), "fda");
                            AddV(Convert.ToString((ulong)command.pdb), "tdb" + c);
                        }
                        break; // double += double

                    default:
                        break;
                }

            }
        }
        public void DefUse(int[] Arr)
        {
            int end = Arr[1] - 1;
            for (int i = 0; i < Arr.Length - 1; i++)
            {
                CollectMarks(end, i, Arr);
                Vals.Clear();
            }
        }

        public void ReplaceCopies(int[] Arr)
        {
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
                    string s = "";
                    int ind = 0;

                
                    unsafe
                    {
           
                        if (Convert.ToString((ulong)command.pia) != "0")
                        {
                            s = Vals[Convert.ToString((ulong)command.pia)];
                        }

                        if (Convert.ToString((ulong)command.pda) != "0")
                        {
                            s = Vals[Convert.ToString((ulong)command.pda)];
                        }
          
                        if (Convert.ToString((ulong)command.pba) != "0")
                        {
                          
                            s = Vals[Convert.ToString((ulong)command.pba)];
                        }
                    }
                    //Console.WriteLine(s);
                    if (s[0] == 't' || s.Length <= 1)
                    {
                        continue;
                    }
                    ind = int.Parse(s.Substring(3));

                    unsafe
                    {
                        if (Convert.ToString((ulong)command.pic) == "0" && Convert.ToString((ulong)command.pdc) == "0"
                           && Convert.ToString((ulong)command.pbc) == "0")
                        {
                            Redundant.Add(c);
                        }
                        else
                            continue;
                        if (command.NumberOfCommand <= 3)
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
                }
                //foreach (var item in Redundant)
                //{
                //    Console.WriteLine(item);
                //}
                Vals.Clear();

            }
        }

        public void Preparing()
        {
            FindLeaders();
            int[] Arr = new int[BasicBlocks.Count()];
            int k = 0;
            foreach (var item in BasicBlocks)
            {
                Arr[k++] = item;
                // Console.WriteLine(item);
            }
            DefUse(Arr);
            while (Redundant.Count != 0)
            {
                foreach (var item in Redundant)
                {
                    DelCommand(item);
                }
                BasicBlocks.Clear();
                Redundant.Clear();
                FindLeaders();
                Arr = new int[BasicBlocks.Count()];
                k = 0;
                foreach (var item in BasicBlocks)
                {
                    Arr[k++] = item;
                    //Console.WriteLine(item);
                }
                DefUse(Arr);
            }
            ReplaceCopies(Arr);
            //Redundant.Remove(8);
            //Redundant.Remove(9);
            //Redundant.Remove(10);
           // Redundant.Remove(1);
            //foreach (var item in Redundant)
            //{
            //    Console.WriteLine(item);
            //    //DelCommand(item);
            //}
            DelCommand(1);


        }
        public unsafe void RunCommands()
        {
            //Preparing();
            //foreach (var item in Commands)
            //{
            //    Console.WriteLine(item.NumberOfCommand);
            //}
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
                        unsafe { *command.pia = command.intVal;}
                        break; // int = intVal
                    case 2:
                        unsafe {*command.pda = command.doubleVal;}
                        break; // double = doubleVal
                    case 3:
                        unsafe { *command.pba = command.boolVal; }
                        break; // bool = boolVal
                    case 4:
                        unsafe { *command.pia = *command.pib; }
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
                        {*command.pba = *command.pib < *command.pic;}
                        break; // bool = int < int
                    case 21:
                        unsafe {
                         
                            *command.pda = *command.pdb / *command.pic; }
                        break; // double = doubleVal / int 
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
                        unsafe { *command.pda += 1.0 / *command.pic; }
                        break; // double += doubleVal / int
                    case 54:
                        unsafe { *command.pia += command.intVal; }
                        break; // int += intVal

                    default:
                        break;
                }
            }

        }

        public unsafe void PrintCommands()
        {
            Preparing();
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
                        unsafe { *command.pia = command.intVal; }
                        break; // int = intVal
                    case 2:
                        unsafe { *command.pda = command.doubleVal; }
                        break; // double = doubleVal
                    case 3:
                        unsafe { *command.pba = command.boolVal; }
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
                        unsafe { *command.pda = command.doubleVal / *command.pib; }
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

                    default:
                        break;
                }
            }

        }
    }
}
