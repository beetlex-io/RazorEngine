using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace ikende.com.RazorEngine.Implement
{
    class Context:IContext
    {
        public Context()
        {
            Model =  new ExpandoObject();
        }
        private Dictionary<string, object> mProperties = new Dictionary<string, object>();

        private Dictionary<string, Action> mSections = new Dictionary<string, Action>();

        public object this[string key]
        {
            get
            {
                object result =null;
                mProperties.TryGetValue(key,out result);
                return result;
            }
            set
            {
                mProperties[key] = value;
            }
        }

        public Dictionary<string, Action> Sections
        {
            get { return mSections; }
        }

        public ITemplate ParentTemplate
        {
            get;
            set;
        }

        public ITemplate ChildTemplate
        {
            get;
            set;
        }    

        public StringBuilder ChildBody
        {
            get;
            set;
        }

        public ITemplateHost TemplateHost
        {
            get;
            set;
        }

        public ITemplate GetTemplate(string name,IContext context)
        {
           return TemplateHost.GetTemplete(name,context);
        }

        public void Dispose()
        {
            mProperties.Clear();
            mSections.Clear();
            TemplateHost = null;
            ChildTemplate = null;
            ParentTemplate = null;
        }

        public dynamic Model
        {
            get;
            set;
        }

       
        
    }
}
