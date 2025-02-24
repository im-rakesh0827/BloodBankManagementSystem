namespace BloodBankManagementSystem.UI.Pages
{
    public partial class DashboardComponent
    {
        private Dictionary<string, int> bloodGroups = new()
        {
            { "A+", 75 }, { "A-", 50 }, { "B+", 60 }, { "B-", 40 },
            { "O+", 80 }, { "O-", 55 }, { "AB+", 30 }, { "AB-", 20 }
        };
        private Dictionary<string, int> bloodRequests7Days = new()
        {
            { "A+", 20 }, { "A-", 15 }, { "B+", 25 }, { "B-", 10 },
            { "O+", 10 }, { "O-", 18 }, { "AB+", 12 }, { "AB-", 80 }
        };

        private Dictionary<string, int> bloodRequestsAllTime = new()
        {
            { "A+", 120 }, { "A-", 115 }, { "B+", 25 }, { "B-", 10 },
            { "O+", 100 }, { "O-", 18 }, { "AB+", 12 }, { "AB-", 80 }
        };
        private Dictionary<string, string> bloodTypeColors = new()
        {
            { "A+", "#4e79a7" }, { "A-", "#f28e2c" }, { "B+", "#e15759" },
            { "B-", "#76b7b2" }, { "O+", "#59a14f" }, { "O-", "#edc949" },
            { "AB+", "#af7aa1" }, { "AB-", "#ff9da7" }
        };

        private List<KeyValuePair<string, int>> sortedBloodRequests;
        private int maxBloodRequest;
        private string pieChartColors;
        protected override void OnInitialized()
        {
            sortedBloodRequests = bloodRequestsAllTime.OrderByDescending(br => br.Value).ToList();
            maxBloodRequest = sortedBloodRequests.Max(br => br.Value);
            int totalRequests = bloodRequestsAllTime.Values.Sum();
            int startPercentage = 0;
            pieChartColors = string.Join(", ", bloodRequestsAllTime.Select(entry =>
            {
                int percent = (int)((entry.Value / (double)totalRequests) * 100);
                string color = bloodTypeColors[entry.Key];
                string segment = $"{color} {startPercentage}%, {color} {startPercentage + percent}%";
                startPercentage += percent;
                return segment;
            }));
        }
    }
}
