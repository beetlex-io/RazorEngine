using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ikende.com.RazorEngine;

namespace ikende.com.RazorEngine.Implement
{
    class StringTemplateInfo:ITemplateInfo
    {
        public StringTemplateInfo(string name, string templateData)
        {
            Name = name;
            TemplateData = templateData;
        }

        private bool mIsInit = false;

        public string Name
        {
            get;
            set;
        }

        public string TemplateData
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
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            System.IO.StreamWriter writer = new System.IO.StreamWriter(stream, Encoding.UTF8);
            writer.Write(TemplateData);
            writer.Flush();
            stream.Position = 0;
            return new System.IO.StreamReader(stream, Encoding.UTF8);
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
