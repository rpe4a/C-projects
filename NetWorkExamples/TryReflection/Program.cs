using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp.RuntimeBinder;

namespace TryReflection
{
    class Program
    {
        static void Main(string[] args)
        {
            var t = typeof(SomeType);

            BindToMemberThenInvokeTheMember(t);
            Console.WriteLine();

            BindToMemberCreateDelegateToMemberThenInvokeTheMember(t);
            Console.WriteLine();

            UseDynamicToBindAndInvokeTheMember(t);
            Console.WriteLine();
        }

        private static void BindToMemberThenInvokeTheMember(Type t)
        {
            Console.WriteLine("BindToMemberThenInvokeTheMember");

            Type ctorArgument = Type.GetType("System.Int32&");
            ConstructorInfo ctor =
                t.GetTypeInfo().DeclaredConstructors.First(c => c.GetParameters()[0].ParameterType == ctorArgument);

            var args = new object[] {12};
            Console.WriteLine("x before ctor call: {0}", args[0]);
            var obj = ctor.Invoke(args);
            Console.WriteLine("Type: " + obj.GetType());
            Console.WriteLine("x after ctor called: {0}", args[0]);

            FieldInfo fi = obj.GetType().GetTypeInfo().GetDeclaredField("m_someField");
            fi.SetValue(obj, 33);
            Console.WriteLine("someField: {0}", fi.GetValue(obj));

            MethodInfo mi = obj.GetType().GetTypeInfo().GetDeclaredMethod("ToString");
            var result = mi.Invoke(obj, null);
            Console.WriteLine("ToString: " + result.ToString());

            PropertyInfo pi = obj.GetType().GetTypeInfo().GetDeclaredProperty("SomeProp");
            try
            {
                pi.SetValue(obj, 0, null);
            }
            catch (TargetInvocationException e)
            {
                if (e.InnerException.GetType() != typeof (ArgumentOutOfRangeException)) throw;

                Console.WriteLine("Property set catch;");
            }

            pi.SetValue(obj, 2, null);

            Console.WriteLine("SomeProp: "+pi.GetValue(obj));

            EventInfo ei = obj.GetType().GetTypeInfo().GetDeclaredEvent("SomeEvent");
            EventHandler eh = new EventHandler(EventCallBack);
            ei.AddEventHandler(obj, eh);
            ei.RemoveEventHandler(obj, eh);
        }

        private static void EventCallBack(object sender, EventArgs e) { }

        private static void UseDynamicToBindAndInvokeTheMember(Type t)
        {
            Console.WriteLine("UseDynamicToBindAndInvokeTheMember");

            var args = new object[] {12};
            Console.WriteLine("x before ctor call: {0}", args[0]);
            dynamic obj = Activator.CreateInstance(t, args);
            Console.WriteLine("Type: " + obj.GetType());
            Console.WriteLine("x after ctor called: {0}", args[0]);

            try
            {
                obj.m_someField = 5;
                int v = (int) obj.m_someField;
                Console.WriteLine("someField: " + v);
            }
            catch (RuntimeBinderException e)
            {
                Console.WriteLine("Failed to access field: ", e.Message);
            }

            var s = (string) obj.ToString();
            Console.WriteLine("ToString " + s);

            try
            {
                obj.SomeProp = 0;
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Property set catch");
            }

            obj.SomeProp = 2;
            int p = (int) obj.SomeProp;
            Console.WriteLine("someField: " + p);

            obj.SomeEvent += new EventHandler(EventCallBack);
            obj.SomeEvent -= new EventHandler(EventCallBack);
        }

        private static void BindToMemberCreateDelegateToMemberThenInvokeTheMember(Type t)
        {
        }

        

        private static void ShowAssemlies()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                Show(0, "Assembly {0}", assembly);


                foreach (var exportedType in assembly.ExportedTypes)
                {
                    Show(1, "Type: {0}", exportedType);

                    foreach (MemberInfo declaredMember in exportedType.GetTypeInfo().DeclaredMembers)
                    {
                        string typeName = String.Empty;
                        if (declaredMember is Type) typeName = "(Nested) Type";
                        if (declaredMember is FieldInfo) typeName = "FieldInfo";
                        if (declaredMember is PropertyInfo) typeName = "PropertyInfo";
                        if (declaredMember is ConstructorInfo) typeName = "ConstructorInfo";
                        if (declaredMember is MethodInfo) typeName = "MethodInfo";
                        if (declaredMember is EventInfo) typeName = "EventInfo";

                        Show(2, "{0}: {1}", typeName, declaredMember);
                    }
                }
            }
        }

        private static void Show(int indent, string format, params object[] args)
        {
            Console.WriteLine(new string(' ', 3* indent) + format, args);
        }
    }
}
