using PGViewer.Model;
using PGViewer.Repository;
using PGViewer.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PGViewer.ViewModel
{
    public class AirportsViewModel: ViewModelBase
    {
        private readonly IAirportRepository repository;

        private List<AirportModel> _items;

        private AirportModel _selectedItem;

        private int _currentPage = 1;
        private int _pageSize = 10;
        private int _totalCount;

        public AirportsViewModel()
        {
            repository = new AirportRepository();

            NextPageCommand = new ViewModelCommand(ExecuteNextPageCommand, CanExecuteNextPageCommand);
            PreviousPageCommand = new ViewModelCommand(ExecutePreviousPageCommand, CanExecutePreviousPageCommand);
            FirstPageCommand = new ViewModelCommand(ExecuteFirstPageCommand, CanExecuteFirstPageCommand);
            LastPageCommand = new ViewModelCommand(ExecuteLastPageCommand, CanExecuteLastPageCommand);
            SaveCommand = new ViewModelCommand(SaveChanges, CanSave);
            DeleteCommand = new ViewModelCommand(Delete, CanDelete);
            AddNewCommand = new ViewModelCommand(AddNewAirport, CanAddAirport);


            LoadData();
        }

        private bool CanSave(object obj)
        {
            return true;
        }

        private void SaveChanges(object obj)
        {
            if (SelectedAirport != null)
            {
                repository.UpdateAirport(SelectedAirport);
                LoadData();
            }
        }

        private bool CanDelete(object obj)
        {
            return SelectedAirport != null;
        }

        private void Delete(object obj)
        {
            if (SelectedAirport != null)
            {
                repository.DeleteAirport(SelectedAirport.Id);
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

        public AirportModel SelectedAirport
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyCnaged(nameof(SelectedAirport));
            }
        }

        public List<AirportModel> Airports
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyCnaged(nameof(Airports));
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

        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand FirstPageCommand { get; }
        public ICommand LastPageCommand { get; }
        public ICommand AddNewCommand { get; }

        public void LoadData()
        {
            var result = repository.GetAll(CurrentPage, PageSize);
            Airports = result.Airports;
            _totalCount = result.TotalCount;

            OnPropertyCnaged(nameof(TotalPages));
        }

        private bool CanAddAirport(object obj)
        {
            return true;
        }

        private void AddNewAirport(object obj)
        {
            var addWindow = new AddAirportWindow();
            var viewModel = new AddAirportViewModel(addWindow, this);
            addWindow.DataContext = viewModel;

            if (addWindow.ShowDialog() == true)
            {
                LoadData();
            }
        }
    }
}
