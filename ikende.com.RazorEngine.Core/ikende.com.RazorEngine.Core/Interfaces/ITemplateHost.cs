using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ikende.com.RazorEngine
{
    public interface ITemplateHost:IDisposable
    {
        Type BaseType { get; set; }

        string BasePath { get; set; }

        string TempleteFilter { get; set; }

        void AddTemplate(string name, string template);

        void AddTempleteFile(string name, string file);

         void LoadTemplateFiles();

        ITemplate GetTemplete(string name,IContext context);

        void Parse(string name, string masterName, object model, System.IO.TextWriter writer);
        
        void Parse(string name, object model, System.IO.TextWriter writer);

        string Parse(string name, object model);

        string Parse(string name,string masterName, object model);

        void AddNamespace(params string[] namespaces);

        string LastCompileSource { get; set; }
        
    }
}
