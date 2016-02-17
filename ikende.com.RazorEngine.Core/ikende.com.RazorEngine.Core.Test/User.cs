using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ikende.com.RazorEngine.Core.Test
{
    public class User
    {
        public User()
        {
            Name = Guid.NewGuid().ToString("N");
            EMail = Guid.NewGuid().ToString("N");
        }
        public string Name { get; set; }
        public string EMail { get; set; }
    }
}
