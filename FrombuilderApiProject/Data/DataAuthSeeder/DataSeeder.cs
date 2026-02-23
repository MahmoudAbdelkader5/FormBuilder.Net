using FormBuilder.Infrastructure.Data;
using FormBuilder.Domian.Entitys.FormBuilder;
using FormBuilder.Domian.Entitys.FromBuilder;
using FormBuilder.Domian.Entitys.froms;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

public static class DataSeeder
{
    public static async Task SeedAsync(FormBuilderDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        // 1. Seed FIELD_TYPES أولاً (يجب أن يكون قبل FORM_BUILDER و FORM_FIELDS)
        if (!await context.FIELD_TYPES.AnyAsync())
        {
            var fieldTypes = new[]
            {
                new FIELD_TYPES
                {
                    TypeName = "Text",
                    DataType = "string",
                    MaxLength = 255,
                    HasOptions = false,
                    AllowMultiple = false,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                },
                new FIELD_TYPES
                {
                    TypeName = "TextArea",
                    DataType = "string",
                    MaxLength = 5000,
                    HasOptions = false,
                    AllowMultiple = false,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                },
                new FIELD_TYPES
                {
                    TypeName = "Number",
                    DataType = "decimal",
                    MaxLength = null,
                    HasOptions = false,
                    AllowMultiple = false,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                },
                new FIELD_TYPES
                {
                    TypeName = "Integer",
                    DataType = "int",
                    MaxLength = null,
                    HasOptions = false,
                    AllowMultiple = false,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                },
                new FIELD_TYPES
                {
                    TypeName = "Email",
                    DataType = "string",
                    MaxLength = 255,
                    HasOptions = false,
                    AllowMultiple = false,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                },
                new FIELD_TYPES
                {
                    TypeName = "Phone",
                    DataType = "string",
                    MaxLength = 20,
                    HasOptions = false,
                    AllowMultiple = false,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                },
                new FIELD_TYPES
                {
                    TypeName = "Date",
                    DataType = "DateTime",
                    MaxLength = null,
                    HasOptions = false,
                    AllowMultiple = false,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                },
                new FIELD_TYPES
                {
                    TypeName = "DateTime",
                    DataType = "DateTime",
                    MaxLength = null,
                    HasOptions = false,
                    AllowMultiple = false,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                },
                new FIELD_TYPES
                {
                    TypeName = "Time",
                    DataType = "TimeSpan",
                    MaxLength = null,
                    HasOptions = false,
                    AllowMultiple = false,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                },
                new FIELD_TYPES
                {
                    TypeName = "Dropdown",
                    DataType = "string",
                    MaxLength = null,
                    HasOptions = true,
                    AllowMultiple = false,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                },
                new FIELD_TYPES
                {
                    TypeName = "Radio",
                    DataType = "string",
                    MaxLength = null,
                    HasOptions = true,
                    AllowMultiple = false,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                },
                new FIELD_TYPES
                {
                    TypeName = "Checkbox",
                    DataType = "string",
                    MaxLength = null,
                    HasOptions = true,
                    AllowMultiple = false,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                },

                new FIELD_TYPES
                {
                    TypeName = "File",
                    DataType = "string",
                    MaxLength = null,
                    HasOptions = false,
                    AllowMultiple = false,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                },
               
                new FIELD_TYPES
                {
                    TypeName = "Password",
                    DataType = "string",
                    MaxLength = 255,
                    HasOptions = false,
                    AllowMultiple = false,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                },
               
                new FIELD_TYPES
                {
                    TypeName = "Boolean",
                    DataType = "bool",
                    MaxLength = null,
                    HasOptions = false,
                    AllowMultiple = false,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                },
                
                new FIELD_TYPES
                {
                    TypeName = "Calculated",
                    DataType = "decimal",
                    MaxLength = null,
                    HasOptions = false,
                    AllowMultiple = false,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                }
            };

            await context.FIELD_TYPES.AddRangeAsync(fieldTypes);
            await context.SaveChangesAsync();
        }

       
    }
}