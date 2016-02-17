using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ikende.com.RazorEngine.Implement
{
    class ForeachSection : IHtmlString
    {
        public object Items
        {
            get;
            set;
        }

        public string Section
        {
            get;
            set;
        }

        public ITemplate Template
        {
            get;
            set;
        }

        public void Execute(IContext context)
        {
            if (Items != null)
            {
                foreach (object item in Items as System.Collections.IEnumerable)
                {
                    try
                    {
                        Template.DataContext = item;
                        Action section = null;
                        if (context.Sections.TryGetValue(Section, out section))
                        {
                            section();
                        }
                        else
                        {
                            throw new RazorException("{0} section not found", Section);
                        }
                    }
                    finally
                    {
                        Template.DataContext = context.Model;
                    }
                }
            }

        }

        public override string ToString()
        {
            return null;
        }
    }
    class ForeachView : IHtmlString
    {
        public object Items
        {
            get;
            set;
        }
        public string View
        {
            get;
            set;
        }
        public ITemplate Template
        {
            get;
            set;
        }
        public void Execute(IContext context)
        {
            if (Items != null)
            {
                ITemplate etemp = context.TemplateHost.GetTemplete(View, context);
                etemp.Writer = Template.Writer;
                foreach (object item in Items as System.Collections.IEnumerable)
                {
                    try
                    {
                        etemp.DataContext = item;
                        etemp.Execute();
                    }
                    finally
                    {
                        etemp.DataContext = context.Model;
                    }
                }
            }
        }
        public override string ToString()
        {
            return null;
        }
    }
}
