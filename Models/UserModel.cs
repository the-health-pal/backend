using System;

namespace health_pal_backend.Models;

public class UserModel
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}
