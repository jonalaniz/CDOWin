using System;
using System.Collections.Generic;

namespace CDOWin.Data;

public class CachedList<T> {
    public IReadOnlyList<T>? Data { get; private set; }
    public DateTime LastUpdated { get; private set; } = DateTime.MinValue;

    public bool IsStale(TimeSpan ttl) => Data == null || DateTime.UtcNow - LastUpdated > ttl;

    public void Update(IReadOnlyList<T> data) {
        Data = data;
        LastUpdated = DateTime.UtcNow;
    }
}
