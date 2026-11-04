using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

class Program
{
    static TelegramBotClient bot;
    static async Task Main(string[] args)
    {
        var token = args.Length > 0
            ? args[0]
            : Environment.GetEnvironmentVariable("BOT_TOKEN")
              ?? throw new InvalidOperationException("Токен не задан");

        using var cts = new CancellationTokenSource();


        bot = new TelegramBotClient(token);

        try
        {
            var me = await bot.GetMe();
            Console.WriteLine("Соединение успешно!");
        }
        catch (Exception ex)
        {
            throw new Exception("unknown bot");
        }

        bot.OnMessage += OnMessage;
        bot.OnError += OnError;

        try { await Task.Delay(Timeout.Infinite, cts.Token); }
        catch (OperationCanceledException) { }

    }

    static async Task OnMessage(Message msg, UpdateType type)
    {
        if (!string.IsNullOrEmpty(msg.Text))
        {
            var (exitCode, output, error) = await Bash.RunBashAsync(msg.Text);
            if (exitCode != 0)
            {
                // Console.WriteLine($"TEXT: {msg.Text}, ExitCode:{exitCode}, output:{output}, error:{error}");
                await bot.SendMessage(msg.Chat.Id, $"Code {exitCode} {error}");
            }
            else
            {
                await bot.SendMessage(msg.Chat.Id, $"{output}");
            }
        }
    }
    static Task OnError(Exception ex, HandleErrorSource source)
    {
        Console.WriteLine($"Ошибка ({source}): {ex}");
        return Task.CompletedTask;
    }

}