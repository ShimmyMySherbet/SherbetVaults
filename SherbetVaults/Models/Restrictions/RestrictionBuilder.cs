﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Rocket.Core.Logging;
using SherbetVaults.Models.Config.Restrictions;

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

        public List<VaultRestrictionGroup> BuildGroups(RestrictionSettings settings, out int errors)
        {
            var err = 0;
            var groups = new List<VaultRestrictionGroup>();
            foreach (var group in settings.Groups)
            {
                var restrictors = new List<IItemRestrictor>();
                foreach (var selector in group.Selectors)
                {
                    try
                    {
                        var restrictor = Build(selector);
                        restrictors.Add(restrictor);
                    }
                    catch (BadSelectorException ex)
                    {
                        err++;
                        Logger.LogError($"Bad Item Selector in Restriction Group {group.GroupID}: {ex.Message}");
                    }
                    catch (Exception exc)
                    {
                        err++;
                        Logger.LogError($"Exception building restrictor '{selector}'");
                        Logger.LogError(exc.Message);
                        Logger.LogError(exc.StackTrace);
                    }
                }

                groups.Add(new VaultRestrictionGroup(group, restrictors));
            }
            errors = err;
            return groups;
        }

        public IItemRestrictor Build(string selector)
        {
            var restrictor = Restrictors.FirstOrDefault(x => x.selector.IsMatch(selector));

            if (restrictor.type == null)
            {
                throw new BadSelectorException($"Unknown selector format '{selector}'");
            }

            return Instantiate(restrictor.type, selector, Plugin);
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
                try
                {
                    return (IItemRestrictor)construct.Invoke(arguments);
                }
                catch (TargetInvocationException ex)
                {
                    if (ex.InnerException != null)
                    {
                        throw ex.InnerException;
                    }
                    throw;
                }
            }
            return null;
        }
    }
}