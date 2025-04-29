using PGViewer.Model;
using PGViewer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace PGViewer.ViewModel
{
    public class AirlinesViewModel: ViewModelBase
    {
        private readonly IAirlineRepository airlineRepository;

        private List<AirlineModel> _airlines;

        private AirlineModel _selectedAirline;

        private int _currentPage = 1;
        private int _pageSize = 10;
        private int _totalCount;

        public AirlinesViewModel()
        {
            airlineRepository = new AirlineRepository();

            NextPageCommand = new ViewModelCommand(ExecuteNextPageCommand, CanExecuteNextPageCommand);
            PreviousPageCommand = new ViewModelCommand(ExecutePreviousPageCommand, CanExecutePreviousPageCommand);
            FirstPageCommand = new ViewModelCommand(ExecuteFirstPageCommand, CanExecuteFirstPageCommand);
            LastPageCommand = new ViewModelCommand(ExecuteLastPageCommand, CanExecuteLastPageCommand);
            SaveCommand = new ViewModelCommand(SaveChanges, CanSaveAirline);
            DeleteCommand = new ViewModelCommand(DeleteAirline, CanDeleteAirline);


            LoadAirlines();
        }

        private bool CanSaveAirline(object obj)
        {
            return true;
        }

        private void SaveChanges(object obj)
        {
            if (SelectedAirline != null)
            {
                airlineRepository.UpdateAirline(SelectedAirline);
                LoadAirlines(); 
            }
        }

        private bool CanDeleteAirline(object obj)
        {
            return SelectedAirline != null;
        }

        private void DeleteAirline(object obj)
        {
            if (SelectedAirline != null)
            {
                airlineRepository.DeleteAirline(SelectedAirline.Id);
                LoadAirlines();
            }
        }

        private bool CanExecuteNextPageCommand(object obj) {
            return CurrentPage < TotalPages;
        }

        private void ExecuteNextPageCommand(object obj) {
            CurrentPage++;
        }

        private bool CanExecutePreviousPageCommand(object obj) {
            return CurrentPage > 1;
        }

        private void ExecutePreviousPageCommand(object obj) {
            CurrentPage--;
        }

        private bool CanExecuteFirstPageCommand(object obj) {
            return CurrentPage > 1;
        }

        private void ExecuteFirstPageCommand(object obj) {
            CurrentPage = 1;
        }

        private bool CanExecuteLastPageCommand(object obj)
        {
            return CurrentPage < TotalPages;
        }

        private void ExecuteLastPageCommand(object obj) {
            CurrentPage = TotalPages;
        }

        public AirlineModel SelectedAirline
        {
            get => _selectedAirline;
            set
            {
                _selectedAirline = value;
                OnPropertyCnaged(nameof(SelectedAirline));
            }
        }

        public List<AirlineModel> Airlines
        {
            get => _airlines;
            set
            {
                _airlines = value;
                OnPropertyCnaged(nameof(Airlines));
            }
        }

        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyCnaged(nameof(CurrentPage));
                LoadAirlines();
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

        private void LoadAirlines()
        {
            var result = airlineRepository.GetAll(CurrentPage, PageSize);
            Airlines = result.Airlines;
            _totalCount = result.TotalCount;

            OnPropertyCnaged(nameof(TotalPages));
        }
    }
}

    
