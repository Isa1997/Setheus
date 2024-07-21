using ParsecSharp;
using static ParsecSharp.Parser;
using static ParsecSharp.Text;

namespace Setheus_lib
{
    public class Grammar
    {
        #region IgnoreDefinitions
        public Parser<char, Unit> oneLineComment => String("//").Right(Match(EndOfLine().Ignore() | EndOfInput()));
        public Parser<char, Unit> multiLineCommnent => String("/*").Right(Match(String("*/").Ignore()));
        public Parser<char, Unit> comment => oneLineComment | multiLineCommnent;
        public Parser<char, Unit> whitespace => SkipMany(OneOf(" \t\n\r").Ignore() | comment);
        #endregion

        #region SpecialCharDefinition
        public Parser<char, char> openBracket => Char('(');
        public Parser<char, char> closeBracket = Char(')');
        public Parser<char, char> doubleQuote => Char('"');
        public Parser<char, char> comma => Char(',');
        public Parser<char, char> underscore => Char('_');
        public Parser<char, char> dot => Char('.');
        public Parser<char, char> dash => Char('-');
        public Parser<char, char> semicolon => Char(';');
        #endregion

        #region Keywords
        public Parser<char, string> INTEGER_Keyword => String("INTEGER");
        public Parser<char, string> INTEGER64_Keyword => String("INTEGER64");
        public Parser<char, string> REAL_Keyword => String("REAL");
        public Parser<char, string> STRING_Keyword => String("STRING");
        public Parser<char, string> GUIDSTRING_Keyword => String("GUIDSTRING");
        public Parser<char, string> CHARACTERGUID_Keyword => String("CHARACTERGUID");
        public Parser<char, string> ITEMGUID_Keyword => String("ITEMGUID");
        public Parser<char, string> TRIGGERGUID_Keyword => String("TRIGGERGUID");
        public Parser<char, string> SPLINEGUID_Keyword => String("SPLINEGUID");
        public Parser<char, string> LEVELTEMPLATEGUID_Keyword => String("LEVELTEMPLATEGUID");
        public Parser<char, string> DB_Keyword => String("DB");
        public Parser<char, string> IF_Keyword => String("IF");
        public Parser<char, string> AND_Keyword => String("AND");
        public Parser<char, string> THEN_Keyword => String("THEN");
        public Parser<char, string> NOT_Keyword => String("NOT");
        public Parser<char, string> QUERY_Keyword => String("QRY");
        public Parser<char, string> PROCEDURE_Keyword => String("PROC");
        public Parser<char, string> GOAL_COMPLETED_Keyword => String("GoalCompleted");
        public Parser<char, string> INIT_Keyword => String("INIT");
        public Parser<char, string> KB_Keyword => String("KB");
        public Parser<char, string> EXIT_Keyword => String("EXIT");

        #endregion

        #region TypeDefinitions
        public Parser<char, char> characters => Any().Except(Char('\n'), Char('\r'), doubleQuote);
        public Parser<char, string> stringValue => Many(characters).AsString();

        public Parser<char, IASTNode> stringType => from _1 in doubleQuote
                                                    from value in stringValue
                                                    from _2 in doubleQuote
                                                    select (IASTNode)new StringLiteral(value);

        // Number literals
        public Parser<char, string> numberString => Many1(OneOf("0123456789")).AsString();
        public Parser<char, char> sign => OneOf("+-");

        public Parser<char, IASTNode> intUnsigned => from value in numberString
                                                     select (IASTNode)new IntegerLieral(value);

        public Parser<char, IASTNode> intSigned => from prefix in sign
                                                   from value in numberString
                                                   select (IASTNode)new IntegerLieral(prefix.ToString() + value.ToString());

        public Parser<char, IASTNode> intType => intSigned | intUnsigned;

        public Parser<char, IASTNode> floatUnsigned => from value in numberString
                                                       from _ in dot
                                                       from fraction in numberString
                                                       select (IASTNode)new RealLitera($"{value}.{fraction}");

        public Parser<char, IASTNode> floatSigned => from prefix in sign
                                                     from value in numberString
                                                     from _ in dot
                                                     from fraction in numberString
                                                     select (IASTNode)new RealLitera($"{prefix}{value}.{fraction}");

        public Parser<char, IASTNode> floatType => floatSigned | floatUnsigned;


