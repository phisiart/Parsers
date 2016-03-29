using System;

public abstract class Token { }

public sealed class IntToken : Token {
    public IntToken(Int32 val) {
        this.Val = val;
    }

    public Int32 Val { get; }
}

public abstract class OpToken : Token { }

public sealed class MultToken : OpToken { }

public sealed class DivToken : OpToken { }

public sealed class AddToken : OpToken { }

public sealed class SubToken : OpToken { }

public sealed class LeftParenToken : OpToken { }

public sealed class RightParenToken : OpToken { }
