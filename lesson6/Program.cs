using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Богатов Максим

namespace lesson6
{
    // Описываем делегат. В делегате описывается сигнатура методов, на
    // которые он сможет ссылаться в дальнейшем (хранить в себе)

    public delegate double Fun(double a,double x);
    public delegate double FunEx2(double x);

    class Program
    {
        static int MyMethod(Student st1, Student st2)//Создаем метод для сравнения для экземпляров
        {
            //Сравниваем две строки
            //return String.Compare(st1.firstName, st2.firstName);
            if (st1.course > st2.course) return 1;
            if (st1.course < st2.course) return -1;
            return 0;
        }

        static int AgeSort(Student st1, Student st2)//Создаем метод для сравнения для экземпляров
        {
            if (st1.age > st2.age) return 1;
            if (st1.age < st2.age) return -1;
            return 0;
        }

        //1.	Изменить программу вывода таблицы функции так, чтобы можно было передавать функции типа double (double, double). 
        //    Продемонстрировать работу на функции с функцией a* x^2 и функцией a* sin(x).
        #region Ex1
        public static void Table(Fun F, double a, double x, double b)
        {
            Console.WriteLine(" ----- A ------ X ------ Func ----");
            while (x <= b)
            {
                Console.WriteLine("| {0,8:0.000} | {1,8:0.000} | {2,8:0.000} |", a, x, F(a,x));
                x += 1;
            }
            Console.WriteLine("---------------------");
        }
        // Создаем метод для передачи его в качестве параметра в Table

        public static double MyFunc1(double a, double x)
        {
            return a * Math.Pow(x,2);
        }

        public static double MyFunc2(double a, double x)
        {
            return a * Math.Sin(x);
        }

        #endregion

        #region Ex2
        //2.Модифицировать программу нахождения минимума функции так, чтобы можно было передавать функцию в виде делегата. 
        //а) Сделать меню с различными функциями и представить пользователю выбор, для какой функции и на каком отрезке находить минимум.Использовать массив(или список) делегатов,
        //в котором хранятся различные функции.
        //б) *Переделать функцию Load, чтобы она возвращала массив считанных значений.Пусть она возвращает минимум через параметр(с использованием модификатора out). 

        public static double F1(double x)
        {
            return x * x - 50 * x + 10;
        }
        public static double F2(double x)
        {
            return x * x+10;
        }
        public static double F3(double x)
        {
            return x * x + x + 10;
        }

        public static void SaveFunc(string fileName, double a, double b, double h, FunEx2 Fex2)
        {
            FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs);
            double x = a;
            while (x <= b)
            {
                bw.Write(Fex2(x));
                x += h;// x=x+h;
            }
            bw.Close();
            fs.Close();
        }
        public static double Load(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader bw = new BinaryReader(fs);
            double min = double.MaxValue;
            double d;
            for (int i = 0; i < fs.Length / sizeof(double); i++)
            {
                // Считываем значение и переходим к следующему
                d = bw.ReadDouble();
                if (d < min) min = d;
            }
            bw.Close();
            fs.Close();
            return min;
        }

        //б) *Переделать функцию Load, чтобы она возвращала массив считанных значений.Пусть она возвращает минимум через параметр(с использованием модификатора out).
        
