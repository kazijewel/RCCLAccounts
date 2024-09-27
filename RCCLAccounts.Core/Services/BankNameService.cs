using Mapster;
using MapsterMapper;
using ProvidentFund.Core.Interfaces;
using ProvidentFund.Core.Models;
using ProvidentFund.Data.Entities;
using ProvidentFund.Data.Interfaces;
using ProvidentFund.Data.Repositories;

namespace ProvidentFund.Core.Services
{
    public class BankNameService : IBankNameService
    {
        private readonly IBankNameRepository _bankNameRepository;
        private readonly IMapper _mapper;

        public BankNameService(IBankNameRepository bankNameRepository, IMapper mapper)
        {
            _bankNameRepository = bankNameRepository;
            _mapper = mapper;
        }

        public async Task CreateAsync(BankNameModel bankNameModel)
        {
            var bankName = bankNameModel.Adapt<BankName>(); // Mapster mapper => Converting EmployeeModel to Employee
            await _bankNameRepository.CreateAsync(bankName);
        }
        public async Task<List<BankNameModel>> GetAllBankNameAsync()
        {
            var bankName = await _bankNameRepository.GetAllBankNameAsync();
            return bankName.Adapt<List<BankNameModel>>();
        }
        public async Task<BankNameModel> GetByIdAsync(int? id)
        {
            var bankName = await _bankNameRepository.GetByIdAsync(id);
            var bankNameModel = bankName.Adapt<BankNameModel>(); // Mapster mapper => Converting Employee to EmployeeModel
            return bankNameModel;
        }
        public async Task UpdateAsync(BankNameModel bankNameModel)
        {
            var bankName = bankNameModel.Adapt<BankName>(); // Mapster mapper => Converting and updating EmployeeModel to Employee
            await _bankNameRepository.UpdateAsync(bankName);
        }
        public async Task DeleteAsync(int id)
        {
            await _bankNameRepository.DeleteAsync(id);
        }
        public async Task<bool> BankExistsAsync(int id)
        {
            return await _bankNameRepository.BankExistsAsync(id);
        }
    }
}
