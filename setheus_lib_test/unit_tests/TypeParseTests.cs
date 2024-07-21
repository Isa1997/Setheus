
namespace setheus_lib_test.unit_tests
{
    [TestClass]
    public class TypeParseTests
    {
        [TestMethod]
        public void String_Parse()
        {
            Grammar grammar = new Grammar();
            string input = "\"test_string***-123\"";
            var result = grammar.stringType.Parse(input);
            Assert.IsTrue(result.Value.ToString() == input, $"Expected: {input} \n Parsed: {result.Value.ToString()}");
        }

        [TestMethod]
        public void Real_UnsignedParse()
        {
            Grammar grammar = new Grammar();
            string input = "222.333";
            var result = grammar.floatType.Parse(input);
            Assert.IsTrue(result.Value.ToString() == input, $"Expected: {input} \n Parsed: {result.Value.ToString()}");
        }

        [TestMethod]
        public void Real_PositiveSignedParse()
        {
            Grammar grammar = new Grammar();
            string input = "+123.25680358";
            var result = grammar.floatType.Parse(input);
            Assert.IsTrue(result.Value.ToString() == input, $"Expected: {input.Replace("+", null)} \n Parsed: {result.Value.ToString()}");
        }

        [TestMethod]
        public void Real_NegativeSignedParse()
        {
            Grammar grammar = new Grammar();
            string input = "-98745.123";
            var result = grammar.floatType.Parse(input);
            Assert.IsTrue(result.Value.ToString() == input, $"Expected: {input} \n Parsed: {result.Value.ToString()}");
        }

        [TestMethod]
        public void Real_ZerosStartParse()
        {
            Grammar grammar = new Grammar();
            string input = "+00001.25680358";
            var result = grammar.floatType.Parse(input);
            Assert.IsTrue(result.Value.ToString() == input, $"Expected: {input.Replace("+", null)} \n Parsed: {result.Value.ToString()}");
        }

        [TestMethod]
        public void Int_UnsignedParse()
        {
            Grammar grammar = new Grammar();
            string input = "1234567890";
            var result = grammar.intType.Parse(input);
            Assert.IsTrue(result.Value.ToString() == input, $"Expected: {input} \n Parsed: {result.Value.ToString()}");
        }

        [TestMethod]
        public void Int_PositiveSignedParse()
        {
            Grammar grammar = new Grammar();
            string input = "+1234567890";
            var result = grammar.intType.Parse(input);
            Assert.IsTrue(result.Value.ToString() == input, $"Expected: {input.Replace("+", null)} \n Parsed: {result.Value.ToString()}");
        }

        [TestMethod]
        public void Int_NegativeSignedParse()
        {
            Grammar grammar = new Grammar();
            string input = "-1234567890";
            var result = grammar.intType.Parse(input);
            Assert.IsTrue(result.Value.ToString() == input, $"Expected: {input} \n Parsed: {result.Value.ToString()}");
        }

        [TestMethod]
        public void Int_PrecedingZerosParse()
        {
            Grammar grammar = new Grammar();
            string input = "-00012300";
            var result = grammar.intType.Parse(input);
            Assert.IsTrue(result.Value.ToString() == input, $"Expected: {input} \n Parsed: {result.Value.ToString()}");
        }

        [TestCategory("Guid")]
        [TestMethod]
        public void Guid_Parse_Success()
        {
            Grammar grammar = new Grammar();
            string input = "123e4567-e89b-12d3-a456-426655440000";
            var result = grammar.guidType.Parse(input);
            Assert.IsTrue(result.Value.ToString() == input, $"Expected: {input} \n Parsed: {result.Value.ToString()}");
        }

        [TestCategory("Guid")]
        [TestMethod]
        [ExpectedException(typeof(ParsecException))]
        public void Guid_Parse_Fail()
        {
            Grammar grammar = new Grammar();
            string input = "123e4567e89b-12d3-a456-426655440000";
            var result = grammar.guidType.Parse(input);
            
            result.Value.ToString();
        }

        [TestCategory("Guid")]
        [TestMethod]
        public void Guid_WithPrefixParse()
        {
            Grammar grammar = new Grammar();
            string prefix = "sdf_asda_CODEE_";
            string input = "123e4567-e89b-12d3-a456-426655440000";
            var result = grammar.guidType.Parse(prefix + input);
            Assert.IsTrue(result.Value.ToString() == input, $"Expected: {input} \n Parsed: {result.Value.ToString()}");
        }
    }
}