using System;

public static class Signals
{
    public static event Action TurbineBuilt;
    public static event Action PlayerKilled;

    public static void RaiseTurbineBuilt() => TurbineBuilt?.Invoke();
    public static void RaisePlayerKilled() => PlayerKilled?.Invoke();
}
