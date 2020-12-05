using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using OsSrMainProc;

namespace OsSrMainProccess
{
    public class MainThreatCreatop
    {
        public static AutoResetEvent mainWaitHandler = new AutoResetEvent(true);
        public static AutoResetEvent firstWaitHandler = new AutoResetEvent(true);
        public static AutoResetEvent secondWaitHandler = new AutoResetEvent(true);
        public static AutoResetEvent thirdWaitHandler = new AutoResetEvent(true);
        public static AutoResetEvent fourthWaitHandler = new AutoResetEvent(false);
        public static int counter  = 0;
        private static readonly string filePath = @"D:\projects\OsFirst.txt";
        static TimeSpan[] timeMatrix = new TimeSpan[3];
        private static int i = 0;
        static Thread firstThread = new Thread(new ThreadStart(FirstFileWriter));
        static Thread secondThread = new Thread(new ThreadStart(SecondFileWriter));
        static Thread thirdThread = new Thread(new ThreadStart(ThirdFileWriter));
        static Thread fourthThread = new Thread(new ThreadStart(FourthThreadWorker));
        static int[] order = new int[] {1,2,3};

        public void FirstIneration()
        {
            mainWaitHandler.WaitOne();
            firstThread.Start();
            mainWaitHandler.WaitOne();
            secondThread.Start();
            mainWaitHandler.WaitOne();
            thirdThread.Start();
            mainWaitHandler.WaitOne();
            fourthThread.Start();
        }

        public static void ThreadStarter(int[] order)
        {
            foreach (var value in order)
            {
                switch (value)
                {
                    case 1:
                        firstThread.Start();
                        firstThread.Join();
                        firstThread.Interrupt();
                        break;
                    case 2:
                        secondThread.Start();
                        secondThread.Join();
                        secondThread.Interrupt();
                        break;
                    case 3:
                        thirdThread.Start();
                        thirdThread.Join();
                        thirdThread.Interrupt();
                        break;
                }
            }

            i = 0;
            fourthThread.Start();
            fourthThread.Join();
            fourthThread.Interrupt();
        }
        public static void FirstFileWriter()
        {
            firstWaitHandler.WaitOne();
            counter++;
            Stopwatch _stopwatch = new Stopwatch();
            
            _stopwatch.Start();
            new FileWriter().WriteProccess(filePath);
            _stopwatch.Stop();
            
            TimeSpan ts = _stopwatch.Elapsed;
            timeMatrix[i] = ts;
            i++;
            mainWaitHandler.Set();
            if (counter == 3)
            {
                counter = 0;
                fourthWaitHandler.Set();
            }
            
            FirstFileWriter();
        }
        
        public static void SecondFileWriter()
        {
            secondWaitHandler.WaitOne();
            counter++;
            Stopwatch _stopwatch = new Stopwatch();
            
            _stopwatch.Start();
            new FileWriter().WriteProccess(filePath);
            _stopwatch.Stop();
            
            TimeSpan ts = _stopwatch.Elapsed;
            timeMatrix[i] = ts;
            i++;
            mainWaitHandler.Set();
            if (counter == 3)
            {
                counter = 0;
                fourthWaitHandler.Set();
            }
            
            SecondFileWriter();
        }
        
        public static void ThirdFileWriter()
        {
            thirdWaitHandler.WaitOne();
            counter++;
            Stopwatch _stopwatch = new Stopwatch();
            
            _stopwatch.Start();
            new FileWriter().WriteProccess(filePath);
            _stopwatch.Stop();
            
            TimeSpan ts = _stopwatch.Elapsed;
            timeMatrix[i] = ts;
            i++;
            mainWaitHandler.Set();
            if (counter == 3)
            {
                counter = 0;
                fourthWaitHandler.Set();
            }
            ThirdFileWriter();
            
        }

        public static void FourthThreadWorker()
        {
            i = 0;
            fourthWaitHandler.WaitOne();
            var minTime = timeMatrix.Min();
            var minTimeIndex = Array.IndexOf(timeMatrix, minTime);
            var tempValue = order[0];
            order[0] = order[minTimeIndex];
            order[minTimeIndex] = tempValue;
            if (order[1] > order[2])
            {
                var temp = order[2];
                order[2] = order[1];
                order[1] = temp;
            }
            
            foreach (var value in order)
            {
                switch (value)
                {
                    case 1:
                        firstWaitHandler.Set();
                        mainWaitHandler.WaitOne();
                        break;
                    case 2:
                        secondWaitHandler.Set();
                        mainWaitHandler.WaitOne();
                        break;
                    case 3:
                        thirdWaitHandler.Set();
                        mainWaitHandler.WaitOne();
                        break;
                }
            }
            FourthThreadWorker();
        }
    }
    
}