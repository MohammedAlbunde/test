using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MCP.SmartBracelet
{
    /// <summary>
    /// Smart Bracelet Health Metrics Dashboard
    /// Created via MCP Integration on April 7, 2025
    /// </summary>
    public class SmartBraceletDashboard
    {
        // Simulated sensor data
        private Random _random = new Random();
        private const int DefaultRefreshRate = 5000; // 5 seconds
        
        // Health metrics
        private double _heartRate;
        private double _bloodOxygen;
        private double _bodyTemperature;
        private int _stepCount;
        private double _caloriesBurned;
        private double _waterIntake;
        private DateTime _lastSyncTime;
        private List<HealthAlert> _activeAlerts = new List<HealthAlert>();
        
        // Historical data
        private List<HealthDataPoint> _healthHistory = new List<HealthDataPoint>();
        
        public SmartBraceletDashboard()
        {
            // Initialize with default values
            _heartRate = 72;
            _bloodOxygen = 98;
            _bodyTemperature = 36.5;
            _stepCount = 0;
            _caloriesBurned = 0;
            _waterIntake = 0;
            _lastSyncTime = DateTime.Now;
        }
        
        /// <summary>
        /// Start the dashboard with simulated data updates
        /// </summary>
        public async Task StartDashboardAsync(int refreshRateMs = DefaultRefreshRate, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("🌟 Smart Bracelet Health Dashboard 🌟");
            Console.WriteLine("====================================");
            Console.WriteLine("Press Ctrl+C to exit\n");
            
            while (!cancellationToken.IsCancellationRequested)
            {
                // Update simulated data
                UpdateSensorData();
                
                // Check for health alerts
                CheckHealthAlerts();
                
                // Display dashboard
                DisplayDashboard();
                
                // Store history
                RecordHealthDataPoint();
                
                // Wait for next refresh
                await Task.Delay(refreshRateMs, cancellationToken);
                Console.Clear();
            }
        }
        
        /// <summary>
        /// Update simulated sensor data with realistic fluctuations
        /// </summary>
        private void UpdateSensorData()
        {
            // Simulate heart rate (60-100 bpm normal range)
            _heartRate += (_random.NextDouble() * 2 - 1) * 2; // +/- 2 bpm max change
            _heartRate = Math.Max(45, Math.Min(180, _heartRate)); // Constrain to reasonable limits
            
            // Simulate blood oxygen (95-100% normal range)
            _bloodOxygen += (_random.NextDouble() * 2 - 1) * 0.5; // +/- 0.5% max change
            _bloodOxygen = Math.Max(85, Math.Min(100, _bloodOxygen));
            
            // Simulate body temperature (36.1-37.2°C normal range)
            _bodyTemperature += (_random.NextDouble() * 2 - 1) * 0.1; // +/- 0.1°C max change
            _bodyTemperature = Math.Max(35, Math.Min(40, _bodyTemperature));
            
            // Simulate steps (increase by 0-50 steps per update)
            int newSteps = _random.Next(0, 51);
            _stepCount += newSteps;
            
            // Simulate calories (based on steps, approximately 0.04 calories per step)
            _caloriesBurned += newSteps * 0.04;
            
            // Simulate water intake (occasional drink)
            if (_random.NextDouble() < 0.1) // 10% chance of drinking water
            {
                _waterIntake += _random.Next(50, 251) / 1000.0; // 50-250 ml
            }
            
            // Update sync time
            _lastSyncTime = DateTime.Now;
        }
        
        /// <summary>
        /// Check for health alerts based on current metrics
        /// </summary>
        private void CheckHealthAlerts()
        {
            // Clear previous alerts
            _activeAlerts.Clear();
            
            // Check heart rate (too high or too low)
            if (_heartRate > 100)
                _activeAlerts.Add(new HealthAlert("High Heart Rate", $"Current: {_heartRate:F1} BPM", AlertSeverity.Warning));
            else if (_heartRate < 60)
                _activeAlerts.Add(new HealthAlert("Low Heart Rate", $"Current: {_heartRate:F1} BPM", AlertSeverity.Warning));
                
            // Check blood oxygen (below 95% is concerning)
            if (_bloodOxygen < 95)
                _activeAlerts.Add(new HealthAlert("Low Blood Oxygen", $"Current: {_bloodOxygen:F1}%", 
                    _bloodOxygen < 90 ? AlertSeverity.Critical : AlertSeverity.Warning));
                    
            // Check body temperature
            if (_bodyTemperature > 37.5)
                _activeAlerts.Add(new HealthAlert("Elevated Body Temperature", $"Current: {_bodyTemperature:F1}°C", 
                    _bodyTemperature > 38.0 ? AlertSeverity.Critical : AlertSeverity.Warning));
            else if (_bodyTemperature < 36.0)
                _activeAlerts.Add(new HealthAlert("Low Body Temperature", $"Current: {_bodyTemperature:F1}°C", AlertSeverity.Warning));
                
            // Check activity level (step count)
            if (_stepCount < 1000 && DateTime.Now.Hour >= 14) // Less than 1000 steps after 2pm
                _activeAlerts.Add(new HealthAlert("Low Activity", "Consider taking a walk", AlertSeverity.Info));
                
            // Check water intake (assume 2L daily target)
            if (_waterIntake < 1.0 && DateTime.Now.Hour >= 14) // Less than 1L after 2pm
                _activeAlerts.Add(new HealthAlert("Low Water Intake", "Consider drinking more water", AlertSeverity.Info));
        }
        
        /// <summary>
        /// Display the dashboard with current health metrics
        /// </summary>
        private void DisplayDashboard()
        {
            Console.WriteLine($"Last Updated: {_lastSyncTime.ToString("HH:mm:ss")}");
            Console.WriteLine("------------------------------------");
            
            // Display vital signs
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("VITAL SIGNS:");
            Console.ResetColor();
            Console.WriteLine($"❤️  Heart Rate: {_heartRate:F1} BPM");
            Console.WriteLine($"🫁 Blood Oxygen: {_bloodOxygen:F1}%");
            Console.WriteLine($"🌡️  Body Temperature: {_bodyTemperature:F1}°C");
            Console.WriteLine();
            
            // Display activity metrics
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("ACTIVITY:");
            Console.ResetColor();
            Console.WriteLine($"👣 Steps: {_stepCount:N0}");
            Console.WriteLine($"🔥 Calories: {_caloriesBurned:F1} kcal");
            Console.WriteLine($"💧 Water Intake: {_waterIntake:F1}L");
            Console.WriteLine();
            
            // Display alerts if any
            if (_activeAlerts.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"ALERTS ({_activeAlerts.Count}):");
                Console.ResetColor();
                
                foreach (var alert in _activeAlerts)
                {
                    switch (alert.Severity)
                    {
                        case AlertSeverity.Critical:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("🚨 CRITICAL: ");
                            break;
                        case AlertSeverity.Warning:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("⚠️ WARNING: ");
                            break;
                        case AlertSeverity.Info:
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write("ℹ️ INFO: ");
                            break;
                    }
                    
                    Console.WriteLine($"{alert.Title} - {alert.Message}");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("✅ No health alerts - All metrics normal");
                Console.ResetColor();
            }
            
            // Display basic stats from history
            if (_healthHistory.Count > 1)
            {
                Console.WriteLine("\n------------------------------------");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("TODAY'S STATS:");
                Console.ResetColor();
                
                var today = _healthHistory.Where(h => h.Timestamp.Date == DateTime.Today).ToList();
                
                if (today.Any())
                {
                    double avgHeartRate = today.Average(h => h.HeartRate);
                    double minHeartRate = today.Min(h => h.HeartRate);
                    double maxHeartRate = today.Max(h => h.HeartRate);
                    
                    Console.WriteLine($"Average Heart Rate: {avgHeartRate:F1} BPM (Min: {minHeartRate:F1}, Max: {maxHeartRate:F1})");
                    Console.WriteLine($"Total Steps: {_stepCount:N0}");
                    Console.WriteLine($"Total Calories: {_caloriesBurned:F1} kcal");
                    Console.WriteLine($"Total Water: {_waterIntake:F1}L");
                }
            }
        }
        
        /// <summary>
        /// Record current health data for historical tracking
        /// </summary>
        private void RecordHealthDataPoint()
        {
            _healthHistory.Add(new HealthDataPoint
            {
                Timestamp = _lastSyncTime,
                HeartRate = _heartRate,
                BloodOxygen = _bloodOxygen,
                BodyTemperature = _bodyTemperature,
                StepCount = _stepCount,
                CaloriesBurned = _caloriesBurned,
                WaterIntake = _waterIntake
            });
            
            // Keep only the most recent 1000 data points to prevent memory issues
            if (_healthHistory.Count > 1000)
            {
                _healthHistory.RemoveAt(0);
            }
        }
    }
    
    /// <summary>
    /// Health data point for historical tracking
    /// </summary>
    public class HealthDataPoint
    {
        public DateTime Timestamp { get; set; }
        public double HeartRate { get; set; }
        public double BloodOxygen { get; set; }
        public double BodyTemperature { get; set; }
        public int StepCount { get; set; }
        public double CaloriesBurned { get; set; }
        public double WaterIntake { get; set; }
    }
    
    /// <summary>
    /// Health alert with title, message, and severity level
    /// </summary>
    public class HealthAlert
    {
        public string Title { get; }
        public string Message { get; }
        public AlertSeverity Severity { get; }
        
        public HealthAlert(string title, string message, AlertSeverity severity = AlertSeverity.Info)
        {
            Title = title;
            Message = message;
            Severity = severity;
        }
    }
    
    /// <summary>
    /// Severity levels for health alerts
    /// </summary>
    public enum AlertSeverity
    {
        Info,
        Warning,
        Critical
    }
    
    /// <summary>
    /// Program to run the Smart Bracelet Dashboard
    /// </summary>
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var dashboard = new SmartBraceletDashboard();
            
            // Set up cancellation token to handle Ctrl+C
            var cancellationTokenSource = new CancellationTokenSource();
            Console.CancelKeyPress += (sender, e) => {
                e.Cancel = true; // Prevent immediate termination
                cancellationTokenSource.Cancel();
            };
            
            try
            {
                await dashboard.StartDashboardAsync(cancellationToken: cancellationTokenSource.Token);
            }
            catch (TaskCanceledException)
            {
                // Expected when cancellation is requested
            }
            finally
            {
                Console.Clear();
                Console.WriteLine("Smart Bracelet Dashboard closed.");
                Console.WriteLine("Thank you for using our health monitoring system!");
            }
        }
    }
}
