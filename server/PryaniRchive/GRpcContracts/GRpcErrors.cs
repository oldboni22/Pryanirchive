using Common.ResultPattern;

namespace GRpcContracts;

public static class GRpcErrors
{
    public static readonly Error EmptyResponse = 
        new Error("GRpc.Empty", "The GRpc response is empty.", ErrorType.NotFound);
}
