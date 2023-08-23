using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PureAirBackend.Models;

namespace PureAirBackend
{
	public class BackendContext : IdentityDbContext<Employee>
	{
		public BackendContext(DbContextOptions<BackendContext> options) : base(options)
		{

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<Employee>()
				.ToTable("Employee");

			modelBuilder.Entity<IdentityRole>(entity =>
			{
				entity.ToTable(name: "Role");
			});
			modelBuilder.Entity<IdentityUserRole<string>>(entity =>
			{
				entity.ToTable("UserRoles");
				//in case you chagned the TKey type
				//  entity.HasKey(key => new { key.UserId, key.RoleId });
			});
		}

		public DbSet<Workspace> Workspaces { get; set; }
		public DbSet<Employee> Employees { get; set; }
		public DbSet<EmployeePass> EmployeePasses { get; set; }
		public DbSet<WorkspaceData> WorkspaceDatas { get; set; }
	}
}
