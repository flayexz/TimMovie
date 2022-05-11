using TimMovie.SharedKernel.Classes;

namespace TimMovie.Core;

public static class StoresForSupport
{
    public static readonly HashSet<(Guid supportId, string supportConectionId)> FreeSupports = new();
    public static readonly HashSet<(Guid? userId, string userConnectionId)> WaitingUsers = new();
    public static readonly BiDictionary<(Guid supportId, string supportConectionId), (Guid? userId, string userConnectionId)>
        CurrentCommunicationForSupport = new();
}