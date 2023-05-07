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
               // Console.WriteLine(command.Tok);
                switch (command.Tok)
                        {
                            case Toks.end:
                                i = Size;
                                break; // stop
                            case Toks.iff:
                                if (*command.pba == false) i = command.Goto;
                                break;
                            case Toks.got:
                                i = command.Goto;
                                break;
                            case Toks.pbaapb:
                                *command.pba = *command.pbb;
                                break;
                            case Toks.pbaapboapb:
                                *command.pba = *command.pbb && *command.pbc;
                                break;
                            case Toks.pbaapboopb:
                                *command.pba = *command.pbb || *command.pbc;
                                break;
                            case Toks.pbaapdobpd:
                                *command.pba = *command.pdb > *command.pdc;
                                break;
                            case Toks.pbaapdobpi:
                                *command.pba = *command.pdb > *command.pic;
                                break;
                            case Toks.pbaapdoepd:
                                *command.pba = *command.pdb == *command.pdc;
                                break;
                            case Toks.pbaapdoepi:
                                *command.pba = *command.pdb == *command.pic;
                                break;
                            case Toks.pbaapdolpd:
                                *command.pba = *command.pdb < *command.pdc;
                                break;
                            case Toks.pbaapdolpi:
                                *command.pba = *command.pdb < *command.pic;
                                break;
                            case Toks.pbaapdowpd:
                                *command.pba = *command.pdb != *command.pdc;
                                break;
                            case Toks.pbaapdowpi:
                                *command.pba = *command.pdb != *command.pic;
                                break;
                            case Toks.pbaapiobpd:
                                *command.pba = *command.pib > *command.pdc;
                                break;
                            case Toks.pbaapiobpi:
                                *command.pba = *command.pib > *command.pic;
                                break;
                            case Toks.pbaapioepd:
                                *command.pba = *command.pib == *command.pdc;
                                break;
                            case Toks.pbaapioepi:
                                *command.pba = *command.pib == *command.pic;
                                break;
                            case Toks.pbaapiolpd:
                                *command.pba = *command.pib < *command.pdc;
                                break;
                            case Toks.pbaapiolpi:
                                *command.pba = *command.pib < *command.pic;
                                break;
                            case Toks.pbaapiowpd:
                                *command.pba = *command.pib != *command.pdc;
                                break;
                            case Toks.pbaapiowpi:
                                *command.pba = *command.pib != *command.pic;
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
                            case Toks.pdaapdompd:
                                *command.pda = *command.pdb * *command.pdc;
                                break;
                            case Toks.pdaapdompi:
                                *command.pda = *command.pdb * *command.pic;
                                break;
                            case Toks.pdaapdonpd:
                                *command.pda = *command.pdb - *command.pdc;
                                break;
                            case Toks.pdaapdonpi:
                                *command.pda = *command.pdb - *command.pic;
                                break;
                            case Toks.pdaapdoppd:
                                *command.pda = *command.pdb + *command.pdc;
                                break;
                            case Toks.pdaapdoppi:
                                *command.pda = *command.pdb + *command.pic;
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
                            case Toks.pdaapiompd:
                                *command.pda = *command.pib * *command.pdc;
                                break;
                            case Toks.pdaapiompi:
                                *command.pda = *command.pib * *command.pic;
                                break;
                            case Toks.pdaapionpd:
                                *command.pda = *command.pib - *command.pdc;
                                break;
                            case Toks.pdaapionpi:
                                *command.pda = *command.pib - *command.pic;
                                break;
                            case Toks.pdaapioppd:
                                *command.pda = *command.pib + *command.pdc;
                                break;
                            case Toks.pdaapioppi:
                                *command.pda = *command.pib + *command.pic;
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
                            case Toks.pdadpdompd:
                                *command.pda /= *command.pdb * *command.pdc;
                                break;
                            case Toks.pdadpdompi:
                                *command.pda /= *command.pdb * *command.pic;
                                break;
                            case Toks.pdadpdonpd:
                                *command.pda /= *command.pdb - *command.pdc;
                                break;
                            case Toks.pdadpdonpi:
                                *command.pda /= *command.pdb - *command.pic;
                                break;
                            case Toks.pdadpdoppd:
                                *command.pda /= *command.pdb + *command.pdc;
                                break;
                            case Toks.pdadpdoppi:
                                *command.pda /= *command.pdb + *command.pic;
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
                            case Toks.pdadpiompd:
                                *command.pda /= *command.pib * *command.pdc;
                                break;
                            case Toks.pdadpiompi:
                                *command.pda /= *command.pib * *command.pic;
                                break;
                            case Toks.pdadpionpd:
                                *command.pda /= *command.pib - *command.pdc;
                                break;
                            case Toks.pdadpionpi:
                                *command.pda /= *command.pib - *command.pic;
                                break;
                            case Toks.pdadpioppd:
                                *command.pda /= *command.pib + *command.pdc;
                                break;
                            case Toks.pdadpioppi:
                                *command.pda /= *command.pib + *command.pic;
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
                            case Toks.pdampdompd:
                                *command.pda *= *command.pdb * *command.pdc;
                                break;
                            case Toks.pdampdompi:
                                *command.pda *= *command.pdb * *command.pic;
                                break;
                            case Toks.pdampdonpd:
                                *command.pda *= *command.pdb - *command.pdc;
                                break;
                            case Toks.pdampdonpi:
                                *command.pda *= *command.pdb - *command.pic;
                                break;
                            case Toks.pdampdoppd:
                                *command.pda *= *command.pdb + *command.pdc;
                                break;
                            case Toks.pdampdoppi:
                                *command.pda *= *command.pdb + *command.pic;
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
                            case Toks.pdampiompd:
                                *command.pda *= *command.pib * *command.pdc;
                                break;
                            case Toks.pdampiompi:
                                *command.pda *= *command.pib * *command.pic;
                                break;
                            case Toks.pdampionpd:
                                *command.pda *= *command.pib - *command.pdc;
                                break;
                            case Toks.pdampionpi:
                                *command.pda *= *command.pib - *command.pic;
                                break;
                            case Toks.pdampioppd:
                                *command.pda *= *command.pib + *command.pdc;
                                break;
                            case Toks.pdampioppi:
                                *command.pda *= *command.pib + *command.pic;
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
                            case Toks.pdanpdompd:
                                *command.pda -= *command.pdb * *command.pdc;
                                break;
                            case Toks.pdanpdompi:
                                *command.pda -= *command.pdb * *command.pic;
                                break;
                            case Toks.pdanpdonpd:
                                *command.pda -= *command.pdb - *command.pdc;
                                break;
                            case Toks.pdanpdonpi:
                                *command.pda -= *command.pdb - *command.pic;
                                break;
                            case Toks.pdanpdoppd:
                                *command.pda -= *command.pdb + *command.pdc;
                                break;
                            case Toks.pdanpdoppi:
                                *command.pda -= *command.pdb + *command.pic;
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
                            case Toks.pdanpiompd:
                                *command.pda -= *command.pib * *command.pdc;
                                break;
                            case Toks.pdanpiompi:
                                *command.pda -= *command.pib * *command.pic;
                                break;
                            case Toks.pdanpionpd:
                                *command.pda -= *command.pib - *command.pdc;
                                break;
                            case Toks.pdanpionpi:
                                *command.pda -= *command.pib - *command.pic;
                                break;
                            case Toks.pdanpioppd:
                                *command.pda -= *command.pib + *command.pdc;
                                break;
                            case Toks.pdanpioppi:
                                *command.pda -= *command.pib + *command.pic;
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
                            case Toks.pdappdompd:
                                *command.pda += *command.pdb * *command.pdc;
                                break;
                            case Toks.pdappdompi:
                                *command.pda += *command.pdb * *command.pic;
                                break;
                            case Toks.pdappdonpd:
                                *command.pda += *command.pdb - *command.pdc;
                                break;
                            case Toks.pdappdonpi:
                                *command.pda += *command.pdb - *command.pic;
                                break;
                            case Toks.pdappdoppd:
                                *command.pda += *command.pdb + *command.pdc;
                                break;
                            case Toks.pdappdoppi:
                                *command.pda += *command.pdb + *command.pic;
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
                            case Toks.pdappiompd:
                                *command.pda += *command.pib * *command.pdc;
                                break;
                            case Toks.pdappiompi:
                                *command.pda += *command.pib * *command.pic;
                                break;
                            case Toks.pdappionpd:
                                *command.pda += *command.pib - *command.pdc;
                                break;
                            case Toks.pdappionpi:
                                *command.pda += *command.pib - *command.pic;
                                break;
                            case Toks.pdappioppd:
                                *command.pda += *command.pib + *command.pdc;
                                break;
                            case Toks.pdappioppi:
                                *command.pda += *command.pib + *command.pic;
                                break;
                            case Toks.piaapi:
                                *command.pia = *command.pib;
                                break;
                            case Toks.piaapiodpi:
                                *command.pia = *command.pib / *command.pic;
                                break;
                            case Toks.piaapiompi:
                                *command.pia = *command.pib * *command.pic;
                                break;
                            case Toks.piaapionpi:
                                *command.pia = *command.pib - *command.pic;
                                break;
                            case Toks.piaapioppi:
                                *command.pia = *command.pib + *command.pic;
                                break;
                            case Toks.piadpi:
                                *command.pia /= *command.pib;
                                break;
                            case Toks.piadpiodpi:
                                *command.pia /= *command.pib / *command.pic;
                                break;
                            case Toks.piadpiompi:
                                *command.pia /= *command.pib * *command.pic;
                                break;
                            case Toks.piadpionpi:
                                *command.pia /= *command.pib - *command.pic;
                                break;
                            case Toks.piadpioppi:
                                *command.pia /= *command.pib + *command.pic;
                                break;
                            case Toks.piampi:
                                *command.pia *= *command.pib;
                                break;
                            case Toks.piampiodpi:
                                *command.pia *= *command.pib / *command.pic;
                                break;
                            case Toks.piampiompi:
                                *command.pia *= *command.pib * *command.pic;
                                break;
                            case Toks.piampionpi:
                                *command.pia *= *command.pib - *command.pic;
                                break;
                            case Toks.piampioppi:
                                *command.pia *= *command.pib + *command.pic;
                                break;
                            case Toks.pianpi:
                                *command.pia -= *command.pib;
                                break;
                            case Toks.pianpiodpi:
                                *command.pia -= *command.pib / *command.pic;
                                break;
                            case Toks.pianpiompi:
                                *command.pia -= *command.pib * *command.pic;
                                break;
                            case Toks.pianpionpi:
                                *command.pia -= *command.pib - *command.pic;
                                break;
                            case Toks.pianpioppi:
                                *command.pia -= *command.pib + *command.pic;
                                break;
                            case Toks.piappi:
                                *command.pia += *command.pib;
                                break;
                            case Toks.piappiodpi:
                                *command.pia += *command.pib / *command.pic;
                                break;
                            case Toks.piappiompi:
                                *command.pia += *command.pib * *command.pic;
                                break;
                            case Toks.piappionpi:
                                *command.pia += *command.pib - *command.pic;
                                break;
                            case Toks.piappioppi:
                                *command.pia += *command.pib + *command.pic;
                                break;
                        }
 
            }
        }
    }
}
