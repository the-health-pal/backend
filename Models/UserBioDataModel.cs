using System;
using Newtonsoft.Json;

namespace health_pal_backend.Models;

public class UserBioDataModel
{
    [JsonProperty("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    [JsonProperty("birthday")]
    public DateOnly BirthDay { get; set; }
    [JsonProperty("weight")]
    public float Weight { get; set; }
    [JsonProperty("height")]
    public float Height { get; set; }
}
