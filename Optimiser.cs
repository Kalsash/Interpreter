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
                       
                        unsafe { *command.pia = command.intVal; }
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
                        unsafe { if (*command.pba == true) i = command.Goto - 2; }
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
                        Console.WriteLine(command.intVal);
                        break; // print(int)
                    case 18:
                        Console.WriteLine(command.doubleVal);
                        break; // print(double)
                    case 19:
                        Console.WriteLine(command.boolVal);
                        break; // print(int)
                    case 20:
                        Console.WriteLine(SymbolTable.mem[command.intVal].i);
                        break; // print(intvar)
                    case 21:
                        Console.WriteLine(SymbolTable.mem[command.intVal].d);
                        break; // print(doublevar)
                    case 22:
                        Console.WriteLine(SymbolTable.mem[command.intVal].b);
                        break; // print(boolvar)

                    default:
                        break;
                }
            }

        }


    }
}
