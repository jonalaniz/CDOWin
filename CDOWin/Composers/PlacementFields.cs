namespace CDOWin.Composers;

public enum PTextFieldName {
    // Placement Specific
    Position,
    //HireDate,
    Day1,
    Day2,
    Day3,
    Day4,
    Day5,
    JobDuties,
    WorkEnvironment,
    Accommodations,
    //Wages,
    Benefits,

    // Client Specific
    ClientName,
    SANumber,
    CaseID,

    // Employer Specific
    EmployerName,
    EmployerPhone,
    SupervisorName,
    SupervisorEmail,
    SupervisorPhone,
    Website,
    Address,
    City,
    State,
    Zip
}

public static class PTextField {
    public static PTextFieldName? Name(string name) => name switch {
        // Placement Specific
        "Customer's job title" => PTextFieldName.Position,
        //"" => PTextFieldName.HireDate,
        "Employment dates for the first 5 days worked, day 1:" => PTextFieldName.Day1,
        "Employment dates for the first 5 days worked, day 2:" => PTextFieldName.Day2,
        "Employment dates for the first 5 days worked, day 3:" => PTextFieldName.Day3,
        "Employment dates for the first 5 days worked, day 4:" => PTextFieldName.Day4,
        "Employment dates for the first 5 days worked, day 5:" => PTextFieldName.Day5,
        "Description of customer's job duties and responsibilities" => PTextFieldName.JobDuties,
        "Describe the customer's employment work setting and environment" => PTextFieldName.WorkEnvironment,
        "Describe any customer accommodations compensatory techniques andor training needs" => PTextFieldName.Accommodations,
        // "" => PTextFieldName.Wages,
        "Describe the customers employment benefits eg insurance vacation sick leave" => PTextFieldName.Benefits,

        // Client Specific
        "Customer Name" => PTextFieldName.ClientName,
        "Service Authorization Number" => PTextFieldName.SANumber,
        "VRS Case ID" => PTextFieldName.CaseID,

        // Employer Specific
        "Employer Name" => PTextFieldName.EmployerName,
        "Employer Main Phone Number" => PTextFieldName.EmployerPhone,
        "Supervisor's Name" => PTextFieldName.SupervisorName,
        "Supervisor's Email" => PTextFieldName.SupervisorEmail,
        "Supervisor's Phone Number(s)" => PTextFieldName.SupervisorPhone,
        "Employer Website" => PTextFieldName.Website,
        "Employer Street Address" => PTextFieldName.Address,
        "Employer City" => PTextFieldName.City,
        "Employer Zip" => PTextFieldName.Zip,
        _ => null
    };
}
