using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows.Forms;

namespace weather_app
{
    public class RoundedPanel : Panel
    {
        // property to set the border radius
        public int BorderRadius { get; set; } = 20;

        // override OnPaint method to draw rounded corners
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // GraphicsPath with rounded corners
            using (var path = new System.Drawing.Drawing2D.GraphicsPath())
            {
                // Define the corners of the panel with arcs (rounded corners)
                path.AddArc(0, 0, BorderRadius, BorderRadius, 180, 90); // top-left
                path.AddArc(Width - BorderRadius - 1, 0, BorderRadius, BorderRadius, 270, 90); // top-right
                path.AddArc(Width - BorderRadius - 1, Height - BorderRadius - 1, BorderRadius, BorderRadius, 0, 90); // bottom-right
                path.AddArc(0, Height - BorderRadius - 1, BorderRadius, BorderRadius, 90, 90); // bottom-left
                path.CloseFigure();

                // set the panel's region to this rounded rectangle
                this.Region = new Region(path);
            }
        }
    }

    public partial class MainPage : Form
    {
        private string ApiKey;

        // curent weather
        private Label cityName;
        private Label currentTemp;
        private Label currentWeatherDesc;
        private Label currentFeelsLike;
        private Label currentMinTemp;
        private Label currentMaxTemp;
        private Label currentPressureLabel;
        private Label currentPressure;
        private Label currentHumidityLabel;
        private Label currentHumidity;
        private Label sunrise;
        private Label sunset;

        private Label morningLabel;
        private Label morningTemp;
        private Label morningDesc;
        private Label afternoonLabel;
        private Label afternoonTemp;
        private Label afternoonDesc;
        private Label nightLabel;
        private Label nightTemp;
        private Label nightDesc;
        private Label midnightLabel;
        private Label midnightTemp;
        private Label midnightDesc;

        private RoundedPanel currentWeatherCard;
        private RoundedPanel airQualityCard;
        private RoundedPanel periodForecastCard;
        private RoundedPanel favoriteCityCard;

        private Label airQualityLabel;
        private Label periodForecastLabel;
        private Label favoriteCityLabel;

        private List<CityWeather> favoriteCities = new List<CityWeather>();

        public MainPage()
        {
            InitializeComponent();
            InitializeLabels();
            InitializeCards();
            InitializePeriodWeatherIcons();
            ApiKey = GetApiKey(); // this will pull the api key from App.config
            LoadWeatherData("Midsayap"); // this will load the initial city data

            // attach the KeyDown event handler to textBox1
            textBox1.KeyDown += new KeyEventHandler(textBox1_KeyDown);

        }

        // method to get the api key from App.config
        private string GetApiKey()
        {
            return ConfigurationManager.AppSettings["ApiKey"];
        }


        private void InitializeLabels()
        {
            // current weather
            cityName = new Label
            {
                cityName.Location = new System.Drawing.Point(10, 10);
                cityName.AutoSize = true;
                cityName.Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Bold);
                cityName.BackColor = Color.Transparent;
            };

            currentTemp = new Label
            {
                currentTemp.Location = new System.Drawing.Point(20, 60);
                currentTemp.AutoSize = true;
                currentTemp.Font = new System.Drawing.Font("Arial", 16, System.Drawing.FontStyle.Bold);
                currentTemp.BackColor = Color.Transparent;
            };

            currentWeatherDesc = new Label
            {
                currentWeatherDesc.Location = new Point(20, 200);
                currentWeatherDesc.BackColor = Color.Transparent;
            };

            currentFeelsLike = new Label
            {
                currentFeelsLike.Location = new System.Drawing.Point(115, 60);
                currentFeelsLike.AutoSize = true;
                currentTemp.BackColor = Color.Transparent;
            };

            currentMinTemp = new Label
            {
                currentMinTemp.BackColor.Transparent;
            };
            currentMaxTemp = new Label
            {
                currentMaxTemp.BackColor.Transparent;
            };

            currentPressureLabel = new Label
            {
                currentPressureLabel.Text = "Pressure";
                currentPressureLabel.Location = new Point(20, 150);
                currentPressureLabel.BackColor.Transparent;
            };

            currentPressure = new Label
            {
                currentPressure.Location = new Point(20, 170);
                currentPressure.BackColor.Transparent;
            };

            currentHumidityLabel = new Label
            {
                currentHumidityLabel.Text = "Humidity";
                currentHumidityLabel.Location = new Point(120, 150);
                currentHumidityLabel.BackColor.Transparent;
            };

            currentHumidity = new Label
            {
                currentHumidity.Location = new Point(120, 170);
                currentHumidity.BackColor.Transparent;
            };

            sunrise = new Label
            {
                sunrise.BackColor.Transparent;
            };
            sunset = new Label
            {
                sunset.BackColor.Transparent;
            };

            // period forecast
            morningLabel = new Label();
            morningTemp = new Label();
            morningDesc = new Label();
            afternoonLabel = new Label();
            afternoonTemp = new Label();
            afternoonDesc = new Label();
            nightLabel = new Label();
            nightTemp = new Label();
            nightDesc = new Label();
            midnightLabel = new Label();
            midnightTemp = new Label();
            midnightDesc = new Label();

            currentWeatherCard.Controls.Add(cityName);
            currentWeatherCard.Controls.Add(currentTemp);
            currentWeatherCard.Controls.Add(currentWeatherDesc);
            currentWeatherCard.Controls.Add(currentFeelsLike);
            currentWeatherCard.Controls.Add(currentPressureLabel);
            currentWeatherCard.Controls.Add(currentPressure);
            currentWeatherCard.Controls.Add(currentHumidityLabel);
            currentWeatherCard.Controls.Add(currentHumidity);
            // currentWeatherCard.Controls.Add(favoriteButton);
        }

        private void InitializeCards()
        {
            // init current weather card
            currentWeatherCard = new RoundedPanel
            {
                Size = new Size(300, 200),
                Location = new Point(30, 50),
                BackColor = Color.White,
                BorderRadius = 20,
                BackgroundImage = Image.FromFile("./resources/air-quality-bg.jpg"),
                BackgroundImageLayout = ImageLayout.Stretch
            };

            this.Controls.Add(currentWeatherCard);

            // init air quality card
            airQualityCard = new RoundedPanel
            {
                Size = new Size(300, 200),
                Location = new Point(350, 50),
                BackColor = Color.White,
                BorderRadius = 20,
            };

            airQualityLabel = new Label
            {
                Text = "Air Quality",
                Location = new Point(10, 10),
                AutoSize = true
            };

            airQualityCard.Controls.Add(airQualityLabel);
            this.Controls.Add(airQualityCard);

            // init favorite city weather card
            periodForecastCard = new RoundedPanel
            {
                Size = new Size(500, 200),
                Location = new Point(30, 270),
                BackColor = Color.White,
                BorderRadius = 20,
                BackgroundImage = Image.FromFile("./resources/period-forecast-bg.png"),
                BackgroundImageLayout = ImageLayout.Stretch
            };

            periodForecastLabel = new Label
            {
                Text = "Hourly Forecast",
                Location = new Point(10, 10),
                AutoSize = true
            };

            periodForecastCard.Controls.Add(periodForecastLabel);
            this.Controls.Add(periodForecastCard);

            // init favorite city weather card
            favoriteCityCard = new RoundedPanel
            {
                Size = new Size(250, 150),
                Location = new Point(350, 270),
                BackColor = Color.White,
                BorderRadius = 20
            };

            favoriteCityLabel = new Label
            {
                Text = "Favorite City",
                Location = new Point(10, 10),
                AutoSize = true
            };

            favoriteCityCard.Controls.Add(favoriteCityLabel);
            // this.Controls.Add(favoriteCityCard);
        }

        private void InitializePeriodWeatherIcons()
        {
            morningIcon = new PictureBox
            {
                Location = new Point(20, 20),
                Size = new Size(40, 40),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent
            };
            morningIcon.Image = Image.FromFile("./resources/weather-icons/rainy.png");
            currentWeatherCard.Controls.Add(morningIcon);

            afternoonIcon = new PictureBox
            {
                Location = new Point(120, 20),
                Size = new Size(40, 40),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent
            };
            afternoonIcon.Image = Image.FromFile("./resources/weather-icons/cloudy.png");
            currentWeatherCard.Controls.Add(afternoonIcon);

            nightIcon = new PictureBox
            {
                Location = new Point(220, 20),
                Size = new Size(40, 40),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent
            };
            nightIcon.Image = Image.FromFile("./resources/weather-icons/thunderstorm.png");
            currentWeatherCard.Controls.Add(nightIcon);

            midnightIcon = new PictureBox
            {
                Location = new Point(320, 20),
                Size = new Size(40, 40),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent
            };
            midnightIcon.Image = Image.FromFile("./resources/weather-icons/snow.png");
            currentWeatherCard.Controls.Add(midnightIcon);
        }

        private async void LoadWeatherData(string city)
        {
            string currentWeatherUrl = $"http://api.openweathermap.org/data/2.5/weather?q={city}&appid={ApiKey}&units=metric";
            string forecastUrl = $"http://api.openweathermap.org/data/2.5/forecast?q={city}&appid={ApiKey}&units=metric";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // current weather data
                    HttpResponseMessage currentWeatherResponse = await client.GetAsync(currentWeatherUrl);
                    currentWeatherResponse.EnsureSuccessStatusCode();
                    string currentWeatherJsonResponse = await currentWeatherResponse.Content.ReadAsStringAsync();
                    var currentWeatherData = JsonConvert.DeserializeObject<WeatherData>(currentWeatherJsonResponse);

                    cityName.Text = $"{currentWeatherData.Name}";
                    currentTemp.Text = $"{currentWeatherData.Main.Temp}°C"; // current temperature
                    currentFeelsLike.Text = $"{currentWeatherData.Main.FeelsLike}°C"; // feels Like
                    currentWeatherDesc.Text = $"{currentWeatherData.Weather[0].Description}"; // weather description
                    currentMinTemp.Text = $"{currentWeatherData.Main.TempMin}°C"; // minimum temperature
                    currentMaxTemp.Text = $"{currentWeatherData.Main.TempMax}°C"; // maximum temperature
                    currentPressure.Text = $"{currentWeatherData.Main.Pressure}hPa"; // pressure
                    currentHumidity.Text = $"{currentWeatherData.Main.Humidity}%"; // humidity

                    // TODO: convert to unix
                    sunrise.Text = $"{currentWeatherData.Sys.Sunrise}";
                    sunrise.Text = $"{currentWeatherData.Sys.Sunset}";

                    Button favoriteButton = new Button
                    {
                        Text = "Favorite",
                        Location = new Point(100, 120),
                        Size = new Size(100, 30)
                    };
                    favoriteButton.Click += (sender, e) => AddToFavorites(city, currentWeatherData); // pass city and weather data to the AddToFavorites method

                    // FORECAST DATA

                    HttpResponseMessage forecastResponse = await client.GetAsync(forecastUrl);
                    forecastResponse.EnsureSuccessStatusCode();
                    string forecastJsonResponse = await forecastResponse.Content.ReadAsStringAsync();
                    var forecastData = JsonConvert.DeserializeObject<ForecastData>(forecastJsonResponse);

                    ForecastItem morning = GetForecastByTime(forecastData.List, 6); // morning - 06:00 am
                    ForecastItem afternoon = GetForecastByTime(forecastData.List, 12); // afternoon - 12:00 pm
                    ForecastItem midnight = GetForecastByTime(forecastData.List, 0); // midnight - 00:00 pm
                    ForecastItem night = GetForecastByTime(forecastData.List, 21); // night - 09:00 pm

                    periodForecastLabel.Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold);

                    morningLabel.Text = "Morning";
                    morningTemp.Text = $"{morning.Main.Temp}°C";
                    morningDesc.Text = $"{morning.Weather[0].Description}";
                    morningLabel.Location = new Point(20, 50);
                    morningTemp.Location = new Point(20, 75);
                    morningDesc.Location = new Point(20, 95);
                    morningTemp.Font = new Font("Arial", 8, System.Drawing.FontStyle.Bold);

                    afternoonLabel.Text = "Afternoon";
                    afternoonTemp.Text = $"{afternoon.Main.Temp}°C";
                    afternoonDesc.Text = $"{afternoon.Weather[0].Description}";
                    afternoonLabel.Location = new Point(120, 50);
                    afternoonTemp.Location = new Point(120, 75);
                    afternoonDesc.Location = new Point(120, 95);
                    afternoonTemp.Font = new Font("Arial", 8, System.Drawing.FontStyle.Bold);

                    nightLabel.Text = "Night";
                    nightTemp.Text = $"{night.Main.Temp}°C";
                    nightDesc.Text = $"{night.Weather[0].Description}";
                    nightLabel.Location = new Point(220, 50);
                    nightTemp.Location = new Point(220, 75);
                    nightDesc.Location = new Point(220, 95);
                    nightTemp.Font = new Font("Arial", 8, System.Drawing.FontStyle.Bold);

                    midnightLabel.Text = "Midnight";
                    midnightTemp.Text = $"{midnight.Main.Temp}°C";
                    midnightDesc.Text = $"{midnight.Weather[0].Description}";
                    midnightLabel.Location = new Point(320, 50);
                    midnightTemp.Location = new Point(320, 75);
                    midnightDesc.Location = new Point(320, 95);
                    midnightTemp.Font = new Font("Arial", 8, System.Drawing.FontStyle.Bold);

                    // reset the background of period forecast labels
                    periodForecastLabel.BackColor = Color.Transparent;

                    morningLabel.BackColor = Color.Transparent;
                    morningTemp.BackColor = Color.Transparent;
                    morningDesc.BackColor = Color.Transparent;

                    afternoonLabel.BackColor = Color.Transparent;
                    afternoonTemp.BackColor = Color.Transparent;
                    afternoonDesc.BackColor = Color.Transparent;

                    nightLabel.BackColor = Color.Transparent;
                    nightTemp.BackColor = Color.Transparent;
                    nightDesc.BackColor = Color.Transparent;

                    midnightLabel.BackColor = Color.Transparent;
                    midnightTemp.BackColor = Color.Transparent;
                    midnightDesc.BackColor = Color.Transparent;

                    // adding controls to the hourly forecast card
                    // these controls are positioned relative to the card
                    // moving the card will move all its child controls together
                    periodForecastCard.Controls.Add(morningLabel);
                    periodForecastCard.Controls.Add(morningTemp);
                    periodForecastCard.Controls.Add(morningDesc);
                    periodForecastCard.Controls.Add(afternoonLabel);
                    periodForecastCard.Controls.Add(afternoonTemp);
                    periodForecastCard.Controls.Add(afternoonDesc);
                    periodForecastCard.Controls.Add(nightLabel);
                    periodForecastCard.Controls.Add(nightTemp);
                    periodForecastCard.Controls.Add(nightDesc);
                    periodForecastCard.Controls.Add(midnightLabel);
                    periodForecastCard.Controls.Add(midnightTemp);
                    periodForecastCard.Controls.Add(midnightDesc);

                    // DisplayWeeklyWeather(forecastData.List);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private void AddToFavorites(string city, WeatherData weatherData)
        {
            // checker if the city is already in favorites
            if (favoriteCities.Any(f => f.Name == city)) return;

            // create a new CityWeather object for this city
            var cityWeather = new CityWeather
            {
                Name = city,
                Temp = weatherData.Main.Temp,
                Description = weatherData.Weather[0].Description
            };

            favoriteCities.Add(cityWeather);
            DisplayFavoriteCities(); // update the ui with the new favorite city card
        }

        private void DisplayFavoriteCities()
        {
            // clear the existing favorite city cards
            favoriteCityCard.Controls.Clear();

            // add a label for the "Favorite Cities" title
            favoriteCityLabel.Text = "Favorite Cities";
            favoriteCityCard.Controls.Add(favoriteCityLabel);

            // create a new card for each favorite city
            int yOffset = 40; // initial offset for positioning cards
            foreach (var cityWeather in favoriteCities)
            {
                var cityCard = new RoundedPanel
                {
                    Size = new Size(250, 100),
                    Location = new Point(10, yOffset),
                    BackColor = Color.White,
                    BorderRadius = 20
                };

                var cityNameLabel = new Label
                {
                    Text = cityWeather.Name,
                    Location = new Point(10, 10),
                    AutoSize = true
                };

                var tempLabel = new Label
                {
                    Text = $"{cityWeather.Temp}°C",
                    Location = new Point(10, 30),
                    AutoSize = true
                };

                var descLabel = new Label
                {
                    Text = cityWeather.Description,
                    Location = new Point(10, 50),
                    AutoSize = true
                };

                var deleteButton = new Button
                {
                    Text = "Delete",
                    Location = new Point(160, 70),
                    Size = new Size(80, 25)
                };
                deleteButton.Click += (sender, e) => RemoveFavorite(cityWeather);

                cityCard.Controls.Add(cityNameLabel);
                cityCard.Controls.Add(tempLabel);
                cityCard.Controls.Add(descLabel);
                cityCard.Controls.Add(deleteButton);

                // this will add the city card to the favorite city panel
                favoriteCityCard.Controls.Add(cityCard);

                yOffset += 120; // this will move the next card down
            }
        }

        private void RemoveFavorite(CityWeather cityWeather)
        {
            // remove the city from the favorite list
            favoriteCities.Remove(cityWeather);
            DisplayFavoriteCities(); // update the ui after removal
        }

        private void DisplayWeeklyWeather(ForecastItem[] forecastList)
        {
            var groupedByDay = forecastList
                .GroupBy(item => DateTime.Parse(item.DtTxt).Date) // group by date
                .OrderBy(group => group.Key); // sort groups by date (?)

            int dayCount = 0;
            foreach (var dayGroup in groupedByDay)
            {
                if (dayCount > 6) break; // 7 days limit

                var dayWeather = GetDayWeather(dayGroup.ToArray());
                DisplayDayWeather(dayCount, dayWeather);
                dayCount++;
            }
        }

        private WeatherForDay GetDayWeather(ForecastItem[] dayForecast)
        {
            var weatherForDay = new WeatherForDay
            {
                Temp = dayForecast.Average(item => item.Main.Temp), // average temperature for the day
                Description = dayForecast.First().Weather[0].Description // first weather description of the day
            };

            return weatherForDay;
        }

        private void DisplayDayWeather(int dayIndex, WeatherForDay dayWeather)
        {
            // weather data for each day
            string dayName = Enum.GetName(typeof(DayOfWeek), (int)DateTime.Now.AddDays(dayIndex).DayOfWeek);

            // dynamic labels for each day's weather
            var dayLabel = new Label
            {
                Text = $"{dayName}: {dayWeather.Temp}°C, {dayWeather.Description}",
                Location = new System.Drawing.Point(350, 250 + (dayIndex * 30)),
                AutoSize = true
            };

            this.Controls.Add(dayLabel);
        }

        private ForecastItem GetForecastByTime(ForecastItem[] forecastList, int hour)
        {
          foreach (var item in forecastList)
          {
            DateTime dt = DateTime.Parse(item.DtTxt);
            if (dt.Hour == hour)
            {
              return item;
            }
          }
          return null; // if no data is found for the specified hour
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // search box
        }

        // checks if the key pressed is Enter key. if so, it calls button1_Click to execute the search
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // prevents the "ding" sound
                button1_Click(this, new EventArgs()); // calls the search button click method
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string city = textBox1.Text.Trim();
            if (!string.IsNullOrEmpty(city))
            {
                LoadWeatherData(city);
            }
            else
            {
                MessageBox.Show("Please enter a city name.");
            }
        }
    }

    public class CityWeather
    {
        public string Name { get; set; }
        public double Temp { get; set; }
        public string Description { get; set; }
    }

    public class WeatherData
    {
        public MainInfo Main { get; set; }
        public WeatherDescription[] Weather { get; set; }
        public SysInfo Sys { get; set; }
        public string Name { get; set; }
    }

    public class SysInfo
    {
        [JsonProperty("sunrise")]
        public long Sunrise { get; set; }

        [JsonProperty("sunset")]
        public long Sunset { get; set; }
    }

    public class MainInfo
    {
        [JsonProperty("temp")]
        public double Temp { get; set; }

        [JsonProperty("feels_like")]
        public double FeelsLike { get; set; }

        [JsonProperty("temp_min")]
        public double TempMin { get; set; }

        [JsonProperty("temp_max")]
        public double TempMax { get; set; }

        [JsonProperty("pressure")]
        public double Pressure { get; set; }

        [JsonProperty("humidity")]
        public double Humidity { get; set; }
    }

    public class ForecastData
    {
        [JsonProperty("list")]
        public ForecastItem[] List { get; set; }
    }

    public class ForecastItem
    {
        [JsonProperty("dt_txt")]
        public string DtTxt { get; set; }

        [JsonProperty("main")]
        public MainInfo Main { get; set; }

        [JsonProperty("weather")]
        public WeatherDescription[] Weather { get; set; }
    }

    public class WeatherDescription
    {
        [JsonProperty("description")]
        public string Description { get; set; }
    }

    public class WeatherForDay
    {
        public double Temp { get; set; }
        public string Description { get; set; }
    }
}
