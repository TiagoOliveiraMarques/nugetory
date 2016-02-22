using System;

namespace nugetory.Configuration
{
    public class ConfigItemInt : ConfigurationItem<int>
    {
        public ConfigItemInt(ConfigurationParser configuration, ConfigItemDefaults<int> defaults)
            : base(configuration, defaults)
        {
        }

        public override string GetValueToString(int value)
        {
            return value.ToString();
        }

        public override int GetValueFromString(string value)
        {
            try
            {
                return int.Parse(value);
            }
            catch (Exception)
            {
                Value = DefaultValue;
                return Value;
            }
        }
    }
}
