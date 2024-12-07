using System.Collections.ObjectModel;
using System.Windows.Input;
using ClinicPro_MVVM_WPF.Data.MedCard;
using ClinicPro_MVVM_WPF.Model;
using ClinicPro_MVVM_WPF.Utils;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using ClinicPro_MVVM_WPF.Data;

namespace ClinicPro_MVVM_WPF.ViewModel.Doctor.MedCard;

public class MedCardSearchVM : BaseViewModel
{
    private readonly MedCardVM _parentVM;
    private readonly IMedCardRepository _medCardRepository; // Используем интерфейс
    private string _searchQuery;
    private string _selectedFilter;
    private bool _isLoading;
    
    private readonly ClinicDbContext _context = new ClinicDbContext();
    
    public ICommand GoToMedCardCommand { get; set; }
    public ICommand ShowMenuMedCardCommand { get; }
    public ICommand SearchCommand { get; }
    
    public MedCardSearchVM(MedCardVM parentVM)
    {
        _parentVM = parentVM;
        _medCardRepository = new MedCardRepository(_context);

        GoToMedCardCommand = new RelayCommand(GoToMedCard);
        ShowMenuMedCardCommand = new RelayCommand(o => _parentVM.ShowMenuMedCard(o));
        SearchCommand = new RelayCommand(async o => await FilterMedCardsAsync());
        
        MedCards = new ObservableCollection<MedCardModel>(); // Инициализируем коллекцию
        LoadInitialMedCardsAsync(); // Загружаем начальный список

        // Список доступных фильтров
        AvailableFilters = new ObservableCollection<string> { "Фамилия", "Дата рождения" }; 
        SelectedFilter = AvailableFilters.FirstOrDefault(); // Выбран по умолчанию
    }
    
    private void GoToMedCard(object parameter)
    {
        if (parameter is int patientId) // Проверяем, что параметр — это ID пациента
        {
            // Создаем новую MedCardInfoVM с нужным ID пациента
            _parentVM.MedCardInfoVM = new MedCardInfoVM(_parentVM, patientId);
        
            // Переключаем представление на MedCardInfoVM
            _parentVM.CurrentMedCardView = _parentVM.MedCardInfoVM;
        }
        else
        {
            MessageBox.Show("Ошибка: некорректный ID пациента", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    
    public ObservableCollection<string> AvailableFilters { get; }

    public ObservableCollection<MedCardModel> MedCards { get; set; }

    public string SearchQuery
    {
        get => _searchQuery;
        set
        {
            _searchQuery = value;
            OnPropertyChanged();
        }
    }

    public string SelectedFilter
    {
        get => _selectedFilter;
        set
        {
            _selectedFilter = value;
            OnPropertyChanged();
        }
    }
    
    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            _isLoading = value; 
            OnPropertyChanged();
        }
    }

    private async Task LoadInitialMedCardsAsync()
    {
        try
        {
            IsLoading = true;
            Console.WriteLine("Na4alo zagruzki...");
            var medCards = await _medCardRepository.GetAllMedCardAsync();
            Console.WriteLine($"Zagrugeno {medCards.Count()} medcard");
            Application.Current.Dispatcher.Invoke(() =>
            {
                MedCards.Clear();
                foreach (var medCard in medCards)
                {
                    MedCards.Add(medCard);
                }
            });
        }
        catch (Exception ex)
        {
            // Обработка ошибки
            MessageBox.Show($"Error asdasd: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
           await Application.Current.Dispatcher.InvokeAsync(()=> IsLoading = false);
        }
    }
    
    private async Task FilterMedCardsAsync()
    {
        try
        {
            IsLoading = true;
            // Загружаем все записи
            var allMedCards = await _medCardRepository.GetAllMedCardAsync();

            IEnumerable<MedCardModel> filteredCards = allMedCards;

            // Фильтрация по введенному тексту
            if (!string.IsNullOrWhiteSpace(SearchQuery))
            {
                if (SelectedFilter == "Фамилия")
                {
                    filteredCards = filteredCards.Where(mc => mc.Patient.lastName.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase));
                }
                else if (SelectedFilter == "Дата рождения" && int.TryParse(SearchQuery, out int year))
                {
                    // Сравниваем только год даты рождения пациента с введенным годом
                    filteredCards = filteredCards.Where(mc => mc.Patient.dateOfBirth.HasValue && mc.Patient.dateOfBirth.Value.Year == year);
                }
            }

            // Обновляем коллекцию для отображения
            MedCards.Clear();
            foreach (var medCard in filteredCards)
            {
                MedCards.Add(medCard);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка фильтрации данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsLoading = false;
        }
    }
    
}