        public Parser<char, IASTNode> guidFormat => from group1 in hex.Repeat(8)
                                                    from _1 in dash
                                                    from group2 in hex.Repeat(4)
                                                    from _2 in dash
                                                    from group3 in hex.Repeat(4)
                                                    from _3 in dash
                                                    from group4 in hex.Repeat(4)
                                                    from _4 in dash
                                                    from group5 in hex.Repeat(12)
                                                    select (IASTNode)new GuidLiteral($"{string.Concat(group1)}-{string.Concat(group2)}-{string.Concat(group3)}-{string.Concat(group4)}-{string.Concat(group5)}");

        public Parser<char, string> prefix => from name in Many(Any().Except(Char(' '), Char('\n'), Char('\r'), Char('\t'), semicolon, openBracket, closeBracket, doubleQuote, dash, underscore))
                                              from _ in underscore
                                              select $"{name}_"; // TODO investigate

        public Parser<char, IASTNode> guidFormatWithPrefix => from _1 in Many(prefix)
                                                              from guid in guidFormat
                                                              select (IASTNode)new GuidLiteral(guid.ToString());

        public Parser<char, IASTNode> guidType => guidFormatWithPrefix | guidFormat;

        public Parser<char, IASTNode> types => stringType | guidType | floatType | intType;
        #endregion

        #region Comparison
        public Parser<char, string> Equal_Keyword => String("==");
        public Parser<char, string> NonEqual_Keyword => String("!=");
        public Parser<char, string> Greater_Keyword => String(">");
        public Parser<char, string> GreaterEqual_Keyword => String(">=");
        public Parser<char, string> Less_Keyword => String("<");
        public Parser<char, string> LessEqual_Keyword => String("<=");

        public Parser<char, IASTNode> equalityTarget => types | variable | typeCast;
        public Parser<char, string> equalitySigns => Equal_Keyword | NonEqual_Keyword | GreaterEqual_Keyword | Greater_Keyword | Less_Keyword | LessEqual_Keyword;
        public Parser<char, IASTNode> equalityCondition => from first in equalityTarget.Between(whitespace)
                                                           from sign in equalitySigns.Between(whitespace)
                                                           from second in equalityTarget.Between(whitespace)
                                                           select (IASTNode)new ComparisonNode(first, sign, second);
        #endregion

        #region TypeCast
        public Parser<char, string> typesKeywords => INTEGER64_Keyword | STRING_Keyword | REAL_Keyword | INTEGER_Keyword
                                                     | GUIDSTRING_Keyword | CHARACTERGUID_Keyword | ITEMGUID_Keyword
                                                     | TRIGGERGUID_Keyword | SPLINEGUID_Keyword | LEVELTEMPLATEGUID_Keyword;
        public Parser<char, IASTNode> castTarget => types | variable;

        public Parser<char, IASTNode> typeCast => from _1 in openBracket.Between(whitespace)
                                                  from type in typesKeywords.Between(whitespace)
                                                  from _2 in closeBracket.Between(whitespace)
                                                  from cast in castTarget.Between(whitespace)
                                                  select (IASTNode)new TypeCast(type, cast);
        #endregion

        #region Identifiers
        // Identifier definitions
        Parser<char, char> identifier => Any().Except(Char(' '), Char('\n'), Char('\r'), Char('\t'), Char(','), semicolon, openBracket, closeBracket, doubleQuote, comma, dot, dash, underscore);
        Parser<char, char> namingCharacter => Any().Except(Char(' '), Char('\n'), Char('\r'), Char('\t'), Char(','), semicolon, openBracket, closeBracket, doubleQuote, comma, dot, dash);
        Parser<char, string> namingIdentifier => Many(namingCharacter).Between(whitespace).AsString();

        // old ?
        Parser<char, string> variablenames => Many(identifier).Between(whitespace).AsString();

        Parser<char, IASTNode> variable => from _ in underscore
                                           from v in variablenames
                                           select (IASTNode)new Variable("_" + v);
        Parser<char, char> hex => HexDigit();
        #endregion

        #region Arguments
        // Argument Definitions
        public Parser<char, IASTNode[]> argumentList => (types | variable | typeCast).Between(whitespace).SeparatedBy(comma).ToArray();

        public Parser<char, ArgumentList> arguments => from _1 in openBracket
                                                       from list in argumentList
                                                       from _2 in closeBracket
                                                       select new ArgumentList(list.ToList());
        #endregion

