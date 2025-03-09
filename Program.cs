using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace MutexHomework09._03
{
    class Program
    {
        static Mutex mutex1 = new Mutex();
        static Mutex mutex2 = new Mutex();
        static Mutex mutex3 = new Mutex();

        static void Main()
        {
            Thread thread1 = new Thread(FirstApp);
            Thread thread2 = new Thread(SecondApp);
            Thread thread3 = new Thread(ThirdApp);

            thread1.Start();
            thread1.Join();  
            thread2.Start();
            thread2.Join();  
            thread3.Start();
        }

        static void FirstApp()
        {
            mutex1.WaitOne();
            try
            {
                FileStream file = new FileStream("C:\\Users\\User\\Desktop\\1array1.dat", FileMode.Create, FileAccess.Write);
                BinaryWriter writer = new BinaryWriter(file);

                Random random = new Random();
                List<int> numbers = new List<int>();

                for (int i = 0; i < 50; i++)
                {
                    numbers.Add(random.Next(1, 1000));
                }

                foreach (var number in numbers)
                {
                    if (IsPrime(number))
                        writer.Write(number);
                }

                writer.Close();
                file.Close();
                Console.WriteLine("Первый поток");
            }
            finally
            {
                mutex1.ReleaseMutex();
            }
        }

        static void SecondApp()
        {
            mutex2.WaitOne();
            try
            {
                FileStream file1 = new FileStream("C:\\Users\\User\\Desktop\\1array1.dat", FileMode.Open, FileAccess.Read);
                BinaryReader reader = new BinaryReader(file1);
                FileStream file = new FileStream("C:\\Users\\User\\Desktop\\2array2.dat", FileMode.Create, FileAccess.Write);
                BinaryWriter writer = new BinaryWriter(file);

                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    int num = reader.ReadInt32();
                    if (IsPrime(num))
                        writer.Write(num);
                }

                writer.Close();
                file.Close();
                reader.Close();
                file1.Close();
                Console.WriteLine("Второй поток");
            }
            finally
            {
                mutex2.ReleaseMutex();
            }
        }

        static void ThirdApp()
        {
            mutex3.WaitOne();
            try
            {
                FileStream file1 = new FileStream("C:\\Users\\User\\Desktop\\2array2.dat", FileMode.Open, FileAccess.Read);
                BinaryReader reader = new BinaryReader(file1);
                FileStream file = new FileStream("C:\\Users\\User\\Desktop\\3array3.dat", FileMode.Create, FileAccess.Write);
                BinaryWriter writer = new BinaryWriter(file);

                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    int num = reader.ReadInt32();
                    if (num % 10 == 7)
                        writer.Write(num);
                }

                writer.Close();
                file.Close();
                reader.Close();
                file1.Close();
                Console.WriteLine("Третий поток");
            }
            finally
            {
                mutex3.ReleaseMutex();
            }
        }

        static bool IsPrime(int number)
        {
            if (number < 2) return false;
            for (int i = 2; i <= Math.Sqrt(number); i++)
            {
                if (number % i == 0) return false;
            }
            return true;
        }
    }
}
