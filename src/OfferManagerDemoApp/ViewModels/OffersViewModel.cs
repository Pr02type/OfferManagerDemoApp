/// <summary>
/// ViewModel für die Angebotsliste (MVVM).
/// Zuständig für: Datenbindung (Items/SelectedOffer), Suche/Filter, Commands (Refresh/Statuswechsel).
/// Hinweis: Diese Demo nutzt bewusst einen Mock-Service; Persistenz ist nicht Teil des Beispiels.
/// </summary>


using OfferManagerDemo.Models;
using OfferManagerDemo.Services;
using System.Collections.ObjectModel;
using System.Runtime.ConstrainedExecution;
using System.Windows.Input;

namespace OfferManagerDemo.ViewModels
{
    public class OffersViewModel : ObservableObject
    {
        private readonly IOfferService _service;

        public OffersViewModel(IOfferService service)
        {
            _service = service;
            RefreshCommand = new RelayCommand(async _ => await LoadAsync());
            AddCommand = new RelayCommand(async _ => await AddAsync());
            SetWonCommand = new RelayCommand(_ => SetStatus("Won"), _ => SelectedOffer is not null);
            SetLostCommand = new RelayCommand(_ => SetStatus("Lost"), _ => SelectedOffer is not null);
        }

        //Sammlung der angezeigten Angebote für das DataGrid.
        public ObservableCollection<Offer> Items { get; } = new();


        private Offer? _selectedOffer;
        
        //Aktuell ausgewähltes Angebot in der Liste.
        public Offer? SelectedOffer
        {
            get => _selectedOffer;
            set { SetProperty(ref _selectedOffer, value); (SetWonCommand as RelayCommand)?.RaiseCanExecuteChanged(); (SetLostCommand as RelayCommand)?.RaiseCanExecuteChanged(); }
        }

        private string? _search;
        //Suchbegriff, einfache Filterung der Liste.
        public string? Search
        {
            get => _search;
            set { if (SetProperty(ref _search, value)) _ = LoadAsync(); }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        //Aktualisiert die Liste aus dem Service
        public ICommand RefreshCommand { get; }
        //Fügt ein neues Angebot hinzu (mit Default-Werten)
        public ICommand AddCommand { get; }
        // Setzt den Status des ausgewählten Angebots auf "Won"
        public ICommand SetWonCommand { get; }
        // Setzt den Status des ausgewählten Angebots auf "Lost"
        public ICommand SetLostCommand { get; }

        public async Task LoadAsync()
        {
            IsBusy = true;
            try
            {
                Items.Clear();
                var list = await _service.GetOffersAsync(Search);
                foreach (var o in list) Items.Add(o);
            }
            finally { IsBusy = false; }
        }

        private async Task AddAsync()
        {
            var created = await _service.CreateAsync(new Offer
            {
                Customer = "Neuer Kunde",
                Amount = 1000m,
                Status = "Open"
            });
            await LoadAsync();
            SelectedOffer = created;
        }

        private async void SetStatus(string status)
        {
            if (SelectedOffer is null) return;
            await _service.UpdateStatusAsync(SelectedOffer.Id, status);
            await LoadAsync();
        }
    }
}
