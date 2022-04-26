using Baya.Core.InterFaces;
using Baya.Core.ViewModels;
using Baya.DataLayer.BayaContextDb;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Baya.DataLayer.Entities;
using Baya.Core.Classes;
using System.Threading.Tasks;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Baya.Core.Services
{
    public class UserService : IUser
    {
        private readonly BayaDbContext _context;
        public UserService(BayaDbContext context)
        {
            _context = context;
        }
        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }

        #region RegisterUser
        public Guid GetRoleId(string rolename)
        {
            return _context.Roles.SingleOrDefault(r => r.RoleName == rolename).RoleId;
        }
        public bool ExistMobileNumber(string mobile)
        {
            return _context.Users.Any(u => u.Mobile == mobile);
        }



        bool IUser.Register(RegisterViewModel viewModel) //
        {
            if (!ExistMobileNumber(viewModel.Mobile))
            {
                User user = new User()
                {
                    Mobile = viewModel.Mobile,
                    Password = HashGenerators.HashWithMD5(HashGenerators.HashWithMD5(viewModel.Password)),
                    RoleId = GetRoleId("User"),
                    IsBlock = false,
                    IsActive = false,
                    Token = null,
                    CountReoport = 0,
                    ActiveCode = CodeGenerators.ActiveCode(),
                };
                _context.Users.Add(user);
                UserDetail userDetail = new UserDetail()
                {
                    UserId = user.UserId,
                    Date = DateConvertor.ToShamsi(),
                    Time = DateConvertor.GetShamsiTime(),
                    UserImg = "avatar.png",

                };
                _context.UserDetails.Add(userDetail);
                _context.SaveChanges();
                try
                {
                    SmsSender.VerifySend(user.Mobile, "", user.ActiveCode);
                }
                catch
                {

                    throw;
                }
                return true;
            }
            return false;

        }
        public void RegisterUser(string mobile)
        {
            string pass = "12345";
            User user = new User()
            {
                
                Mobile = mobile,
                Password = HashGenerators.HashWithMD5(HashGenerators.HashWithMD5(pass)),
                RoleId = GetRoleId("User"),
                IsBlock = false,
                IsActive = false,
                Token = null,
                CountReoport = 0,
                ActiveCode = CodeGenerators.ActiveCode(),
            };
            _context.Users.Add(user);
            UserDetail userDetail = new UserDetail()
            {
                UserId = user.UserId,
                Date = DateConvertor.ToShamsi(),
                Time = DateConvertor.GetShamsiTime(),
                UserImg = "avatar.png",

            };
            _context.UserDetails.Add(userDetail);
            _context.SaveChanges();
            try
            {
                SmsSender.VerifySend(user.Mobile, "", user.ActiveCode);
            }
            catch
            {

                throw;
            }
        }
        public void RegisteredUserToActive(string mobile)
        {
            var user = _context.Users.SingleOrDefault(u => u.Mobile == mobile);
            try
            {
                SmsSender.VerifySend(user.Mobile, "", user.ActiveCode);
            }
            catch
            {

                throw;
            }
        }

        public void RegisterUpdateUser(string mobile, string password)
        {
            var user = _context.Users.SingleOrDefault(u => u.Mobile == mobile);
            user.IsActive = true;
            user.ActiveCode = CodeGenerators.ActiveCode();
            user.Password = HashGenerators.HashWithMD5(HashGenerators.HashWithMD5(password));

            _context.SaveChanges();


        }

        User IUser.ActiveUser(ActiveViewModel viewModel)//
        {
            User user = _context.Users.SingleOrDefault(u => u.IsActive == false && u.ActiveCode == viewModel.ActiveCode);
            if (user != null)
            {
                user.IsActive = true;
                user.ActiveCode = CodeGenerators.ActiveCode();
                _context.SaveChanges();
                return user;
            }
            return null;
        }

        public void SendActiveCodeAgain(string mobile)
        {
            var user = _context.Users.SingleOrDefault(u => u.Mobile == mobile);

            user.ActiveCode = CodeGenerators.ActiveCode();
            _context.SaveChanges();

            //string activecode = _context.Users.SingleOrDefault(u => u.Mobile == mobile).ActiveCode;

            try
            {
                SmsSender.VerifySend(user.Mobile, "", user.ActiveCode);
            }
            catch
            {

                throw;
            }


        }


        #endregion

        #region Login
        User IUser.LoginUser(LoginViewModel viewModel)
        {
            return _context.Users.SingleOrDefault(u => u.Mobile == viewModel.Mobile && u.Password == HashGenerators.HashWithMD5(HashGenerators.HashWithMD5(viewModel.Password)));
        }
        async Task<User> IUser.LoginUserAfterRegister(string mobile)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Mobile == mobile);
        }
       async Task<User> IUser.LoginUser(string mobile, string password)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Mobile == mobile && u.Password == HashGenerators.HashWithMD5(HashGenerators.HashWithMD5(password)));
        }
       
        public bool UserIsActivate(string mobilenumber)//
        {
            return _context.Users.Any(u => u.Mobile == mobilenumber && u.IsActive == false);
        }

        public bool UserIsActivate(string mobilenumber, string activecode)
        {
            return _context.Users.Any(u => u.Mobile == mobilenumber && u.ActiveCode == activecode);
        }
        public bool ExistMobileNumberOfActive(string mobile)
        {
            return _context.Users.Any(u => u.Mobile == mobile && u.IsActive == true);
        }

        public void ChangePassUser(string mobile, string password)
        {
            var user = _context.Users.SingleOrDefault(u => u.Mobile == mobile);
            user.ActiveCode = CodeGenerators.ActiveCode();
            user.Password = HashGenerators.HashWithMD5(HashGenerators.HashWithMD5(password));

            _context.SaveChanges();
        }

        public bool UserIsBlocked(string mobilenumber)
        {
            return _context.Users.Any(u => u.Mobile == mobilenumber && u.IsBlock == false);
        }


        public bool ExistUser(string mobile, string password)
        {
            return _context.Users.Any(u => u.Mobile == mobile && u.Password == HashGenerators.HashWithMD5(HashGenerators.HashWithMD5(password)));
        }
        public string GetRoleName(Guid id)
        {
            return _context.Roles.SingleOrDefault(r => r.RoleId == id).RoleName;
        }



        #endregion

        #region Profile
        public async Task<UserProfileViewModel> GetUserForProfile(Guid IdUser)
        {

            var getuser = await _context.Users.Include(u => u.UserDetail).SingleOrDefaultAsync(u => u.UserId == IdUser);
            UserProfileViewModel user = new UserProfileViewModel()
            {
                Date = getuser.UserDetail.Date,
                ImageUser = getuser.UserDetail.UserImg,
                Name = getuser.UserDetail.Name,
                Family = getuser.UserDetail.Family,
                Time = getuser.UserDetail.Time,
                Wallet = getuser.Wallet
            };
            return user;
        }

        public async Task<UserDetail> GetUserById(Guid id)
        {
            return await _context.UserDetails.FindAsync(id);
        }

        //int IUser.UpdateProfileUser(Guid id, UpdateProfileViewModel viewModel)
        //{
        //    UserDetail detail = _context.UserDetails.Find(id);
        //    if (detail != null)
        //    {

        //        if (viewModel.ImageUser != null)
        //        {
        //            try
        //            {
        //                string filename = viewModel.ImageUser.FileName;
        //                string strExt = Path.GetExtension(filename); //گرفتن پسوند فایل
        //                filename = CodeGenerators.FileCode() + strExt;

        //                if (strExt != ".jpg" && strExt != ".jpeg" && strExt != ".png")
        //                {
        //                    return 2;
        //                }
        //                if (!ImageValidator.IsImage(viewModel.ImageUser)) //چک کردن تصویر
        //                {
        //                    return 3;
        //                }


        //                string imgpath = "";


        //                int width = 300;
        //                int height = 300;
        //                imgpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/panel/userpanel/avatar/" + filename);
        //                using var image = Image.Load(viewModel.ImageUser.OpenReadStream());
        //                image.Mutate(x => x.Resize(width, height));
        //                image.Save(imgpath);

        //                File.Delete("wwwroot/panel/userpanel/avatar/" + detail.UserImg);



        //                detail.UserImg = filename;
        //                detail.Name = viewModel.Name;
        //                detail.Family = viewModel.Family;
        //                detail.Address = viewModel.Address;
        //                detail.EmailUser = viewModel.EmailUser;
        //                detail.PostCode = viewModel.PostCode;
        //                detail.NationalCode = viewModel.NationalCode;

        //                _context.SaveChanges();

        //            }
        //            catch
        //            {

        //                throw;
        //            }



        //        }
        //        else
        //        {
        //            try
        //            {
        //                detail.Name = viewModel.Name;
        //                detail.Family = viewModel.Family;
        //                detail.Address = viewModel.Address;
        //                detail.EmailUser = viewModel.EmailUser;
        //                detail.PostCode = viewModel.PostCode;
        //                detail.NationalCode = viewModel.NationalCode;

        //                _context.SaveChanges();
        //            }
        //            catch 
        //            {

        //                throw;
        //            }
                   
        //        }

        //        return 4;
        //    }
        //    else
        //    {
        //        return 1;
        //    }
        //}

        





        #endregion
    }
}
