using Common.ResultPattern;

namespace GRpcContracts;

public static class GRpcErrors
{
    public static readonly Error GRpcResponseEmpty = 
        new Error("GRpc.NoResponce", "The gRPC response is empty.", ErrorType.Exception);
}