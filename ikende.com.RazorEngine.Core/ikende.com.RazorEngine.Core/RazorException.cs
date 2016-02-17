using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ikende.com.RazorEngine
{
    public class RazorException:Exception
    {
        public RazorException()
        {
        }
        public RazorException(string message)
            : base(message)
        {
        }
        public RazorException(string format, params object[] data) : base(string.Format(format, data)) { }

        public RazorException(string message, Exception interError) : base(message, interError) { }
    }
}
