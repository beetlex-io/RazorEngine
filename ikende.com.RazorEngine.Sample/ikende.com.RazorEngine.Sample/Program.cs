using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ikende.com.RazorEngine.Sample.Model;
namespace ikende.com.RazorEngine.Sample
{
    class Program
    {
        private static Interfaces.ITemplateHost mHost;
     
        static void Main(string[] args)
        {
            mHost = Razor.CreateHost();
            mHost.AddNamespace("ikende.com.RazorEngine.Sample.Model");
            mHost.AddNamespace("System.Collections");
            mHost.LoadTemplateFiles();
           // StringTemplate();
           // StringTemplate_Section();
            StringTemplate_Master();
          //  FileTemplate();
           // FileTemplate_Section();
           // FileTemplate_Master();
            
            Console.Read();
        }

        private static void StringTemplate()
        {
            string template = 
@"<users>@foreach(User user in (System.Collections.IEnumerable)Model){<user>
<name>@user.Name</name>
<email>@user.EMail</email>
</user>}</users>";
            mHost.AddStringTemplate("/list", template);
            string value = mHost.Parse("/list", new Model.User[] {
             new User{ Name="henryfan", EMail="henryfan@msn.com"},
             new User{Name="bbq", EMail="bbq@msn.com"}
            });
            
            Console.Write(value);
        }

        private static void StringTemplate_Section()
        {
            string template = 
@"@section item{@{User user = (User)ForeachItem;}<user>
    <name>@user.Name</name>
    <email>@user.EMail</email>
    </user>}";
            string template1 = "<users>@ForeachSection(\"item\",(IEnumerable)Model)</users>";         
            mHost.AddStringTemplate("/list", template+template1);
            string value = mHost.Parse("/list", new Model.User[] {
             new User{ Name="henryfan", EMail="henryfan@msn.com"},
             new User{Name="bbq", EMail="bbq@msn.com"}
            });

            Console.Write(value);
        }

        private static void StringTemplate_Master()
        {
            string master = "<body><h1>master page</h1><div>@RenderBody()</div></body>";
            string body = 
@"@{User user=(User)Model;}<p><span>Name:</span>@user.Name</p>
<p><span>EMail:</span>@user.EMail</p>";
            mHost.AddStringTemplate("/master", master);
            mHost.AddStringTemplate("/body", body);
            string value = mHost.Parse("/body", "/master",
                new User { Name = "henryfan", EMail = "henryfan@msn.com" });
            Console.WriteLine(value);
            master = "<body><h1> change master page</h1><div>@RenderBody()</div></body>";
            mHost.AddStringTemplate("/master", master);
            value = mHost.Parse("/body", "/master",
                new User { Name = "henryfan", EMail = "henryfan@msn.com" });
            Console.WriteLine(value);
        }

        private static void FileTemplate()
        {
            string value = mHost.Parse("/views/index", 
             new User{ Name="henryfan", EMail="henryfan@msn.com"});
            Console.Write(value);
        }

        private static void FileTemplate_Section()
        {

            string value = mHost.Parse("/views/earchSection", new Model.User[] {
             new User{ Name="henryfan", EMail="henryfan@msn.com"},
             new User{Name="bbq", EMail="bbq@msn.com"}
            });

            Console.Write(value);
        }

        private static void FileTemplate_Master()
        {
            string value = mHost.Parse("/views/index", "/views/master",
             new User { Name = "henryfan", EMail = "henryfan@msn.com" });
            Console.WriteLine(value);
        }
    }
}
