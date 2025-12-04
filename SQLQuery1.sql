-- 1. DEĞİŞKENLERİ TANIMLAYALIM
DECLARE @Email NVARCHAR(256) = 'b231210006@sakarya.edu.tr' -- << BURAYA KENDİ MAİLİNİ YAZ!
DECLARE @RoleName NVARCHAR(256) = 'Admin'

DECLARE @UserId NVARCHAR(450)
DECLARE @RoleId NVARCHAR(450)

-- 2. SENİN ID'Nİ BULALIM
SELECT @UserId = Id FROM AspNetUsers WHERE Email = @Email

-- 3. ADMIN ROLÜNÜN ID'SİNİ BULALIM
SELECT @RoleId = Id FROM AspNetRoles WHERE Name = @RoleName

-- 4. EĞER BULDUYSAK VE DAHA ÖNCE YETKİ YOKSA EKLEYELİM
IF (@UserId IS NOT NULL AND @RoleId IS NOT NULL)
BEGIN
    IF NOT EXISTS (SELECT * FROM AspNetUserRoles WHERE UserId = @UserId AND RoleId = @RoleId)
    BEGIN
        INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES (@UserId, @RoleId)
        PRINT 'TEBRİKLER! Kullanıcı başarıyla Admin yapıldı.'
    END
    ELSE
    BEGIN
        PRINT 'Bu kullanıcı zaten Admin yetkisine sahip.'
    END
END
ELSE
BEGIN
    PRINT 'HATA: Kullanıcı veya Rol bulunamadı. Mail adresini kontrol et!'
END