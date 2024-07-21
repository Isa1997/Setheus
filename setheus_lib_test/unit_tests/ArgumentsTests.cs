
namespace setheus_lib_test.unit_tests
{
    [TestClass]
    public class ArgumentTests
    {
        [TestMethod]
        public void Parse_Empty_Args()
        {
            Grammar grammar = new Grammar();
            string input = "()";
            var result = grammar.arguments.Parse(input);
            Assert.IsTrue(result.Value.ToString() == input, $"Expected: {input} \n Parsed: {result.Value.ToString()}");
        }

        [TestMethod]
        public void Parse_One_Arg()
        {
            Grammar grammar = new Grammar();
            string input = "(12345)";
            var result = grammar.arguments.Parse(input);
            Assert.IsTrue(result.Value.ToString() == input, $"Expected: {input} Parsed: {result.Value.ToString()}");
        }

        [TestMethod]
        public void Parse_One_Identifier_Args()
        {
            Grammar grammar = new Grammar();
            string input = "(_abc)";
            string expected = input;
            var result = grammar.arguments.Parse(input);
            Assert.IsTrue(result.Value.ToString() == expected, $"Expected: {expected} Parsed: {result.Value.ToString()}");
        }

        [TestMethod]
        public void Parse_Multiple_Args()
        {
            Grammar grammar = new Grammar();
            string input = "(12345, 12345, \"test\", 123e4567-e89b-12d3-a456-426655440000)";
            var result = grammar.arguments.Parse(input);
            Assert.IsTrue(result.Value.ToString() == input, $"Expected: {input} Parsed: {result.Value.ToString()}");
        }

        [TestMethod]
        public void Parse_Multiple_With_Cast_Args()
        {
            Grammar grammar = new Grammar();
            string input = "(12345, (CHARACTERGUID)12345, \"test\", asdas_dasda_123e4567-e89b-12d3-a456-426655440000)";
            string expected = "(12345, CHARACTERGUID -> 12345, \"test\", 123e4567-e89b-12d3-a456-426655440000)";
            var result = grammar.arguments.Parse(input);
            Assert.IsTrue(result.Value.ToString() == expected, $"Expected: {expected} Parsed: {result.Value.ToString()}");
        }

        [TestMethod]
        public void Parse_Multiple_With_Identifier_Args()
        {
            Grammar grammar = new Grammar();
            string input = "(12345, _abc, (CHARACTERGUID)\"test\", asdas_dasda_123e4567-e89b-12d3-a456-426655440000)";
            string expected = "(12345, _abc, CHARACTERGUID -> \"test\", 123e4567-e89b-12d3-a456-426655440000)";
            var result = grammar.arguments.Parse(input);
            Assert.IsTrue(result.Value.ToString() == expected, $"Expected: {expected} Parsed: {result.Value.ToString()}");
        }
    }
}
