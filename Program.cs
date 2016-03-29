using System;
using System.Collections.Generic;

namespace Parsers {
    class MainClass {
        public static void Main(string[] args) {
            var tokens = new List<Token> {
                new IntToken(1),
                new MultToken(),
                new IntToken(2)
            };
            var parser = new ExceptionBasedParser(tokens);
            var expr = parser.MultiplicativeExpr();

            tokens = new List<Token> {
                new IntToken(1),
                new AddToken(),
                new IntToken(2)
            };
            parser = new ExceptionBasedParser(tokens);
            expr = parser.AdditiveExpr();

            tokens = new List<Token> {
                new IntToken(1),
                new AddToken(),
                new IntToken(2),
                new MultToken(),
                new IntToken(3),
                new MultToken(),
                new LeftParenToken(),
                new IntToken(4),
                new AddToken(),
                new IntToken(5),
                new RightParenToken()
            };

            parser = new ExceptionBasedParser(tokens);
            expr = parser.AdditiveExprRecursive();

        }
    }
}
