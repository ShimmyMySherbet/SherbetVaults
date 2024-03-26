using System.Collections.Generic;

namespace SherbetVaults.Models.Config.Restrictions
{
    public class RestrictionSettings
    {
        public bool AdminsBypassRestrictions = true;
        public bool ShowMessages = true;
        public bool Enabled = false;

        public List<RestrictionGroup> Groups = new();

        public static RestrictionSettings Default
        {
            get
            {
                return new RestrictionSettings()
                {
                    Groups = new List<RestrictionGroup>()
                    {
                        new()
                        {
                            GroupID = "Group1",
                            Selectors = new List<string>()
                            {
                                "1",
                                "255-259",
                                "Type:Throwable",
                                "Table:Police*",
                                "Slot:Primary",
                                "Workshop:2136497468",
                            }
                        }
                    }
                };
            }
        }
    }
}