
namespace Setheus_lib
{
    public interface IASTNode
    {
        public void ToDot(StreamWriter output);
        public string GetDotName();
    }


    #region Type Literals
    public class StringLiteral(string value) : IASTNode
    {
        public string GetDotName()
        {
            return $"\"({GetHashCode()}) StringLiteral: {value}\"";
        }

        public void ToDot(StreamWriter output)
        {

        }

        public override string ToString()
        {
            return $"\"{value}\"";
        }
    }

    public class IntegerLieral(string value) : IASTNode
    {
        public string GetDotName()
        {
            return $"\"({GetHashCode()}) IntegerLieral: {value}\"";
        }

        public void ToDot(StreamWriter output)
        {
        }

        public override string ToString()
        {
            return value;
        }
    }

    public class RealLitera(string value) : IASTNode
    {
        public string GetDotName()
        {
            return $"\"({GetHashCode()}) RealLitera: {value}\"";
        }

        public void ToDot(StreamWriter output)
        {
        }

        public override string ToString()
        {
            return value;
        }
    }

    public class GuidLiteral(string value) : IASTNode
    {
        public string GetDotName()
        {
            return $"\"({GetHashCode()}) GuidLiteral: {value}\"";
        }

        public void ToDot(StreamWriter output)
        {

        }

        public override string ToString()
        {
            return value;
        }
    }
    #endregion

    #region Identifiers
    public class Variable(string name) : IASTNode
    {
        public string GetDotName()
        {
            return $"\"({GetHashCode()}) Variable: {name}\"";
        }

        public void ToDot(StreamWriter output)
        {
        }

        public override string ToString()
        {
            return name;
        }
    }
    #endregion

    #region Arguments
    public class ArgumentList(List<IASTNode> args) : IASTNode
    {
        public string GetDotName()
        {
            return $"\"({GetHashCode()}) ArgumentList\"";
        }

        public void ToDot(StreamWriter output)
        {
            string dotName = GetDotName();

            foreach (var arg in args)
            {
                output.Write($"{dotName} -> {arg.GetDotName()}\n");
            }

            foreach (var arg in args)
            {
                arg.ToDot(output);
            }
        }

        public override string ToString()
        {
            string result = "(";
            for (int i = 0; i < args.Count; i++)
            {
                if (i != 0)
                {
                    result += ", ";
                }
                result += args[i].ToString();
            }

            return result + ")";
        }
    }
    #endregion

    #region TypeCast
    public class TypeCast(string castType, IASTNode node) : IASTNode
    {
        public string GetDotName()
        {
            return $"\"({GetHashCode()}) TypeCast: {castType}\"";
        }

        public void ToDot(StreamWriter output)
        {
            output.Write($"{GetDotName()} -> {node.GetDotName()}\n");
            node.ToDot(output);
        }

        public override string ToString()
        {
            return castType + " -> " + node.ToString();
        }
    }
    #endregion

    #region CommonStatements
    public class DBNode(string name, ArgumentList args) : IASTNode
    {
        public string GetDotName()
        {
            return $"\"({GetHashCode()}) DBNode_{name}\"";
        }

        public void ToDot(StreamWriter output)
        {
            string dotName = GetDotName();

            output.Write($"{dotName} -> {args.GetDotName()}\n");

            args.ToDot(output);
        }
        public override string ToString()
        {
            return $"DB_{name}{args}";
        }
    }

    public class NOTNode(IASTNode node) : IASTNode
    {
        public string GetDotName()
        {
            return $"\"({GetHashCode()}) NOT\"";
        }

        public void ToDot(StreamWriter output)
        {
            output.Write($"{GetDotName()} -> {node.GetDotName()}\n");
            node.ToDot(output);
        }

        public override string ToString()
        {
            return "NOT " + node.ToString();
        }
    }

    public class EngineNode(string name, ArgumentList args) : IASTNode
    {
        public string GetDotName()
        {
            return $"\"({GetHashCode()}) Call: {name}\"";
        }

        public void ToDot(StreamWriter output)
        {
            output.Write($"{GetDotName()} -> {args.GetDotName()}\n");
            args.ToDot(output);
        }

        public override string ToString()
        {
            return $"{name}{args}";
        }
    }
    #endregion

    #region ActionStatements
    public class GoalCompletedNode() : IASTNode
    {
        public string GetDotName()
        {
            return $"\"({GetHashCode()}) Goal Completed\"";
        }

