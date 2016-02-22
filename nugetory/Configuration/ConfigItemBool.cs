using System;

namespace nugetory.Configuration
{
    public class ConfigItemBool : ConfigurationItem<bool>
    {
        public ConfigItemBool(ConfigurationParser configuration, ConfigItemDefaults<bool> defaults)
            : base(configuration, defaults)
        {
        }

        public override string GetValueToString(bool value)
        {
            return value.ToString();
        }

        public override bool GetValueFromString(string value)
        {
            try
            {
                return bool.Parse(value);
            }
            catch (Exception)
            {
                Value = DefaultValue;
                return Value;
            }
        }
    }
}
