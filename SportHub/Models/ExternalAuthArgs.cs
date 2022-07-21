using SportHub.Services.Exceptions.ExternalAuthExceptions;
using System;
using System.ComponentModel.DataAnnotations;

namespace SportHub.Models
{
    public class ExternalAuthArgs
    {
        [Required]
        public string UserToken { get; set; }

        [Required]
        public string AuthProvider { get; set; }

        [Required]
        public bool IsCreationRequired { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [RegularExpression(@"^[a-zA-Z''-'\s]+$", ErrorMessage = "Such characters are not allowed.")]
        [MaxLength(100)]
        [MinLength(1)]
        public string? FirstName { get; set; }

        [RegularExpression(@"^[a-zA-Z''-'\s]+$", ErrorMessage = "Such characters are not allowed.")]
        [MaxLength(100)]
        [MinLength(1)]
        public string? LastName { get; set; }
    }
}
