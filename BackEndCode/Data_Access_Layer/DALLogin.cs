using Data_Access_Layer.Repository;
using Data_Access_Layer.Repository.Entities;
using Data_Access_Layer.Repository.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Data_Access_Layer
{
    public class DALLogin
    {
        private readonly AppDbContext _cIDbContext;
        public DALLogin(AppDbContext cIDbContext)
        {
            _cIDbContext = cIDbContext;
        }


        public User LoginUser(LoginUserModel user)
        {
            User userObj = new User();
            try
            {
                var query = from u in _cIDbContext.User
                            where u.EmailAddress == user.EmailAddress && u.IsDeleted == false
                            select new
                            {
                                u.Id,
                                u.FirstName,
                                u.LastName,
                                u.PhoneNumber,
                                u.EmailAddress,
                                u.UserType,
                                u.Password,
                                UserImage = u.UserImage
                            };

                var userData = query.FirstOrDefault();

                if (userData != null)
                {
                    if (userData.Password == user.Password)
                    {
                        userObj.Id = userData.Id;
                        userObj.FirstName = userData.FirstName;
                        userObj.LastName = userData.LastName;
                        userObj.PhoneNumber = userData.PhoneNumber;
                        userObj.EmailAddress = userData.EmailAddress;
                        userObj.UserType = userData.UserType;
                        userObj.UserImage = userData.UserImage;
                        userObj.Message = "Login Successfully";
                    }
                    else
                    {
                        userObj.Message = "Incorrect Password.";
                    }
                }
                else
                {
                    userObj.Message = "Email Address Not Found.";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return userObj;
        }

        public async Task<string> UpdateUser(User user)
        {
            try
            {
                var existedUser = await _cIDbContext.User.Where(u => u.Id == user.Id && !u.IsDeleted).FirstOrDefaultAsync();
                if (existedUser != null)
                {
                    existedUser.FirstName = user.FirstName;
                    existedUser.LastName = user.LastName;
                    existedUser.PhoneNumber = user.PhoneNumber;
                    existedUser.EmailAddress = user.EmailAddress;
                    existedUser.Password = user.Password;
                    existedUser.UserType = user.UserType;
                    existedUser.ModifiedDate = DateTime.UtcNow;
                    existedUser.UserType = "user";
                    await _cIDbContext.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("user with given id doesnt exist or deleted");
                }
                var existedUserDetail = await _cIDbContext.UserDetail.Where(ud => ud.UserId == user.Id && !ud.IsDeleted).FirstOrDefaultAsync();
                if (existedUserDetail != null)
                {
                    existedUserDetail.FirstName = user.FirstName;
                    existedUserDetail.LastName = user.LastName;
                    existedUserDetail.PhoneNumber = user.PhoneNumber;
                    existedUserDetail.EmailAddress = user.EmailAddress;
                    existedUserDetail.Name = user.FirstName;
                    existedUserDetail.Surname = user.LastName;
                    await _cIDbContext.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("user with given id doesnt exist or deleted");
                }
                return "User Updated Successfully";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Register(User user)
        {
            string result = string.Empty;
            try
            {
                bool emailExists = _cIDbContext.User.Any(u => u.EmailAddress == user.EmailAddress && !u.IsDeleted);
                if (!emailExists)
                {
                    int maxEmployeeId = 0;
                    string maxEmployeeIdStr = _cIDbContext.UserDetail.Max(ud => ud.EmployeeId);
                    if (!string.IsNullOrEmpty(maxEmployeeIdStr))
                    {
                        if (int.TryParse(maxEmployeeIdStr, out int parsedEmployeeId))
                        {
                            maxEmployeeId = parsedEmployeeId;
                        }
                        else
                        {
                            throw new Exception("Error while converting string to int.");
                        }
                    }
                    int newEmployeeId = maxEmployeeId + 1;

                    var newUser = new User
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        PhoneNumber = user.PhoneNumber,
                        EmailAddress = user.EmailAddress,
                        Password = user.Password,
                        UserType = user.UserType,
                        CreatedDate = DateTime.UtcNow,
                        IsDeleted = false
                    };
                    _cIDbContext.User.Add(newUser);
                    _cIDbContext.SaveChanges();
                    var newUserDetail = new UserDetail
                    {
                        UserId = newUser.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        PhoneNumber = user.PhoneNumber,
                        EmailAddress = user.EmailAddress,
                        UserType = user.UserType,
                        Name = user.FirstName,
                        Surname = user.LastName,
                        EmployeeId = newEmployeeId.ToString(),
                        Department = "IT",
                        Status = true
                    };
                    _cIDbContext.UserDetail.Add(newUserDetail);
                    _cIDbContext.SaveChanges();
                    result = "User Register Successfully";
                }
                else
                {
                    throw new Exception("Email is Already Exist.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            try
            {
                User user = await _cIDbContext.User.FirstAsync(x => x.Id == id && !x.IsDeleted);
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }

    
}


  
