using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace CounterAppFinal
{
    public partial class MainPage : ContentPage
    {
        private readonly JsonFileService _jsonFileService;
        public ObservableCollection<CounterModel> Counters { get; set; } = new ObservableCollection<CounterModel>();

        public MainPage()
        {
            InitializeComponent();
            _jsonFileService = new JsonFileService();
            LoadCounters();
            BindingContext = this;
        }

        private async void LoadCounters()
        {
            var counters = await _jsonFileService.LoadCountersAsync();
            Counters.Clear();
            foreach (var counter in counters)
            {
                Counters.Add(counter);
            }
        }

        private async void OnAddCounterClicked(object sender, EventArgs e)
        {
            string counterName = CounterNameEntry.Text;
            string counterValueText = CounterValueEntry.Text;

            if (string.IsNullOrWhiteSpace(counterName) || !int.TryParse(counterValueText, out int counterValue))
            {
                await DisplayAlert("Invalid Input", "Please enter a valid name and initial value.", "OK");
                return;
            }

            var newCounter = new CounterModel
            {
                Name = counterName,
                Value = counterValue
            };

            Counters.Add(newCounter);
            await SaveCountersAsync();

            CounterNameEntry.Text = string.Empty;
            CounterValueEntry.Text = string.Empty;
        }

        private async void OnIncrementClicked(object sender, EventArgs e)
        {
            var counter = (sender as Button).BindingContext as CounterModel;
            if (counter != null)
            {
                counter.Value++;
                await SaveCountersAsync();
            }
        }

        private async void OnDecrementClicked(object sender, EventArgs e)
        {
            var counter = (sender as Button).BindingContext as CounterModel;
            if (counter != null)
            {
                counter.Value--;
                await SaveCountersAsync();
            }
        }

        private async void OnDeleteCounterClicked(object sender, EventArgs e)
        {
            var counter = (sender as Button).BindingContext as CounterModel;
            if (counter != null)
            {
                bool confirm = await DisplayAlert("Confirm Delete", $"Are you sure you want to delete the counter '{counter.Name}'?", "Yes", "No");
                if (confirm)
                {
                    Counters.Remove(counter);
                    await SaveCountersAsync();
                }
            }
        }

        private async Task SaveCountersAsync()
        {
            await _jsonFileService.SaveCountersAsync(new List<CounterModel>(Counters));
        }
    }
}