        public void ToDot(StreamWriter output)
        {
        }

        public override string ToString()
        {
            return "GoalCompleted";
        }
    }

    public class ProcedureNode(string name, ArgumentList args, List<IASTNode> ifBlock, List<IASTNode> thenBlock) : IASTNode
    {
        public string GetDotName()
        {
            return $"\"({GetHashCode()}) ProcedureNode\"";
        }

        public void ToDot(StreamWriter output)
        {
            string procedureName = GetDotName();
            string thenDotName = $"\"({GetHashCode()}) THEN\"";
            string ifandDotName = $"\"({GetHashCode()}) AND\"";
            string thenandDotName = $"\"({ifandDotName.GetHashCode()}) AND\"";

            if(ifBlock.Count > 0)
            {
                output.Write($"{procedureName} -> {ifandDotName}\n");
                output.Write($"{ifandDotName} -> {name}\n");
                output.Write($"{name} -> {args.GetDotName()}");
                args.ToDot(output);
            }
            else
            {
                output.Write($"{procedureName} -> {name}\n");
                output.Write($"{name} -> {args.GetDotName()}");
                args.ToDot(output);
            }

            foreach (var x in ifBlock)
            {
                output.Write($"{ifandDotName} -> {x.GetDotName()}\n");
                x.ToDot(output);
            }


            output.Write($"{procedureName} -> {thenDotName}\n");

            if(thenBlock.Count > 1)
            {
                output.Write($"{thenDotName} -> {thenandDotName}\n");

                foreach (var x in thenBlock)
                {
                    output.Write($"{thenandDotName} -> {x.GetDotName()}\n");
                    x.ToDot(output);
                }
            }
            else
            {
                foreach (var x in thenBlock)
                {
                    output.Write($"{thenDotName} -> {x.GetDotName()}\n");
                    x.ToDot(output);
                }
            }
        }

        public override string ToString()
        {
            string result = "Procedure => ";
            result += $"{name}{args}\n{{\n";
            foreach (var x in ifBlock)
            {
                result += "\t" + x.ToString() + "\n";
            }
            result += "}\n";

            result += "THEN\n{\n";
            foreach (var x in thenBlock)
            {
                result += "\t" + x.ToString() + "\n";
            }
            result += "}\n";

            return result;
        }
    }

    #endregion

    #region TriggerStatements
    public class RuleNode(List<IASTNode> ifBlock, List<IASTNode> thenBlock) : IASTNode
    {
        public string GetDotName()
        {
            return $"\"({GetHashCode()}) RuleNode\"";
        }

        public void ToDot(StreamWriter output)
        {
            string ifDotName = $"\"({GetHashCode()}) IF\"";
            string thenDotName = $"\"({GetHashCode()}) THEN\"";
            string ifandDotName = $"\"({GetHashCode()}) AND\"";
            string thenandDotName = $"\"({ifandDotName.GetHashCode()}) AND\"";

            output.Write($"{GetDotName()} -> {ifDotName}\n");
            output.Write($"{ifDotName} -> {ifandDotName}\n");

            foreach (var x in ifBlock)
            {
                output.Write($"{ifandDotName} -> {x.GetDotName()}\n");
                x.ToDot(output);
            }

            output.Write($"{ifDotName} -> {thenDotName}\n");
            output.Write($"{thenDotName} -> {thenandDotName}\n");

            foreach (var x in thenBlock)
            {
                output.Write($"{thenandDotName} -> {x.GetDotName()}\n");
                x.ToDot(output);
            }
        }

        public override string ToString()
        {
            string result = "IF\n{\n";
            foreach (var x in ifBlock)
            {
                result += "\t" + x.ToString() + "\n";
            }
            result += "}\n";

            result += "THEN\n{\n";
            foreach (var x in thenBlock)
            {
                result += "\t" + x.ToString() + "\n";
            }
            result += "}\n";

            return result;
        }
    }

    public class ComparisonNode(IASTNode first, string sign, IASTNode second) : IASTNode
    {
        public string GetDotName()
        {
            return $"\"({GetHashCode()}) Comparison: {sign}\"";
        }

        public void ToDot(StreamWriter output)
        {
            output.Write($"{GetDotName()} -> {first.GetDotName()}\n");
            output.Write($"{GetDotName()} -> {second.GetDotName()}\n");
            first.ToDot(output);
            second.ToDot(output);
        }

