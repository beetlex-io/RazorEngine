
namespace ikende.com.RazorEngine.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.CodeDom.Compiler;
    using System.Reflection;
    using Microsoft.CSharp;
    using System.Web.Razor;
    using System.Xml;
    using System.Web;
    using System.Web.Razor.Generator;

    class RazorComplie
    {
        static RazorComplie()
        {
            ReferencedNamespaces = new List<string>();
            ReferencedNamespaces.Add("System");
            ReferencedNamespaces.Add("System.Text");
            ReferencedNamespaces.Add("System.Collections.Generic");
            ReferencedNamespaces.Add("System.IO");

            ReferencedAssemblies = new List<string>();
            LoadConfigNamespaces();

        }

        static bool mIsLoadConfigNamespaces = false;

        static void LoadConfigNamespaces()
        {
            lock (typeof(RazorComplie))
            {
                if (!mIsLoadConfigNamespaces)
                {
                    string path = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
                    if (System.IO.File.Exists(path))
                    {
                        try
                        {
                            XmlDocument config = new XmlDocument();
                            config.Load(path);
                            XmlNode node = config.LastChild["system.web.webPages.razor"];
                            if (node != null)
                            {
                                XmlNode spaces = node["pages"]["namespaces"];
                                foreach (XmlNode ns in spaces.ChildNodes)
                                {
                                    if (ns.Name == "add")
                                    {
                                        AddNamespace(ns.Attributes["namespace"].Value);
                                    }
                                }
                            }
                        }
                        catch
                        {
                        }
                    }
                    mIsLoadConfigNamespaces = true;
                }
            }
        }

        public static void AddAssemblies(params string[] assemblies)
        {
            foreach (string item in assemblies)
            {
                if (!ReferencedAssemblies.Contains(item))
                    ReferencedAssemblies.Add(item);
            }
        }

        public static void AddNamespace(params string[] namespaces)
        {
            foreach (string item in namespaces)
            {
                if (!ReferencedNamespaces.Contains(item))
                    ReferencedNamespaces.Add(item);
            }
        }

        private static void AddDomainAssemblies()
        {
            foreach (Assembly item in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    AddAssemblies(item.Location);
                }
                catch
                {
                }
            }
            foreach (string dll in System.IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory))
            {
                try
                {
                    Assembly.LoadFile(dll);
                    AddAssemblies(dll);
                }
                catch
                {
                }
            }
        }

        public static void BuilderTemplate(ITemplateInfo templateinfo,ITemplateHost host)
        {
            Type type = host.BaseType;

            string typename = "_" + Guid.NewGuid().ToString("N");
            RazorTemplateEngine engine = CreateHost(type, "_" + type.Name, typename);

            AddNamespace(type.Namespace);
            GeneratorResults razorResults = null;
            using (System.IO.StreamReader reader = templateinfo.GetCode())
            {
                razorResults = engine.GenerateCode(reader);
                CSharpCodeProvider codeProvider = new CSharpCodeProvider();
                CodeGeneratorOptions options = new CodeGeneratorOptions();
                string LastGeneratedCode = null;
                using (StringWriter writer = new StringWriter())
                {
                    codeProvider.GenerateCodeFromCompileUnit(razorResults.GeneratedCode, writer, options);
                    templateinfo.Host.LastCompileSource = writer.ToString();
                }
                CompilerParameters compilerParameters = new CompilerParameters(new string[0]);
                compilerParameters.OutputAssembly = "tmp_assembly" + typename;
                compilerParameters.GenerateInMemory = false;
                AddDomainAssemblies();
                foreach (string item in ReferencedAssemblies)
                    compilerParameters.ReferencedAssemblies.Add(item);
                compilerParameters.GenerateInMemory = true;
                CompilerResults compilerResults = codeProvider.CompileAssemblyFromDom(compilerParameters, razorResults.GeneratedCode);
                if (compilerResults.Errors.Count > 0)
                {
                    string errormessage = null;

                    int throwindex = 1;
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    reader.BaseStream.Seek(0, SeekOrigin.Begin);
                    using (System.IO.StreamReader line = new StreamReader(reader.BaseStream, reader.CurrentEncoding))
                    {

                        ParserResults presult = engine.ParseTemplate(line);

                        if (presult.ParserErrors.Count > 0)
                        {
                            throwindex = presult.ParserErrors[0].Location.LineIndex + 1;
                            errormessage = presult.ParserErrors[0].Message;
                            reader.BaseStream.Seek(0, SeekOrigin.Begin);
                            using (System.IO.StreamReader readcode = new StreamReader(reader.BaseStream, reader.CurrentEncoding))
                            {
                                string code = readcode.ReadLine();
                                while (code != null)
                                {
                                    sb.AppendLine(code);
                                    code = readcode.ReadLine();

                                }
                            }

                        }
                        else
                        {
                            throwindex = compilerResults.Errors[0].Line;
                            errormessage = compilerResults.Errors[0].ErrorText;
                            sb.Append(LastGeneratedCode);
                        }

                    }

                    templateinfo.CompileError = errormessage;
                    throw new RazorException(templateinfo.CompileError);

                }
                templateinfo.Assembly = compilerResults.CompiledAssembly;
                templateinfo.TemplateType = compilerResults.CompiledAssembly.GetType("_" + type.Name + "." + typename);

            }

        }

        private static List<string> ReferencedNamespaces { get; set; }

        private static IList<string> ReferencedAssemblies { get; set; }

        private static RazorTemplateEngine CreateHost(Type basetype, string generatedNamespace, string generatedClass)
        {
            Type baseClassType = basetype;

            RazorEngineHost host = new RazorEngineHost(new CSharpRazorCodeLanguage());
            host.DefaultBaseClass = baseClassType.FullName;
            host.DefaultClassName = generatedClass;
            host.DefaultNamespace = generatedNamespace;
            GeneratedClassContext gcc = host.GeneratedClassContext;
            gcc.DefineSectionMethodName = "DefineSection";
            host.GeneratedClassContext = gcc;
            foreach (string ns in ReferencedNamespaces)
            {
                host.NamespaceImports.Add(ns);
            }

            return new RazorTemplateEngine(host);
        }
    }
}


