using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDOWin.Models {
    public record class Employer(
        int id,
        string? name,
        string? address1,
        string? address2,
        string? city,
        string? state,
        string? zip,
        string? phone,
        string? fax,
        string? email,
        string? website,
        string? notes,
        string? supervisor,
        string? supervisorPhone,
        string? supervisorEmail
        );
}
