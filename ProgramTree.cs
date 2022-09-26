using System.Collections.Generic;

namespace ProgramTree
{
    public enum AssignType { Assign, AssignPlus, AssignMinus, AssignMult, AssignDivide };

    public abstract class Node // базовый класс для всех узлов    
    {
        public Dictionary<string, int> Id_Dict = new Dictionary<string, int>();
        public abstract void Visit();
    }

    public abstract class ExprNode : Node // базовый класс для всех выражений
    {
    }

    public class IdNode : ExprNode
    {
        public string Name { get; set; }
        public IdNode(string name) { Name = name; }

        public override void Visit()
        {
            System.Console.WriteLine("Зашел в IdNode");
            var a = 0;
            if (Id_Dict.TryGetValue(Name, out a) == false)
            {
                Id_Dict.Add(Name, 0);
            }
        }
    }

    public  class IntNumNode : ExprNode
    {
        public int Num { get; set; }
        public IntNumNode(int num) { Num = num; }

        public override void Visit()
        {
            System.Console.WriteLine("Зашел в IntNumNode");

        }

    }

    public abstract class StatementNode : Node // базовый класс для всех операторов
    {
    }

    public class AssignNode : StatementNode
    {

        public IdNode Id { get; set; }
        public ExprNode Expr { get; set; }
        public AssignType AssOp { get; set; }
        public AssignNode(IdNode id, ExprNode expr, AssignType assop = AssignType.Assign)
        {
            Id = id;
            Expr = expr;
            AssOp = assop;
        }

        public override void Visit()
        {
            System.Console.WriteLine("Зашел в AssignNode");
            Id.Visit();
            Expr.Visit();
            
        }
    }

    public class CycleNode : StatementNode
    {
        public ExprNode Expr { get; set; }
        public StatementNode Stat { get; set; }
        public CycleNode(ExprNode expr, StatementNode stat)
        {
            Expr = expr;
            Stat = stat;
        }
        public override void Visit()
        {
            System.Console.WriteLine("Зашел в CycleNode");
            Expr.Visit();
            Stat.Visit();
        }
    }
    public class WhileNode : StatementNode
    {
        public ExprNode Expr { get; set; }
        public StatementNode Stat { get; set; }
        public WhileNode(ExprNode expr, StatementNode stat)
        {
            Expr = expr;
            Stat = stat;
        }

        public override void Visit()
        {
            System.Console.WriteLine("Зашел в WhileNode");
            Expr.Visit();
            Stat.Visit();
        }
    }

    public class BlockNode : StatementNode
    {
        public List<StatementNode> StList = new List<StatementNode>();
        public BlockNode(StatementNode stat)
        {
            Add(stat);
        }
        public void Add(StatementNode stat)
        {
            StList.Add(stat);
        }

        public override void Visit()
        {
            System.Console.WriteLine("Зашел в BlockNode");
            foreach (var st in this.StList)
                st.Visit();
        }
    }

}