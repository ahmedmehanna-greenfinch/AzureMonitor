namespace AzureMonitor.Interfaces
{
    /// <summary>
    ///     Provide an individual identifier for the class.
    ///     The return format should mirror the column order, if applicable.
    /// </summary>
    public interface IModel
    {
        object Id { get; }
    }
}
