using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Protection.PlayReady;

namespace CDOWin.Models {
    public record class Reminder(
         int id,
         int clientID,
         string? clientName,
         string description,
         bool complete,
         DateTime date
         ) {
        public DateTime localDate => date.ToLocalTime();
    }
}
