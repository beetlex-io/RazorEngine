using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ikende.com.RazorEngine
{
    public interface IHtmlString
    {
        void Execute(IContext context);
    }
}
