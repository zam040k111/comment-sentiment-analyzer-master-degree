using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace GameStore.BLL.Services.Validation
{
    public static class ValidationHelper
    {
        private static readonly Dictionary<string, Dictionary<string, string>> ValidationMessage =
            new Dictionary<string, Dictionary<string, string>>
            {
                {
                    "GameDto",
                    new Dictionary<string, string>
                    {
                        {"Key", "The Key field must be unique."},
                        {"PublisherId", "The Publisher field is invalid"},
                        {"GamePlatformTypesId", "The Platform types field is invalid"},
                        {"GameGenresId", "The Genres field is invalid"}
                    }
                },
                {
                    "GenreDto",
                    new Dictionary<string, string>
                    {{"Name", "With the same Parent genre the Name field must be unique."}}
                },
                {
                    "PlatformTypeDto",
                    new Dictionary<string, string>
                    {{"Type", "Platform type with the same name already exists."}}
                },
                {
                    "PublisherDto",
                    new Dictionary<string, string>
                    {{"CompanyName", "Publisher with the same company name already exists."}}
                },
                {
                    "OrderDto",
                    new Dictionary<string, string>
                    {
                        {"OrderDetails", "There are no items in the order. Order should contains at least one item."},
                        {"TotalPrice", "Not enough units in stock to satisfy the order."}
                    }
                }
            };

        public static string GetMessage<TServiceDto, TProperty>(this TServiceDto entity, Expression<Func<TServiceDto, TProperty>> prop)
            => ValidationMessage[entity.GetType().Name][prop.GetPropertyAccess().Name];

        public static string GetPropName<TServiceDto, TProperty>(this TServiceDto entity, Expression<Func<TServiceDto, TProperty>> prop)
            => prop.GetPropertyAccess().Name;
    }
}
