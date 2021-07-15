using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using WebApiTest.Models;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Http;

namespace WebApiTest.Conrollers
{
    [Route("/api/operations")]
    [ApiController]
    public class OperationsController : ControllerBase
    {
        OperationsContext db;
        public OperationsController(OperationsContext context)
        {
            db = context;


            if (!db.Operations.Any())
            {

                int m, d, y, contr, cnt;
                string tp, use, contf, artf;
                Random rnd = new Random();
                DateTime st, en;
                for (int i = 1; i <= 1000; i++)
                {
                    y = rnd.Next(2019, 2021);
                    m = rnd.Next(1, 12);
                    d = rnd.Next(1, 20);
                    st = new DateTime(y, m, d);
                    en = new DateTime(y, m, d + rnd.Next(0, 4));
                    cnt = rnd.Next(0, 2);
                    if (cnt == 1) { tp = "Admission"; } else { tp = "Payout"; }
                    contr = rnd.Next(1, 5000);
                    use = contr.ToString();
                    contf = String.Concat("CR_", use);
                    contr = rnd.Next(1, 1000);
                    use = contr.ToString();
                    artf = String.Concat("AR_", use);
                    db.Operations.Add(new Operation { Start = st, End = en, Type = tp, Value = rnd.Next(0, 10000), Contragent = contf, Article = artf });
                    db.SaveChanges();
                }
            }
        }


        /// <summary>
        /// Получение полного списка операций
        /// </summary>
        /// <response code="200" >Список получен</response>
        [HttpGet("/api/operations/all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Operation>>> Get()
        {
            return await db.Operations.ToListAsync();
        }

        /// <summary>
        /// Получение операции по id
        /// </summary>
        /// <response code="200" >Операция получена</response>
        /// <response code="404" >Операция не найдена, проверьте id</response>
        [HttpGet("/api/operations/id/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Operation>> Get(int id)
        {



            Operation oper = await db.Operations.FirstOrDefaultAsync(x => x.Id == id);
            if (oper == null)
                return NotFound();

            return new ObjectResult(oper);
        }



        /// <summary>
        /// Добавление операции !не указывайте id при добавлении! 
        /// </summary>
        /// <response code="200" >Операция добавлена</response>
        /// <response code="400" >Операция не добавлена, проверьте данные</response>
        [HttpPost("/api/operations/post")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Operation>> Post(Operation oper)
        {
            if (oper == null || !db.Articles.Any(x => x.Name == oper.Article) || !db.Contragents.Any(x => x.Name == oper.Contragent))
            {
                return BadRequest();
            }

            db.Operations.Add(oper);
            await db.SaveChangesAsync();
            return Ok(oper);
        }

        /// <summary>
        /// Изменение операции !id обязателен чтобы найти запись! 
        /// </summary>
        /// <response code="200" >Операция изменена</response>
        /// <response code="400" >Операция не изменена, проверьте данные</response>
        /// <response code="404" >Операция не найдена, проверьте id</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<Operation>> Put(Operation oper)
        {
            if (oper == null || !db.Articles.Any(x => x.Name == oper.Article) || !db.Contragents.Any(x => x.Name == oper.Contragent) )
            {
                return BadRequest();
            }
            if (!db.Operations.Any(x => x.Id == oper.Id))
            {
                return NotFound();
            }

            db.Update(oper);
            await db.SaveChangesAsync();
            return Ok(oper);
        }

        /// <summary>
        /// Удаление операции по id
        /// </summary>
        /// <response code="200" >Операция удалена</response>
        /// <response code="404" >Операция не найдена, проверьте id</response>
        [HttpDelete("/api/operations/delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Operation>> Delete(int id)
        {
            Operation oper = db.Operations.FirstOrDefault(x => x.Id == id);
            if (oper == null)
            {
                return NotFound();
            }
            db.Operations.Remove(oper);
            await db.SaveChangesAsync();
            return Ok(oper);
        }
    }
}
