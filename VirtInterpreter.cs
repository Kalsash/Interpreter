using ProgramTree;
using SimpleLang.Visitors;
using SimpleParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLang
{
    internal class VirtInterpreter : Visitor<RunTimeValue>
    {
        Dictionary<string, RunTimeValue> Vars_Dict = VirtSymbolTable.Vars;
        public override RunTimeValue VisitIdNode(IdNode id)
        {
            if (Vars_Dict.ContainsKey(id.Name))
            {
                return Vars_Dict[id.Name];
            }
            return new RunTimeValue(0);
        }
        public override RunTimeValue VisitAssignNode(AssignNode a)
        {
            a.Id.Eval(this);
            var val = a.Expr.Eval(this);
            SymbolTable.SetValue(a.Id.Name, val);
            return new RunTimeValue(0);
        }

    }
}
