namespace CDOWin.Views.Clients.Dialogs;

public enum ClientEditType {
    Personal,
    Case,
    Employment,
    Conditions,
    Contact
}

public enum PersonalField {
    FirstName,
    LastName,
    DL,
    SSN,
    Languages,
    Race,
    Address1,
    Address2,
    City,
    Zip,
    Education
}

public enum CaseField {
    CaseID,
    Status,
    Benefit,
    Premiums
}

public enum EmploymentField {
    Disability,
    CriminalCharge,
    Transportation
}

public enum ArrangementsField {
    EmploymentGoal,
    Conditions
}

public enum ContactField {
    Phone1,
    Phone1Identity,
    Phone2,
    Phone2Identity,
    Phone3,
    Phone3Identity,
    Email,
    EmailIdentity,
    Email2,
    Email2Identity
}

public enum CheckboxTag {
    ResumeRequired,
    ResumeCompleted,
    VideoInterviewRequired,
    VideoInterviewCompleted,
    ReleasesCompleted,
    OrientationCompleted,
    DataSheetCompleted,
    ElevatorSpeechCompleted
}