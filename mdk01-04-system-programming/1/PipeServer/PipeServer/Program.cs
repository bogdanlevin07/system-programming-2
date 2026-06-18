using System;
using System.IO;
using System.IO.Pipes;

public class Server
{
    public static void Main()
    {
        while (true)
        {
            using (var server = new NamedPipeServerStream("mypipe", PipeDirection.InOut))
            {
                Console.WriteLine("Сервер ожидает подключения...");
                server.WaitForConnection();
                Console.WriteLine("Клиент подключен.");

                using (var reader = new StreamReader(server))
                using (var writer = new StreamWriter(server) { AutoFlush = true })
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line == "/time")
                        {
                            writer.WriteLine(DateTime.Now.ToString("HH:mm:ss"));
                        }
                        else if (line.StartsWith("/echo"))
                        {
                            string text = line.Length > 5 ? line.Substring(line.StartsWith("/echo ") ? 6 : 5) : "";
                            writer.WriteLine(string.IsNullOrEmpty(text) ? "(пусто)" : text);
                        }
                        else if (line == "/exit")
                        {
                            writer.WriteLine("До свидания!");
                            break;
                        }
                        else
                        {
                            writer.WriteLine("Неизвестная команда.");
                        }
                    }
                }
                Console.WriteLine("Клиент отключен.");
            }
        }
    }
}