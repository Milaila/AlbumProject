using System.Data.Entity.ModelConfiguration;
using DAL.Entities;

namespace DAL.Configurations
{
    public class EvaluationConfiguration : EntityTypeConfiguration<Evaluation>
    {
        public EvaluationConfiguration()
        {
            Property(x => x.Mark)
                .IsRequired();
        }
    }
}
