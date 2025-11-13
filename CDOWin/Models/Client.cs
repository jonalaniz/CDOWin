using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDOWin.Models {
    public record class Client(
        int id,
        string firstName,
        string lastName,
        string counselor,
        Reminder[] reminders,
        DateTime? startDate,
        string? ssn,
        string? caseID,
        string? address1,
        string? address2,
        string city,
        string state,
        string? zip,
        DateTime? dob,
        string? driversLicense,
        string? phone1,
        string? phone1Identity,
        string? phone2,
        string? phone2Identity,
        string? phone3,
        string? phone3Identity,
        string? email,
        string? emailIdentity,
        string? email2,
        string? email2Identity,
        string disability,
        int? counselorID,
        string? counselorEmail,
        string? counselorPhone,
        string? counselorFax,
        string? clientNotes,
        string? conditions,
        string? documentFolder,
        bool? active,
        string? employmentGoal,
        int? employerID,
        string? status,
        string? benefit,
        string? criminalCharge,
        string? education,
        string? transportation,
        bool? resumeRequired,
        bool? resumeCompleted,
        bool? videoInterviewRequired,
        bool? videoInterviewCompleted,
        bool? releasesCompleted,
        bool? orientationCompleted,
        bool? dataSheetCompleted,
        bool? elevatorSpeechCompleted,
        string? race,
        string? fluentLanguages,
        string? premiums
        ) {
        public DateTime? startDateLocal => startDate?.ToLocalTime();
        public DateTime? doblocal => dob?.ToLocalTime();
    }
}
