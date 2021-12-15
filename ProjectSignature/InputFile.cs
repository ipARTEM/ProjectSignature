using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSignature
{
    public class InputFile
    {
        public string filePath;
        public long fileSize;
        long partSize;
        static FileInfo file;

        public InputFile(string filePath, string partSize)
        {
            this.filePath = GetPath(filePath);
            fileSize = GetFileSize(file);
            this.partSize = GetPartSizeValue(partSize);
        }

        static string GetPath(string _filePath)
        {
            string filePath = "";
            try
            {
                file = new FileInfo(_filePath);
                if (file.Exists == false)
                {
                    Console.WriteLine("Файл не существует. Укажите иной путь к файлу в виде аргумента в командной строке и повторите попытку. Нажмите любую клавишу для выхода.");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
                else
                {
                    filePath = _filePath;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                throw;
            }

            return filePath;
        }

        static long GetPartSizeValue(string _partSize)
        {
            long partSize;

            try
            {
                partSize = long.Parse(_partSize);

                if ((partSize / 1000000) * 4 > SystemState.ramCounter.NextValue())
                {
                    Console.WriteLine("Недостаточно доступной RAM и блок данного размера не сможет быть обработан. Укажите другое значение размера блока в виде аргумента в командной строке и повторите попытку. Нажмите любую клавишу для выхода.");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                throw;
            }
            return partSize;
        }

        static long GetFileSize(FileInfo file)
        {
            return (file.Length);
        }

        public void Run()
        {
            long startByte = 0;

            for (int i = 0; i <= Math.Ceiling((decimal)(fileSize / partSize)); i++)
            {

                FilePart part = new FilePart(i, startByte, partSize, filePath);

                ThreadManager.CreateThread(part);

                startByte += part.partSize;
            }
        }
    }
}
