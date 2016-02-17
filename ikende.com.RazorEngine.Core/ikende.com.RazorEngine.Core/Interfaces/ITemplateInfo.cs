using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ikende.com.RazorEngine
{
    interface ITemplateInfo
    {
        string CompileError { get; set; }
        System.Reflection.Assembly Assembly { get; set; }
        ITemplateHost Host { get; set; }
        Type TemplateType { get; set; }
        string Name { get; set; }
        System.IO.StreamReader GetCode();
        void Builder(ITemplateHost host);
        ITemplate GetTemplate(ITemplateHost host);
        void Clean();
    }
}
