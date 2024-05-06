using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
//Add 
builder.Services.AddControllersWithViews();

var app = builder.Build();

//Edit
//app.MapGet("/", () => "Hello World!");

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "Images")),
            RequestPath = "/Images"

});


app.MapControllerRoute(

    name: "default",
    pattern: "{controller=Product}/{action=Index}/{id?}"

    );


app.Run();