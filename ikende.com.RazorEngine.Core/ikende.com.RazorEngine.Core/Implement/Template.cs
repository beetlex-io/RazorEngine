using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ikende.com.RazorEngine;
using System.Dynamic;
using System.Reflection;

namespace ikende.com.RazorEngine.Implement
{
    public class Template : ITemplate
    {


        private ExpandoObject mViewData = new ExpandoObject();

        public dynamic ViewData
        {
            get
            {
                return mViewData;
            }
        }

        public dynamic Model
        {
            get
            {
                return Context.Model;
            }
        }

        public object this[string key]
        {
            get
            {
                return Context[key];
            }
            set
            {
                Context[key] = value;
            }
        }

        public dynamic DataContext { get; set; }

        public IContext Context
        {
            get;
            set;
        }

        public IHtmlString RenderBody()
        {
            RenderBody rb = new RenderBody();
            rb.Writer = Writer;
            rb.Execute(Context);
            return rb;
        }

        public string Layout
        {
            get;
            set;
        }

        public void DefineSection(string name, Action action)
        {
            Context.Sections[name] = action;
        }

        public IHtmlString RenderSection(string name)
        {
            RenderSection section = new RenderSection();
            section.Name = name;
            section.Writer = Writer;
            section.Execute(Context);
            return section;
        }

        public IHtmlString RenderPage(string name)
        {
           return RenderPage(name, null);
           
        }

        public virtual void Execute()
        {

        }

        public void Write(object value)
        {
            Writer.Write(value);
        }

        public IHtmlString Raw(object value)
        {
            Raw raw = new Raw();
            raw.Writer = Writer;
            raw.Value = value !=null?value.ToString():"";
            raw.Execute(Context);
            return raw;
        }

        public System.IO.TextWriter Writer
        {
            get;
            set;
        }
      
        public void WriteLiteral(string literal)
        {
            Writer.WriteLine(literal);
        }

        public bool IsSectionDefined(string name)
        {
            return Context.Sections.ContainsKey(name);
        }

        public IHtmlString RenderPage(string name, params object[] data)
        {
            RenderPage page = new RenderPage();
            page.Name = name;
            page.Writer = Writer;
            page.Parameter = data;
            page.Execute(Context);
          
            return page;

        }

     
      
       

        public IHtmlString EachSection(string name, object items)
        {
            ForeachSection fs = new ForeachSection();
            fs.Section = name;
            fs.Template = this;
            fs.Items = items;
            fs.Execute(Context);
            return fs;
        }

        public IHtmlString Each(string name, object items)
        {
            ForeachView view = new ForeachView();
            view.View = name;
            view.Items = items;
            view.Template = this;
            view.Execute(Context);
            return view;
        }


        public virtual void WriteAttribute(string name, PositionTagged<string> prefix, PositionTagged<string> suffix, params HtmlAttributeValue[] values)
        {
            Writer.Write(prefix.Value);
            foreach (HtmlAttributeValue av in values)
            {
                Writer.Write(av.Value.Value);
            }
            Writer.Write(suffix.Value);
        }
    }
}
