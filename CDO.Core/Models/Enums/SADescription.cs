namespace CDO.Core.Models.Enums;

public class SADescription {
    private SADescription(string description) {
        Description = description;
    }

    public string Description { get; private set; }

    public static SADescription BenchmarkAEnhanced {
        get { return new SADescription("Benchmark A ENHANCED Job Placement - 5 Days"); }
    }

    public static SADescription BenchmarkBEnhanced {
        get { return new SADescription("Benchmark B ENHANCED Job Placement - 45 Days"); }
    }

    public static SADescription BenchmarkCEnhanced {
        get { return new SADescription("Benchmark C ENHANCED Job Placement - 90 Days"); }
    }

    public static SADescription BenchmarkABasic {
        get { return new SADescription("Benchmark A BASIC Job Placement - 5 Days"); }
    }

    public static SADescription BenchmarkBBasic {
        get { return new SADescription("Benchmark B BASIC Job Placement - 45 Days"); }
    }

    public static SADescription BenchmarkCBasic {
        get { return new SADescription("Benchmark C BASIC Job Placement - 90 Days"); }
    }

    public static SADescription AutismPremium {
        get { return new SADescription("Autism Premium"); }
    }

    public static SADescription CriminalBackground {
        get { return new SADescription("Criminal Background Premium"); }
    }

    public static SADescription WagePremium {
        get { return new SADescription("Wage Premium"); }
    }

    public static SADescription ProfessionalPremium {
        get { return new SADescription("Professional Premium"); }
    }

    public static SADescription JobSkill {
        get { return new SADescription("Job Skills Training"); }
    }
    public override string ToString() {
        return Description;
    }

    public static SADescription[] AllItems() {
        return [
            SADescription.BenchmarkAEnhanced,
            SADescription.BenchmarkBEnhanced,
            SADescription.BenchmarkCEnhanced,
            SADescription.BenchmarkABasic,
            SADescription.BenchmarkBBasic,
            SADescription.BenchmarkCBasic,
            SADescription.AutismPremium,
            SADescription.CriminalBackground,
            SADescription.WagePremium,
            SADescription.ProfessionalPremium,
            SADescription.JobSkill
        ];
    }
}