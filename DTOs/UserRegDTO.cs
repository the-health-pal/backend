using System.ComponentModel.DataAnnotations;

namespace health_pal_backend.DTOs;

public record class UserRegDTO
(
    [Required]string Name,
    [Required]string Email,
    [Required]string Password
);
