﻿using System;
using System.Collections.Generic;

public sealed class ExceptionBasedParser {
    public ExceptionBasedParser(List<Token> tokens) {
        this.Tokens = tokens;
        this.Pos = 0;
        this.Farthest = 0;
    }

    public List<Token> Tokens { get; }

    private Int32 _pos;
    public Int32 Pos {
        get {
            return _pos;
        }
        set {
            _pos = value;
            this.Farthest = Math.Max(this.Farthest, _pos);
        }
    }

    public Int32 Farthest { get; private set; }

}

public static class ExceptionBasedParserExtensions {
    
    // expr
    //   : additive-expr
    // 
    // additive-expr
    //   : multiplicative-expr [ + multiplicative-expr ]*
    // 
    // multiplicative-expr
    //   : unary-expr [ * unary-expr ]*
    // 
    // unary-expr
    //   : int-expr
    //   | ( additive-expr )

    // expr
    //   : additive-expr
    public static Expr Expr(this ExceptionBasedParser _) {
        return _.AdditiveExpr();
    }

    public static IntExpr IntExpr(this ExceptionBasedParser _) {
        var token = _.Tokens[_.Pos] as IntToken;

        if (token == null) {
            throw new Exception("failed.");
        }

        _.Pos++;
        return new IntExpr(token.Val);
    }

    public static void Operator<Op>(this ExceptionBasedParser _) where Op : Token {
        var token = _.Tokens[_.Pos] as Op;

        if (token == null) {
            throw new Exception("failed.");
        }

        _.Pos++;
    }

    // '(' additive-expr ')'
    public static Expr ParenEnclosedExpr(this ExceptionBasedParser _) {
        _.Operator<LeftParenToken>();
        var expr = _.AdditiveExpr();
        _.Operator<RightParenToken>();
        return expr;
    }

    // unary-expr
    //   : int-expr
    //   | paren-enclosed-expr
    public static Expr UnaryExpr(this ExceptionBasedParser _) {
        var pos = _.Pos;

        try { return _.ParenEnclosedExpr(); } catch { _.Pos = pos; }

        try { return _.IntExpr(); } catch { _.Pos = pos; }

        throw new Exception("failed.");
    }

    // multiplicative-expr
    //   : unary-expr [ * unary-expr ]*
    public static Expr MultiplicativeExpr(this ExceptionBasedParser _) {
        var left = _.UnaryExpr();

        for (;;) {
            var pos = _.Pos;
            try {
                _.Operator<MultToken>();
                var right = _.UnaryExpr();
                left = new MultExpr(left, right);
            } catch {
                _.Pos = pos;
                return left;
            }
        }
    }

    // additive-expr
    //   : multiplicative-expr [ + multiplicative-expr ]*
    public static Expr AdditiveExpr(this ExceptionBasedParser _) {
        var left = _.MultiplicativeExpr();

        for (;;) {
            var pos = _.Pos;
            try {
                _.Operator<AddToken>();
                var right = _.MultiplicativeExpr();
                left = new AddExpr(left, right);
            } catch {
                _.Pos = pos;
                return left;
            }
        }
    }

    // additive-expr
    //   : multiplicative-expr [ + additive-expr ]?
    public static Expr AdditiveExprRecursive(this ExceptionBasedParser _) {
        var left = _.MultiplicativeExpr();

        var pos = _.Pos;
        try {
            _.Operator<AddToken>();
            var right = _.AdditiveExprRecursive();
            return new AddExpr(left, right);
        } catch {
            _.Pos = pos;
            return left;
        }
    }

}