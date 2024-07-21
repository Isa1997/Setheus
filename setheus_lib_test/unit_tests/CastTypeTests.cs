
namespace setheus_lib_test.unit_tests
{
    [TestClass]
    public class CastTypeTests
    {
        [TestMethod]
        public void Cast_Guid_To_CharacterGuid()
        {
            Grammar grammar = new Grammar();
            string guid = "123e4567-e89b-12d3-a456-426655440000";
            string cast = "CHARACTERGUID";
            string expected = $"{cast} -> {guid}";
            var result = grammar.typeCast.Parse($"({cast}){guid}");
            Assert.IsTrue(result.Value.ToString() == expected, $"Expected: {expected} \n Parsed: {result.Value.ToString()}");
        }

        [TestMethod]
        public void Cast_String_To_Int()
        {
            Grammar grammar = new Grammar();
            string guid = "\"string\"";
            string cast = "INTEGER64";
            string input = $"({cast}){guid}";
            string expected = $"{cast} -> {guid}";
            var result = grammar.typeCast.Parse(input);
            Assert.IsTrue(result.Value.ToString() == expected, $"Expected: {expected} \n Parsed: {result.Value.ToString()}");
        }

        [TestMethod]
        public void Cast_Variable_To_ItemGuid()
        {
            Grammar grammar = new Grammar();
            string guid = "_sasdas";
            string cast = "ITEMGUID";
            string input = $"({cast}){guid}";
            string expected = $"{cast} -> {guid}";
            var result = grammar.typeCast.Parse(input);
            Assert.IsTrue(result.Value.ToString() == expected, $"Expected: {expected} \n Parsed: {result.Value.ToString()}");
        }
    }
}
