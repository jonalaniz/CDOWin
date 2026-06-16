using System;
using System.Collections.Generic;
using System.Text;

namespace CDO.Core.DTOs.Admin; 
public class AdminClientNote() {
    required public int Id { get; init; }
    required public string ClientName { get; init; }
    required public int ClientId { get; init; }
    required public DateTime Date { get; init; }
    required public string Note { get; init; }
    public string? Author { get; init; }
}
