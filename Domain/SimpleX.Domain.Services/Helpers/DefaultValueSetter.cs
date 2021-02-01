using Microsoft.Extensions.Logging;
using SimpleX.Data.Entities;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SimpleX.Domain.Services.Helpers
{
    public class DefaultValueSetter
    {
        private readonly ILogger _logger;

        public DefaultValueSetter(
            ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory?.CreateLogger<DefaultValueSetter>() ??
                throw new ArgumentNullException(nameof(loggerFactory));
                
        }

        public void SetValues(List<DefaultValue> defaults, object dto)
        {
            try
            {
                foreach (var def in defaults)
                {
                    PropertyInfo prop = dto.GetType().GetProperty(def.PropertyName, BindingFlags.Public | BindingFlags.Instance);
                    if (null != prop && prop.CanWrite)
                    {
                        try
                        {
                            //var val = Convert.ChangeType(def.PropertyValue, Type.GetType(def.PropertyType, false, true));
                            var val = Convert.ChangeType(def.PropertyValue, prop.PropertyType);
                            prop.SetValue(dto, val, null);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning($"Could not set {dto.GetType().Name} property '{def.PropertyName}' to '{def.PropertyValue}' ({def.PropertyType}){Environment.NewLine}{ex.Message}");
                            throw;
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
