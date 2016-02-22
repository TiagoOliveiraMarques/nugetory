using System;

namespace nugetory.Configuration
{
    public abstract class ConfigurationItem<T>
    {
        protected ConfigurationItem(ConfigurationParser configuration, ConfigItemDefaults<T> defaults)
        {
            Configuration = configuration;
            Section = defaults.Section;
            Key = defaults.Key;
            DefaultValue = defaults.DefaultValue;
        }

        protected ConfigurationParser Configuration { get; set; }
        public string Section { get; set; }
        public string Key { get; set; }
        public T DefaultValue { get; set; }

        public bool IgnoreEnvVar { get; set; }

        public T Value
        {
            get
            {
                string value = GetValue(Section, Key);
                return value != null ? GetValueFromString(value) : DefaultValue;
            }
            set
            {
                if (Configuration != null)
                {
                    Configuration.AddSetting(Section, Key, GetValueToString(value));
                    Configuration.SaveSettings();
                }
            }
        }

        public abstract string GetValueToString(T value);
        public abstract T GetValueFromString(string value);

        private string GetValue(string section, string key)
        {
            string res = IgnoreEnvVar ? null : Environment.GetEnvironmentVariable(section + "_" + key);
            if (res != null)
                return res;

            if (Configuration != null)
                res = Configuration.GetSetting(section, key);

            return res;
        }
    }
}
