using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ikende.com.RazorEngine.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        public UnitTest1()
        {
            mHost = Razor.CreateHost();
            mHost.LoadTemplateFiles();
        }

        private ITemplateHost mHost;

        [TestMethod]
        public void AddStringTemplate()
        {
            mHost.AddTemplate("/test", "@Raw(DataContext.EMail)");
            string value = mHost.Parse("/test", new { EMail = "henryfan@msn.com" });
            Console.Write(value);
        }

        [TestMethod]
        public void Raw()
        {
            string value = mHost.Parse("/views/raw", null);
            Console.Write(value);
        }
        [TestMethod]
        public void Master()
        {
            string value = mHost.Parse("/views/index", "/views/master",
               new { EMail = "henryfan@msn.com", Name = "henryfan" });
            Console.Write(value);
        }

        [TestMethod]
        public void ForearchSection()
        {
            string value = mHost.Parse("/views/foreachSection", new { Items = new User[] { new User(), new User() } });
            Console.Write(value);
        }
    }

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
