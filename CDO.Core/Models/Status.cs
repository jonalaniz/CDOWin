using System;
using System.Collections.Generic;
using System.Text;

namespace CDO.Core.Models;

public sealed class Status {
    public string Value { get; }

    private Status(string value) { Value = value; }

    public static readonly Status SSA = new("SSA");
    public static readonly Status VR1 = new("VR1");
    public static readonly Status VR2 = new("VR2");
    public static readonly Status SUP1 = new("SUP1");
    public static readonly Status SUP2 = new("SUP2");

    public static readonly Status[] All = [SSA, VR1, VR2, SUP1, SUP2];
}
