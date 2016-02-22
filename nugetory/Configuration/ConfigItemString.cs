namespace nugetory.Configuration
{
    public class ConfigItemString : ConfigurationItem<string>
    {
        public ConfigItemString(ConfigurationParser configuration, ConfigItemDefaults<string> defaults)
            : base(configuration, defaults)
        {
        }

        public override string GetValueToString(string value)
        {
            return value;
        }

        public override string GetValueFromString(string value)
        {
            if (value != null)
                return value;

            Value = DefaultValue;
            return Value;
        }
    }
}
