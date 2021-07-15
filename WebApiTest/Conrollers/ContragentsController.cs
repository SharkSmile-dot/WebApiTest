using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using WebApiTest.Models;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;


namespace WebApiTest.Conrollers
{

    [Route("/api/contragents")]
    [ApiController]
    public class ContragentsController : ControllerBase
    {
        OperationsContext db;
        public ContragentsController(OperationsContext context)
        {
            db = context;


            if (!db.Contragents.Any())
            {

                string use, contf;

                for (int i = 1; i <= 5000; i++)
                {


                    use = i.ToString();
                    contf = String.Concat("CR_", use);

                    db.Contragents.Add(new Contragent { Name = contf });
                    db.SaveChanges();
                }

                
            }
        }


        /// <summary>
        /// Выводит список всех контрагентов
        /// </summary>
        /// <response code="200" >Список получен</response>
        [HttpGet("api/contragents/all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Contragent>>> Get()
        {
            return await db.Contragents.ToListAsync();
        }

        /// <summary>
        /// Получение контрагента по id
        /// </summary>
        /// <response code="200" >Контрагент получен</response>
        /// <response code="404" >Контрагент не найден</response>
        [HttpGet("/api/contragents/id/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Contragent>> Get(int id)
        {



            Contragent oper = await db.Contragents.FirstOrDefaultAsync(x => x.Id == id);
            if (oper == null)
                return NotFound();

            return new ObjectResult(oper);
        }

        /// <summary>
        /// Добавление контрагента !не указывайте id при добавлении! 
        /// </summary>
        /// <response code="200" >Контрагент создан</response>
        /// <response code="400" >Запись не создана, проверьте данные</response>
        [HttpPost("/api/contragents/post")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Contragent>> Post(Contragent oper)
        {
            if (oper == null)
            {
                return BadRequest();
            }

            db.Contragents.Add(oper);
            await db.SaveChangesAsync();
            return Ok(oper);
        }

        /// <summary>
        /// Изменение контрагента !id обязателен чтобы найти запись! 
        /// </summary>
        /// <response code="200" >Контрагент изменен</response>
        /// <response code="400" >Запись не изменена, проверьте данные</response>
        /// <response code="404" >Запись не найдена, проверьте id</response>
        [HttpPut("/api/contragents/put")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Contragent>> Put(Contragent oper)
        {
            if (oper == null)
            {
                return BadRequest();
            }
            if (!db.Contragents.Any(x => x.Id == oper.Id))
            {
                return NotFound();
            }

            db.Update(oper);
            await db.SaveChangesAsync();
            return Ok(oper);
        }

        /// <summary>
        /// Удаление контрагента по id
        /// </summary>
        /// <response code="200" >Контрагент удален</response>
        /// <response code="404" >Запись не найдена, проверьте id</response>
        [HttpDelete("/api/contragents/delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Contragent>> Delete(int id)
        {
            Contragent oper = db.Contragents.FirstOrDefault(x => x.Id == id);
            if (oper == null)
            {
                return NotFound();
            }
            db.Contragents.Remove(oper);
            await db.SaveChangesAsync();
            return Ok(oper);
        }
    }
}
