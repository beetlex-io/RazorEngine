using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ikende.com.RazorEngine
{
    public class Razor
    {
        public static ITemplateHost CreateHost()
        {
            return new Implement.TemplateHost();
        }
        public static ITemplateHost CreateHost(string basepath, string filter = "*.cshtml")
        {
            Implement.TemplateHost host = new Implement.TemplateHost();
            host.BasePath = basepath;
            host.TempleteFilter = filter;
            return host;
        }
    }
}
