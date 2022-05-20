using MedicineService.Models;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MedicineService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoredMedicineController : ControllerBase
    {
        private readonly IStoredMedicineRepository _storedMedicineRepository;

        public StoredMedicineController(IStoredMedicineRepository storedMedicineRepository)
        {
            _storedMedicineRepository = storedMedicineRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var storedMedicines = _storedMedicineRepository.GetStoredMedicines();
            return new OkObjectResult(storedMedicines);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id, int batchNo)
        {
            var storedMedicine = _storedMedicineRepository.GetStoredMedicineByID(id, batchNo);
            return new OkObjectResult(storedMedicine);
        }

        [HttpPost]
        public IActionResult Post([FromBody] StoredMedicine storedMedicine)
        {
            using (var scope = new TransactionScope())
            {
                _storedMedicineRepository.InsertStoredMedicine(storedMedicine);
                scope.Complete();
                return CreatedAtAction(nameof(Get), new { id = storedMedicine.MedicineId }, storedMedicine);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] StoredMedicine storedMedicine)
        {
            if (storedMedicine != null)
            {
                using (var scope = new TransactionScope())
                {
                    _storedMedicineRepository.UpdateStoredMedicine(storedMedicine);
                    scope.Complete();
                    return new OkResult();
                }
            }
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _storedMedicineRepository.DeleteStoredMedicine(id);
            return new OkResult();
        }
    }
}
