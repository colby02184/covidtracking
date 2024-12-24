using HealthMonitor.Data.Repositories;
using HealthMonitor.Data;
using HealthMonitor.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace HealthMonitor.Tests
{
    [TestClass]
    public class CovidCaseRepositoryTests
    {
        private ApplicationDbContext _dbContext = null!;
        private CovidCaseRepository _repository = null!;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "covidtracker")
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _repository = new CovidCaseRepository(_dbContext);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [TestMethod]
        public async Task GetAllAsync_ReturnsAllCovidData()
        {
            // Arrange
            var covidDataList = new List<CovidData> { new(), new() };
            await _dbContext.CovidData.AddRangeAsync(covidDataList);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public async Task AddAsync_AddsCovidData()
        {
            // Arrange
            var covidData = new CovidData();

            // Act
            await _repository.AddAsync(covidData);

            // Assert
            var addedData = await _dbContext.CovidData.FindAsync(covidData.Id);
            Assert.IsNotNull(addedData);
        }

        [TestMethod]
        public async Task UpdateAsync_UpdatesCovidData()
        {
            // Arrange
            var covidData = new CovidData();
            await _dbContext.CovidData.AddAsync(covidData);
            await _dbContext.SaveChangesAsync();

            // Act
            covidData.State = "BlahState"; 
            await _repository.UpdateAsync(covidData);

            // Assert
            var updatedData = await _dbContext.CovidData.FindAsync(covidData.Id);
            Assert.AreEqual("BlahState", updatedData.State);
        }

        [TestMethod]
        public async Task DeleteAsync_DeletesCovidData()
        {
            // Arrange
            var covidData = new CovidData { Id = 1 };
            await _dbContext.CovidData.AddAsync(covidData);
            await _dbContext.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync(covidData.Id);

            // Assert
            var deletedData = await _dbContext.CovidData.FindAsync(covidData.Id);
            Assert.IsNull(deletedData);
        }
    }
}
