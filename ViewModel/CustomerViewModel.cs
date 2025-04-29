using PGViewer.Model;
using PGViewer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PGViewer.ViewModel
{
    public class CustomerViewModel: ViewModelBase
    {
        private readonly ICustomerRepository repository;

        private List<CustomerModel> _items;

        private CustomerModel _selectedItem;

        private int _currentPage = 1;
        private int _pageSize = 10;
        private int _totalCount;

        public CustomerViewModel()
        {
            repository = new CustomerRepository();

            NextPageCommand = new ViewModelCommand(ExecuteNextPageCommand, CanExecuteNextPageCommand);
            PreviousPageCommand = new ViewModelCommand(ExecutePreviousPageCommand, CanExecutePreviousPageCommand);
            FirstPageCommand = new ViewModelCommand(ExecuteFirstPageCommand, CanExecuteFirstPageCommand);
            LastPageCommand = new ViewModelCommand(ExecuteLastPageCommand, CanExecuteLastPageCommand);
            SaveCommand = new ViewModelCommand(SaveChanges, CanSave);
            DeleteCommand = new ViewModelCommand(Delete, CanDelete);
            AddNewCommand = new ViewModelCommand(AddNewCustomer, CanAddCustomer);

            LoadData();
        }

        private bool CanAddCustomer(object obj)
        {
            return true;
        }

        private void AddNewCustomer(object obj)
        {
            var newCustomer = new CustomerModel
            {
                Id = -1,
                Name = "New Customer",
            };

            Customers.Insert(0, newCustomer);
            SelectedCustomer = newCustomer;
        }

        private bool CanSave(object obj)
        {
            return true;
        }

        private void SaveChanges(object obj)
        {
            if (SelectedCustomer == null)
            {
                return;
            }

            try
            {
                if (SelectedCustomer.Id == -1)
                {
                    SelectedCustomer.Id = repository.AddCustomer(SelectedCustomer);
                }
                else
                {
                    repository.UpdateCustomer(SelectedCustomer);
                }

                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data: {ex.Message}",
                              "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanDelete(object obj)
        {
            return SelectedCustomer != null;
        }

        private void Delete(object obj)
        {
            if (SelectedCustomer != null)
            {
                repository.DeleteCustomer(SelectedCustomer.Id);
                LoadData();
            }
        }

        private bool CanExecuteNextPageCommand(object obj)
        {
            return CurrentPage < TotalPages;
        }

        private void ExecuteNextPageCommand(object obj)
        {
            CurrentPage++;
        }

        private bool CanExecutePreviousPageCommand(object obj)
        {
            return CurrentPage > 1;
        }

        private void ExecutePreviousPageCommand(object obj)
        {
            CurrentPage--;
        }

        private bool CanExecuteFirstPageCommand(object obj)
        {
            return CurrentPage > 1;
        }

        private void ExecuteFirstPageCommand(object obj)
        {
            CurrentPage = 1;
        }

        private bool CanExecuteLastPageCommand(object obj)
        {
            return CurrentPage < TotalPages;
        }

        private void ExecuteLastPageCommand(object obj)
        {
            CurrentPage = TotalPages;
        }

        public CustomerModel SelectedCustomer
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyCnaged(nameof(SelectedCustomer));
            }
        }

        public List<CustomerModel> Customers
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyCnaged(nameof(Customers));
            }
        }

        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyCnaged(nameof(CurrentPage));
                LoadData();
            }
        }

        public int PageSize
        {
            get => _pageSize;
            set
            {
                _pageSize = value;
                Console.WriteLine($"Page size : {value}");
                OnPropertyCnaged(nameof(PageSize));
                CurrentPage = 1;
            }
        }

        public int TotalPages => (_totalCount + PageSize - 1) / PageSize;

        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }

        public ICommand AddNewCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand FirstPageCommand { get; }
        public ICommand LastPageCommand { get; }

        private void LoadData()
        {
            var result = repository.GetAll(CurrentPage, PageSize);
            Customers = result.Customers;
            _totalCount = result.TotalCount;

            OnPropertyCnaged(nameof(TotalPages));
        }
    }
}
