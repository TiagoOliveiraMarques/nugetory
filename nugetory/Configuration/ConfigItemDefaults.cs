namespace nugetory.Configuration
{
    public class ConfigItemDefaults<T>
    {
        public ConfigItemDefaults(string section, string key, T defaultValue)
        {
            Section = section;
            Key = key;
            DefaultValue = defaultValue;
        }

        public string Section { get; set; }
        public string Key { get; set; }
        public T DefaultValue { get; set; }
    }
}
