﻿using System;
using System.Collections.Generic;

namespace GitCommands.Config
{
    /// <summary>
    ///   ConfigSection
    ///   Sections can be defined as:
    ///   [section "subsection"] (subsection is case sensitive)
    ///   or
    ///   [section.subsection] (subsection is case insensitive)
    /// </summary>
    public class ConfigSection
    {
        internal ConfigSection(string name)
        {
            Keys = new Dictionary<string, IList<string>>();

            if (name.Contains("\"")) //[section "subsection"] case sensitive
            {
                SectionName = name.Substring(0, name.IndexOf('\"')).Trim();
                SubSection = name.Substring(name.IndexOf('\"') + 1, name.LastIndexOf('\"') - name.IndexOf('\"') - 1);
            }
            else if (!name.Contains("."))
            {
                SectionName = name.Trim(); // [section] case sensitive
            }
            else
            {
                var subSectionIndex = name.IndexOf('.');

                if (subSectionIndex < 1)
                    throw new Exception("Invalid section name: " + name);

                SectionName = name.Substring(0, subSectionIndex).Trim();
                SubSection = name.Substring(subSectionIndex + 1).Trim();
            }
        }

        internal IDictionary<string, IList<string>> Keys { get; set; }
        public string SectionName { get; set; }
        public string SubSection { get; set; }

        public void SetValue(string key, string value)
        {
            if (string.IsNullOrEmpty(value))
                Keys.Remove(key);
            else
                Keys[key] = new List<string> {value};
        }

        public void AddValue(string key, string value)
        {
            if (!Keys.ContainsKey(key))
                Keys[key] = new List<string>();

            Keys[key].Add(value);
        }

        public string GetValue(string key)
        {
            return Keys.ContainsKey(key) && Keys[key].Count > 0 ? Keys[key][0] : string.Empty;
        }

        public IList<string> GetValues(string key)
        {
            return Keys.ContainsKey(key) ? Keys[key] : new List<string>();
        }

        public override string ToString()
        {
            return
                string.IsNullOrEmpty(SubSection)
                    ? string.Concat("[", SectionName, "]")
                    : string.Concat("[", SectionName, " \"", SubSection, "\"]");
        }
    }
}