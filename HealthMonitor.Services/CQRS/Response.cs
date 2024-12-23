namespace HealthMonitor.Services.CQRS
{
    /// <summary>
    /// Represents a standardized response structure for API responses and command/query results.
    /// </summary>
    /// <typeparam name="T">The type of the data being returned in the response.</typeparam>
    public class Response<T>
    {
        /// <summary>
        /// Gets or sets a value indicating whether the operation was successful.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Gets or sets the data being returned as part of the response.
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Gets or sets the list of errors if the operation was unsuccessful.
        /// </summary>
        public List<ResponseError> Errors { get; set; } = new List<ResponseError>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Response{T}"/> class.
        /// </summary>
        public Response()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Response{T}"/> class with success status and optional data.
        /// </summary>
        /// <param name="data">The data to include in the response.</param>
        /// <param name="isSuccess">Indicates whether the operation was successful.</param>
        public Response(T data, bool isSuccess = true)
        {
            Data = data;
            IsSuccess = isSuccess;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Response{T}"/> class with success status, optional data, and errors.
        /// </summary>
        /// <param name="data">The data to include in the response.</param>
        /// <param name="isSuccess">Indicates whether the operation was successful.</param>
        /// <param name="errors">The list of errors.</param>
        public Response(T data, bool isSuccess, List<ResponseError> errors)
        {
            Data = data;
            IsSuccess = isSuccess;
            Errors = errors;
        }
    }

    /// <summary>
    /// Represents an error in a response.
    /// </summary>
    public class ResponseError
    {
        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the error code (optional).
        /// </summary>
        public string Code { get; set; }
    }
}
