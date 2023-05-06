using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleParser;
namespace SimpleLang
{
    internal class Run
    {
        public Run()
        {

        }
        unsafe public void Execute(ThreeAddress[] Commands, int Size)
        {
            Console.WriteLine("Выполнение программы");
            for (int i = 0; i < Size; i++)
            {
                var command = Commands[i];
               // Console.WriteLine(command.token);
                switch (command.NumberOfCommand)
                {
                    case 0:
                        i = Size;
                        break; // stop                
                    case 12:
                        i = command.Goto - 2;
                        break; // goto
                    case 13:
                        unsafe { if (*command.pba == true) i = command.Goto - 2; }
                        break; // if                 
                    case 17:
                        unsafe { Console.WriteLine(*command.pia); }
                        break; // print(int)
                    case 18:
                        unsafe { Console.WriteLine(*command.pda); }
                        break; // print(double)
                    case 19:
                        unsafe { Console.WriteLine(*command.pba); }
                        break; // print(int)
                    case 22:
                        unsafe
                        {

                            if (*command.pba == false) i = command.Goto;
                        }
                        break; // if
                    case 23:
                        i = command.Goto;
                        break;

                    default:
                        switch (command.Tok)
                        {
                            case Toks.pbaapb:
                                *command.pba = *command.pbb;
                                break;
                            case Toks.pbaapboapb:
                                *command.pba = *command.pbb && *command.pbc;
                                break;
                            case Toks.pbaapboavb:
                                *command.pba = *command.pbb && command.boolVal;
                                break;
                            case Toks.pbaapboopb:
                                *command.pba = *command.pbb || *command.pbc;
                                break;
                            case Toks.pbaapboovb:
                                *command.pba = *command.pbb || command.boolVal;
                                break;
                            case Toks.pbaapdobpd:
                                *command.pba = *command.pdb > *command.pdc;
                                break;
                            case Toks.pbaapdobpi:
                                *command.pba = *command.pdb > *command.pic;
                                break;
                            case Toks.pbaapdobvd:
                                *command.pba = *command.pdb > command.doubleVal;
                                break;
                            case Toks.pbaapdobvi:
                                *command.pba = *command.pdb > command.intVal;
                                break;
                            case Toks.pbaapdoepd:
                                *command.pba = *command.pdb == *command.pdc;
                                break;
                            case Toks.pbaapdoepi:
                                *command.pba = *command.pdb == *command.pic;
                                break;
                            case Toks.pbaapdoevd:
                                *command.pba = *command.pdb == command.doubleVal;
                                break;
                            case Toks.pbaapdoevi:
                                *command.pba = *command.pdb == command.intVal;
                                break;
                            case Toks.pbaapdolpd:
                                *command.pba = *command.pdb < *command.pdc;
                                break;
                            case Toks.pbaapdolpi:
                                *command.pba = *command.pdb < *command.pic;
                                break;
                            case Toks.pbaapdolvd:
                                *command.pba = *command.pdb < command.doubleVal;
                                break;
                            case Toks.pbaapdolvi:
                                *command.pba = *command.pdb < command.intVal;
                                break;
                            case Toks.pbaapdowpd:
                                *command.pba = *command.pdb != *command.pdc;
                                break;
                            case Toks.pbaapdowpi:
                                *command.pba = *command.pdb != *command.pic;
                                break;
                            case Toks.pbaapdowvd:
                                *command.pba = *command.pdb != command.doubleVal;
                                break;
                            case Toks.pbaapdowvi:
                                *command.pba = *command.pdb != command.intVal;
                                break;
                            case Toks.pbaapiobpd:
                                *command.pba = *command.pib > *command.pdc;
                                break;
                            case Toks.pbaapiobpi:
                                *command.pba = *command.pib > *command.pic;
                                break;
                            case Toks.pbaapiobvd:
                                *command.pba = *command.pib > command.doubleVal;
                                break;
                            case Toks.pbaapiobvi:
                                *command.pba = *command.pib > command.intVal;
                                break;
                            case Toks.pbaapioepd:
                                *command.pba = *command.pib == *command.pdc;
                                break;
                            case Toks.pbaapioepi:
                                *command.pba = *command.pib == *command.pic;
                                break;
                            case Toks.pbaapioevd:
                                *command.pba = *command.pib == command.doubleVal;
                                break;
                            case Toks.pbaapioevi:
                                *command.pba = *command.pib == command.intVal;
                                break;
                            case Toks.pbaapiolpd:
                                *command.pba = *command.pib < *command.pdc;
                                break;
                            case Toks.pbaapiolpi:
                                *command.pba = *command.pib < *command.pic;
                                break;
                            case Toks.pbaapiolvd:
                                *command.pba = *command.pib < command.doubleVal;
                                break;
                            case Toks.pbaapiolvi:
                                *command.pba = *command.pib < command.intVal;
                                break;
                            case Toks.pbaapiowpd:
                                *command.pba = *command.pib != *command.pdc;
                                break;
                            case Toks.pbaapiowpi:
                                *command.pba = *command.pib != *command.pic;
                                break;
                            case Toks.pbaapiowvd:
                                *command.pba = *command.pib != command.doubleVal;
                                break;
                            case Toks.pbaapiowvi:
                                *command.pba = *command.pib != command.intVal;
                                break;
                            case Toks.pbaavb:
                                *command.pba = command.boolVal;
                                break;
                            case Toks.pbaavboapb:
                                *command.pba = command.boolVal && *command.pbc;
                                break;
                            case Toks.pbaavboavb:
                                *command.pba = command.boolVal && command.boolVal;
                                break;
                            case Toks.pbaavboopb:
                                *command.pba = command.boolVal || *command.pbc;
                                break;
                            case Toks.pbaavboovb:
                                *command.pba = command.boolVal || command.boolVal;
                                break;
                            case Toks.pbaavdobpd:
                                *command.pba = command.doubleVal > *command.pdc;
                                break;
                            case Toks.pbaavdobpi:
                                *command.pba = command.doubleVal > *command.pic;
                                break;
                            case Toks.pbaavdobvd:
                                *command.pba = command.doubleVal > command.doubleVal;
                                break;
                            case Toks.pbaavdobvi:
                                *command.pba = command.doubleVal > command.intVal;
                                break;
                            case Toks.pbaavdoepd:
                                *command.pba = command.doubleVal == *command.pdc;
                                break;
                            case Toks.pbaavdoepi:
                                *command.pba = command.doubleVal == *command.pic;
                                break;
                            case Toks.pbaavdoevd:
                                *command.pba = command.doubleVal == command.doubleVal;
                                break;
                            case Toks.pbaavdoevi:
                                *command.pba = command.doubleVal == command.intVal;
                                break;
                            case Toks.pbaavdolpd:
                                *command.pba = command.doubleVal < *command.pdc;
                                break;
                            case Toks.pbaavdolpi:
                                *command.pba = command.doubleVal < *command.pic;
                                break;
                            case Toks.pbaavdolvd:
                                *command.pba = command.doubleVal < command.doubleVal;
                                break;
                            case Toks.pbaavdolvi:
                                *command.pba = command.doubleVal < command.intVal;
                                break;
                            case Toks.pbaavdowpd:
                                *command.pba = command.doubleVal != *command.pdc;
                                break;
                            case Toks.pbaavdowpi:
                                *command.pba = command.doubleVal != *command.pic;
                                break;
                            case Toks.pbaavdowvd:
                                *command.pba = command.doubleVal != command.doubleVal;
                                break;
                            case Toks.pbaavdowvi:
                                *command.pba = command.doubleVal != command.intVal;
                                break;
                            case Toks.pbaaviobpd:
                                *command.pba = command.intVal > *command.pdc;
                                break;
                            case Toks.pbaaviobpi:
                                *command.pba = command.intVal > *command.pic;
                                break;
                            case Toks.pbaaviobvd:
                                *command.pba = command.intVal > command.doubleVal;
                                break;
                            case Toks.pbaaviobvi:
                                *command.pba = command.intVal > command.intVal;
                                break;
                            case Toks.pbaavioepd:
                                *command.pba = command.intVal == *command.pdc;
                                break;
                            case Toks.pbaavioepi:
                                *command.pba = command.intVal == *command.pic;
                                break;
                            case Toks.pbaavioevd:
                                *command.pba = command.intVal == command.doubleVal;
                                break;
                            case Toks.pbaavioevi:
                                *command.pba = command.intVal == command.intVal;
                                break;
                            case Toks.pbaaviolpd:
                                *command.pba = command.intVal < *command.pdc;
                                break;
                            case Toks.pbaaviolpi:
                                *command.pba = command.intVal < *command.pic;
                                break;
                            case Toks.pbaaviolvd:
                                *command.pba = command.intVal < command.doubleVal;
                                break;
                            case Toks.pbaaviolvi:
                                *command.pba = command.intVal < command.intVal;
                                break;
                            case Toks.pbaaviowpd:
                                *command.pba = command.intVal != *command.pdc;
                                break;
                            case Toks.pbaaviowpi:
                                *command.pba = command.intVal != *command.pic;
                                break;
                            case Toks.pbaaviowvd:
                                *command.pba = command.intVal != command.doubleVal;
                                break;
                            case Toks.pbaaviowvi:
                                *command.pba = command.intVal != command.intVal;
                                break;
                            case Toks.pdaapd:
                                *command.pda = *command.pdb;
                                break;
                            case Toks.pdaapdodpd:
                                *command.pda = *command.pdb / *command.pdc;
                                break;
                            case Toks.pdaapdodpi:
                                *command.pda = *command.pdb / *command.pic;
                                break;
                            case Toks.pdaapdodvd:
                                *command.pda = *command.pdb / command.doubleVal;
                                break;
                            case Toks.pdaapdodvi:
                                *command.pda = *command.pdb / command.intVal;
                                break;
                            case Toks.pdaapdompd:
                                *command.pda = *command.pdb * *command.pdc;
                                break;
                            case Toks.pdaapdompi:
                                *command.pda = *command.pdb * *command.pic;
                                break;
                            case Toks.pdaapdomvd:
                                *command.pda = *command.pdb * command.doubleVal;
                                break;
                            case Toks.pdaapdomvi:
                                *command.pda = *command.pdb * command.intVal;
                                break;
                            case Toks.pdaapdonpd:
                                *command.pda = *command.pdb - *command.pdc;
                                break;
                            case Toks.pdaapdonpi:
                                *command.pda = *command.pdb - *command.pic;
                                break;
                            case Toks.pdaapdonvd:
                                *command.pda = *command.pdb - command.doubleVal;
                                break;
                            case Toks.pdaapdonvi:
                                *command.pda = *command.pdb - command.intVal;
                                break;
                            case Toks.pdaapdoppd:
                                *command.pda = *command.pdb + *command.pdc;
                                break;
                            case Toks.pdaapdoppi:
                                *command.pda = *command.pdb + *command.pic;
                                break;
                            case Toks.pdaapdopvd:
                                *command.pda = *command.pdb + command.doubleVal;
                                break;
                            case Toks.pdaapdopvi:
                                *command.pda = *command.pdb + command.intVal;
                                break;
                            case Toks.pdaapi:
                                *command.pda = *command.pib;
                                break;
                            case Toks.pdaapiodpd:
                                *command.pda = *command.pib / *command.pdc;
                                break;
                            case Toks.pdaapiodpi:
                                *command.pda = *command.pib / *command.pic;
                                break;
                            case Toks.pdaapiodvd:
                                *command.pda = *command.pib / command.doubleVal;
                                break;
                            case Toks.pdaapiodvi:
                                *command.pda = *command.pib / command.intVal;
                                break;
                            case Toks.pdaapiompd:
                                *command.pda = *command.pib * *command.pdc;
                                break;
                            case Toks.pdaapiompi:
                                *command.pda = *command.pib * *command.pic;
                                break;
                            case Toks.pdaapiomvd:
                                *command.pda = *command.pib * command.doubleVal;
                                break;
                            case Toks.pdaapiomvi:
                                *command.pda = *command.pib * command.intVal;
                                break;
                            case Toks.pdaapionpd:
                                *command.pda = *command.pib - *command.pdc;
                                break;
                            case Toks.pdaapionpi:
                                *command.pda = *command.pib - *command.pic;
                                break;
                            case Toks.pdaapionvd:
                                *command.pda = *command.pib - command.doubleVal;
                                break;
                            case Toks.pdaapionvi:
                                *command.pda = *command.pib - command.intVal;
                                break;
                            case Toks.pdaapioppd:
                                *command.pda = *command.pib + *command.pdc;
                                break;
                            case Toks.pdaapioppi:
                                *command.pda = *command.pib + *command.pic;
                                break;
                            case Toks.pdaapiopvd:
                                *command.pda = *command.pib + command.doubleVal;
                                break;
                            case Toks.pdaapiopvi:
                                *command.pda = *command.pib + command.intVal;
                                break;
                            case Toks.pdaavd:
                                *command.pda = command.doubleVal;
                                break;
                            case Toks.pdaavdodpd:
                                *command.pda = command.doubleVal / *command.pdc;
                                break;
                            case Toks.pdaavdodpi:
                                *command.pda = command.doubleVal / *command.pic;
                                break;
                            case Toks.pdaavdodvd:
                                *command.pda = command.doubleVal / command.doubleVal;
                                break;
                            case Toks.pdaavdodvi:
                                *command.pda = command.doubleVal / command.intVal;
                                break;
                            case Toks.pdaavdompd:
                                *command.pda = command.doubleVal * *command.pdc;
                                break;
                            case Toks.pdaavdompi:
                                *command.pda = command.doubleVal * *command.pic;
                                break;
                            case Toks.pdaavdomvd:
                                *command.pda = command.doubleVal * command.doubleVal;
                                break;
                            case Toks.pdaavdomvi:
                                *command.pda = command.doubleVal * command.intVal;
                                break;
                            case Toks.pdaavdonpd:
                                *command.pda = command.doubleVal - *command.pdc;
                                break;
                            case Toks.pdaavdonpi:
                                *command.pda = command.doubleVal - *command.pic;
                                break;
                            case Toks.pdaavdonvd:
                                *command.pda = command.doubleVal - command.doubleVal;
                                break;
                            case Toks.pdaavdonvi:
                                *command.pda = command.doubleVal - command.intVal;
                                break;
                            case Toks.pdaavdoppd:
                                *command.pda = command.doubleVal + *command.pdc;
                                break;
                            case Toks.pdaavdoppi:
                                *command.pda = command.doubleVal + *command.pic;
                                break;
                            case Toks.pdaavdopvd:
                                *command.pda = command.doubleVal + command.doubleVal;
                                break;
                            case Toks.pdaavdopvi:
                                *command.pda = command.doubleVal + command.intVal;
                                break;
                            case Toks.pdaavi:
                                *command.pda = command.intVal;
                                break;
                            case Toks.pdaaviodpd:
                                *command.pda = command.intVal / *command.pdc;
                                break;
                            case Toks.pdaaviodpi:
                                *command.pda = command.intVal / *command.pic;
                                break;
                            case Toks.pdaaviodvd:
                                *command.pda = command.intVal / command.doubleVal;
                                break;
                            case Toks.pdaaviodvi:
                                *command.pda = command.intVal / command.intVal;
                                break;
                            case Toks.pdaaviompd:
                                *command.pda = command.intVal * *command.pdc;
                                break;
                            case Toks.pdaaviompi:
                                *command.pda = command.intVal * *command.pic;
                                break;
                            case Toks.pdaaviomvd:
                                *command.pda = command.intVal * command.doubleVal;
                                break;
                            case Toks.pdaaviomvi:
                                *command.pda = command.intVal * command.intVal;
                                break;
                            case Toks.pdaavionpd:
                                *command.pda = command.intVal - *command.pdc;
                                break;
                            case Toks.pdaavionpi:
                                *command.pda = command.intVal - *command.pic;
                                break;
                            case Toks.pdaavionvd:
                                *command.pda = command.intVal - command.doubleVal;
                                break;
                            case Toks.pdaavionvi:
                                *command.pda = command.intVal - command.intVal;
                                break;
                            case Toks.pdaavioppd:
                                *command.pda = command.intVal + *command.pdc;
                                break;
                            case Toks.pdaavioppi:
                                *command.pda = command.intVal + *command.pic;
                                break;
                            case Toks.pdaaviopvd:
                                *command.pda = command.intVal + command.doubleVal;
                                break;
                            case Toks.pdaaviopvi:
                                *command.pda = command.intVal + command.intVal;
                                break;
                            case Toks.pdadpd:
                                *command.pda /= *command.pdb;
                                break;
                            case Toks.pdadpdodpd:
                                *command.pda /= *command.pdb / *command.pdc;
                                break;
                            case Toks.pdadpdodpi:
                                *command.pda /= *command.pdb / *command.pic;
                                break;
                            case Toks.pdadpdodvd:
                                *command.pda /= *command.pdb / command.doubleVal;
                                break;
                            case Toks.pdadpdodvi:
                                *command.pda /= *command.pdb / command.intVal;
                                break;
                            case Toks.pdadpdompd:
                                *command.pda /= *command.pdb * *command.pdc;
                                break;
                            case Toks.pdadpdompi:
                                *command.pda /= *command.pdb * *command.pic;
                                break;
                            case Toks.pdadpdomvd:
                                *command.pda /= *command.pdb * command.doubleVal;
                                break;
                            case Toks.pdadpdomvi:
                                *command.pda /= *command.pdb * command.intVal;
                                break;
                            case Toks.pdadpdonpd:
                                *command.pda /= *command.pdb - *command.pdc;
                                break;
                            case Toks.pdadpdonpi:
                                *command.pda /= *command.pdb - *command.pic;
                                break;
                            case Toks.pdadpdonvd:
                                *command.pda /= *command.pdb - command.doubleVal;
                                break;
                            case Toks.pdadpdonvi:
                                *command.pda /= *command.pdb - command.intVal;
                                break;
                            case Toks.pdadpdoppd:
                                *command.pda /= *command.pdb + *command.pdc;
                                break;
                            case Toks.pdadpdoppi:
                                *command.pda /= *command.pdb + *command.pic;
                                break;
                            case Toks.pdadpdopvd:
                                *command.pda /= *command.pdb + command.doubleVal;
                                break;
                            case Toks.pdadpdopvi:
                                *command.pda /= *command.pdb + command.intVal;
                                break;
                            case Toks.pdadpi:
                                *command.pda /= *command.pib;
                                break;
                            case Toks.pdadpiodpd:
                                *command.pda /= *command.pib / *command.pdc;
                                break;
                            case Toks.pdadpiodpi:
                                *command.pda /= *command.pib / *command.pic;
                                break;
                            case Toks.pdadpiodvd:
                                *command.pda /= *command.pib / command.doubleVal;
                                break;
                            case Toks.pdadpiodvi:
                                *command.pda /= *command.pib / command.intVal;
                                break;
                            case Toks.pdadpiompd:
                                *command.pda /= *command.pib * *command.pdc;
                                break;
                            case Toks.pdadpiompi:
                                *command.pda /= *command.pib * *command.pic;
                                break;
                            case Toks.pdadpiomvd:
                                *command.pda /= *command.pib * command.doubleVal;
                                break;
                            case Toks.pdadpiomvi:
                                *command.pda /= *command.pib * command.intVal;
                                break;
                            case Toks.pdadpionpd:
                                *command.pda /= *command.pib - *command.pdc;
                                break;
                            case Toks.pdadpionpi:
                                *command.pda /= *command.pib - *command.pic;
                                break;
                            case Toks.pdadpionvd:
                                *command.pda /= *command.pib - command.doubleVal;
                                break;
                            case Toks.pdadpionvi:
                                *command.pda /= *command.pib - command.intVal;
                                break;
                            case Toks.pdadpioppd:
                                *command.pda /= *command.pib + *command.pdc;
                                break;
                            case Toks.pdadpioppi:
                                *command.pda /= *command.pib + *command.pic;
                                break;
                            case Toks.pdadpiopvd:
                                *command.pda /= *command.pib + command.doubleVal;
                                break;
                            case Toks.pdadpiopvi:
                                *command.pda /= *command.pib + command.intVal;
                                break;
                            case Toks.pdadvd:
                                *command.pda /= command.doubleVal;
                                break;
                            case Toks.pdadvdodpd:
                                *command.pda /= command.doubleVal / *command.pdc;
                                break;
                            case Toks.pdadvdodpi:
                                *command.pda /= command.doubleVal / *command.pic;
                                break;
                            case Toks.pdadvdodvd:
                                *command.pda /= command.doubleVal / command.doubleVal;
                                break;
                            case Toks.pdadvdodvi:
                                *command.pda /= command.doubleVal / command.intVal;
                                break;
                            case Toks.pdadvdompd:
                                *command.pda /= command.doubleVal * *command.pdc;
                                break;
                            case Toks.pdadvdompi:
                                *command.pda /= command.doubleVal * *command.pic;
                                break;
                            case Toks.pdadvdomvd:
                                *command.pda /= command.doubleVal * command.doubleVal;
                                break;
                            case Toks.pdadvdomvi:
                                *command.pda /= command.doubleVal * command.intVal;
                                break;
                            case Toks.pdadvdonpd:
                                *command.pda /= command.doubleVal - *command.pdc;
                                break;
                            case Toks.pdadvdonpi:
                                *command.pda /= command.doubleVal - *command.pic;
                                break;
                            case Toks.pdadvdonvd:
                                *command.pda /= command.doubleVal - command.doubleVal;
                                break;
                            case Toks.pdadvdonvi:
                                *command.pda /= command.doubleVal - command.intVal;
                                break;
                            case Toks.pdadvdoppd:
                                *command.pda /= command.doubleVal + *command.pdc;
                                break;
                            case Toks.pdadvdoppi:
                                *command.pda /= command.doubleVal + *command.pic;
                                break;
                            case Toks.pdadvdopvd:
                                *command.pda /= command.doubleVal + command.doubleVal;
                                break;
                            case Toks.pdadvdopvi:
                                *command.pda /= command.doubleVal + command.intVal;
                                break;
                            case Toks.pdadvi:
                                *command.pda /= command.intVal;
                                break;
                            case Toks.pdadviodpd:
                                *command.pda /= command.intVal / *command.pdc;
                                break;
                            case Toks.pdadviodpi:
                                *command.pda /= command.intVal / *command.pic;
                                break;
                            case Toks.pdadviodvd:
                                *command.pda /= command.intVal / command.doubleVal;
                                break;
                            case Toks.pdadviodvi:
                                *command.pda /= command.intVal / command.intVal;
                                break;
                            case Toks.pdadviompd:
                                *command.pda /= command.intVal * *command.pdc;
                                break;
                            case Toks.pdadviompi:
                                *command.pda /= command.intVal * *command.pic;
                                break;
                            case Toks.pdadviomvd:
                                *command.pda /= command.intVal * command.doubleVal;
                                break;
                            case Toks.pdadviomvi:
                                *command.pda /= command.intVal * command.intVal;
                                break;
                            case Toks.pdadvionpd:
                                *command.pda /= command.intVal - *command.pdc;
                                break;
                            case Toks.pdadvionpi:
                                *command.pda /= command.intVal - *command.pic;
                                break;
                            case Toks.pdadvionvd:
                                *command.pda /= command.intVal - command.doubleVal;
                                break;
                            case Toks.pdadvionvi:
                                *command.pda /= command.intVal - command.intVal;
                                break;
                            case Toks.pdadvioppd:
                                *command.pda /= command.intVal + *command.pdc;
                                break;
                            case Toks.pdadvioppi:
                                *command.pda /= command.intVal + *command.pic;
                                break;
                            case Toks.pdadviopvd:
                                *command.pda /= command.intVal + command.doubleVal;
                                break;
                            case Toks.pdadviopvi:
                                *command.pda /= command.intVal + command.intVal;
                                break;
                            case Toks.pdampd:
                                *command.pda *= *command.pdb;
                                break;
                            case Toks.pdampdodpd:
                                *command.pda *= *command.pdb / *command.pdc;
                                break;
                            case Toks.pdampdodpi:
                                *command.pda *= *command.pdb / *command.pic;
                                break;
                            case Toks.pdampdodvd:
                                *command.pda *= *command.pdb / command.doubleVal;
                                break;
                            case Toks.pdampdodvi:
                                *command.pda *= *command.pdb / command.intVal;
                                break;
                            case Toks.pdampdompd:
                                *command.pda *= *command.pdb * *command.pdc;
                                break;
                            case Toks.pdampdompi:
                                *command.pda *= *command.pdb * *command.pic;
                                break;
                            case Toks.pdampdomvd:
                                *command.pda *= *command.pdb * command.doubleVal;
                                break;
                            case Toks.pdampdomvi:
                                *command.pda *= *command.pdb * command.intVal;
                                break;
                            case Toks.pdampdonpd:
                                *command.pda *= *command.pdb - *command.pdc;
                                break;
                            case Toks.pdampdonpi:
                                *command.pda *= *command.pdb - *command.pic;
                                break;
                            case Toks.pdampdonvd:
                                *command.pda *= *command.pdb - command.doubleVal;
                                break;
                            case Toks.pdampdonvi:
                                *command.pda *= *command.pdb - command.intVal;
                                break;
                            case Toks.pdampdoppd:
                                *command.pda *= *command.pdb + *command.pdc;
                                break;
                            case Toks.pdampdoppi:
                                *command.pda *= *command.pdb + *command.pic;
                                break;
                            case Toks.pdampdopvd:
                                *command.pda *= *command.pdb + command.doubleVal;
                                break;
                            case Toks.pdampdopvi:
                                *command.pda *= *command.pdb + command.intVal;
                                break;
                            case Toks.pdampi:
                                *command.pda *= *command.pib;
                                break;
                            case Toks.pdampiodpd:
                                *command.pda *= *command.pib / *command.pdc;
                                break;
                            case Toks.pdampiodpi:
                                *command.pda *= *command.pib / *command.pic;
                                break;
                            case Toks.pdampiodvd:
                                *command.pda *= *command.pib / command.doubleVal;
                                break;
                            case Toks.pdampiodvi:
                                *command.pda *= *command.pib / command.intVal;
                                break;
                            case Toks.pdampiompd:
                                *command.pda *= *command.pib * *command.pdc;
                                break;
                            case Toks.pdampiompi:
                                *command.pda *= *command.pib * *command.pic;
                                break;
                            case Toks.pdampiomvd:
                                *command.pda *= *command.pib * command.doubleVal;
                                break;
                            case Toks.pdampiomvi:
                                *command.pda *= *command.pib * command.intVal;
                                break;
                            case Toks.pdampionpd:
                                *command.pda *= *command.pib - *command.pdc;
                                break;
                            case Toks.pdampionpi:
                                *command.pda *= *command.pib - *command.pic;
                                break;
                            case Toks.pdampionvd:
                                *command.pda *= *command.pib - command.doubleVal;
                                break;
                            case Toks.pdampionvi:
                                *command.pda *= *command.pib - command.intVal;
                                break;
                            case Toks.pdampioppd:
                                *command.pda *= *command.pib + *command.pdc;
                                break;
                            case Toks.pdampioppi:
                                *command.pda *= *command.pib + *command.pic;
                                break;
                            case Toks.pdampiopvd:
                                *command.pda *= *command.pib + command.doubleVal;
                                break;
                            case Toks.pdampiopvi:
                                *command.pda *= *command.pib + command.intVal;
                                break;
                            case Toks.pdamvd:
                                *command.pda *= command.doubleVal;
                                break;
                            case Toks.pdamvdodpd:
                                *command.pda *= command.doubleVal / *command.pdc;
                                break;
                            case Toks.pdamvdodpi:
                                *command.pda *= command.doubleVal / *command.pic;
                                break;
                            case Toks.pdamvdodvd:
                                *command.pda *= command.doubleVal / command.doubleVal;
                                break;
                            case Toks.pdamvdodvi:
                                *command.pda *= command.doubleVal / command.intVal;
                                break;
                            case Toks.pdamvdompd:
                                *command.pda *= command.doubleVal * *command.pdc;
                                break;
                            case Toks.pdamvdompi:
                                *command.pda *= command.doubleVal * *command.pic;
                                break;
                            case Toks.pdamvdomvd:
                                *command.pda *= command.doubleVal * command.doubleVal;
                                break;
                            case Toks.pdamvdomvi:
                                *command.pda *= command.doubleVal * command.intVal;
                                break;
                            case Toks.pdamvdonpd:
                                *command.pda *= command.doubleVal - *command.pdc;
                                break;
                            case Toks.pdamvdonpi:
                                *command.pda *= command.doubleVal - *command.pic;
                                break;
                            case Toks.pdamvdonvd:
                                *command.pda *= command.doubleVal - command.doubleVal;
                                break;
                            case Toks.pdamvdonvi:
                                *command.pda *= command.doubleVal - command.intVal;
                                break;
                            case Toks.pdamvdoppd:
                                *command.pda *= command.doubleVal + *command.pdc;
                                break;
                            case Toks.pdamvdoppi:
                                *command.pda *= command.doubleVal + *command.pic;
                                break;
                            case Toks.pdamvdopvd:
                                *command.pda *= command.doubleVal + command.doubleVal;
                                break;
                            case Toks.pdamvdopvi:
                                *command.pda *= command.doubleVal + command.intVal;
                                break;
                            case Toks.pdamvi:
                                *command.pda *= command.intVal;
                                break;
                            case Toks.pdamviodpd:
                                *command.pda *= command.intVal / *command.pdc;
                                break;
                            case Toks.pdamviodpi:
                                *command.pda *= command.intVal / *command.pic;
                                break;
                            case Toks.pdamviodvd:
                                *command.pda *= command.intVal / command.doubleVal;
                                break;
                            case Toks.pdamviodvi:
                                *command.pda *= command.intVal / command.intVal;
                                break;
                            case Toks.pdamviompd:
                                *command.pda *= command.intVal * *command.pdc;
                                break;
                            case Toks.pdamviompi:
                                *command.pda *= command.intVal * *command.pic;
                                break;
                            case Toks.pdamviomvd:
                                *command.pda *= command.intVal * command.doubleVal;
                                break;
                            case Toks.pdamviomvi:
                                *command.pda *= command.intVal * command.intVal;
                                break;
                            case Toks.pdamvionpd:
                                *command.pda *= command.intVal - *command.pdc;
                                break;
                            case Toks.pdamvionpi:
                                *command.pda *= command.intVal - *command.pic;
                                break;
                            case Toks.pdamvionvd:
                                *command.pda *= command.intVal - command.doubleVal;
                                break;
                            case Toks.pdamvionvi:
                                *command.pda *= command.intVal - command.intVal;
                                break;
                            case Toks.pdamvioppd:
                                *command.pda *= command.intVal + *command.pdc;
                                break;
                            case Toks.pdamvioppi:
                                *command.pda *= command.intVal + *command.pic;
                                break;
                            case Toks.pdamviopvd:
                                *command.pda *= command.intVal + command.doubleVal;
                                break;
                            case Toks.pdamviopvi:
                                *command.pda *= command.intVal + command.intVal;
                                break;
                            case Toks.pdanpd:
                                *command.pda -= *command.pdb;
                                break;
                            case Toks.pdanpdodpd:
                                *command.pda -= *command.pdb / *command.pdc;
                                break;
                            case Toks.pdanpdodpi:
                                *command.pda -= *command.pdb / *command.pic;
                                break;
                            case Toks.pdanpdodvd:
                                *command.pda -= *command.pdb / command.doubleVal;
                                break;
                            case Toks.pdanpdodvi:
                                *command.pda -= *command.pdb / command.intVal;
                                break;
                            case Toks.pdanpdompd:
                                *command.pda -= *command.pdb * *command.pdc;
                                break;
                            case Toks.pdanpdompi:
                                *command.pda -= *command.pdb * *command.pic;
                                break;
                            case Toks.pdanpdomvd:
                                *command.pda -= *command.pdb * command.doubleVal;
                                break;
                            case Toks.pdanpdomvi:
                                *command.pda -= *command.pdb * command.intVal;
                                break;
                            case Toks.pdanpdonpd:
                                *command.pda -= *command.pdb - *command.pdc;
                                break;
                            case Toks.pdanpdonpi:
                                *command.pda -= *command.pdb - *command.pic;
                                break;
                            case Toks.pdanpdonvd:
                                *command.pda -= *command.pdb - command.doubleVal;
                                break;
                            case Toks.pdanpdonvi:
                                *command.pda -= *command.pdb - command.intVal;
                                break;
                            case Toks.pdanpdoppd:
                                *command.pda -= *command.pdb + *command.pdc;
                                break;
                            case Toks.pdanpdoppi:
                                *command.pda -= *command.pdb + *command.pic;
                                break;
                            case Toks.pdanpdopvd:
                                *command.pda -= *command.pdb + command.doubleVal;
                                break;
                            case Toks.pdanpdopvi:
                                *command.pda -= *command.pdb + command.intVal;
                                break;
                            case Toks.pdanpi:
                                *command.pda -= *command.pib;
                                break;
                            case Toks.pdanpiodpd:
                                *command.pda -= *command.pib / *command.pdc;
                                break;
                            case Toks.pdanpiodpi:
                                *command.pda -= *command.pib / *command.pic;
                                break;
                            case Toks.pdanpiodvd:
                                *command.pda -= *command.pib / command.doubleVal;
                                break;
                            case Toks.pdanpiodvi:
                                *command.pda -= *command.pib / command.intVal;
                                break;
                            case Toks.pdanpiompd:
                                *command.pda -= *command.pib * *command.pdc;
                                break;
                            case Toks.pdanpiompi:
                                *command.pda -= *command.pib * *command.pic;
                                break;
                            case Toks.pdanpiomvd:
                                *command.pda -= *command.pib * command.doubleVal;
                                break;
                            case Toks.pdanpiomvi:
                                *command.pda -= *command.pib * command.intVal;
                                break;
                            case Toks.pdanpionpd:
                                *command.pda -= *command.pib - *command.pdc;
                                break;
                            case Toks.pdanpionpi:
                                *command.pda -= *command.pib - *command.pic;
                                break;
                            case Toks.pdanpionvd:
                                *command.pda -= *command.pib - command.doubleVal;
                                break;
                            case Toks.pdanpionvi:
                                *command.pda -= *command.pib - command.intVal;
                                break;
                            case Toks.pdanpioppd:
                                *command.pda -= *command.pib + *command.pdc;
                                break;
                            case Toks.pdanpioppi:
                                *command.pda -= *command.pib + *command.pic;
                                break;
                            case Toks.pdanpiopvd:
                                *command.pda -= *command.pib + command.doubleVal;
                                break;
                            case Toks.pdanpiopvi:
                                *command.pda -= *command.pib + command.intVal;
                                break;
                            case Toks.pdanvd:
                                *command.pda -= command.doubleVal;
                                break;
                            case Toks.pdanvdodpd:
                                *command.pda -= command.doubleVal / *command.pdc;
                                break;
                            case Toks.pdanvdodpi:
                                *command.pda -= command.doubleVal / *command.pic;
                                break;
                            case Toks.pdanvdodvd:
                                *command.pda -= command.doubleVal / command.doubleVal;
                                break;
                            case Toks.pdanvdodvi:
                                *command.pda -= command.doubleVal / command.intVal;
                                break;
                            case Toks.pdanvdompd:
                                *command.pda -= command.doubleVal * *command.pdc;
                                break;
                            case Toks.pdanvdompi:
                                *command.pda -= command.doubleVal * *command.pic;
                                break;
                            case Toks.pdanvdomvd:
                                *command.pda -= command.doubleVal * command.doubleVal;
                                break;
                            case Toks.pdanvdomvi:
                                *command.pda -= command.doubleVal * command.intVal;
                                break;
                            case Toks.pdanvdonpd:
                                *command.pda -= command.doubleVal - *command.pdc;
                                break;
                            case Toks.pdanvdonpi:
                                *command.pda -= command.doubleVal - *command.pic;
                                break;
                            case Toks.pdanvdonvd:
                                *command.pda -= command.doubleVal - command.doubleVal;
                                break;
                            case Toks.pdanvdonvi:
                                *command.pda -= command.doubleVal - command.intVal;
                                break;
                            case Toks.pdanvdoppd:
                                *command.pda -= command.doubleVal + *command.pdc;
                                break;
                            case Toks.pdanvdoppi:
                                *command.pda -= command.doubleVal + *command.pic;
                                break;
                            case Toks.pdanvdopvd:
                                *command.pda -= command.doubleVal + command.doubleVal;
                                break;
                            case Toks.pdanvdopvi:
                                *command.pda -= command.doubleVal + command.intVal;
                                break;
                            case Toks.pdanvi:
                                *command.pda -= command.intVal;
                                break;
                            case Toks.pdanviodpd:
                                *command.pda -= command.intVal / *command.pdc;
                                break;
                            case Toks.pdanviodpi:
                                *command.pda -= command.intVal / *command.pic;
                                break;
                            case Toks.pdanviodvd:
                                *command.pda -= command.intVal / command.doubleVal;
                                break;
                            case Toks.pdanviodvi:
                                *command.pda -= command.intVal / command.intVal;
                                break;
                            case Toks.pdanviompd:
                                *command.pda -= command.intVal * *command.pdc;
                                break;
                            case Toks.pdanviompi:
                                *command.pda -= command.intVal * *command.pic;
                                break;
                            case Toks.pdanviomvd:
                                *command.pda -= command.intVal * command.doubleVal;
                                break;
                            case Toks.pdanviomvi:
                                *command.pda -= command.intVal * command.intVal;
                                break;
                            case Toks.pdanvionpd:
                                *command.pda -= command.intVal - *command.pdc;
                                break;
                            case Toks.pdanvionpi:
                                *command.pda -= command.intVal - *command.pic;
                                break;
                            case Toks.pdanvionvd:
                                *command.pda -= command.intVal - command.doubleVal;
                                break;
                            case Toks.pdanvionvi:
                                *command.pda -= command.intVal - command.intVal;
                                break;
                            case Toks.pdanvioppd:
                                *command.pda -= command.intVal + *command.pdc;
                                break;
                            case Toks.pdanvioppi:
                                *command.pda -= command.intVal + *command.pic;
                                break;
                            case Toks.pdanviopvd:
                                *command.pda -= command.intVal + command.doubleVal;
                                break;
                            case Toks.pdanviopvi:
                                *command.pda -= command.intVal + command.intVal;
                                break;
                            case Toks.pdappd:
                                *command.pda += *command.pdb;
                                break;
                            case Toks.pdappdodpd:
                                *command.pda += *command.pdb / *command.pdc;
                                break;
                            case Toks.pdappdodpi:
                                *command.pda += *command.pdb / *command.pic;
                                break;
                            case Toks.pdappdodvd:
                                *command.pda += *command.pdb / command.doubleVal;
                                break;
                            case Toks.pdappdodvi:
                                *command.pda += *command.pdb / command.intVal;
                                break;
                            case Toks.pdappdompd:
                                *command.pda += *command.pdb * *command.pdc;
                                break;
                            case Toks.pdappdompi:
                                *command.pda += *command.pdb * *command.pic;
                                break;
                            case Toks.pdappdomvd:
                                *command.pda += *command.pdb * command.doubleVal;
                                break;
                            case Toks.pdappdomvi:
                                *command.pda += *command.pdb * command.intVal;
                                break;
                            case Toks.pdappdonpd:
                                *command.pda += *command.pdb - *command.pdc;
                                break;
                            case Toks.pdappdonpi:
                                *command.pda += *command.pdb - *command.pic;
                                break;
                            case Toks.pdappdonvd:
                                *command.pda += *command.pdb - command.doubleVal;
                                break;
                            case Toks.pdappdonvi:
                                *command.pda += *command.pdb - command.intVal;
                                break;
                            case Toks.pdappdoppd:
                                *command.pda += *command.pdb + *command.pdc;
                                break;
                            case Toks.pdappdoppi:
                                *command.pda += *command.pdb + *command.pic;
                                break;
                            case Toks.pdappdopvd:
                                *command.pda += *command.pdb + command.doubleVal;
                                break;
                            case Toks.pdappdopvi:
                                *command.pda += *command.pdb + command.intVal;
                                break;
                            case Toks.pdappi:
                                *command.pda += *command.pib;
                                break;
                            case Toks.pdappiodpd:
                                *command.pda += *command.pib / *command.pdc;
                                break;
                            case Toks.pdappiodpi:
                                *command.pda += *command.pib / *command.pic;
                                break;
                            case Toks.pdappiodvd:
                                *command.pda += *command.pib / command.doubleVal;
                                break;
                            case Toks.pdappiodvi:
                                *command.pda += *command.pib / command.intVal;
                                break;
                            case Toks.pdappiompd:
                                *command.pda += *command.pib * *command.pdc;
                                break;
                            case Toks.pdappiompi:
                                *command.pda += *command.pib * *command.pic;
                                break;
                            case Toks.pdappiomvd:
                                *command.pda += *command.pib * command.doubleVal;
                                break;
                            case Toks.pdappiomvi:
                                *command.pda += *command.pib * command.intVal;
                                break;
                            case Toks.pdappionpd:
                                *command.pda += *command.pib - *command.pdc;
                                break;
                            case Toks.pdappionpi:
                                *command.pda += *command.pib - *command.pic;
                                break;
                            case Toks.pdappionvd:
                                *command.pda += *command.pib - command.doubleVal;
                                break;
                            case Toks.pdappionvi:
                                *command.pda += *command.pib - command.intVal;
                                break;
                            case Toks.pdappioppd:
                                *command.pda += *command.pib + *command.pdc;
                                break;
                            case Toks.pdappioppi:
                                *command.pda += *command.pib + *command.pic;
                                break;
                            case Toks.pdappiopvd:
                                *command.pda += *command.pib + command.doubleVal;
                                break;
                            case Toks.pdappiopvi:
                                *command.pda += *command.pib + command.intVal;
                                break;
                            case Toks.pdapvd:
                                *command.pda += command.doubleVal;
                                break;
                            case Toks.pdapvdodpd:
                                *command.pda += command.doubleVal / *command.pdc;
                                break;
                            case Toks.pdapvdodpi:
                                *command.pda += command.doubleVal / *command.pic;
                                break;
                            case Toks.pdapvdodvd:
                                *command.pda += command.doubleVal / command.doubleVal;
                                break;
                            case Toks.pdapvdodvi:
                                *command.pda += command.doubleVal / command.intVal;
                                break;
                            case Toks.pdapvdompd:
                                *command.pda += command.doubleVal * *command.pdc;
                                break;
                            case Toks.pdapvdompi:
                                *command.pda += command.doubleVal * *command.pic;
                                break;
                            case Toks.pdapvdomvd:
                                *command.pda += command.doubleVal * command.doubleVal;
                                break;
                            case Toks.pdapvdomvi:
                                *command.pda += command.doubleVal * command.intVal;
                                break;
                            case Toks.pdapvdonpd:
                                *command.pda += command.doubleVal - *command.pdc;
                                break;
                            case Toks.pdapvdonpi:
                                *command.pda += command.doubleVal - *command.pic;
                                break;
                            case Toks.pdapvdonvd:
                                *command.pda += command.doubleVal - command.doubleVal;
                                break;
                            case Toks.pdapvdonvi:
                                *command.pda += command.doubleVal - command.intVal;
                                break;
                            case Toks.pdapvdoppd:
                                *command.pda += command.doubleVal + *command.pdc;
                                break;
                            case Toks.pdapvdoppi:
                                *command.pda += command.doubleVal + *command.pic;
                                break;
                            case Toks.pdapvdopvd:
                                *command.pda += command.doubleVal + command.doubleVal;
                                break;
                            case Toks.pdapvdopvi:
                                *command.pda += command.doubleVal + command.intVal;
                                break;
                            case Toks.pdapvi:
                                *command.pda += command.intVal;
                                break;
                            case Toks.pdapviodpd:
                                *command.pda += command.intVal / *command.pdc;
                                break;
                            case Toks.pdapviodpi:
                                *command.pda += command.intVal / *command.pic;
                                break;
                            case Toks.pdapviodvd:
                                *command.pda += command.intVal / command.doubleVal;
                                break;
                            case Toks.pdapviodvi:
                                *command.pda += command.intVal / command.intVal;
                                break;
                            case Toks.pdapviompd:
                                *command.pda += command.intVal * *command.pdc;
                                break;
                            case Toks.pdapviompi:
                                *command.pda += command.intVal * *command.pic;
                                break;
                            case Toks.pdapviomvd:
                                *command.pda += command.intVal * command.doubleVal;
                                break;
                            case Toks.pdapviomvi:
                                *command.pda += command.intVal * command.intVal;
                                break;
                            case Toks.pdapvionpd:
                                *command.pda += command.intVal - *command.pdc;
                                break;
                            case Toks.pdapvionpi:
                                *command.pda += command.intVal - *command.pic;
                                break;
                            case Toks.pdapvionvd:
                                *command.pda += command.intVal - command.doubleVal;
                                break;
                            case Toks.pdapvionvi:
                                *command.pda += command.intVal - command.intVal;
                                break;
                            case Toks.pdapvioppd:
                                *command.pda += command.intVal + *command.pdc;
                                break;
                            case Toks.pdapvioppi:
                                *command.pda += command.intVal + *command.pic;
                                break;
                            case Toks.pdapviopvd:
                                *command.pda += command.intVal + command.doubleVal;
                                break;
                            case Toks.pdapviopvi:
                                *command.pda += command.intVal + command.intVal;
                                break;
                            case Toks.piaapi:
                                *command.pia = *command.pib;
                                break;
                            case Toks.piaapiodpi:
                                *command.pia = *command.pib / *command.pic;
                                break;
                            case Toks.piaapiodvi:
                                *command.pia = *command.pib / command.intVal;
                                break;
                            case Toks.piaapiompi:
                                *command.pia = *command.pib * *command.pic;
                                break;
                            case Toks.piaapiomvi:
                                *command.pia = *command.pib * command.intVal;
                                break;
                            case Toks.piaapionpi:
                                *command.pia = *command.pib - *command.pic;
                                break;
                            case Toks.piaapionvi:
                                *command.pia = *command.pib - command.intVal;
                                break;
                            case Toks.piaapioppi:
                                *command.pia = *command.pib + *command.pic;
                                break;
                            case Toks.piaapiopvi:
                                *command.pia = *command.pib + command.intVal;
                                break;
                            case Toks.piaavi:
                                *command.pia = command.intVal;
                                break;
                            case Toks.piaaviodpi:
                                *command.pia = command.intVal / *command.pic;
                                break;
                            case Toks.piaaviodvi:
                                *command.pia = command.intVal / command.intVal;
                                break;
                            case Toks.piaaviompi:
                                *command.pia = command.intVal * *command.pic;
                                break;
                            case Toks.piaaviomvi:
                                *command.pia = command.intVal * command.intVal;
                                break;
                            case Toks.piaavionpi:
                                *command.pia = command.intVal - *command.pic;
                                break;
                            case Toks.piaavionvi:
                                *command.pia = command.intVal - command.intVal;
                                break;
                            case Toks.piaavioppi:
                                *command.pia = command.intVal + *command.pic;
                                break;
                            case Toks.piaaviopvi:
                                *command.pia = command.intVal + command.intVal;
                                break;
                            case Toks.piadpi:
                                *command.pia /= *command.pib;
                                break;
                            case Toks.piadpiodpi:
                                *command.pia /= *command.pib / *command.pic;
                                break;
                            case Toks.piadpiodvi:
                                *command.pia /= *command.pib / command.intVal;
                                break;
                            case Toks.piadpiompi:
                                *command.pia /= *command.pib * *command.pic;
                                break;
                            case Toks.piadpiomvi:
                                *command.pia /= *command.pib * command.intVal;
                                break;
                            case Toks.piadpionpi:
                                *command.pia /= *command.pib - *command.pic;
                                break;
                            case Toks.piadpionvi:
                                *command.pia /= *command.pib - command.intVal;
                                break;
                            case Toks.piadpioppi:
                                *command.pia /= *command.pib + *command.pic;
                                break;
                            case Toks.piadpiopvi:
                                *command.pia /= *command.pib + command.intVal;
                                break;
                            case Toks.piadvi:
                                *command.pia /= command.intVal;
                                break;
                            case Toks.piadviodpi:
                                *command.pia /= command.intVal / *command.pic;
                                break;
                            case Toks.piadviodvi:
                                *command.pia /= command.intVal / command.intVal;
                                break;
                            case Toks.piadviompi:
                                *command.pia /= command.intVal * *command.pic;
                                break;
                            case Toks.piadviomvi:
                                *command.pia /= command.intVal * command.intVal;
                                break;
                            case Toks.piadvionpi:
                                *command.pia /= command.intVal - *command.pic;
                                break;
                            case Toks.piadvionvi:
                                *command.pia /= command.intVal - command.intVal;
                                break;
                            case Toks.piadvioppi:
                                *command.pia /= command.intVal + *command.pic;
                                break;
                            case Toks.piadviopvi:
                                *command.pia /= command.intVal + command.intVal;
                                break;
                            case Toks.piampi:
                                *command.pia *= *command.pib;
                                break;
                            case Toks.piampiodpi:
                                *command.pia *= *command.pib / *command.pic;
                                break;
                            case Toks.piampiodvi:
                                *command.pia *= *command.pib / command.intVal;
                                break;
                            case Toks.piampiompi:
                                *command.pia *= *command.pib * *command.pic;
                                break;
                            case Toks.piampiomvi:
                                *command.pia *= *command.pib * command.intVal;
                                break;
                            case Toks.piampionpi:
                                *command.pia *= *command.pib - *command.pic;
                                break;
                            case Toks.piampionvi:
                                *command.pia *= *command.pib - command.intVal;
                                break;
                            case Toks.piampioppi:
                                *command.pia *= *command.pib + *command.pic;
                                break;
                            case Toks.piampiopvi:
                                *command.pia *= *command.pib + command.intVal;
                                break;
                            case Toks.piamvi:
                                *command.pia *= command.intVal;
                                break;
                            case Toks.piamviodpi:
                                *command.pia *= command.intVal / *command.pic;
                                break;
                            case Toks.piamviodvi:
                                *command.pia *= command.intVal / command.intVal;
                                break;
                            case Toks.piamviompi:
                                *command.pia *= command.intVal * *command.pic;
                                break;
                            case Toks.piamviomvi:
                                *command.pia *= command.intVal * command.intVal;
                                break;
                            case Toks.piamvionpi:
                                *command.pia *= command.intVal - *command.pic;
                                break;
                            case Toks.piamvionvi:
                                *command.pia *= command.intVal - command.intVal;
                                break;
                            case Toks.piamvioppi:
                                *command.pia *= command.intVal + *command.pic;
                                break;
                            case Toks.piamviopvi:
                                *command.pia *= command.intVal + command.intVal;
                                break;
                            case Toks.pianpi:
                                *command.pia -= *command.pib;
                                break;
                            case Toks.pianpiodpi:
                                *command.pia -= *command.pib / *command.pic;
                                break;
                            case Toks.pianpiodvi:
                                *command.pia -= *command.pib / command.intVal;
                                break;
                            case Toks.pianpiompi:
                                *command.pia -= *command.pib * *command.pic;
                                break;
                            case Toks.pianpiomvi:
                                *command.pia -= *command.pib * command.intVal;
                                break;
                            case Toks.pianpionpi:
                                *command.pia -= *command.pib - *command.pic;
                                break;
                            case Toks.pianpionvi:
                                *command.pia -= *command.pib - command.intVal;
                                break;
                            case Toks.pianpioppi:
                                *command.pia -= *command.pib + *command.pic;
                                break;
                            case Toks.pianpiopvi:
                                *command.pia -= *command.pib + command.intVal;
                                break;
                            case Toks.pianvi:
                                *command.pia -= command.intVal;
                                break;
                            case Toks.pianviodpi:
                                *command.pia -= command.intVal / *command.pic;
                                break;
                            case Toks.pianviodvi:
                                *command.pia -= command.intVal / command.intVal;
                                break;
                            case Toks.pianviompi:
                                *command.pia -= command.intVal * *command.pic;
                                break;
                            case Toks.pianviomvi:
                                *command.pia -= command.intVal * command.intVal;
                                break;
                            case Toks.pianvionpi:
                                *command.pia -= command.intVal - *command.pic;
                                break;
                            case Toks.pianvionvi:
                                *command.pia -= command.intVal - command.intVal;
                                break;
                            case Toks.pianvioppi:
                                *command.pia -= command.intVal + *command.pic;
                                break;
                            case Toks.pianviopvi:
                                *command.pia -= command.intVal + command.intVal;
                                break;
                            case Toks.piappi:
                                *command.pia += *command.pib;
                                break;
                            case Toks.piappiodpi:
                                *command.pia += *command.pib / *command.pic;
                                break;
                            case Toks.piappiodvi:
                                *command.pia += *command.pib / command.intVal;
                                break;
                            case Toks.piappiompi:
                                *command.pia += *command.pib * *command.pic;
                                break;
                            case Toks.piappiomvi:
                                *command.pia += *command.pib * command.intVal;
                                break;
                            case Toks.piappionpi:
                                *command.pia += *command.pib - *command.pic;
                                break;
                            case Toks.piappionvi:
                                *command.pia += *command.pib - command.intVal;
                                break;
                            case Toks.piappioppi:
                                *command.pia += *command.pib + *command.pic;
                                break;
                            case Toks.piappiopvi:
                                *command.pia += *command.pib + command.intVal;
                                break;
                            case Toks.piapvi:
                                *command.pia += command.intVal;
                                break;
                            case Toks.piapviodpi:
                                *command.pia += command.intVal / *command.pic;
                                break;
                            case Toks.piapviodvi:
                                *command.pia += command.intVal / command.intVal;
                                break;
                            case Toks.piapviompi:
                                *command.pia += command.intVal * *command.pic;
                                break;
                            case Toks.piapviomvi:
                                *command.pia += command.intVal * command.intVal;
                                break;
                            case Toks.piapvionpi:
                                *command.pia += command.intVal - *command.pic;
                                break;
                            case Toks.piapvionvi:
                                *command.pia += command.intVal - command.intVal;
                                break;
                            case Toks.piapvioppi:
                                *command.pia += command.intVal + *command.pic;
                                break;
                            case Toks.piapviopvi:
                                *command.pia += command.intVal + command.intVal;
                                break;
                        }

                        break;
                }
            }
        }
    }
}