        public override string ToString()
        {
            return $"{first} {sign} {second}";
        }
    }

    public class QueryConditionNode(string name, ArgumentList args, List<IASTNode> ifBlock, List<IASTNode> thenBlock) : IASTNode
    {
        public string GetDotName()
        {
            return $"\"({GetHashCode()}) QueryCondition\"";
        }

        public void ToDot(StreamWriter output)
        {
            string queryName = GetDotName();
            string thenDotName = $"\"({GetHashCode()}) THEN\"";
            string ifandDotName = $"\"({GetHashCode()}) AND\"";
            string thenandDotName = $"\"({ifandDotName.GetHashCode()}) AND\"";

            output.Write($"{queryName} -> {ifandDotName}\n");

            foreach (var x in ifBlock)
            {
                output.Write($"{ifandDotName} -> {x.GetDotName()}\n");
                x.ToDot(output);
            }

            output.Write($"{queryName} -> {thenDotName}\n");
            output.Write($"{thenDotName} -> {thenandDotName}\n");

            foreach (var x in thenBlock)
            {
                output.Write($"{thenandDotName} -> {x.GetDotName()}\n");
                x.ToDot(output);
            }
        }

        public override string ToString()
        {
            string result = "Query => ";
            result += $"{name}{args} \n {{ \n";
            foreach (var x in ifBlock)
            {
                result += "\t" + x.ToString() + "\n";
            }
            result += "}\n";

            result += "THEN\n{\n";
            foreach (var x in thenBlock)
            {
                result += "\t" + x.ToString() + "\n";
            }
            result += "}\n";

            return result;
        }
    }
    #endregion

    #region Sections
    public class InitSection(List<IASTNode> actionBlock) : IASTNode
    {
        public string GetDotName()
        {
            return $"\"({GetHashCode()}) Init\"";
        }

        public void ToDot(StreamWriter output)
        {
            string initName = GetDotName();
            foreach (var action in actionBlock)
            {
                output.Write($"{initName} -> {action.GetDotName()}\n");
                action.ToDot(output);
            }
        }

        public override string ToString()
        {
            string result = "";
            foreach (var action in actionBlock)
            {
                result += action.ToString() + "\n";
            }

            return result;
        }
    }
    public class KBSection(List<IASTNode> triggers) : IASTNode
    {
        public string GetDotName()
        {
            return $"\"({GetHashCode()}) KBSection\"";
        }

        public void ToDot(StreamWriter output)
        {
            string sectionName = GetDotName();
            foreach (var definition in triggers)
            {
                output.Write($"{sectionName} -> {definition.GetDotName()}\n");
                definition.ToDot(output);
            }
        }

        public override string ToString()
        {
            string result = "";
            foreach (var definition in triggers)
            {
                result += definition.ToString() + "\n";
            }

            return result;
        }
    }

    public class EXITSection(List<IASTNode> actionBlock) : IASTNode
    {
        public string GetDotName()
        {
            return $"\"({GetHashCode()}) EXIT\"";
        }

        public void ToDot(StreamWriter output)
        {
            string sectionName = GetDotName();
            foreach (var action in actionBlock)
            {
                output.Write($"{sectionName} -> {action.GetDotName()}\n");
                action.ToDot(output);
            }
        }

        public override string ToString()
        {
            string result = "";
            foreach (var action in actionBlock)
            {
                result += action.ToString() + "\n";
            }

            return result;
        }
    }
    #endregion

    #region Program
    public class ProgramNode(IASTNode init, IASTNode kb, IASTNode exit) : IASTNode
    {
        public string GetDotName()
        {
            return $"\"({GetHashCode()}) Program\"";
        }

        public void ToDot(StreamWriter output)
        {
            string initName = GetDotName();
            output.Write($"{initName} -> {init.GetDotName()}\n");
            output.Write($"{initName} -> {kb.GetDotName()}\n");
            output.Write($"{initName} -> {exit.GetDotName()}\n");

            init.ToDot(output);
            kb.ToDot(output);
            exit.ToDot(output);
        }

        public override string ToString()
        {
            string result = "";
            result += "INIT\n";
            result += init.ToString();
            result += "KB\n";
            result += kb.ToString();
            result += "EXIT\n";
            result += exit.ToString();
            return result;
        }
    }
    #endregion
}
