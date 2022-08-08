using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CompanyMicroservice.Services;
using System.Linq;
using SharedMicroservice.DTO;
using Microsoft.Extensions.Logging;

namespace CompanyMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly ITenantService _tenantService;
        private readonly ILogger _logger;
        public CompanyController(ICompanyService companyService, ITenantService tenantService, ILogger<CompanyController> logger)
        {
            _companyService = companyService;
            _tenantService = tenantService;
            _logger = logger;
        }

        /// <summary>
        /// Retorna todas as empresas por tenant
        /// </summary>
        /// <param name="tenantId">Id do Tenant</param>
        /// <returns>List de objetos company contendo os dados das empresas.</returns>
        [HttpGet]
        public IActionResult Get([FromQuery] int tenantId)
        {
            var companies = _companyService.ReturnAll(tenantId);
            return new OkObjectResult(companies);
        }

        /// <summary>
        /// Retorna uma empresa em específico (pelo Id)
        /// </summary>
        /// <param name="id">Id da Empresa</param>
        /// <returns>Objeto company contendo os dados da empresa solicitada.</returns>
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var company = _companyService.ReturnById(id);
            if (company == null)
                return new NotFoundResult();
            return new OkObjectResult(company);
        }

        /// <summary>
        /// Inclui uma empresa
        /// </summary>
        /// <param name="companyDTO">Objeto Company</param>
        /// <param name="tenantId">Id do Tenant</param>
        /// <returns>Id da empresa cadastrada.</returns>
        [HttpPost]
        public IActionResult Post([FromBody] CompanyDTO companyDTO, [FromQuery] int tenantId)
        {
            companyDTO.TenantId = tenantId;
            int dto_id = _companyService.Add(companyDTO);
            return CreatedAtAction(nameof(Get), new { id = dto_id });
        }

        /// <summary>
        /// Altera uma empresa
        /// </summary>
        /// <param name="companyDTO">Objeto Company</param>
        /// <returns>Ok/NoOk result.</returns>
        [HttpPut]
        public IActionResult Put([FromBody] CompanyDTO companyDTO)
        {
            if (companyDTO != null)
            {
                _companyService.Alter(companyDTO);
                return new OkResult();
            }
            return new NoContentResult();
        }

        /// <summary>
        /// Exclui uma empresa
        /// </summary>
        /// <param name="id">Id da empresa</param>
        /// <returns>Ok result ou exception.</returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _companyService.Remove(id);
            return new OkResult();
        }
    }
}
