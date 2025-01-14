namespace ProfileService.Models
{
    /// <summary>
    /// Represents the error details to be displayed in the application views.
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the request where the error occurred.
        /// </summary>
        public string? RequestId { get; set; }

        /// <summary>
        /// Gets a value indicating whether the RequestId should be shown.
        /// </summary>
        /// <remarks>
        /// This property evaluates whether the <see cref="RequestId"/> is null or empty.
        /// </remarks>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}