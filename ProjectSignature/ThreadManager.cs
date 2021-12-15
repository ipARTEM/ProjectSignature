using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSignature
{
    static class ThreadManager
    {
        public static int threadsCounter = 1;
        static Semaphore sem = new Semaphore(1, 1);
        static int i = 0;
        static Object thisLock = new Object();
        static bool ok;

        static public void CreateThread(object obj)
        {
            Thread thread = new Thread(new ParameterizedThreadStart(FilePartOperations));

            FilePart fp = (FilePart)obj;

            lock (thisLock)
            {
                ok = false;
                do
                {
                    if (SystemState.IsValid(fp.partSize, threadsCounter))
                    {
                        thread.Name = "Поток " + i.ToString();
                        thread.Start(obj);
                        i++;
                        Interlocked.Increment(ref threadsCounter);
                        ok = true;
                    }
                    else Thread.Sleep(0);
                } while (ok == false);
            }
        }

        static public void FilePartOperations(object obj)
        {

            FilePart part = (FilePart)obj;

            FileStream fs = File.OpenRead(part.filePath);

            part.ReadBytes(fs, part.startByte);
            part.GetHash(part.bytesOfPart);

            fs.Close();

            sem.WaitOne();
            part.ShowResult(part);
            sem.Release();

            Interlocked.Decrement(ref threadsCounter);

            part.Dispose();

        }
    }
}
