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
        public unsafe void RunCommands()
        {
            for (int i = 0; i < c; i++)
            {
                var command = Commands[i];
                switch (command.NumberOfCommand)
                {

                    case 0:
                        i = c;
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
                        unsafe { *command.pia = *command.pib + *command.pic; }
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
                        unsafe { *command.pba = *command.pib < *command.pic; }
                        break; // bool = int < int
                    case 21:
                        unsafe { *command.pda = *command.pdb / *command.pic; }
                        break; // double = doubleVal / int 
                    case 22:
                        unsafe { if (*command.pba == false) i = command.Goto; }
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

                    default:
                        break;
                }
            }

        }


    }
}
