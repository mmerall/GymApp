using Microsoft.AspNetCore.Identity;
using GymApp.Models;

namespace GymApp.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider service)
        {
            // Kullanıcı Yöneticisi ve Rol Yöneticisi
            var userManager = service.GetService<UserManager<IdentityUser>>();
            var roleManager = service.GetService<RoleManager<IdentityRole>>();

            // 1. ROLLERİ OLUŞTUR
            await roleManager.CreateAsync(new IdentityRole("Admin"));
            await roleManager.CreateAsync(new IdentityRole("Member"));

            // 2. ADMİN KULLANICISINI OLUŞTUR (Ödevde istenen bilgiler)
            // Lütfen buradaki mail adresini kendi öğrenci numaranla değiştir!
            var adminEmail = "b231210006@sakarya.edu.tr";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var newAdmin = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                // Şifre: sau (Ödevde istenen)
                // Ancak Identity kuralları gereği basit şifre kabul etmeyebilir.
                // Eğer hata alırsan "Sau.123" gibi güçlü bir şifre yap.
                await userManager.CreateAsync(newAdmin, "Sau.123!");

                // Kullanıcıya Admin rolünü ver
                await userManager.AddToRoleAsync(newAdmin, "Admin");
            }
        }
    }
}