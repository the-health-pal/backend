using System;

namespace health_pal_backend.Models;

public class UserBioDataModel
{
    public int Id { get; set; }
    public DateOnly BirthDay { get; set; }
    public float Weight { get; set; }
    public float Height { get; set; }
}
