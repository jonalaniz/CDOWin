using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDOWin.Models {
    public record class Counselor(
        int id,
        string name,
        string? email,
        string? phone,
        string? fax,
        string? notes,
        string? secretaryName,
        string? secretaryEmail
        );
}
