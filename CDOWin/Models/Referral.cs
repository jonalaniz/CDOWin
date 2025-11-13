using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDOWin.Models {
    public record class Referral(
        string id,
        int? placementNumber,
        string? employerID,
        int? clientID,
        int? counselorID,
        string? poNumber,
        string? supervisor,
        string? supervisorEmail,
        string? supervisorPhone,
        string? position,
        string? salary,
        float? daysOnJob,
        string? clientName,
        string? counselorName,
        bool? active,
        string? website,
        string? descriptionOfDuties,
        string? numbersOfHoursWorking,
        string? firstFiveDays1,
        string? firstFiveDays2,
        string? firstFiveDays3,
        string? firstFiveDays4,
        string? firstFiveDays5,
        string? descriptionOfWorkSchedule,
        string? hourlyOrMonthlyWages,
        DateTime? hireDate,
        DateTime? endDate
        ) {
        public DateTime? hireDateLocal => hireDate?.ToLocalTime();
        public DateTime? endDateLocal => endDate?.ToLocalTime();
    }
}
