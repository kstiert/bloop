using System;
using System.Collections.Generic;

namespace Bloop.Plugin
{
    public class Query
    {
        /// <summary>
        /// Raw query, this includes action keyword if it has
        /// We didn't recommend use this property directly. You should always use Search property.
        /// </summary>
        public string RawQuery { get; internal set; }

        /// <summary>
        /// Search part of a query.
        /// This will not include action keyword if exclusive plugin gets it, otherwise it should be same as RawQuery.
        /// Since we allow user to switch a exclusive plugin to generic plugin, so this property will always give you the "real" query part of
        /// the query
        /// </summary>
        public string Search { get; internal set; }

        internal string GetActionKeyword()
        {
            if (!string.IsNullOrEmpty(RawQuery))
            {
                var strings = RawQuery.Split(' ');
                if (strings.Length > 0)
                {
                    return strings[0];
                }
            }

            return string.Empty;
        }

        internal bool IsIntantQuery { get; set; }

        /// <summary>
        /// Return first search split by space if it has
        /// </summary>
        public string FirstSearch
        {
            get
            {
                return SplitSearch(0);
            }
        }

        /// <summary>
        /// strings from second search (including) to last search
        /// </summary>
        public string SecondToEndSearch
        {
            get
            {
                if (string.IsNullOrEmpty(Search)) return string.Empty;

                var strings = Search.Split(' ');
                if (strings.Length > 1)
                {
                    return Search.Substring(Search.IndexOf(' ') + 1);
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// Return second search split by space if it has
        /// </summary>
        public string SecondSearch
        {
            get
            {
                return SplitSearch(1);
            }
        }

        /// <summary>
        /// Return third search split by space if it has
        /// </summary>
        public string ThirdSearch
        {
            get
            {
                return SplitSearch(2);
            }
        }

        private string SplitSearch(int index)
        {
            if (string.IsNullOrEmpty(Search)) return string.Empty;

            var strings = Search.Split(' ');
            if (strings.Length > index)
            {
                return strings[index];
            }

            return string.Empty;
        }

        public override string ToString()
        {
            return RawQuery;
        }

        public Query(string rawQuery)
        {
            RawQuery = rawQuery;
        }
    }
}
