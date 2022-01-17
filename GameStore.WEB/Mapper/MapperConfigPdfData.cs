using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.WEB.Interfaces;
using GameStore.WEB.Models;

namespace GameStore.WEB.Mapper
{
    public class MapperConfigPdfData : Profile
    {
        public MapperConfigPdfData()
        {
            CreateMap<OrderViewModel, IPdfObjectData>()
                .ForMember(obj => obj.ColumnWidths,
                    options => options.MapFrom(order => new List<float> 
                        { 50, 70, 190, 50, 60, 60 }))
                .ForMember(obj => obj.ColumnNames,
                    options => options.MapFrom(order => new List<string> 
                        { "ID", "Item", "Description", "Quantity", "Unit Cost", "Total" }))
                .ForMember(obj => obj.Footer,
                    options => options.MapFrom(order => 
                        $"Total price: {order.TotalPrice:F2}$"))
                .ForMember(obj => obj.Headers,
                    options => options.MapFrom(order => new List<string> 
                        { $"Order# {order.Id}", $"{order.DateTime:g}" }))
                .ForMember(obj => obj.Rows,
                    options => options.MapFrom(order => order.OrderDetails.Select(od => new List<string>
                    {
                        od.Id.ToString(),
                        od.Product.Key,
                        od.Product.Name,
                        od.Quantity.ToString(),
                        $"{od.Product.Price:F2}$",
                        $"{od.Price:F2}$"
                    })));
        }
    }
}
