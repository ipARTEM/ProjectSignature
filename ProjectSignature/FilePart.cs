using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSignature
{
    public class FilePart : IDisposable
    {
        public int numberOfPart;
        public long startByte;
        public long partSize;
        public byte[] bytesOfPart;
        public byte[] hashOfPart;
        public string filePath;

        SHA256 hs = SHA256.Create();
        private bool disposed = false;

        public FilePart(int numberOfPart, long startByte, long partSize, string filePath)
        {
            this.numberOfPart = numberOfPart;
            this.startByte = startByte;
            this.partSize = partSize;
            this.filePath = filePath;
            bytesOfPart = null;
            hashOfPart = null;
        }

        public byte[] ReadBytes(FileStream fs, long startByte)
        {

            try
            {
                bytesOfPart = new byte[partSize];
                fs.Seek(startByte, SeekOrigin.Current);
                fs.Read(bytesOfPart, 0, (int)partSize);
            }
            catch (OutOfMemoryException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                throw;
            }

            return bytesOfPart;
        }

        public byte[] GetHash(byte[] bytes)
        {

            hashOfPart = hs.ComputeHash(bytes);
            return hashOfPart;
        }

        public void ShowResult(FilePart part)
        {
            Console.WriteLine("Блок № " + part.numberOfPart);
            Console.WriteLine();
            foreach (var hash in part.hashOfPart)
            {
                Console.Write(String.Format("{0:X2}", hash));
            }
            Console.WriteLine();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    bytesOfPart = null;
                    hashOfPart = null;
                }
                disposed = true;
            }
        }

        ~FilePart()
        {
            Dispose(false);
        }

    }
}