        #region Statements
        public Parser<char, IASTNode> DB => from _1 in DB_Keyword
                                            from _2 in underscore
                                            from name in namingIdentifier
                                            from args in arguments
                                            select (IASTNode)new DBNode(name, args);

        public Parser<char, IASTNode> notDB => from _1 in NOT_Keyword.Between(whitespace)
                                               from dbnode in DB
                                               select (IASTNode)new NOTNode(dbnode);
        public Parser<char, IASTNode> EngineCall => from name in variablenames
                                                    from args in arguments
                                                    select (IASTNode)new EngineNode(name, args);
        public Parser<char, IASTNode> goalCompleted => GOAL_COMPLETED_Keyword.Between(whitespace).Map(l => (IASTNode)new GoalCompletedNode());
        #endregion

        #region Blocks
        public Parser<char, IASTNode> triggerStatements => DB | notDB | EngineCall | equalityCondition; // Note only first in trigger

        public Parser<char, IASTNode[]> triggerBlock => triggerStatements.Between(whitespace).SeparatedBy(AND_Keyword).ToArray();
        public Parser<char, IASTNode> optionaTriggerBlock => from _1 in AND_Keyword.Between(whitespace)
                                                             from condition in triggerStatements
                                                             select condition;
        public Parser<char, IASTNode> actionStatements => DB | notDB | goalCompleted | EngineCall; // Proc call == Engine Call?
        public Parser<char, IASTNode> actionLine => from act in actionStatements
                                                    from _ in semicolon.Between(whitespace)
                                                    select act;
        public Parser<char, IASTNode[]> actionBlock => Many1(actionLine).Between(whitespace).Map(l => l.ToArray());
        #endregion

        #region Rules
        public Parser<char, IASTNode> rule => from _1 in IF_Keyword
                                              from ifBlock in triggerBlock
                                              from _2 in THEN_Keyword
                                              from thenBlock in actionBlock
                                              select (IASTNode)new RuleNode(ifBlock.ToList(), thenBlock.ToList());
        #endregion

        #region Queries
        public Parser<char, IASTNode> Query => from _1 in QUERY_Keyword
                                               from _2 in Many1(OneOf(" \t\n\r"))
                                               from startLetter in identifier
                                               from rest in namingIdentifier
                                               from args in arguments
                                               from conditionBlock in Many(optionaTriggerBlock).Between(whitespace)
                                               from _3 in THEN_Keyword
                                               from thenBlock in actionBlock
                                               select (IASTNode)new QueryConditionNode($"{startLetter}{rest}", args, conditionBlock.ToList(), thenBlock.ToList());

        #endregion

        #region Procedures
        public Parser<char, IASTNode> Procedure => from _1 in PROCEDURE_Keyword
                                                   from _ in Many1(OneOf(" \t\n\r"))
                                                   from startLetter in identifier
                                                   from rest in namingIdentifier
                                                   from args in arguments
                                                   from conditionBlock in Many(optionaTriggerBlock).Between(whitespace)
                                                   from _5 in THEN_Keyword
                                                   from thenBlock in actionBlock
                                                   select (IASTNode)new ProcedureNode($"{startLetter}{rest}", args, conditionBlock.ToList(), thenBlock.ToList());
        #endregion

        #region Program

        public Parser<char, IASTNode> kbSectionStatements => Query | Procedure | rule;
        public Parser<char, IASTNode[]> kbSectionDefinitions => Many1(kbSectionStatements).Between(whitespace).Map(l => l.ToArray());

        public Parser<char, IASTNode> initSection => from _ in INIT_Keyword.Between(whitespace)
                                                     from actions in actionBlock
                                                     select (IASTNode)new InitSection(actions.ToList());
        public Parser<char, IASTNode> exitSection => from _ in EXIT_Keyword.Between(whitespace)
                                                     from actions in actionBlock
                                                     select (IASTNode)new EXITSection(actions.ToList());
        public Parser<char, IASTNode> kbSection => from _ in KB_Keyword.Between(whitespace)
                                                   from trigerDefs in kbSectionDefinitions
                                                   select (IASTNode)new KBSection(trigerDefs.ToList());

        public Parser<char, IASTNode> program => from init in initSection
                                                 from kb in kbSection
                                                 from exit in exitSection
                                                 select (IASTNode)new ProgramNode(init, kb, exit);
        #endregion
    }
}