using GymManagementDAL.Data.Context;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Classes
{
    internal class HealthRecordRepository : IHealthRecordRepository
    {
        private readonly GymDbContext _dbContext;

        public HealthRecordRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Add(HealthRecord record)
        {
            _dbContext.HealthRecords.Add(record);
            return _dbContext.SaveChanges();
        }

        public int Delete(int id)
        {
            var record = _dbContext.HealthRecords.Find(id);
            if (record == null) return 0;

            _dbContext.HealthRecords.Remove(record);
            return _dbContext.SaveChanges();
        }

        public IEnumerable<HealthRecord> GetAll() => _dbContext.HealthRecords.ToList();

        public HealthRecord? GetById(int id) => _dbContext.HealthRecords.Find(id);

        public int Update(HealthRecord record)
        {
            _dbContext.HealthRecords.Update(record);
            return _dbContext.SaveChanges();
        }
    }
}
