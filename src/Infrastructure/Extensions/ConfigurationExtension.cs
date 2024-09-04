using Core.Base;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Facades.Common.Extensions;

public static class ConfigurationExtension
{
    public static EntityTypeBuilder<T> UnderscoreTable<T>(this EntityTypeBuilder<T> builder)
        where T : class
        => builder.ToTable(typeof(T).Name.Underscore());

    public static EntityTypeBuilder<T> HasBaseEntity<T>(this EntityTypeBuilder<T> builder)
        where T : BaseEntity
    {
        builder.HasKey(x => x.Id);
        return builder;
    }
}