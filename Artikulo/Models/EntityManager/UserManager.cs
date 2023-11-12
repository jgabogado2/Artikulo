using Artikulo.Models.DB;
using Artikulo.Models.ViewModel;
using BCrypt.Net;

namespace Artikulo.Models.EntityManager
{
    public class UserManager
    {
        private (string hashedPassword, string salt) HashPassword(string password)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);
            return (hashedPassword, salt);
        }
        public void AddUserAccount(UserModel user)
        {
            using (MyDBContext db = new MyDBContext())
            {

                // Hashing password
                (string hashedPassword, string salt) = HashPassword(user.Password);
                //Add checking here if login exists
                SystemUsers newSysUser = new SystemUsers
                {
                    LoginName = user.LoginName,
                    PasswordEncryptedText = hashedPassword,
                    Salt = salt,
                    CreatedDateTime = DateTime.Now,
                    ModifiedBy = 1,
                    ModifiedDateTime = DateTime.Now
                };

                db.SystemUsers.Add(newSysUser);
                db.SaveChanges();

                int newUserId = db.SystemUsers.First(u => u.LoginName == newSysUser.LoginName).UserID;

                Users newUser = new Users
                {
                    UserID = newUserId,
                    AccountImage = user.AccountImage,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Address = user.Address,
                    PhoneNumber = user.PhoneNumber,
                    Gender = user.Gender,
                    CreatedBy = 1,
                    CreatedDateTime = DateTime.Now,
                    ModifiedBy = 1,
                    ModifiedDateTime = DateTime.Now
                };
                db.Users.Add(newUser);
                db.SaveChanges();

                int roleId = db.Role.First(r => r.RoleName == "Member").RoleID;

                UserRole userRole = new UserRole
                {
                    UserID = newUserId,
                    LookUpRoleID = roleId,
                    IsActive = true,
                    CreatedBy = newUserId,
                    CreatedDateTime = DateTime.Now,
                    ModifiedBy = newUserId,
                    ModifiedDateTime = DateTime.Now,
                };

                db.UserRole.Add(userRole);
                db.SaveChanges();
            }
        }

        public bool VerifyPassword(string loginName, string currentPassword)
        {
            using (MyDBContext db = new MyDBContext())
            {
                SystemUsers user = db.SystemUsers.FirstOrDefault(u => u.LoginName == loginName);

                if (user != null)
                {
                    // Retrieve the salt and hashed password from the database
                    string salt = user.Salt;
                    string storedHashedPassword = user.PasswordEncryptedText;

                    // Hash the current password with the retrieved salt
                    string hashedPassword = HashPassword(currentPassword, salt);

                    // Compare the newly hashed password with the stored hashed password
                    return hashedPassword == storedHashedPassword;
                }

                return false; // User not found
            }
        }

        private string HashPassword(string password, string salt)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }

        public void UpdateUserAccount(UserModel user)
        {
            using (MyDBContext db = new MyDBContext())
            {
                // Check if a user with the given login name already exists
                SystemUsers existingSysUser = db.SystemUsers.FirstOrDefault(u => u.LoginName == user.LoginName);
                if (existingSysUser != null)
                {
                    Users existingUser = db.Users.FirstOrDefault(u => u.ProfileID == existingSysUser.UserID);

                    if (existingUser != null)
                    {
                        // Auto increments the Modified By at every update
                        int currentModifiedBy = existingSysUser.ModifiedBy;
                        int newModifiedBy = currentModifiedBy + 1;

                        // Update the existing user
                        existingSysUser.ModifiedBy = newModifiedBy;
                        existingSysUser.ModifiedDateTime = DateTime.Now;

                        // Hash New Password
                        string newSalt = BCrypt.Net.BCrypt.GenerateSalt();
                        string newHashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password, newSalt);

                        // You can also update other properties of the user as needed   
                        existingSysUser.PasswordEncryptedText = newHashedPassword;
                        existingSysUser.Salt = newSalt;
                        existingUser.AccountImage = user.AccountImage;
                        existingUser.FirstName = user.FirstName;
                        existingUser.LastName = user.LastName;
                        existingUser.Email = user.Email;
                        existingUser.Address = user.Address;
                        existingUser.PhoneNumber = user.PhoneNumber;
                        existingUser.Gender = user.Gender;

                        db.SaveChanges();
                    }
                }
                else
                {
                    // Add a new user since the user doesn't exist
                    SystemUsers newSysUser = new SystemUsers
                    {
                        LoginName = user.LoginName,
                        PasswordEncryptedText = HashPassword(user.Password),
                        CreatedDateTime = DateTime.Now,
                        ModifiedBy = 1,
                        ModifiedDateTime = DateTime.Now
                    };

                    string HashPassword(string password)
                    {
                        string salt = BCrypt.Net.BCrypt.GenerateSalt();
                        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);
                        return hashedPassword;
                    }

                    db.SystemUsers.Add(newSysUser);
                    db.SaveChanges();

                    int newUserId = newSysUser.UserID;

                    Users newUser = new Users
                    {
                        UserID = newUserId,
                        AccountImage = user.AccountImage,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Address = user.Address,
                        PhoneNumber = user.PhoneNumber,
                        Gender = user.Gender,
                        CreatedBy = newUserId,
                        CreatedDateTime = DateTime.Now,
                        ModifiedBy = 1,
                        ModifiedDateTime = DateTime.Now
                    };

                    db.Users.Add(newUser);
                    db.SaveChanges();
                }
            }
        }

        public UsersModel GetAllUsers()
        {
            UsersModel list = new UsersModel();

            using (MyDBContext db = new MyDBContext())
            {
                var users = from u in db.Users
                            join us in db.SystemUsers
                            on u.UserID equals us.UserID
                            join ur in db.UserRole
                            on u.UserID equals ur.UserID
                            join r in db.Role
                            on ur.LookUpRoleID equals r.RoleID
                            select new { u, us, r, ur };

                list.Users = users.Select(records => new UserModel()
                {
                    AccountImage = records.u.AccountImage ?? string.Empty,
                    UserID = records.us.UserID,
                    ProfileID = records.u.ProfileID,
                    LoginName = records.us.LoginName,
                    Password = records.us.PasswordEncryptedText,
                    FirstName = records.u.FirstName,
                    LastName = records.u.LastName,
                    Email = records.u.Email,
                    Address = records.u.Address,
                    PhoneNumber = records.u.PhoneNumber,
                    Gender = records.u.Gender,
                    CreatedBy = records.u.CreatedBy,
                    RoleID = records.ur.LookUpRoleID,
                    RoleName = records.r.RoleName
                }).ToList();
            }
            return list;
        }

        public bool IsLoginNameExist(string loginName)
        {
            using (MyDBContext db = new MyDBContext())
            {
                return db.SystemUsers.Where(u => u.LoginName.Equals(loginName)).Any();
            }
        }
        

        public string GetUserPassword(string loginName)
        {
            using (MyDBContext db = new MyDBContext())
            {
                var user = db.SystemUsers.FirstOrDefault(o => o.LoginName.ToLower().Equals(loginName)); // Get the first matching user

                if (user != null && user.PasswordEncryptedText != null)
                {
                    return user.PasswordEncryptedText;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public bool IsUserInRole(string loginName, string roleName)
        {
            using (MyDBContext db = new MyDBContext())
            {
                SystemUsers su = db.SystemUsers.Where(o => o.LoginName.ToLower().Equals(loginName))?.FirstOrDefault();

                if (su != null)
                {
                    var roles = from ur in db.UserRole
                                join r in db.Role on ur.LookUpRoleID equals
                                r.RoleID
                                where r.RoleName.Equals(roleName) &&
                                ur.UserID.Equals(su.UserID)
                                select r.RoleName;
                    if (roles != null)
                    {
                        return roles.Any();
                    }
                }
                return false;
            }
        }
    }
}
