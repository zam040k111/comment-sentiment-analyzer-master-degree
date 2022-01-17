using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;

namespace GameStore.WEB.Extensions
{
    public static class FormFileExtensions
    {
        public static void FileToBase64<TModel>(this IFormFile formFile, Expression<Func<TModel, string>> propTarget, TModel model)
        {
            if (formFile != null)
            {
                using (BinaryReader br = new BinaryReader(formFile.OpenReadStream()))
                {
                    var bytes = br.ReadBytes((int)formFile.Length);

                    if (propTarget.Body is MemberExpression bodyMember)
                    {
                        if (bodyMember.Member is PropertyInfo property)
                        {
                            property.SetValue(model, Convert.ToBase64String(bytes), null);
                        }
                    }
                }
            }
        }
    }
}
