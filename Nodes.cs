using ProgramTree;
using QUT.Gppg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SimpleLang
{
    public class Nodes
    {
        //public Dictionary<string, int> Id_Dict = new Dictionary<string, int>();
        //public int n = 0;
        //public virtual void GoToIdNode(IdNode id) 
        //{
        //    System.Console.WriteLine("Зашел в IdNode");
        //}
        //public virtual void GoToIntNumNode(IntNumNode num) 
        //{
        //    System.Console.WriteLine("Зашел в IntNumNode");
        //    n = num.Num;

        //}
        //public virtual void GoToAssignNode(AssignNode a) 
        //{
        //    System.Console.WriteLine("Зашел в AssignNode");
        //    a.Id.Nodes(this);
        //    a.Expr.Nodes(this);
        //    var b = 0;
        //    if (Id_Dict.TryGetValue(a.Id.Name, out b) == false)
        //    {
        //        Id_Dict.Add(a.Id.Name, n);
        //    }
        //}
        //public virtual void GoToCycleNode(CycleNode c) 
        //{
        //    System.Console.WriteLine("Зашел в CycleNode");
        //    c.Expr.Nodes(this);
        //    c.Stat.Nodes(this);
        //}
        //public virtual void GoToWhileNode(WhileNode w)
        //{
        //    System.Console.WriteLine("Зашел в WhileNode");
        //    w.Expr.Nodes(this);
        //    w.Stat.Nodes(this);
        //}
        //public virtual void GoToBlockNode(BlockNode bl) 
        //{
        //    System.Console.WriteLine("Зашел в BlockNode");
        //    foreach (var st in bl.StList)
        //        st.Nodes(this);
        //}

    }
}
