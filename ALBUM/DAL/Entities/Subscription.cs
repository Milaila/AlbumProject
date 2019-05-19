using DAL.Abstracts;

namespace DAL.Entities
{
    public class Subscription : Entity
    {
        public int SubscriberProfileId { get; set; }
        public virtual Profile SubscriberProfile { get; set; }
        public int SubscriptionProfileId { get; set; }
        public virtual Profile SubscriptionProfile { get; set; }
    }
}
