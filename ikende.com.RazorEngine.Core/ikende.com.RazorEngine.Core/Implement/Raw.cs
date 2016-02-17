using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ikende.com.RazorEngine.Implement
{
    class Raw:IHtmlString
    {
        public string Value
        {
            get;
            set;
        }

        public System.IO.TextWriter Writer
        {
            get;
            set;
        }
        public void Execute(IContext context)
        {
            Writer.Write(System.Web.HttpUtility.HtmlEncode( Value));
        }
        public override string ToString()
        {
            return null;
        }
    }
}