        public static double [] Load2(string fileName,out double min)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader bw = new BinaryReader(fs);            
            double [] d=new double [fs.Length / sizeof(double)];
            min = double.MaxValue;
            for (int i = 0; i < fs.Length / sizeof(double); i++)
            {
                // Считываем значение и переходим к следующему
                d[i] = bw.ReadDouble();
                if (d[i] < min) min = d[i];
            }
            bw.Close();
            fs.Close();
            return d;
        }

        #endregion

        static void Main()
        {
            double a = 2;
            double x = -2;
            double b = 2;

            try
            {

                //1.	Изменить программу вывода таблицы функции так, чтобы можно было передавать функции типа double (double, double). 
                //    Продемонстрировать работу на функции с функцией a* x^2 и функцией a* sin(x).

                Console.WriteLine("Таблица функции a* x^2:");
                Table(MyFunc1, a, x,b);

                Console.WriteLine("Таблица функции a* sin(x):");
                Table(MyFunc2, a, x, b);

                //2.Модифицировать программу нахождения минимума функции так, чтобы можно было передавать функцию в виде делегата. 

                Console.WriteLine("Минимум функции:");
                SaveFunc("data.bin", -100, 100, 10,F1);
                Console.WriteLine(Load("data.bin"));

                //а) Сделать меню с различными функциями и представить пользователю выбор, для какой функции и на каком отрезке находить минимум.Использовать массив(или список) делегатов,
                //в котором хранятся различные функции.

                List<FunEx2> FuncList = new List<FunEx2> {F1,F2,F3};

                Console.WriteLine("Выберите функцию для поиска минимума Введите 1 или 2 или 3:");
                string fnum =Console.ReadLine();

                Console.WriteLine("Введите отрезок:");
                int h = Convert.ToInt32(Console.ReadLine());

                double min;

                //switch (fnum)
                //{
                //    case "1":
                //        SaveFunc("data.bin", -100, 100, h, F1);
                //        Console.WriteLine(Load("data.bin"));
                //        break;
                //    case "2":
                //        SaveFunc("data.bin", -100, 100, h, F2);
                //        Console.WriteLine(Load("data.bin"));
                //        break;
                //    case "3":
                //        SaveFunc("data.bin", -100, 100, h, F3);
                //        Console.WriteLine(Load("data.bin"));
                //        break;
                //    default:
                //    break;
                //}

                //б) *Переделать функцию Load, чтобы она возвращала массив считанных значений.Пусть она возвращает минимум через параметр(с использованием модификатора out).

                switch (fnum)
                {
                    case "1":
                        SaveFunc("data.bin", -100, 100, h, F1);
                        Load2("data.bin",out min);
                        Console.WriteLine(min);
                        break;
                    case "2":
                        SaveFunc("data.bin", -100, 100, h, F2);
                        Load2("data.bin", out min);
                        Console.WriteLine(min);
                        break;
                    case "3":
                        SaveFunc("data.bin", -100, 100, h, F3);
                        Load2("data.bin", out min);
                        Console.WriteLine(min);
                        break;
                    default:
                        break;
                }


            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

            //3.Переделать программу Пример использования коллекций для решения следующих задач:
            //а) Подсчитать количество студентов учащихся на 5 и 6 курсах;
            //б) подсчитать сколько студентов в возрасте от 18 до 20 лет на каком курсе учатся(*частотный массив);
            //в) отсортировать список по возрасту студента;
            //г) *отсортировать список по курсу и возрасту студента;

            int bakalav = 0;
            int magistr = 0;
            int fivecrs = 0;
            int sixcrs = 0;
            //Создаем список студентов
            List<Student> list = new List<Student>();
            //DateTime dt = DateTime.Now;
            StreamReader sr = new StreamReader("students_4.csv");
            while (!sr.EndOfStream)
            {
                try
                {
                    string[] s = sr.ReadLine().Split(';');
                    //Добавляем в список новый экземляр класса Student
                    Student t;
                    t = new Student(s[0], s[1], s[2], s[3], s[4], int.Parse(s[5]), Convert.ToInt32(s[6]), int.Parse(s[7]), s[8]);
                    list.Add(t);

                    //Одновременно подсчитываем кол-во бакалавров и магистров
                    if (t.course < 5) bakalav++; else magistr++;

                    if (t.course == 5) fivecrs++;
                    if(t.course == 6) sixcrs++;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            sr.Close();
            //for (int i = 0; i < list.Count; i++) ;

            list.Sort(MyMethod); //сортировка по курсу
            // foreach (var v in list) Console.WriteLine(v);
            Console.WriteLine("Всего студентов:" + list.Count);
            Console.WriteLine("Магистров:{0}", magistr);
            Console.WriteLine("Бакалавров:{0}", bakalav);
            Console.WriteLine("Студентов 5го курса:{0}", fivecrs);
            Console.WriteLine("Студентов 6го курса:{0}", sixcrs);

            //б) подсчитать сколько студентов в возрасте от 18 до 20 лет на каком курсе учатся(*частотный массив);
            Dictionary<int, int> dic = new Dictionary<int, int>();
            foreach (var item in list)
            {
                if (item.age >= 18 && item.age <= 20)
                {
                    if (!dic.ContainsKey(item.course)) dic.Add(item.course, 1);
                    else dic[item.course]++;
                }                                 
                
            }

            Console.WriteLine("Студентов в возрасте от 18 до 20 лет:");

            foreach (var item in dic)
            {

                Console.WriteLine("--- Курс -- Кол-во --");
                    Console.WriteLine("| {0,8:0.000} | {1,8:0.000}|", item.Key, item.Value);
                    x += 1;              

            }

            Console.WriteLine("---------------------");

            list.Sort(AgeSort); //сортировка по возрасту

            //Console.WriteLine(DateTime.Now - dt);

            //4.  * *Считайте файл различными способами.Смотрите “Пример записи файла различными способами”. 
            //Создайте методы, которые возвращают массив byte(FileStream, BufferedStream), строку для StreamReader и массив int для BinaryReader.

            long kbyte = 1024;
            long mbyte = 1024 * kbyte;
            long gbyte = 1024 * mbyte;
            long size = mbyte;
            //Write FileStream
            //Write BinaryStream
            //Write StreamReader/StreamWriter
            //Write BufferedStream
            Console.WriteLine("Запись");
            Console.WriteLine("FileStream. Milliseconds:{0}", FileStreamSample("bigdata0.bin", size));
            Console.WriteLine("BinaryStream. Milliseconds:{0}", BinaryStreamSample("bigdata1.bin", size));
            Console.WriteLine("StreamWriter. Milliseconds:{0}", StreamWriterSample("bigdata2.bin", size));
            Console.WriteLine("BufferedStream. Milliseconds:{0}", BufferedStreamSample("bigdata3.bin", size));
            Console.WriteLine("Чтение");
            Console.WriteLine("ReadFileStream. Milliseconds:{0}", ReadFileStreamSample("bigdata0.bin", size));
            Console.WriteLine("ReadBinaryStream. Milliseconds:{0}", ReadBinaryStreamSample("bigdata1.bin", size));
            Console.WriteLine("StreamRead. Milliseconds:{0}", StreamReaderSample("bigdata2.bin", size));
            Console.WriteLine("ReadBufferedStream. Milliseconds:{0}", ReadBufferedStreamSample("bigdata3.bin", size));


            Console.ReadKey();
        }

        static long FileStreamSample(string filename, long size)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
            //FileStream fs = new FileStream("D:\\temp\\bigdata.bin", FileMode.CreateNew, FileAccess.Write);
            for (int i = 0; i < size; i++)
                fs.WriteByte(0);
            fs.Close();
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }

        static long BinaryStreamSample(string filename, long size)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs);
            for (int i = 0; i < size; i++)
                bw.Write((byte)0);
            fs.Close();
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }

        static long StreamWriterSample(string filename, long size)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            for (int i = 0; i < size; i++)
                sw.Write(0);
            fs.Close();
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }

        static long BufferedStreamSample(string filename, long size)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
            int countPart = 4;//количество частей
            int bufsize = (int)(size / countPart);
            byte[] buffer = new byte[size];
            BufferedStream bs = new BufferedStream(fs, bufsize);
            //bs.Write(buffer, 0, (int)size);//Error!
            for (int i = 0; i < countPart; i++)
                bs.Write(buffer, 0, (int)bufsize);
            fs.Close();
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }

        static long ReadFileStreamSample(string filename, long size)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            //FileStream fs = new FileStream("D:\\temp\\bigdata.bin", FileMode.CreateNew, FileAccess.Write);
            for (int i = 0; i < size; i++)
                fs.ReadByte();
            fs.Close();
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }

        static long ReadBinaryStreamSample(string filename, long size)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            BinaryReader bw = new BinaryReader(fs);
            for (int i = 0; i < size; i++)
                bw.Read();
            fs.Close();
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }

        static long StreamReaderSample(string filename, long size)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            StreamReader sw = new StreamReader(fs);
            for (int i = 0; i < size; i++)
                sw.Read();
            fs.Close();
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }

        static long ReadBufferedStreamSample(string filename, long size)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            int countPart = 4;//количество частей
            int bufsize = (int)(size / countPart);
            byte[] buffer = new byte[size];
            BufferedStream bs = new BufferedStream(fs, bufsize);
            //bs.Write(buffer, 0, (int)size);//Error!
            for (int i = 0; i < countPart; i++)
                bs.Read(buffer, 0, (int)bufsize);
            fs.Close();
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }

    }


}
