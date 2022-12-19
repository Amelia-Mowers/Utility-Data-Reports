using System;
using System.Diagnostics;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Utility.Data.Tables;

namespace Utility.Data.Reports
{
    public class Report
    {
        public enum SerializeType
        {
            Console,
            Html
        }

        List<Section> Sections;

        public Report()
        {
            Sections = new List<Section>();
        }

        public void Add(string header, string body, Table table)
        {
            Sections.Add(new Section(header, body, table));
        }

        public void Prepend(string header, string body, Table table)
        {
            Sections.Insert(0, new Section(header, body, table));
        }

        public string Collate(SerializeType type)
        {
            var sectionStrings = new List<string>();
            if (type == SerializeType.Console)
            {
                string nl = Environment.NewLine;
                foreach (var s in Sections)
                {
                    var sb = new StringBuilder(s.Header);

                    if (s.Body != null)
                    {
                        sb.Append(nl);
                        sb.Append(s.Body);
                    }

                    if (s.Table != null)
                    {
                        sb.Append(nl);
                        sb.Append(s.Table.Display(nl));
                    }

                    sectionStrings.Add(sb.ToString());
                }

                return String.Join(nl + nl, sectionStrings);
            }
            else if (type == SerializeType.Html)
            {
                foreach (var s in Sections)
                {
                    var sb = new StringBuilder("<h3>" + s.Header + "</h3>");

                    if (s.Body != null)
                    {
                        sb.Append("<pre>" + s.Body + "</pre>");
                    }

                    if (s.Table != null)
                    {
                        sb.Append(s.Table.DisplayHtml());
                    }
                    sectionStrings.Add(sb.ToString());
                }

                return String.Join("", sectionStrings);
            }
            else
            {
                throw new Exception("Collate unsupported serialize type");
            }
        }

        class Section
        {
            public string Header;
            public string Body;
            public Table Table;
            public Section(string header, string body, Table table)
            {
                Header = header;
                Body = body;
                Table = table;
            }

        }

    }

}