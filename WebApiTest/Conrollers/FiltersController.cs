using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using WebApiTest.Models;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;



namespace WebApiTest.contollers
{
    [Route("/api/operations/filter")]
    [ApiController]


    public class FiltersController : ControllerBase
    {
        OperationsContext db;
        private List<Operation> oper;
        private List<Operation> operf;


        public FiltersController(OperationsContext context)
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
        /// Фильтр операций по массиву статей 
        /// </summary>
        /// <response code="200" >Операции получены</response>
        [HttpPut("/api/operations/articles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Operation>> Make(string[] check)
        {

           // string[] check = { "AR_311", "AR_27" };
            string[] control;
            string[] child;
            child = new string[150];
            control = new string[150];
            for (int i = 0; i < child.Length; i++) { child[i] = "1"; control[i] = "1"; }
            int p, d;
            bool rep = false;
            p = check.Length;
            d = p;
            for (int i = 0; i < check.Length; i++)
            {
                control[i] = check[i];
            }

            IQueryable<string> conc;
            for (int i = 0; i < check.Length; i++)
            {
                if (db.Articles.Where(x => x.Parent == check[i]).Any())
                {

                    conc = from Article in db.Articles where Article.Parent == check[i] select Article.Name;
                    foreach (var s in conc)
                    {
                        for (int j = 0; j < control.Length; j++) { if (s == control[j]) { rep = true; } }
                        if (rep == true) { rep = false; break; }


                        control[i + p] = s;
                        child[i] = s;

                    }


                    d++;
                }
            }
            for (int i = 0; i < child.Length; i++)
            {
                if (db.Articles.Where(x => x.Parent == child[i]).Any())
                {
                    conc = from Article in db.Articles where Article.Parent == child[i] select Article.Name;
                    foreach (var s in conc)
                    {
                        for (int j = 0; j < control.Length; j++) { if (s == control[j]) { rep = true; } }
                        if (rep == true) { rep = false; break; }
                        control[i + d] = s;
                    }
                }
            }
            oper = db.Operations.Where(x => x.Article == control[0]).ToList();
            for (int i = 1; i < control.Length; i++)
            {
                operf = db.Operations.Where(x => x.Article == control[i]).ToList();

                oper = oper.Concat(operf).ToList();
            }
            return oper;
        }
        /// <summary>
        /// Фильтр операций по массиву контрагентов 
        /// </summary>
        /// <response code="200" >Операции найдены</response>
        [HttpPut("/api/operations/contragents/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Operation>> Make1(string[] check) 
        {
           // string[] check = { "CR_1","CR_2"};
            oper = db.Operations.Where(x => x.Contragent == check[0]).ToList();
            for (int i = 1; i < check.Length; i++)
            {
                operf = db.Operations.Where(x => x.Contragent == check[i]).ToList();

                oper = oper.Concat(operf).ToList();
            }
            return oper;


        }
        /// <summary>
        /// Фильтр операций по дате начала  
        /// </summary>
        /// <response code="200" >Операции найдены</response>
        [HttpGet("/api/operations/datestart{check}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Operation>> Make2(DateTime check)
        {
            oper = db.Operations.Where(x => x.Start == check).ToList();
            
            return oper;


        }
        /// <summary>
        /// Фильтр операций по дате окончания
        /// </summary>
        /// <response code="200" >Операции найдены</response>
        [HttpGet("/api/operations/dateend{check}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Operation>> Make3(DateTime check)
        {
            oper = db.Operations.Where(x => x.End == check).ToList();

            return oper;
        }


    }
}
