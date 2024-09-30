using Microsoft.EntityFrameworkCore;
using RCCLAccounts.Data.Entities;
using RCCLAccounts.Data.Interfaces;

namespace RCCLAccounts.Data.Repositories
{
	public class EmployeeCpfledgerRepository : IEmployeeCpfledgerRepository
	{
		private readonly AppDbContext _context;
		public EmployeeCpfledgerRepository(AppDbContext context)
		{
			_context = context;
		}
		public async Task CreateAsync(EmployeeCpfledger employeeCpfledger)
		{
			await _context.EmployeeCpfledgers.AddAsync(employeeCpfledger);
		}

        public async Task<int> CreateRangeAsync(List<EmployeeCpfledger> employeeCpfledgers)
        {
            await _context.EmployeeCpfledgers.AddRangeAsync(employeeCpfledgers);
            return await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
		{
            //_context.EmployeeCpfledgers.Remove(await GetByIdAsync(id));
            _context.EmployeeCpfledgers.RemoveRange(await GetByIdAsync(id));
			await _context.SaveChangesAsync();
		}

		public async Task<bool> EmployeeCpfledgerExistsAsync(int id)
		{
			return await _context.EmployeeCpfledgers.AnyAsync(e => e.Id == id);
		}

		public async Task<List<EmployeeCpfledger>> GetAllEmployeeCpfledgerAsync()
		{
			return await _context.EmployeeCpfledgers.ToListAsync();
		}
       
        public async Task<List<EmployeeCpfledger>> GetByIdAsync(string id)
        {
			return await _context.EmployeeCpfledgers.Where(m => m.TransactionId == id).ToListAsync();

        }

        public async Task UpdateAsync(EmployeeCpfledger employeeCpfledger)
		{
			_context.Entry(employeeCpfledger).State = EntityState.Modified;
			await _context.SaveChangesAsync();
		}
        public async Task<List<EmployeeCpfledger>> GetAllCPFDepositeByTransIdAsync(string id)
        {
            return await _context.EmployeeCpfledgers.Include(x => x.EmployeeInfo).Include(x=>x.EmployeeInfo.branch).Where(m => m.TransactionId == id).ToListAsync();

        }
    }
}
