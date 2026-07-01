using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CDO.Core.DTOs.Admin;

public record class ClientActivity(
    string Id,
    DateTime Date,
    string UserName,
    string UserID,
    string Action
    );