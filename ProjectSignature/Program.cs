
using ProjectSignature;

if (args.Length > 0)
{
    Console.WriteLine("Yes args!!!");
    foreach (var i in args)
    {
        Console.WriteLine(i);
    }
    string path = args[0];                   // @"D:\Big\BigFile2.txt"; ~ 70Gb
    string partSize = args[1];               //"1000";

    InputFile file = new InputFile(path, partSize);

    file.Run();
}
else
{
    Console.WriteLine("No args!!!");
}
Console.ReadLine();