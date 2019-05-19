using System;
using DAL.Abstracts;
using System.Collections.Generic;

namespace DAL.Entities
{
    public class Profile : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? ImageFileId { get; set; }
        public virtual File ImageFile { get; set; }
        public string UserId { get; set; }

        public virtual ICollection<Subscription> Subscribers { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }
        public virtual ICollection<Evaluation> Evaluations { get; set; }
        public virtual ICollection<Image> Images { get; set; }

        public Profile()
        {
            Subscriptions = new HashSet<Subscription>();
            Subscribers = new HashSet<Subscription>();
            Evaluations = new HashSet<Evaluation>();
            Images = new HashSet<Image>();
        }
    }
    
}
