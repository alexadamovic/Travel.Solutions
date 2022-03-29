using System;
using System.Text.Json.Serialization;

namespace Travel.Models
{
  public class Review
  {
    public int ReviewId { get; set; }
    public string Body { get; set; }
    public int Rating { get; set; }
    public DateTime Date { get; set; }
    public int DestinationId { get; set; }

    [JsonIgnore]
    public virtual Destination Destination { get; set; }
    
  }
}