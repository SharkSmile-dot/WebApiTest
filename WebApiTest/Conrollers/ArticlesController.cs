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
    [Route("/api/articles")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        OperationsContext db;
        public ArticlesController(OperationsContext context)
        {
            db = context;


            if (!db.Articles.Any())
            {

                string use, artf,artf1;
                int count = 0,u;
                for (int i = 1; i <= 1000; i++)
                {

                    u = i;
                    use = u.ToString();
                    artf = String.Concat("AR_", use);
                    u++;
                    use = u.ToString();
                    artf1 = String.Concat("AR_", use);
                    if (i <= 150)
                    {
                        if (count != 2) { db.Articles.Add(new Article { Name = artf, Parent = artf1 }); count++; db.SaveChanges(); }
                        else
                        { count = 0; db.Articles.Add(new Article { Name = artf }); db.SaveChanges(); }


                    }
                    else

                    { db.Articles.Add(new Article { Name = artf }); db.SaveChanges(); }

                }

                
            }
        }


        /// <summary>
        /// Получение полного списка статей
        /// </summary>
        /// <response code="200" >Список получен</response>
        [HttpGet("/api/articles/all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Article>>> Get()
        {
            return await db.Articles.ToListAsync();
        }

        /// <summary>
        /// Получение статьи по id
        /// </summary>
        /// <response code="200" >Статья получена</response>
        /// <response code="404" >Статья не найдена, проверьте id</response>
        [HttpGet("/api/articles/id/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Article>> Get(int id)
        {



            Article oper = await db.Articles.FirstOrDefaultAsync(x => x.Id == id);
            if (oper == null)
                return NotFound();

            return new ObjectResult(oper);
        }



        /// <summary>
        /// Добавление статьи !не указывайте id при добавлении! Контрагент и статья добавляются в формате "CR_int","AR_int"!!!Максимальная вложенность 2! 
        /// </summary>
        /// <response code="200" >Статья добавлена</response>
        /// <response code="400" >Статья не добавлена, проверьте данные</response>
        [HttpPost("/api/articles/post")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Article>> Post(Article oper)
        {
            if (oper == null)
            {
                return BadRequest();
            }

            db.Articles.Add(oper);
            await db.SaveChangesAsync();
            return Ok(oper);
        }

        /// <summary>
        /// Изменение статьи !id обязателен чтобы найти запись! Контрагент и статья изменяются в формате "CR_int","AR_int"!!!Максимальная вложенность 2! 
        /// </summary>
        /// <response code="200" >Статья изменена</response>
        /// <response code="400" >Статья не изменена, проверьте данные</response>
        /// <response code="404" >Статья не найдена, проверьте id</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        
        public async Task<ActionResult<Article>> Put(Article oper)
        {
            if (oper == null)
            {
                return BadRequest();
            }
            if (!db.Articles.Any(x => x.Id == oper.Id))
            {
                return NotFound();
            }

            db.Update(oper);
            await db.SaveChangesAsync();
            return Ok(oper);
        }

        /// <summary>
        /// Удаление статьи по id
        /// </summary>
        /// <response code="200" >Статья удалена</response>
        /// <response code="404" >Статья не найдена, проверьте id</response>
        [HttpDelete("/api/articles/delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Article>> Delete(int id)
        {
            Article oper = db.Articles.FirstOrDefault(x => x.Id == id);
            if (oper == null)
            {
                return NotFound();
            }
            db.Articles.Remove(oper);
            await db.SaveChangesAsync();
            return Ok(oper);
        }
    }
}