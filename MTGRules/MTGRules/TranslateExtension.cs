using MTGRules.Resources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MTGRules
{
    [ContentProperty(nameof(Key))]
    public class TranslateExtension : IMarkupExtension
    {
        public static Dictionary<String, ResourceManager> resourceManagersByName =
            new Dictionary<String, ResourceManager> {
                { "Main", MainResources.ResourceManager },
                { "About", AboutResources.ResourceManager }
            };

        public string Resources { get; set; }
        public string Key { get; set; }

        public TranslateExtension()
        {
            Resources = "Main";
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Key == null)
                return string.Empty;

            if (resourceManagersByName.TryGetValue(Resources, out ResourceManager resourceManager))
            {
                var translation = resourceManager.GetString(Key);

                if (translation == null)
                {
#if DEBUG
                    throw new ArgumentException(
                        string.Format("Key '{0}' was not found in resources '{1}'.", Key, Resources),
                        nameof(Key));
#else
                    translation = Key;
#endif
                }

                return translation;
            }
            else
            {
#if DEBUG
                throw new ArgumentException(
                    string.Format("Resources '{0}' was not found in resource managers.", Resources),
                    nameof(Resources));
#else
                return Key;
#endif
            }
        }
    }
}