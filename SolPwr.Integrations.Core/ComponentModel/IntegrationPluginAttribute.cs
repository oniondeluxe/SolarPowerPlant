using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OnionDlx.SolPwr.ComponentModel
{
    public class IntegrationPluginAttribute : Attribute
    {
        readonly string _pluginIdentifier;
        readonly Type _pluginType;

        public string PluginIdentifier
        {
            get { return _pluginIdentifier; }
        }


        public Type PluginType
        {
            get { return _pluginType; }
        }


        public static IntegrationPluginAttribute FromAssembly(CustomAttributeData attrib)
        {
            if (attrib.AttributeType == typeof(IntegrationPluginAttribute))
            {
                var arg0 = attrib.ConstructorArguments[0];
                var arg1 = attrib.ConstructorArguments[1];
                var arg2 = attrib.ConstructorArguments[2];

                // This will be loaded by the plugin FW
                var loadingType = arg0.Value as Type;
                return new IntegrationPluginAttribute(loadingType, (string)arg1.Value, (string)arg2.Value);
            }

            return null;
        }


        public IntegrationPluginAttribute(Type implementation, string pluginIdentifier, string description)
        {
            _pluginIdentifier = pluginIdentifier;
            _pluginType = implementation;
        }
    }
}
