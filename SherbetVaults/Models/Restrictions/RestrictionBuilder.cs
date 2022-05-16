using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace SherbetVaults.Models.Restrictions
{
    public class RestrictionBuilder
    {
        public SherbetVaultsPlugin Plugin;

        public List<(Regex selector, Type type)> Restrictors { get; } = new List<(Regex selector, Type t)>();

        public RestrictionBuilder(SherbetVaultsPlugin plugin)
        {
            Plugin = plugin;
        }

        public void Init()
        {
            var myTypes = typeof(RestrictionBuilder).Assembly.GetExportedTypes();

            foreach (var type in myTypes.Where(x => typeof(IItemRestrictor).IsAssignableFrom(x)))
            {
                if (type.IsInterface || type.IsAbstract)
                {
                    continue;
                }

                var selectorTag = type.GetCustomAttribute<SelectorAttribute>();
                if (selectorTag == null)
                {
                    continue;
                }

                Restrictors.Add((selectorTag.Regex, type));
            }
        }

        public IItemRestrictor Build(string selector)
        {
            var matchingType = Restrictors.FirstOrDefault(x => x.selector.IsMatch(selector));

            return Instantiate(matchingType.type, selector, Plugin);
        }

        public IItemRestrictor Instantiate(Type t, string selector, SherbetVaultsPlugin plugin)
        {
            var constructors = t.GetConstructors();

            var availableObjects = new object[]
            {
                selector,
                plugin,
                plugin.Database,
                plugin.Config,
                plugin.ItemTable,
                plugin.VaultManager
            };

            foreach (var construct in constructors)
            {
                var valid = true;
                var parameters = construct.GetParameters();
                var arguments = new object[parameters.Length];

                for (int i = 0; i < parameters.Length; i++)
                {
                    var instance = availableObjects.FirstOrDefault
                        (x => parameters[i].ParameterType.IsAssignableFrom(x.GetType()));

                    if (instance == null)
                    {
                        valid = false;
                        break;
                    }
                    arguments[i] = instance;
                }

                if (!valid)
                {
                    continue;
                }

                return (IItemRestrictor)construct.Invoke(arguments);
            }

            return null;
        }
    }
}