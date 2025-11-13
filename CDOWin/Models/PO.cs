using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDOWin.Models {
    public record class Po(
        string id,
        int clientID,
        string description,
        string? office,
        int? employerID,
        double? unitCost,
        string? unitOfMeasurement,
        DateTime startDate,
        DateTime endDate
        ) {
        public DateTime? startDateLocal => startDate.ToLocalTime();
        public DateTime? endDateLocal => endDate.ToLocalTime();
    }
}
