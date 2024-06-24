using BinaryArchive00.Extractor.Commands;

var builder = CoconaApp.CreateBuilder();
var app = builder.Build();

app.AddSubCommand("extract", x =>
{
    x.AddCommand(ExtractArchive.Command);
    x.AddCommand("all-excel", ExtractAllExcel.Command);
});

app.Run();
