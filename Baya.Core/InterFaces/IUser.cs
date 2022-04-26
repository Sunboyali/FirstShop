using Baya.Core.ViewModels;
using Baya.DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Baya.Core.InterFaces
{
  public interface IUser :IDisposable
    {
        #region Register

        Guid GetRoleId(string rolename);
        //Guid GetRoleName(string rolename);
        bool Register(RegisterViewModel viewModel);//

        void RegisterUser(string mobile);
        void RegisteredUserToActive(string mobile);
        void RegisterUpdateUser(string mobile, string password);
        bool ExistMobileNumber(string mobile);
       

        User ActiveUser(ActiveViewModel viewModel);//

        void SendActiveCodeAgain(string mobile);


        #endregion
        #region Login
        User LoginUser(LoginViewModel viewModel);//
        Task<User> LoginUserAfterRegister(string mobile);
        Task<User> LoginUser(string mobile, string password);
        bool ExistUser(string mobile, string password);



        bool ExistMobileNumberOfActive(string mobile);

        void ChangePassUser(string mobile, string password);

        bool UserIsActivate(string mobilenumber);
        bool UserIsActivate(string mobilenumber,string activecode);
        bool UserIsBlocked(string mobilenumber);

        string GetRoleName(Guid id);
        #endregion

        #region Profile
        Task<UserProfileViewModel> GetUserForProfile(Guid IdUser);

        Task<UserDetail> GetUserById(Guid id);
        //int UpdateProfileUser(Guid id, UpdateProfileViewModel viewModel);


        #endregion


    }
}
