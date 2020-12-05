using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace OsSrMainProc
{
    public class FileWriter
    {
        public void WriteProccess(string filePath)
        {
            int count = File.ReadLines(@"D:\projects\OsFirst.txt").Count();
            a(count);
            using (StreamWriter sw = new StreamWriter(filePath, false))
            {
                var rowsCount = RowCountRandomiser();
                var rowsArray = RowRandomiser(rowsCount);

                foreach (var row in rowsArray)
                {
                    sw.WriteLine(row);
                }
            }
        }

        private int RowCountRandomiser()
        {
            var random = new Random();
            int randomNumber = random.Next(1000, 100000);
            return randomNumber;
        }

        private string[] RowRandomiser(int rowsCount)
        {
            var random = new Random();
            var rowSize = random.Next(1, 100);
            var stringArrayResult = new string[rowsCount];
            for (int i = 0; i < rowsCount; i++)
            {
                stringArrayResult[i] = StringGenerator(rowSize);
            }

            return stringArrayResult;
        }

        private string StringGenerator(int rowSize)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[rowSize];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            return new String(stringChars);
        }

        private  void a(int count)
        {
            if (count > 10000)
            {
                var ab = Process.GetProcesses();
                /*ab.Kill();*/
            }
        }
    }
}