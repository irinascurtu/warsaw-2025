using MassTransit;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Domain.Saga
{
    //public class OrderStateMap : IEntityTypeConfiguration<SagaOrderState>
    //{
    //    //public void Configure(EntityTypeBuilder<SagaOrderState> builder)
    //    //{
    //    //    builder.HasKey(x => x.OrderId);

    //    //    builder.Property(x => x.CurrentState).HasMaxLength(64);
    //    //    builder.Property(x => x.OrderId);
    //    //    builder.Property(x => x.Amount);
    //    //    builder.Property(x => x.CreatedAt);
    //    //    builder.Property(x => x.PaidAt);
    //    //    builder.Property(x => x.CancelledAt);

    //    //    // Optionally configure other aspects like indexes
    //    //    builder.HasIndex(x => x.OrderId);
    //    //}

    //    //protected override void Configure(EntityTypeBuilder<OrderState> entity, ModelBuilder model)
    //    //{
    //    //    entity.Property(x => x.CurrentState).HasMaxLength(64);
    //    //    entity.Property(x => x.OrderId);
    //    //    entity.Property(x => x.Amount);
    //    //    entity.Property(x => x.CreatedAt);
    //    //    entity.Property(x => x.PaidAt);
    //    //    entity.Property(x => x.CancelledAt);
    //    //}
    //}
}
