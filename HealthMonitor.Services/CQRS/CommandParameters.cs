namespace HealthMonitor.Services.CQRS
{
    /// <summary>
    /// Represents the parameters required for executing a commands.
    /// </summary>
    /// <typeparam name="T">The type of data associated with the command.</typeparam>
    public class CommandParameters<T>
    {
        /// <summary>
        /// Gets or sets the user ID or network ID associated with the command.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the page or component name where the command is being triggered.
        /// </summary>
        public string? PageOrComponent { get; set; }

        /// <summary>
        /// Gets or sets a key that can be used as a tag to aid in searching with in logs files.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the message or additional details about the command's purpose.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the original data before the command modifies it.
        /// </summary>
        public T OriginalData { get; set; }

        /// <summary>
        /// Gets or sets the new data or payload associated with the command.
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether audit logging should be performed for this command.
        /// </summary>
        public bool AuditLog { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandParameters{T}"/> class.
        /// </summary>
        public CommandParameters()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandParameters{T}"/> class with specified data.
        /// </summary>
        /// <param name="data">The data for the command.</param>
        /// <param name="auditLog">Indicates whether to enable audit logging.</param>
        public CommandParameters(T data, bool auditLog = false)
        {
            Data = data;
            AuditLog = auditLog;
        }
    }
}
