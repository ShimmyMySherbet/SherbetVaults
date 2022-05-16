using System;
using System.Text.RegularExpressions;

namespace SherbetVaults.Models.Restrictions
{
    public class SelectorAttribute : Attribute
    {
        public string Selector { get; }
        public Regex Regex { get; }

        public SelectorAttribute(string regexSelector)
        {
            Selector = regexSelector;
        }
    }
}