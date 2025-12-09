using System;
using System.Collections.Generic;
using System.Text;

namespace CDO.Core.Models;

public sealed class Benefit {
    public string Value { get; }

    private Benefit(string value) { Value = value; }

    public static readonly Benefit None = new("None");
    public static readonly Benefit SSA = new("SSA");
    public static readonly Benefit SSDI = new("SSDI");

    public static readonly Benefit[] All = [None, SSA, SSDI];
}
