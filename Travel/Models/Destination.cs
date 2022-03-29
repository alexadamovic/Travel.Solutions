using System.Collections.Generic;

namespace Travel.Models
{
  public class Destination
  {
    public Destination()
    {
      this.Reviews = new HashSet<Review>();
    }
    public int DestinationId { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public double AverageRating { get; set; }
    public virtual ICollection<Review> Reviews { get; set; }

    public void FindAverage()
    {
      double totalScore = 0;
      double totalReviews = 0;
      foreach (Review review in this.Reviews)
      {
        totalScore += review.Rating;
        totalReviews ++;
      }
      double total = totalScore / totalReviews;
      this.AverageRating = total;

    }
  }
}