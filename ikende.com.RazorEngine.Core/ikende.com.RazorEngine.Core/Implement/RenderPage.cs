using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ikende.com.RazorEngine.Implement
{
    class RenderPage:IHtmlString
    {
        public string Name { get; set; }

        public System.IO.TextWriter Writer { get; set; }

        public object[] Parameter
        {
            get;
            set;
        }

        public void Execute(IContext context)
        {

            ITemplate template = context.GetTemplate(Name, context);
            template.Writer = Writer;
            template.Context = context;
            if (Parameter != null)
            {
                IDictionary<string, object> dict = (IDictionary<string, object>)template.ViewData;
                foreach (object item in Parameter)
                {
                    foreach (PropertyInfo p in item.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    {
                        dict[p.Name] = p.GetValue(item, null);
                    }
                }
            }
            template.Execute();
        }

        public override string ToString()
        {
            return null;
        }
    }
}
