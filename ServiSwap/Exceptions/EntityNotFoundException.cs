namespace ServiSwap.Exceptions;

public class EntityNotFoundException(string entityName, int entityId) : Exception($"{entityName} with ID {entityId} was not found.")
{
    public string EntityName { get; } = entityName;

    public int EntityId { get; } = entityId;
}