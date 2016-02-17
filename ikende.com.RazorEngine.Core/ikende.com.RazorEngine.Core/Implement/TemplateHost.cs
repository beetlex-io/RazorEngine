using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ikende.com.RazorEngine.Implement
{
    class TemplateHost:ITemplateHost
    {
        public TemplateHost()
        {
            BasePath = AppDomain.CurrentDomain.BaseDirectory;
            BaseType = typeof(Template);
            TempleteFilter = "*.cshtml";
        }

        private Dictionary<string, ITemplateInfo> mTemplates = new Dictionary<string, ITemplateInfo>(256);

        [ThreadStatic]
        private static StringBuilder mBody;

        private void LoadFolderTemplate(string folder, string basepath)
        {
            foreach (string file in System.IO.Directory.GetFiles(folder, TempleteFilter))
            {
                System.IO.FileInfo finfo = new System.IO.FileInfo(file);
                AddTempleteFile(basepath + System.IO.Path.GetFileNameWithoutExtension(finfo.Name), file);
            }
            foreach (string childfolder in System.IO.Directory.GetDirectories(folder))
            {
                System.IO.DirectoryInfo info = new System.IO.DirectoryInfo(childfolder);
                LoadFolderTemplate(childfolder, basepath + info.Name + "/");
            }
        }

        public string BasePath
        {
            get;
            set;
        }

        public string TempleteFilter
        {
            get;
            set;
        }

        public void AddTemplate(string name, string template)
        {
            try
            {
                name = name.ToLower();
                lock (mTemplates)
                {
                    ITemplateInfo info = null;
                    mTemplates.TryGetValue(name, out info);
                    if (info == null)
                    {
                        info = new StringTemplateInfo(name, template);
                        mTemplates[name] = info;
                    }
                    ((Implement.StringTemplateInfo)info).TemplateData = template;
                    info.Host = this;
                    info.Clean();

                }
            }
            catch (RazorException re)
            {
                throw re;
            }
            catch (Exception e_)
            {
                throw new RazorException("add  string template error " + e_.Message, e_);
            }
        }

        public void AddTempleteFile(string name, string file)
        {
            try
            {
                name = name.ToLower();
                lock (mTemplates)
                {
                    ITemplateInfo info = null;
                    mTemplates.TryGetValue(name, out info);
                    if (info == null)
                    {
                        info = new FileTemplateInfo(name, file);
                        mTemplates[name] = info;
                    }
                    ((Implement.FileTemplateInfo)info).TemplateFile = file;
                    info.Host = this;
                    info.Clean();
                }
            }
           
            catch (Exception e_)
            {
                throw new RazorException("add  file template error "+e_.Message, e_);
            }
        }
     
        public ITemplate GetTemplete(string name,IContext context)
        {
            name = name.ToLower();
            ITemplateInfo result = null;
            mTemplates.TryGetValue(name, out result);
            if(result ==null)
                throw new RazorException("{0} template not found", name);
            ITemplate template= result.GetTemplate(this);
            template.DataContext = context.Model;
            template.Context = context;
          
            return template;
        }



        public void Parse(string name, object model, System.IO.TextWriter writer)
        {
            Parse(name, null, model, writer);
        }


        public void Parse(string name, string masterName, object model, System.IO.TextWriter writer)
        {
            using (Context contex = new Context())
            {
                contex.TemplateHost = this;
                if (model != null)
                {
                    var x = contex.Model as IDictionary<string, Object>;
                   
                    foreach(System.Reflection.PropertyInfo property in model.GetType().GetProperties( System.Reflection.BindingFlags.Public| System.Reflection.BindingFlags.Instance))
                    {
                        x.Add(property.Name, property.GetValue(model, null));
                      
                    }
                }
                
                contex.ChildTemplate = GetTemplete(name,contex);
                if (mBody == null)
                    mBody = new StringBuilder();
                mBody.Clear();
                System.IO.StringWriter bwriter = new System.IO.StringWriter(mBody);
                contex.ChildTemplate.Writer = bwriter;
                contex.ChildTemplate.Execute();
                contex.ChildBody = mBody;
                if (!string.IsNullOrEmpty( contex.ChildTemplate.Layout))
                    masterName = contex.ChildTemplate.Layout;
                if (!string.IsNullOrEmpty(masterName))
                {
                    contex.ParentTemplate = GetTemplete(masterName,contex);
                    contex.ParentTemplate.Writer = writer;
                    contex.ParentTemplate.Execute();
                }
                else
                {
                    writer.Write(mBody);
                }
            }
        }

        public void LoadTemplateFiles()
        {
            LoadFolderTemplate(BasePath, "/");
        }
          
        public void Dispose()
        {
            mTemplates.Clear();
        }

        public string Parse(string name, object model)
        {
            return Parse(name, null, model);
        }

        public string Parse(string name, string masterName, object model)
        {
            StringBuilder sb = new StringBuilder();
            System.IO.StringWriter writer = new System.IO.StringWriter(sb);
            Parse(name, masterName, model, writer);
            return sb.ToString();
        }


        public void AddNamespace(params string[] namespaces)
        {
            foreach (string ns in namespaces)
            {
                Common.RazorComplie.AddNamespace(ns);
            }
        }


        public string LastCompileSource
        {
            get;
            set;
        }

        public Type BaseType
        {
            get;
            set;
        }
    }
}
