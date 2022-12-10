using ProgramTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLang
{
    public abstract class VirtVisitor<T>
    {
        public virtual T VisitIdNode(IdNode id) { return (T)Convert.ChangeType(id, typeof(T)); }
        public virtual T VisitAssignNode(AssignNode a) { return (T)Convert.ChangeType(a, typeof(T)); }
        public virtual T VisitBinOpNode(BinOpNode binop) { return (T)Convert.ChangeType(binop, typeof(T)); }
        public virtual T VisitIfNode(IfNode i) { return (T)Convert.ChangeType(i, typeof(T)); }

    }
}
