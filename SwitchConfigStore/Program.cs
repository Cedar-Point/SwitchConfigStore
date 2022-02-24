using SwitchConfigStore;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<TFTPServer>();
var app = builder.Build();
app.Services.GetService<TFTPServer>();
app.MapGet("/{IPAddress}", async (con) => {
    StreamWriter sw = new StreamWriter(con.Response.Body);
    string IP = (string)con.Request.RouteValues["IPAddress"];
    TFTPServer server = con.RequestServices.GetService<TFTPServer>();
    Console.WriteLine(DateTime.Now.ToString() + ": Sending config for: " + IP);
    if (server.Configs.ContainsKey(IP))
    {
        con.Response.Body.Write(server.Configs[IP].ToArray());
    }
    else
    {
        sw.WriteLine("Config not found.");
        await sw.FlushAsync();
    }
    await con.Response.Body.FlushAsync();
});
app.Run();