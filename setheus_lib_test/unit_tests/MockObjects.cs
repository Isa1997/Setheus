
namespace setheus_lib_test.unit_tests
{
    public class MockObjects
    {
        public Tuple<IASTNode, string> CreateDBNodeWithArguments()
        {
            var argument1 = new StringLiteral("string_argument");
            var argument2 = new IntegerLieral("12345");
            var argument3 = new RealLitera("12345.123");
            var argument4 = new TypeCast("CHARACTERGUID", new GuidLiteral("ad9a3327-4456-42a7-9bf4-7ad60cc9e54f"));
            var nodeName = "MyPrefix_Other_Prefix";

            string input = $"""
                    DB_{nodeName}({argument1},{argument2},{argument3},{argument4});
                """;

            return new Tuple<IASTNode, string>(new DBNode(nodeName, new ArgumentList(new List<IASTNode>([argument1, argument2, argument3, argument4]))), input);
        }

        public Tuple<IASTNode, string> CreateDBNodeWithoutArguments()
        {
            var nodeName = "MyPrefix_Other_Prefix";
            var node = new DBNode(nodeName, new ArgumentList([]));
            var input = $"""
                DB_{nodeName}();
                """;

            return new Tuple<IASTNode, string>(node, input);
        }
    }
}
