using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ikende.com.RazorEngine.Implement
{
    class RenderSection:IHtmlString
    {
        public string Name { get; set; }
        public System.IO.TextWriter Writer { get; set; }
        public void Execute(IContext context)
        {
           
            Action result = null;
            if (context.Sections.TryGetValue(Name, out result))
            {
                context.ChildTemplate.Writer = Writer;
                result();
            }
        }
        public override string ToString()
        {
            return null;
        }
    }
}
