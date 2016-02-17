using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ikende.com.RazorEngine
{
    public interface IContext:IDisposable
    {
        dynamic Model { get; set; }

        object this[string key] { get; set; }

        Dictionary<string, Action> Sections { get; }

        ITemplate ParentTemplate { get; set; }

        ITemplate ChildTemplate { get; set; }

        StringBuilder ChildBody { get; set; }

        ITemplateHost TemplateHost
        {
            get;
            set;
        }

        ITemplate GetTemplate(string name, IContext context);
    }
}
