using AutoMapper; // To use MapperConfiguration.
using Northwind.EntityModels; // To use Cart.
using Northwind.ViewModels; // TO use Summary.

namespace MappingObjects.Mappers;

public static class CartToSummaryMapper
{
    public static MapperConfiguration GetMapperConfiguration()
    {
        MapperConfiguration config = new(cfg =>
        {
            // Configure the mapper using projections.
            cfg.CreateMap<Cart, Summary>()
                // Map the first and last names formatted to the full name.
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src =>
                    string.Format($"{src.Customer.FirstName} {src.Customer.LastName}")
                ))
                // Map the sum of items to the Total member.
                .ForMember(dest => dest.Total, opt => opt.MapFrom(
                    src => src.Items.Sum(item => item.UnitPrice * item.Quantity)));
        });
        return config;
    }
}
