using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order;

namespace Talabat.Repository.Data.Configrations
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(O => O.SubTotal).HasColumnType("decimal(18,2)");
            builder.Property(O => O.Statues)
                   .HasConversion(OStatus => OStatus.ToString(), OStatues =>(OrderStatues) Enum.Parse(typeof(OrderStatues), OStatues));

            builder.OwnsOne(O => O.ShippingAddress, SA => SA.WithOwner());

            builder.HasOne(O => O.DeliveryMethod).WithMany().OnDelete(DeleteBehavior.NoAction);

        }
    }
}
