using HttpServer;

var cancellationTokenSource = new CancellationTokenSource();

var server = new Server(4221);
await server.Start(cancellationTokenSource.Token);