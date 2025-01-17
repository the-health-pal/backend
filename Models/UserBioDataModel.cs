using System;
using Newtonsoft.Json;

namespace health_pal_backend.Models;

public class UserBioDataModel
{
    [JsonProperty("userid")]
    public int UserId { get; set; }
    [JsonProperty("birthday")]
    public DateOnly BirthDay { get; set; }
    [JsonProperty("weight")]
    public float Weight { get; set; }
    [JsonProperty("height")]
    public float Height { get; set; }
}
