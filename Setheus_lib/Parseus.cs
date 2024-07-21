using ParsecSharp;
using static ParsecSharp.Parser;
using static ParsecSharp.Text;

namespace Setheus_lib
{
    public class Parseus
    {
        public static Parser<char, IASTNode> Parser { get; } = CreateParser();

        private static Parser<char, IASTNode> CreateParser()
        {
            Grammar g = new Grammar();
            return g.program.End();
        }

        public Result<char, IASTNode> Parse(string json)
            => Parser.Parse(json);
    }
}
