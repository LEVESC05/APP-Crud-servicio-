using Microsoft.EntityFrameworkCore;
using AppCrud.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using AppCrud.Services.Services.Empleado;
using AppCrud.Services.Services.Authentication;
using AppCrud.Services.Services.Password;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme) //para que no se me olvide esto genera una cookie para reconocer al usuario 
    .AddCookie(options =>
    {
        options.LoginPath = "/Acceso/Login";
        options.AccessDeniedPath = "/Acceso/Login";
    });

builder.Services.AddAuthorization();

builder.Services.AddDbContext<AppDBContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("CadenaSQL"));
});

builder.Services.AddScoped<IEmpleadoService, EmpleadoService>();
builder.Services.AddScoped<IAutenticacionService, AutenticacionService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication(); //esto me ayuda a iniciar sesion 
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Acceso}/{action=Login}/{id?}")
    .WithStaticAssets();


app.Run();
