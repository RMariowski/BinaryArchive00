using BinaryArchive00.Extractor.Commands;

var builder = CoconaApp.CreateBuilder();
var app = builder.Build();

app.AddCommand("extract", ExtractArchive.Command);

app.Run();
