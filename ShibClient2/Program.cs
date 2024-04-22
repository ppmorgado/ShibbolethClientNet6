using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShibClient2.Data;
using UW.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
                                 options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

//builder.Services.AddDefaultIdentity<IdentityUser>(
//                                                      options =>
//                                                      {
//                                                          options.SignIn.RequireConfirmedAccount = true;
//                                                          options.SignIn.RequireConfirmedEmail = false;
//                                                          options.SignIn.RequireConfirmedPhoneNumber = false;
//                                                      }
//                                                     )
//                    .AddRoles<IdentityRole>()
//                    .AddEntityFrameworkStores<ApplicationDbContext>();


// DefaultAuthentication and DefaultSignInScheme selected in the .AddDefaultIdentity
builder.Services.AddAuthentication()
    .AddUWShibboleth(
        authenticationScheme: ShibbolethDefaults.AuthenticationScheme,
        displayName: "UW-Madison NetID",
        options =>
        {
            options.UseChallenge = true;  // required to process the Shibboleth login
            options.CallbackPath = new Microsoft.AspNetCore.Http.PathString("/secure");
        }
    );

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultScheme = ShibbolethDefaults.AuthenticationScheme;
//}
// )
//     .AddUWShibboleth(authenticationScheme: ShibbolethDefaults.AuthenticationScheme, displayName: "shibboleth", options =>
//     {
//         options.UseChallenge = true;  // required to process the Shibboleth login
//         options.CallbackPath = new Microsoft.AspNetCore.Http.PathString("/secure");
//         //options.ProcessChallenge = true;   // readonly on v8
//         //options.ChallengePath = new Microsoft.AspNetCore.Http.PathString("/secure");  // readonly on v8 // must match Path in shibboleth2.xml
//     });


builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
