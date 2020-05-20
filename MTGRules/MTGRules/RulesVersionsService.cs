using System;
using System.Collections.Generic;
using System.Text;

namespace MTGRules
{
    public static class RulesVersionsService
    {
        public static readonly List<RulesSource> RulesSources = new List<RulesSource> {
            new RulesSource("MagicCompRules_20150123.txt",
                            new Uri("http://media.wizards.com/2015/docs/MagicCompRules_20150123.txt"),
                            new DateTime(2015, 1, 23),
                            Encoding.GetEncoding("windows-1252")),

            new RulesSource("MagicCompRules_20160722.txt",
                            new Uri("http://media.wizards.com/2016/docs/MagicCompRules_20160722.txt"),
                            new DateTime(2016, 7, 22),
                            Encoding.GetEncoding("windows-1252")),

            new RulesSource("MagicCompRules_CN2_Update_20160826.txt",
                            new Uri("http://media.wizards.com/2016/docs/MagicCompRules_CN2_Update_20160826.txt"),
                            new DateTime(2016, 8, 26),
                            Encoding.GetEncoding("UTF-16")),

            new RulesSource("MagicCompRules_20160930.txt",
                            new Uri("http://media.wizards.com/2016/docs/MagicCompRules_20160930.txt"),
                            new DateTime(2016, 9, 30),
                            Encoding.GetEncoding("UTF-16")),

            new RulesSource("MagicCompRules_20161111.txt",
                            new Uri("http://media.wizards.com/2016/docs/MagicCompRules_20161111.txt"),
                            new DateTime(2016, 11, 11),
                            Encoding.GetEncoding("windows-1252")),

            new RulesSource("MagicCompRules_20170119.txt",
                            new Uri("http://media.wizards.com/2017/downloads/MagicCompRules_20170119.txt"),
                            new DateTime(2017, 1, 19),
                            Encoding.GetEncoding("windows-1252")),

            new RulesSource("MagicCompRules_20170428.txt",
                            new Uri("http://media.wizards.com/2017/downloads/MagicCompRules_20170428.txt"),
                            new DateTime(2017, 4, 28),
                            Encoding.GetEncoding("windows-1252")),

            new RulesSource("MagicCompRules_20170605.txt",
                            new Uri("http://media.wizards.com/2017/downloads/MagicCompRules_20170605.txt"),
                            new DateTime(2017, 6, 5),
                            Encoding.GetEncoding("windows-1252")),

            new RulesSource("MagicCompRules_20170707.txt",
                            new Uri("http://media.wizards.com/2017/downloads/MagicCompRules_20170707.txt"),
                            new DateTime(2017, 7, 7),
                            Encoding.GetEncoding("windows-1252")),

            new RulesSource("MagicCompRules 20170825.txt",
                            new Uri("http://media.wizards.com/2017/downloads/MagicCompRules%2020170825.txt"),
                            new DateTime(2017, 8, 25),
                            Encoding.GetEncoding("windows-1252")),

            new RulesSource("MagicCompRules 20170925.txt",
                            new Uri("http://media.wizards.com/2017/downloads/MagicCompRules%2020170925.txt"),
                            new DateTime(2017, 9, 25),
                            Encoding.GetEncoding("windows-1252")),

            new RulesSource("MagicCompRules%2020180119.txt",
                            new Uri("http://media.wizards.com/2018/downloads/MagicCompRules%2020180119.txt"),
                            new DateTime(2018, 1, 19),
                            Encoding.GetEncoding("windows-1252")),

            new RulesSource("MagicCompRules%2020180413.txt",
                            new Uri("http://media.wizards.com/2018/downloads/MagicCompRules%2020180413.txt"),
                            new DateTime(2018, 4, 13),
                            Encoding.GetEncoding("windows-1252")),

            new RulesSource("MagicCompRules%2020180608.txt",
                            new Uri("http://media.wizards.com/2018/downloads/MagicCompRules%2020180608.txt"),
                            new DateTime(2018, 6, 8),
                            Encoding.GetEncoding("windows-1252")),

            new RulesSource("MagicCompRules%2020180713.txt",
                            new Uri("http://media.wizards.com/2018/downloads/MagicCompRules%2020180713.txt"),
                            new DateTime(2018, 7, 13),
                            Encoding.GetEncoding("windows-1252")),

            new RulesSource("MagicCompRules%2020180810.txt",
                            new Uri("http://media.wizards.com/2018/downloads/MagicCompRules%2020180810.txt"),
                            new DateTime(2018, 8, 10),
                            Encoding.GetEncoding("windows-1252")),

            new RulesSource("MagicCompRules%2020181005.txt",
                            new Uri("http://media.wizards.com/2018/downloads/MagicCompRules%2020181005.txt"),
                            new DateTime(2018, 10, 5),
                            Encoding.GetEncoding("windows-1252")),

            new RulesSource("MagicCompRules%2020190125.txt",
                            new Uri("http://media.wizards.com/2019/downloads/MagicCompRules%2020190125.txt"),
                            new DateTime(2019, 1, 25),
                            Encoding.GetEncoding("windows-1252")),

            new RulesSource("MagicCompRules%2020190712.txt",
                            new Uri("https://media.wizards.com/2019/downloads/MagicCompRules%2020190712.txt"),
                            new DateTime(2019, 7, 12),
                            Encoding.GetEncoding("UTF-8")),

            new RulesSource("MagicCompRules%2020190823.txt",
                            new Uri("https://media.wizards.com/2019/downloads/MagicCompRules%2020190823.txt"),
                            new DateTime(2019, 8, 23),
                            Encoding.GetEncoding("UTF-8")),

            new RulesSource("MagicCompRules%2020191004.txt",
                            new Uri("https://media.wizards.com/2019/downloads/MagicCompRules%2020191004.txt"),
                            new DateTime(2019, 10, 04),
                            Encoding.GetEncoding("UTF-8")),

            new RulesSource("MagicCompRules%2020200122.txt",
                            new Uri("https://media.wizards.com/2020/downloads/MagicCompRules%2020200122.txt"),
                            new DateTime(2020, 01, 22),
                            Encoding.GetEncoding("UTF-8")),

            new RulesSource("MagicCompRules%2020200417.txt",
                            new Uri("https://media.wizards.com/2020/downloads/MagicCompRules%2020200417.txt"),
                            new DateTime(2020, 04, 17),
                            Encoding.GetEncoding("UTF-8"))
        };
    }
}
