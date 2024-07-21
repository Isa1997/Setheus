
namespace setheus_lib_test.unit_tests
{
    [TestClass]
    public class SectionTests
    {
        [TestMethod]
        public void KBSection_Procedure_Parse()
        {
            Grammar grammar = new Grammar();

            var procedure_name = "test_proc";
            var action1 = "CharacterResurrect";

            var argument1 = new StringLiteral("string");
            var argumentList1 = new ArgumentList(new List<IASTNode>([argument1]));
            var argumen2 = new Variable("_Char");
            var argumentList2 = new ArgumentList(new List<IASTNode>([argumen2]));

            var EngineCall = new EngineNode(action1, argumentList2);

            var procedure = new ProcedureNode(procedure_name,argumentList1, new List<IASTNode>([EngineCall, EngineCall]), new List<IASTNode>([EngineCall, EngineCall, EngineCall, EngineCall]));

            string input = $"""
                KB
                PROC
                {procedure_name}({argument1})
                AND
                {action1}({argumen2})
                AND
                {action1}({argumen2})
                THEN
                {action1}({argumen2});
                {action1}({argumen2});
                {action1}({argumen2});
                {action1}({argumen2});

                """;

            var result = grammar.kbSection.Parse(input);
            
            Assert.IsTrue(result.Value.ToString().Trim() == procedure.ToString().Trim(), $"Expected: {procedure.ToString().Trim()} \n Parsed: {result.Value.ToString().Trim()}");
        }
    }
}
