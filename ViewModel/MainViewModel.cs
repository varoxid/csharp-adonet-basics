using FontAwesome.Sharp;
using PGViewer.Model;
using PGViewer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace PGViewer.ViewModel
{
    public class MainViewModel: ViewModelBase
    {
        private UserAccountModel _currentUserAccount;
        private ViewModelBase _currentChildView;
        private IUserRepository userRepository;
        private string _caption;
        private IconChar _icon;

        public UserAccountModel CurrentUserAccount 
        { 
            get => _currentUserAccount; 
            set 
            { 
                _currentUserAccount = value; 
                OnPropertyCnaged(nameof(CurrentUserAccount)); 
            } 
        }
        public ViewModelBase CurrentChildView
        {
            get
            {
                return _currentChildView;
            }
            set
            {
                _currentChildView = value;
                OnPropertyCnaged(nameof(CurrentChildView));
            }
        }

        public string Caption
        {
            get
            {
                return _caption;
            }
            set
            {
                _caption = value;
                OnPropertyCnaged(nameof(Caption));
            }
        }
        public IconChar Icon
        {
            get
            {
                return _icon;
            }
            set
            {
                _icon = value;
                OnPropertyCnaged(nameof(Icon));
            }
        }


        public MainViewModel()
        {
            userRepository = new UserRepository();
            CurrentUserAccount = new UserAccountModel();

            ShowCustomerViewCommand = new ViewModelCommand(ExecuteShowCustomerViewCommand);
            ShowAirportsViewCommand = new ViewModelCommand(ExecuteShowAirportsViewCommand);
            ShowAirlinesViewCommand = new ViewModelCommand(ExecuteShowAirlinesViewCommand);
            ExecuteShowCustomerViewCommand(null);

            LoadCurrentUserData();
        }

        public ICommand ShowCustomerViewCommand { get; }
        public ICommand ShowAirportsViewCommand { get; }
        public ICommand ShowAirlinesViewCommand { get; }

        private void ExecuteShowCustomerViewCommand(object obj)
        {
            CurrentChildView = new CustomerViewModel();
            Caption = "Customers";
            Icon = IconChar.UserGroup;
        }

        private void ExecuteShowAirportsViewCommand(object obj)
        {
            CurrentChildView = new AirportsViewModel();
            Caption = "Airports";
            Icon = IconChar.Plane;
        }

        private void ExecuteShowAirlinesViewCommand(object obj)
        {
            CurrentChildView = new AirlinesViewModel();
            Caption = "Airlines";
            Icon = IconChar.PlaneDeparture;
        }

        private void LoadCurrentUserData()
        {
            var user = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            if (user != null)
            {
                CurrentUserAccount = new UserAccountModel()
                {
                    Username = user.Username,
                    DisplayName = $"{user.Name}"
                };

                Console.WriteLine($"User: {CurrentUserAccount.DisplayName}");
            }
            else 
            {
                CurrentUserAccount = null; 
            }
        }
    }
}
