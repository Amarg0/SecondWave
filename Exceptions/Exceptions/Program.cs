using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
/* Создать три класса Triangle, Quadrangle и Circle
 * Конструкторы у них соответственно:
 * -длины 3-х сторон (Int32) -длины 4-х сторон(Int32) -длина радиусы(Int32)
 * При создании (в конструкторе) проверяется возможность существования фигуры с переданными параметрами
 * и выбрасывается исключение (TriangleEx, QUadEx..), если фигуру создать нельзя.
 * Создать тип исключения GeometryException с виртуальным автосвойством Parameters,
 * которое возвращает массив параметров типа Int32.
 * Создать три производных исключения TriangleEx,QuadEx,CircleEx, которые создаются, если в 
 * соотв. класса произошли ошибки. В основной программа в цикле обрабатывает созданием фигур
 * (можно рандомом) и протоколировать исключения от треуг и четырехуг в файл.
 * Исключения, которые приходят от любой фигуры протоколировать отдельно в др. файл, при этом в первый файл они тоже
 * в итоге должны попадать.*/

namespace Exceptions
{
    [Serializable]
    public class GeometryException : Exception
    {
        public int[] Parameters { get; set; }
        public String str { get; set; }

        public GeometryException(params int[] values)
        {
            Parameters = values;
            foreach (var i in values)
            {
                str += i.ToString() + ",";
            }
        }

        public override string Message
        {
            get { return "GeometryException. Невозможно создать фигуру со след. параметрами:"+str.Remove(str.Length-1,1); }
        }
    }

public sealed class TriangleException : GeometryException
    {
        private String str { get; set; }
        public TriangleException(params int[] values):base(values)
        {
            foreach (var i in values)
            {
                str += i.ToString() + ",";
            }
        }

        public override string Message
        {
            get { return "TrinagleException. Нельзя создать треугольник со сторонами: "+str.Remove(5,1); }
        }
    }
    public sealed class QuadrangleException : GeometryException
    {
        private String str { get; set; }
        public QuadrangleException(params int[] values):base(values)
        {
            foreach (var i in values)
            {
                str += i.ToString() + ",";
            }
        }

        public override string Message
        {
            get { return "QuadrangleException. Нельзя создать четырехугольник со сторонами: "+str.Remove(str.Length-1,1); }
        }
    }

    public sealed class CircleException : GeometryException
    {
        private String str { get; set; }
        public CircleException(params int[] values):base(values)
        {
            foreach (var i in values)
            {
                str += i.ToString() + ",";
            }
        }

        public override string Message
        {
            get { return "CircleException. Нельзя создать окружность с радиусом: "+str.Remove(str.Length-1,1); }
        }
    }
    class Triangle
    {
        public int A { get; set; }
        public int B { get; set; }
        public int C { get; set; }

        public Triangle(int a, int b, int c)
        {
            if (a < b + c && b < a + c && c < a + b)
            {
                A = a;
                B = b;
                C = c;
            }
            else
            {
                throw new TriangleException(a,b,c);
            }

        }
    }

    class Quadrangle
    {
        public int A { get; set; } 
        public int B { get; set; }
        public int C { get; set; }
        public int D { get; set; }

        public Quadrangle(int a, int b, int c, int d)
        {
            Check(a,b,c,d);
            A = a;
            B = b;
            C = c;
            D = d;
        }

        static void Check(int a, int b, int c, int d)
        {
            if (a > b + c + d || b > a + c + d || c > a + b + d || d > a + c + b)
                throw new QuadrangleException(a, b, c, d);
        }
    }

    class Circle
    {
        public int Radius { get; set; }

        public Circle(int r)
        {
            Check(r);
            Radius = r;
        }

        static void Check(int r)
        {
            if (r < 0)
            {
                throw new CircleException(r);
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Quadrangle lol = new Quadrangle(1, 2, 3, 10);
            }
            catch (QuadrangleException quadrangle)
            {
                Console.WriteLine(quadrangle.Message);
                try
                {
                    throw new QuadrangleException().GetBaseException();
                }
                catch (GeometryException geometry)
                {
                    Console.WriteLine(geometry.Message);
                }
            }
        }
    }
}
//string file_adress = "c:\\Users\\Amarg0\\Desktop\\101\\" + "GeomtryException.txt";
//FileStream GeomException = new FileStream(file_adress,FileMode.OpenOrCreate);
//StreamWriter writer = new StreamWriter(file_adress);
//writer.WriteLine("Geom");
//writer.Close();
