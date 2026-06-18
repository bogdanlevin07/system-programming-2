using System;
using System.IO;
using System.IO.Pipes;

public class Client
{
    public static void Main()
    {
        using (var client = new NamedPipeClientStream(".", "mypipe", PipeDirection.InOut))
        {
            Console.WriteLine("Подключение к серверу...");
            client.Connect();
            Console.WriteLine("Подключено.");

            using (var reader = new StreamReader(client))
            using (var writer = new StreamWriter(client) { AutoFlush = true })
            {
                while (true)
                {
                    Console.Write("Введите команду: ");
                    string input = Console.ReadLine();
                    if (string.IsNullOrEmpty(input)) continue;

                    writer.WriteLine(input);

                    string response = reader.ReadLine();
                    Console.WriteLine("Ответ: " + response);

                    if (input == "/exit")
                        break;
                }
            }
        }
    }
}