namespace ServiSwap.Exceptions;

public class BusinessException(ErrorCode errorCode, string message) : Exception(message)
{
    public ErrorCode ErrorCode { get; } = errorCode;
}