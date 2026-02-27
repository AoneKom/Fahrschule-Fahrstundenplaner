using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fahrstundenplaner.ViewModels; 
using Fahrstundenplaner.Models;   

namespace Fahrstundenplaner.Tests
{
    [TestClass]
    public class MainViewModelTests
    {
        [TestMethod]
        public void CheckForConflict_ShouldReturnTrue()
        {
            var vm = new MainViewModel();
           
            var stunde = new Fahrstunde
            {
                Startzeit = "01.01.2026 10:00",
                LehrerName = "Kominch",
                StudentName = "Test Student",
                LessonType = "Übungsfahrt"
            };

            vm.Fahrstunden.Add(stunde);
            Assert.IsTrue(vm.CheckForConflict("01.01.2026 10:00", "Kominch"));
        }
    }
}