using PGViewer.Model;
using PGViewer.Repository;
using PGViewer.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace PGViewer.ViewModel
{
    public class LoginViewModel: ViewModelBase
    {
        private string _username;
        private SecureString _password;
        private string _errorMessage;
        private bool _isViewVisible=true;

        private IUserRepository userRepository;

        public string Username 
        { 
            get => _username; 
            set 
            { 
                _username = value; 
                OnPropertyCnaged(nameof(Username));
            }
        }

        public SecureString Password 
        { 
            get => _password; 
            set 
            { 
                _password = value; 
                OnPropertyCnaged(nameof(Password));
            }
        }

        public string ErrorMessage 
        { 
            get => _errorMessage; 
            set 
            { 
                _errorMessage = value; 
                OnPropertyCnaged(nameof(ErrorMessage));
            }
        }

        public bool IsViewVisible 
        { 
            get => _isViewVisible; 
            set 
            { 
                _isViewVisible = value; 
                OnPropertyCnaged(nameof(IsViewVisible));
            }
        }

        public ICommand LoginCommand { get; }
        public ICommand RecoverPasswordCommand { get; }
        public ICommand ShowPasswordCommand { get; }
        public ICommand RememberPasswordCommand { get; }

        public LoginViewModel()
        {
            userRepository = new UserRepository();
            LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
            RecoverPasswordCommand = new ViewModelCommand(p => ExecuteRecoverPasswordCommand("",""));
        }

        private bool CanExecuteLoginCommand(object obj)
        {
            if (string.IsNullOrWhiteSpace(Username) || Username.Length < 3 || Password == null || Password.Length < 3)
            {
                return false;
            }

            return true;
        }

        private void ExecuteLoginCommand(object obj)
        {
            var isValidUser = userRepository.AuthenticateUser(new System.Net.NetworkCredential(Username, Password));
            Console.WriteLine($"Valid user : {isValidUser}");
            if (isValidUser)
            {
                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(Username), null);
                IsViewVisible = false;
            }
            else 
            {
                ErrorMessage = "Invalid username or password";
            }
        }
        private void ExecuteRecoverPasswordCommand(string username, string email)
        {
            throw new NotImplementedException();
        }
    }
}
