using ParsecSharp;
using Setheus_lib;

namespace Setheus
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var parser = new Parseus();
            var result = parser.Parse("""
                INIT
                DB_MyPrefix_AtLeastOneFruit(_ss, 2, "sss");	
                DB_MyPrefix_AtLeastOneFruit(_ss, 2, "sss");	
                DB_MyPrefixsasd_sads_dsas_AtLeastOneFruit(_ss, 2, "sss");	
                DB_AtLeastOneFruit(_ss, 2, "sss");	

                KB
                PROC
                PROC_Overview_TeleportAlive((CHARACTERGUID)_Char, (GUIDSTRING)_Destination)
                AND
                DB_AtLeastOneFruit(_ss, 2, "sss")
                THEN
                CharacterResurrect(_Char);

                EXIT
                NOT DB_MyPrefix_AtLeastOneFruit(1);
                """);

            Console.WriteLine(result.Value.ToString());
            
            using(StreamWriter writter = new StreamWriter("output.dot"))
            {
                writter.Write("digraph ScriptASTTree {\n");
                result.Value.ToDot(writter);
                writter.Write("}\n");

                writter.Flush();
                writter.Close();
            }
        }
    }
}