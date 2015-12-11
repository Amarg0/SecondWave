using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace Lenta
{
    class  Widget
    {
        public static Semaphore CSemaphore = new Semaphore(1,1);
        public static Semaphore ModSemaphore = new Semaphore(1,1);
        public static Semaphore WidgetSemaphore = new Semaphore(1,1);

        Object A;
        Thread C = new Thread(Get_C);

        public Widget(Object obj)
        {
            WidgetSemaphore.WaitOne();
            A = new Module(obj);
            //A = new Module(obj);
            C.Start(obj);
            WidgetSemaphore.Release();
        }

        public static void Get_C(Object obj)
        {
            CSemaphore.WaitOne();
            Thread.Sleep(3000);
            Console.WriteLine("Получена деталь C для №{0}",obj.ToString());
            CSemaphore.Release();
        }
    }

    class Module
    {
        public static Semaphore ASemaphore = new Semaphore(1, 1);
        public static Semaphore BSemaphore = new Semaphore(1, 1);
        public static Semaphore ModSemaphore = new Semaphore(1,1);

        Thread ModA = new Thread(Get_A);
        Thread ModB = new Thread(Get_B);

        public Module(Object obj)
        {
            ModSemaphore.WaitOne();
            ModA.Start(obj);
            ModB.Start(obj);
            ModSemaphore.Release();
        }

        public static void Get_A(Object obj)
        {
            ASemaphore.WaitOne();
            Thread.Sleep(1000);
            Console.WriteLine("Получена деталь A для №{0}",obj.ToString());
            ASemaphore.Release();
        }

        public static void Get_B(Object obj)
        {
            BSemaphore.WaitOne();
            Thread.Sleep(2000);
            Console.WriteLine("Получена деталь B для №{0}",obj.ToString());
            BSemaphore.Release();
        }
    }

    class Lenta
    {
        public static List<Widget> Widgets = new List<Widget>();
        public static Semaphore LentaSemaphore = new Semaphore(1,5);

        public static void AddWidg(Object obj)
        {
            LentaSemaphore.WaitOne();
            Widgets.Add(new Widget(obj));
            LentaSemaphore.Release();
        }
    }
    class Program
    {
        public static Semaphore MainSemaphore = new Semaphore(1, 5);
        static List<Thread> thread = new List<Thread>();
        public static int i, ToUse, ToGet = 5;
        static void Main(string[] args)
        {
            Thread Use_widget = new Thread(GetTouch);
            Use_widget.Start();
            while (true)
            {
                if (i < ToGet)
                {
                    MainSemaphore.WaitOne();
                    Console.WriteLine("Начинаю выпуск детали №{0}",i);
                    thread.Add(new Thread(Lenta.AddWidg));
                    thread[i].Start(i.ToString());
                    i++;
                    MainSemaphore.Release();
                }
            }
        }

        static void GetTouch()
        {
            ConsoleKeyInfo cki = new ConsoleKeyInfo();
            while (true)
                if (Console.KeyAvailable == true)
                {
                    cki = Console.ReadKey();
                    if (cki.Key == ConsoleKey.Enter)
                    {
                        Console.WriteLine("Деталь №{0} была использована",ToUse);
                        ToGet++;
                        ToUse++;
                    }
                }
        }
    }
}
