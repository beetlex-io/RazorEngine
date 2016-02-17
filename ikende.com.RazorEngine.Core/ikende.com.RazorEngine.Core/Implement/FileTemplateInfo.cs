using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ikende.com.RazorEngine;

namespace ikende.com.RazorEngine.Implement
{
    class FileTemplateInfo : ITemplateInfo
    {

        public FileTemplateInfo(string name, string file)
        {
            Name = name;
            TemplateFile = file;
        }


        private bool mIsInit = false;

        public string Name
        {
            get;
            set;
        }


        public string TemplateFile
        {
            get;
            set;
        }

        public void Builder(ITemplateHost host)
        {

            Common.RazorComplie.BuilderTemplate(this,host);

        }

        public ITemplate GetTemplate(ITemplateHost host)
        {
            lock (this)
            {
                if (!mIsInit)
                {
                    Builder(host);
                    mIsInit = true;
                }
                return (ITemplate)Activator.CreateInstance(TemplateType);
            }
        }

        public string CompileError
        {
            get;
            set;
        }

        public System.Reflection.Assembly Assembly
        {
            get;
            set;
        }

        public Type TemplateType
        {
            get;
            set;
        }


        public System.IO.StreamReader GetCode()
        {
            return new System.IO.StreamReader(TemplateFile, Encoding.UTF8);
        }


        public void Clean()
        {
            lock (this)
            {
                mIsInit = false;
            }
        }


        public ITemplateHost Host
        {
            get;
            set;
        }
    }
}
