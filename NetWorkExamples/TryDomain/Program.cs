using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TryDomain
{
    class Program
    {
        static void Main(string[] args)
        {
            Marshaling();
        }

        private static void Marshaling()
        {
//получаем текущий домен приложения
            AppDomain currentDomain = AppDomain.CurrentDomain;

            AppDomain threadDomain = Thread.GetDomain();

            Console.WriteLine("currentDomain : {0}", currentDomain.FriendlyName);
            Console.WriteLine("threadDomain : {0}", threadDomain.FriendlyName);

            Console.WriteLine("************************************************");
            Console.WriteLine();

            //получаем сборку в домене, содержащем метод Main
            string exeAssembly = Assembly.GetEntryAssembly().FullName;
            Console.WriteLine("currentAssembly: {0}", exeAssembly);
            Console.WriteLine("************************************************");
            Console.WriteLine();

            AppDomain ap2 = null;

            Console.WriteLine("{0}Demo #1:", Environment.NewLine);
            Console.WriteLine("\tДоступ к объектам другого домена приложений с продвижением по ссылке");

            //Создаем новый домен
            ap2 = AppDomain.CreateDomain("AP #1", null, null);
            MarshalByRefType mbrt = null;

            //Загружаем нашу сборку в новый домен, конструируем объект и продвигаем его обратно в наш домен(получаем ссылку на представитель)
            mbrt = (MarshalByRefType) ap2.CreateInstanceAndUnwrap(exeAssembly, typeof(MarshalByRefType).FullName);

            Console.WriteLine("Type = {0}", mbrt.GetType());
            Console.WriteLine("IsProxy = {0}", RemotingServices.IsTransparentProxy(mbrt));

            mbrt.SomeMethod();

            AppDomain.Unload(ap2);

            try
            {
                mbrt.SomeMethod();
                Console.WriteLine("success.");
            }
            catch
            {
                Console.WriteLine("fail.");
            }

            Console.WriteLine("************************************************");
            Console.WriteLine();

            Console.WriteLine("{0}Demo #2:", Environment.NewLine);
            Console.WriteLine("\tДоступ к объектам другого домена приложений с продвижением по значению");

            //Создаем новый домен
            ap2 = AppDomain.CreateDomain("AP #2", null, null);

            //Загружаем нашу сборку в новый домен, конструируем объект и продвигаем его обратно в наш домен(получаем ссылку на представитель)
            mbrt = (MarshalByRefType)ap2.CreateInstanceAndUnwrap(exeAssembly, typeof(MarshalByRefType).FullName);

            Console.WriteLine("mbrt Type = {0}", mbrt.GetType());
            Console.WriteLine("mbrt IsProxy = {0}", RemotingServices.IsTransparentProxy(mbrt));
            
            //Метод возвращает копию возвращенного объекта. продвижение объекта происходит по значению
            MarshalByValType mrbv =  mbrt.MethodWithReturn();

            Console.WriteLine("mrbv Type = {0}", mrbv.GetType());

            //Убеждаемся что это не прокси объект
            Console.WriteLine("mrbv IsProxy = {0}", RemotingServices.IsTransparentProxy(mrbv));

            Console.WriteLine("Returned object created " + mrbv.ToString());

            //выгружаем домен
            AppDomain.Unload(ap2);

            try
            {
                //пробуем вызвать метод на возвращенном объекте, т.к. он не прокси все работает как надо
                Console.WriteLine("Returned object created " + mrbv.ToString());
                Console.WriteLine("success.");
            }
            catch
            {
                Console.WriteLine("fail.");
            }

            Console.WriteLine("************************************************");
            Console.WriteLine();

            Console.WriteLine("{0}Demo #3:", Environment.NewLine);
            Console.WriteLine("\tДоступ к объектам другого домена приложений без использования механизма продвижения");

            //Создаем новый домен
            ap2 = AppDomain.CreateDomain("AP #3", null, null);

            //Загружаем нашу сборку в новый домен, конструируем объект и продвигаем его обратно в наш домен(получаем ссылку на представитель)
            mbrt = (MarshalByRefType)ap2.CreateInstanceAndUnwrap(exeAssembly, typeof(MarshalByRefType).FullName);

            Console.WriteLine("mbrt Type = {0}", mbrt.GetType());
            Console.WriteLine("mbrt IsProxy = {0}", RemotingServices.IsTransparentProxy(mbrt)); 

            //Продвижение объекта не происходит, т.к. нет атрибута [Serializable] или он не наследуется от MarshalByRefObject
            NonMarshalableType nmt = mbrt.MethodArgAndReturn(AppDomain.CurrentDomain.FriendlyName);

            Console.WriteLine("nmt Type = {0}", nmt.GetType());

            //Убеждаемся что это не прокси объект
            Console.WriteLine("nmt IsProxy = {0}", RemotingServices.IsTransparentProxy(nmt));

            Console.WriteLine("Returned object created " + nmt.ToString());

            //выгружаем домен
            AppDomain.Unload(ap2);

            try
            {
                //пробуем вызвать метод на возвращенном объекте, т.к. он не прокси все работает как надо
                Console.WriteLine("Returned object created " + nmt.ToString());
                Console.WriteLine("success.");
            }
            catch
            {
                Console.WriteLine("fail.");
            }
        }
    }
}
