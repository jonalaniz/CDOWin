namespace CDO.Core.Models.Enums;

public class SAType {
    private SAType(string description, int value, string um) {
        Description = description;
        UM = um;
        Value = value;
    }

    public string Description { get; private set; }
    public string UM { get; private set; }
    public int Value { get; private set; }

    public static SAType BenchmarkAEnhanced {
        get { return new SAType("Benchmark A ENHANCED Job Placement - 5 Days", 1200, "Service"); }
    }

    public static SAType BenchmarkBEnhanced {
        get { return new SAType("Benchmark B ENHANCED Job Placement - 45 Days", 735, "Service"); }
    }

    public static SAType BenchmarkCEnhanced {
        get { return new SAType("Benchmark C ENHANCED Job Placement - 90 Days", 1470, "Service"); }
    }

    public static SAType BenchmarkABasic {
        get { return new SAType("Benchmark A BASIC Job Placement - 5 Days", 1103, "Service"); }
    }

    public static SAType BenchmarkBBasic {
        get { return new SAType("Benchmark B BASIC Job Placement - 45 Days", 551, "Service"); }
    }

    public static SAType BenchmarkCBasic {
        get { return new SAType("Benchmark C BASIC Job Placement - 90 Days", 1103, "Service"); }
    }

    public static SAType AutismPremium {
        get { return new SAType("Autism Premium", 735, "Each"); }
    }

    public static SAType CriminalBackground {
        get { return new SAType("Criminal Background Premium", 613, "Each"); }
    }

    public static SAType WagePremium {
        get { return new SAType("Wage Premium", 613, "Each"); }
    }

    public static SAType ProfessionalPremium {
        get { return new SAType("Professional Premium", 613, "Each"); }
    }

    public static SAType JobSkill {
        get { return new SAType("Job Skills Training", 46, "Hour"); }
    }
    public override string ToString() {
        return Description;
    }

    public static SAType[] AllItems() {
        return [
            SAType.BenchmarkAEnhanced,
            SAType.BenchmarkBEnhanced,
            SAType.BenchmarkCEnhanced,
            SAType.BenchmarkABasic,
            SAType.BenchmarkBBasic,
            SAType.BenchmarkCBasic,
            SAType.AutismPremium,
            SAType.CriminalBackground,
            SAType.WagePremium,
            SAType.ProfessionalPremium,
            SAType.JobSkill
        ];
    }
}