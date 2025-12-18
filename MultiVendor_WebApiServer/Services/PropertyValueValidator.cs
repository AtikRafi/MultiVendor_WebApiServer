using MultiVendor_WebApiServer.Models;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace MultiVendor_WebApiServer.Services
{
    public static class PropertyValueValidator
    {
        public static void ValidatePropertyValue(CategoryProperty prop, string? value)
        {

            if (value is null)
                throw new ValidationException($"{prop.Name} cannot be null.");

            switch (prop.DataType)
            {
                case PropertyDataType.String:
                    if (string.IsNullOrWhiteSpace(value))
                        throw new ValidationException($"{prop.Name} cannot be empty.");
                    break;

                case PropertyDataType.Boolean:
                    if (!bool.TryParse(value, out _))
                        throw new ValidationException($"{prop.Name} must be true or false.");
                    break;

                case PropertyDataType.Date:
                    if (!DateTime.TryParse(value, CultureInfo.InvariantCulture, out _))
                        throw new ValidationException($"{prop.Name} must be a valid date.");
                    break;

                case PropertyDataType.Number:
                    if (!decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
                        throw new ValidationException($"{prop.Name} must be a valid number.");
                    break;
            }
        }

    }
}
