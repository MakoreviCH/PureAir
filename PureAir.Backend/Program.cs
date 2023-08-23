using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PureAirBackend.Configs;
using PureAirBackend.Models;

namespace PureAirBackend
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			builder.Services.AddDbContext<BackendContext>(options =>
				options.UseSqlServer(builder.Configuration.GetConnectionString("BackendContext") ?? throw new InvalidOperationException("Connection string 'BackendContext' not found.")));
			// Add services to the container.
			builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));

			builder.Services.AddAuthentication(options =>
				{
					options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
					options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
					options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				})
				.AddJwtBearer(jwt =>
				{
					var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JwtConfig:Secret").Value);

					jwt.SaveToken = true;
					jwt.TokenValidationParameters = new TokenValidationParameters()
					{
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(key),
						ValidateIssuer = false, // for dev
						ValidateAudience = false, // for dev
						RequireExpirationTime = false, // for dev -- need to be updated when refresh token is added
						ValidateLifetime = true
							
					};
				});
			var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

			builder.Services.AddCors(o => o.AddPolicy(name: MyAllowSpecificOrigins,
				builder =>
				{
					builder.WithOrigins("https://localhost:3000")
						.AllowAnyMethod()
						.AllowAnyOrigin()
						.AllowAnyHeader();

				}));
			builder.Services.AddDefaultIdentity<Employee>(options => options.SignIn.RequireConfirmedAccount = false)
				.AddRoles<IdentityRole>()
				.AddEntityFrameworkStores<BackendContext>();
			builder.Services.AddControllers(options => options.SuppressAsyncSuffixInActionNames=false);
			builder.Services.AddControllers();
			builder.Services.AddControllers().AddNewtonsoftJson(x =>
				x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}
			using (var scope = app.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				var userManager = services.GetRequiredService<UserManager<Employee>>();
				var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
				await ContextSeed.SeedRolesAsync(userManager, roleManager);
				var superUser = new Employee
				{
					UserName = builder.Configuration["AppSettings:AdminUserEmail"],
					Email = builder.Configuration["AppSettings:AdminUserEmail"],
					PhoneNumber = builder.Configuration["AppSettings:AdminUserPhone"],
				};
				//Ensure you have these values in your appsettings.json file
				var userPWD = builder.Configuration["AppSettings:AdminUserPassword"];
				var _user = await userManager.FindByEmailAsync(builder.Configuration["AppSettings:AdminUserEmail"]);

				if (_user == null)
				{
					var createPowerUser = await userManager.CreateAsync(superUser, userPWD);
					if (createPowerUser.Succeeded)
					{
						//here we tie the new user to the role
						await userManager.AddToRoleAsync(superUser, "Admin");

					}
				}
			}
			app.UseCors(MyAllowSpecificOrigins);
			//app.UseHttpsRedirection();
			app.UseAuthentication();
			app.UseAuthorization();
			app.MapControllers();

			app.Run();
		}
	}
}