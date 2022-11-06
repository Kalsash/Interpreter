using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramTree;

namespace SimpleLang.Visitors
{
    public abstract class Visitor<T>
    {
        public virtual T VisitIdNode(IdNode id) { return (T)Convert.ChangeType(id, typeof(T)); }
        public virtual T VisitFuncNode(FuncNode f) { return (T)Convert.ChangeType(f, typeof(T)); }
        public virtual T VisitIntNumNode(IntNumNode num) { return (T)Convert.ChangeType(num, typeof(T)); }
        public virtual T VisitRealNumNode(RealNumNode num) { return (T)Convert.ChangeType(num, typeof(T)); }
        public virtual T VisitBinOpNode(BinOpNode binop) { return (T)Convert.ChangeType(binop, typeof(T)); }
        public virtual T VisitAssignNode(AssignNode a) { return (T)Convert.ChangeType(a, typeof(T)); }
        public virtual T VisitLoopNode(LoopNode l) { return (T)Convert.ChangeType(l, typeof(T)); }
        public virtual T VisitBlockNode(BlockNode bl) { return (T)Convert.ChangeType(bl, typeof(T)); }
        public virtual T VisitWhileNode(WhileNode w) { return (T)Convert.ChangeType(w, typeof(T)); }
        public virtual T VisitWriteNode(WriteNode w) { return (T)Convert.ChangeType(w, typeof(T)); }
    }
}
