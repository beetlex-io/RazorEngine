using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ikende.com.RazorEngine.Implement
{
    class RenderBody:IHtmlString
    {
        public System.IO.TextWriter Writer
        {
            get;
            set;
        }

        public void Execute(IContext context)
        {
            if (context.ChildTemplate != null && context.ChildBody != null && context.ChildBody.Length > 0)
            {
                Writer.Write(context.ChildBody);
            }
        }
        public override string ToString()
        {
            return null;
        }
    }
}
