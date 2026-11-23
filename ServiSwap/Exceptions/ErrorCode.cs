namespace ServiSwap.Exceptions;

public enum ErrorCode
{
    InvalidUsername,
    InvalidEmail,
    InvalidStreetNumber,
    InvalidStreetName,
    InvalidCity,
    InvalidUsernameFormat,
    InvalidEmailFormat,
    InvalidPasswordFormat,
    InvalidServiceName,
    InvalidServiceDescription,
    InvalidStateTransition,
    RequesterCannotBeProvider,
    InvalidLastName,
    InvalidFirstName,
    NotEnoughCredits,
    ServiceAlreadyRequested,
    UserAlreadyExist,
    AddressNotFound,
    NotServiceProvider,
    NotServiceRequester,
    InvalidStateTransaction
}