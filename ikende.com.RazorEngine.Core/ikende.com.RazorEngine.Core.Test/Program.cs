using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ikende.com.RazorEngine.Core.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            ITemplateHost mHost = Razor.CreateHost();

            mHost.LoadTemplateFiles();

            mHost.AddTemplate("/master", "<h1>主页面</h1>@RenderBody()");

            mHost.AddTemplate("/user", "<div><h3>用户信息</h3><p>email:@DataContext.EMail</p><p>name:@DataContext.Name</p></div>");

            mHost.AddTemplate("/users", "<div class=\"user-list\"><h2>用户列表</h2>@Each(\"/user\",Model.Items)<div>");


            string value = mHost.Parse("/user", new User { EMail = "henryfan@msn.com", Name = "henry" });
            Console.WriteLine(value);


            value = mHost.Parse("/users", new { Items = new User[] {
                new User { EMail = "henryfan@msn.com", Name = "henry" } ,
                new User { EMail = "henryfan1@msn.com", Name = "henry1" }
            
            } });
            Console.WriteLine(value);

            value = mHost.Parse("/users","/master" ,new
            {
                Items = new User[] {
                new User { EMail = "henryfan@msn.com", Name = "henry" } ,
                new User { EMail = "henryfan1@msn.com", Name = "henry1" }
            
            }
            });
            Console.WriteLine(value);


            mHost.AddTemplate("/test", "@Raw(DataContext.EMail)");
            value = mHost.Parse("/test", new { EMail = "henryfan@msn.com<a>" });
            Console.Write(value);


            value = mHost.Parse("/views/raw", null);
            Console.Write(value);

            value = mHost.Parse("/views/index", "/views/master",
               new { EMail = "henryfan@msn.com", Name = "henryfan" });
            Console.Write(value);

            value = mHost.Parse("/views/foreachSection", new { Items = new User[] { new User(), new User() } });
            Console.Write(value);

            Console.Read();

        }
        static void test(object state)
        {
        }
        static void test2(object state)
        {
        }
    }
}
