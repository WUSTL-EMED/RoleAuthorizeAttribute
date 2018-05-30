using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

//TODO: Config transform to add section?
namespace RoleAuthorize.Config
{
    public static class RoleConfig
    {
        private static readonly char[] Delimiter = new char[] { ',' };

        public static IEnumerable<string> GetRoles(string name)
        {
            var config = GetConfig(name);
            if (config == null)
                return Enumerable.Empty<string>();
            return config.Roles.Split(Delimiter).Select(_ => _.Trim()).Where(_ => !string.IsNullOrEmpty(_));
        }

        public static IEnumerable<string> GetUsers(string name)
        {
            var config = GetConfig(name);
            if (config == null)
                return Enumerable.Empty<string>();
            return config.Users.Split(Delimiter).Select(_ => _.Trim()).Where(_ => !string.IsNullOrEmpty(_));
        }

        public static bool DefaultAllow
        {
            get
            {
                var config = ConfigurationManager.GetSection(RoleSettingsSection.SectionName) as RoleSettingsSection;
                if (config == null)
                    return false;
                return config.DefaultAllow;
            }
        }

        public static bool Authenticated403
        {
            get
            {
                var config = ConfigurationManager.GetSection(RoleSettingsSection.SectionName) as RoleSettingsSection;
                if (config == null)
                    return false;
                return config.Authenticated403;
            }
        }

        private static RoleConfigElement GetConfig(string name)
        {
            var config = ConfigurationManager.GetSection(RoleSettingsSection.SectionName) as RoleSettingsSection;
            if (config == null)
                throw new ConfigurationErrorsException(Resource.ConfigNotFound);
            return config.Roles[name];
        }
    }

    [SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface", Justification = "ConfigurationElementCollection is the recommended base class for collections of ConfigurationElement objects.")]
    internal class RoleCollection : ConfigurationElementCollection
    {
        internal const string _ElementName = "role";

        public RoleCollection()
        {
            //ServerConfigElement details = (ServerConfigElement)CreateNewElement();
            //if (!string.IsNullOrEmpty(details.Name))
            //    BaseAdd(details, false);
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        protected override string ElementName
        {
            get
            {
                return _ElementName;
            }
        }

        public RoleConfigElement this[int index]
        {
            get
            {
                return (RoleConfigElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);
                BaseAdd(index, value);
            }
        }

        new public RoleConfigElement this[string name]
        {
            get
            {
                return (RoleConfigElement)BaseGet(name);
            }
        }

        public void Add(RoleConfigElement details)
        {
            BaseAdd(details);
        }

        public void Clear()
        {
            BaseClear();
        }

        public int IndexOf(RoleConfigElement details)
        {
            return BaseIndexOf(details);
        }

        public void Remove(RoleConfigElement details)
        {
            if (details == null)
                throw new ArgumentNullException(nameof(details));

            if (BaseIndexOf(details) >= 0)
                BaseRemove(details.Name);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new RoleConfigElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((RoleConfigElement)element).Name;
        }
    }

    internal class RoleConfigElement : ConfigurationElement
    {
        private const string _Name = "name";
        private const string _Roles = "roles";
        private const string _Users = "users";

        [ConfigurationProperty(_Name, IsKey = true, IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)this[_Name];
            }
        }

        [ConfigurationProperty(_Roles, DefaultValue = default(string))]
        public string Roles
        {
            get
            {
                return (string)this[_Roles];
            }
        }

        [ConfigurationProperty(_Users, DefaultValue = default(string))]
        public string Users
        {
            get
            {
                return (string)this[_Users];
            }
        }
    }

    internal class RoleSettingsSection : ConfigurationSection
    {
        public const string SectionName = "roleAuthorize/authorizationRoles";
        private const string _DefaultAllow = "defaultAllow";
        private const string _Authenticated403 = "authenticated403";

        [ConfigurationProperty(_DefaultAllow, IsRequired = false, DefaultValue = false)]
        public bool DefaultAllow
        {
            get
            {
                return (bool)base[_DefaultAllow];
            }
        }

        [ConfigurationProperty(_Authenticated403, IsRequired = false, DefaultValue = false)]
        public bool Authenticated403
        {
            get
            {
                return (bool)base[_Authenticated403];
            }
        }

        [ConfigurationProperty("", IsDefaultCollection = true, IsRequired = true)]
        [ConfigurationCollection(typeof(RoleCollection), AddItemName = RoleCollection._ElementName)]
        public RoleCollection Roles
        {
            get
            {
                return (RoleCollection)base[string.Empty];
            }
        }

        protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
        {
            switch (name)
            {
                case "xmlns":
                case "xmlns:xsi":
                case "xsi:noNamespaceSchemaLocation":
                    return true;
                default:
                    return base.OnDeserializeUnrecognizedAttribute(name, value);
            }
        }
    }
}