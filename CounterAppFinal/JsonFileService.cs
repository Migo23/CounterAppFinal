using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace CounterAppFinal
{
    public class JsonFileService
    {
        public async Task<List<CounterModel>> LoadCountersAsync()
        {
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "counters.json");

            if (!File.Exists(filePath))
            {
                return new List<CounterModel>();
            }

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                return await JsonSerializer.DeserializeAsync<List<CounterModel>>(stream);
            }
        }

        public async Task SaveCountersAsync(List<CounterModel> counters)
        {
            string json = JsonSerializer.Serialize(counters);
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = Path.Combine(desktopPath, "counters.json");

            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                using (var writer = new StreamWriter(stream))
                {
                    await writer.WriteAsync(json);
                }
            }
        }
    }
}
