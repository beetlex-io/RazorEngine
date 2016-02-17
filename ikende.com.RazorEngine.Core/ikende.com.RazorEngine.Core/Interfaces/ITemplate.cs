using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace ikende.com.RazorEngine
{
    public interface ITemplate
    {
        dynamic ViewData
        {
            get;
        }

        dynamic DataContext { get; set; }

        dynamic Model { get; }

        object this[string key] { get; set; }

        IContext Context { get; set; }

        IHtmlString RenderBody();

        string Layout { get; set; }

        void DefineSection(string name, Action action);

        IHtmlString RenderSection(string name);

        bool IsSectionDefined(string name);

        IHtmlString RenderPage(string name);

        System.IO.TextWriter Writer
        {
            get;
            set;
        }

        IHtmlString RenderPage(string name, params object[] data);
 
        void Execute();

        IHtmlString Raw(object value);

      

        void WriteAttribute(string name, PositionTagged<string> prefix, PositionTagged<string> suffix, params HtmlAttributeValue[] values);

        IHtmlString Each(string name, object items);

        IHtmlString EachSection(string name, object items);   

        void Write(object value);

        void WriteLiteral(string literal);
    }

    internal class HashCodeCombiner
    {
        private long _combinedHash64 = 0x1505L;

        public int CombinedHash
        {
            get { return _combinedHash64.GetHashCode(); }
        }

        public HashCodeCombiner Add(IEnumerable e)
        {
            if (e == null)
            {
                Add(0);
            }
            else
            {
                int count = 0;
                foreach (object o in e)
                {
                    Add(o);
                    count++;
                }
                Add(count);
            }
            return this;
        }

        public HashCodeCombiner Add(int i)
        {
            _combinedHash64 = ((_combinedHash64 << 5) + _combinedHash64) ^ i;
            return this;
        }

        public HashCodeCombiner Add(object o)
        {
            int hashCode = (o != null) ? o.GetHashCode() : 0;
            Add(hashCode);
            return this;
        }

        public static HashCodeCombiner Start()
        {
            return new HashCodeCombiner();
        }
    }

     [DebuggerDisplay("({Position})\"{Value}\"")]
    public class PositionTagged<T>
    {
        private PositionTagged()
        {
            Position = 0;
            Value = default(T);
        }

        public PositionTagged(T value, int offset)
        {
            Position = offset;
            Value = value;
        }

        public int Position { get; private set; }
        public T Value { get; private set; }

        public override bool Equals(object obj)
        {
            PositionTagged<T> other = obj as PositionTagged<T>;
            return other != null &&
                   other.Position == Position &&
                   Equals(other.Value, Value);
        }

        public override int GetHashCode()
        {
            return HashCodeCombiner.Start()
                .Add(Position)
                .Add(Value)
                .CombinedHash;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static implicit operator T(PositionTagged<T> value)
        {
            return value.Value;
        }

        public static implicit operator PositionTagged<T>(Tuple<T, int> value)
        {
            return new PositionTagged<T>(value.Item1, value.Item2);
        }

        public static bool operator ==(PositionTagged<T> left, PositionTagged<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PositionTagged<T> left, PositionTagged<T> right)
        {
            return !Equals(left, right);
        }
    }

     public class HtmlAttributeValue
    {
         public HtmlAttributeValue(PositionTagged<string> prefix, PositionTagged<object> value, bool literal)
        {
            Prefix = prefix;
            Value = value;
            Literal = literal;
        }

        public PositionTagged<string> Prefix { get; private set; }

        public PositionTagged<object> Value { get; private set; }

        public bool Literal { get; private set; }


        public static HtmlAttributeValue FromTuple(Tuple<Tuple<string, int>, Tuple<object, int>, bool> value)
        {
            return new HtmlAttributeValue(value.Item1, value.Item2, value.Item3);
        }


        public static HtmlAttributeValue FromTuple(Tuple<Tuple<string, int>, Tuple<string, int>, bool> value)
        {
            return new HtmlAttributeValue(value.Item1, new PositionTagged<object>(value.Item2.Item1, value.Item2.Item2), value.Item3);
        }


        public static implicit operator HtmlAttributeValue(Tuple<Tuple<string, int>, Tuple<object, int>, bool> value)
        {
            return FromTuple(value);
        }


        public static implicit operator HtmlAttributeValue(Tuple<Tuple<string, int>, Tuple<string, int>, bool> value)
        {
            return FromTuple(value);
        }
    }
}
