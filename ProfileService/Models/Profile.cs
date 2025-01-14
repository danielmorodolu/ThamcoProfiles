using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProfileService.Models
{
    /// <summary>
    /// Represents a user profile in the system.
    /// </summary>
    public class Profile
    {
        /// <summary>
        /// Gets or sets the unique identifier for the profile.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the first name of the user.
        /// </summary>
        /// <remarks>
        /// The first name must be between 2 and 50 characters.
        /// </remarks>
        [Display(Name = "First Name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First Name must be between 2 and 50 characters.")]
        public string? FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the user.
        /// </summary>
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user. This field is required.
        /// </summary>
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public required string Email { get; set; }

        /// <summary>
        /// Gets or sets the payment address of the user.
        /// </summary>
        /// <remarks>
        /// The address must be up to 500 characters long.
        /// </remarks>
        [StringLength(500, ErrorMessage = "Address must be up to 500 characters long.")]
        [Display(Name = "Payment Address")]
        public string? PaymentAddress { get; set; }

        /// <summary>
        /// Gets or sets the password for the user. This field is required.
        /// </summary>
        /// <remarks>
        /// The password must be between 8 and 100 characters and include at least:
        /// one uppercase letter, one lowercase letter, one number, and one special character.
        /// </remarks>
        [Required]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", 
            ErrorMessage = "Password must have at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the user.
        /// </summary>
        /// <remarks>
        /// The phone number must be in international format (e.g., +1234567890).
        /// </remarks>
        [Phone]
        [RegularExpression(@"^\+?[1-9]\d{10,14}$", ErrorMessage = "The phone number must be in international format (e.g., +1234567890).")]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the Auth0 user ID for the user.
        /// </summary>
        [Column(TypeName = "nvarchar(255)")]
        public string? Auth0UserId { get; set; }
    }
}