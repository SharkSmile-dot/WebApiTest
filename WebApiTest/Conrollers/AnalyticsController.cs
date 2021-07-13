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
    [Route("/api/analytics")]
    [ApiController]


    public class AnalyticsController : ControllerBase
    {
        OperationsContext db;
        
        


        public AnalyticsController(OperationsContext context)
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
         /// Аналитика по операциям дата начала операции
         /// </summary>
         /// <response code="200" >Аналитика получена</response>
         /// <response code="400">Аналитика не проведена, проверьте данные</response>
         [HttpGet("/api/analytics/2/datestart/{check}")]
         [ProducesResponseType(StatusCodes.Status200OK)]
         [ProducesResponseType(StatusCodes.Status400BadRequest)]
         public async Task<List<Analytics>> Main(DateTime check)
         {
             if (check == null) { BadRequest(); }

             int sum1 = db.Operations.Where(x => x.Start == check).Sum(u => u.Value);
             int coun1 = db.Operations.Count(x => x.Start == check);

             int sum2 = db.Operations.Where(x => x.Start == check).Where(t => t.Type == "Admission").Sum(u => u.Value);
             int coun2 = db.Operations.Where(t => t.Type == "Admission").Count(x => x.Start == check);

             int sum3 = db.Operations.Where(x => x.Start == check).Where(p => p.Type == "Payout").Sum(u => u.Value);
             int coun3 = db.Operations.Where(p => p.Type == "Payout").Count(x => x.Start == check);
             db.Analytics.Add(new Analytics { SumALL = sum1, CountALL = coun1, SumADMISSION = sum2, CountADMISSION = coun2, SumPAYOUT = sum3, CountPAYOUT = coun3 });
             db.SaveChanges();
             var cnt = await db.Analytics.ToListAsync();



             return cnt;

         }
         /// <summary>
         /// Аналитика по операциям дата конца операции
         /// </summary>
         /// <response code="200" >Аналитика получена </response>
         /// <response code="400">Аналитика не проведена, проверьте данные</response>
         [HttpGet("/api/analytics/2/dateend/{check}")]
         [ProducesResponseType(StatusCodes.Status200OK)]
         [ProducesResponseType(StatusCodes.Status400BadRequest)]
         public async Task<List<Analytics>> Main1(DateTime check)
         {
             if (check == null) { BadRequest(); }

             int sum1 = db.Operations.Where(x => x.End == check).Sum(u => u.Value);
             int coun1 = db.Operations.Count(x => x.End == check);

             int sum2 = db.Operations.Where(x => x.End == check).Where(t => t.Type == "Admission").Sum(u => u.Value);
             int coun2 = db.Operations.Where(t => t.Type == "Admission").Count(x => x.End == check);

             int sum3 = db.Operations.Where(x => x.End == check).Where(p => p.Type == "Payout").Sum(u => u.Value);
             int coun3 = db.Operations.Where(p => p.Type == "Payout").Count(x => x.End == check);
             db.Analytics.Add(new Analytics { SumALL = sum1, CountALL = coun1, SumADMISSION = sum2, CountADMISSION = coun2, SumPAYOUT = sum3, CountPAYOUT = coun3 });
             db.SaveChanges();
             var cnt = await db.Analytics.ToListAsync();



             return cnt;

         }
         /// <summary>
         /// Аналитика операций по статьям !О том как изменить массив статей читайте в README!
         /// </summary>
         /// <response code="200" >Аналитика получена </response>
         [HttpGet("/api/analytics/2/article")]
         public async Task<List<Analytics>> Main()
         {
             string[] check = { "AR_311", "AR_26", "AR_24" };
             string[] control;
             string[] child;
             child = new string[150];
             control = new string[150];
             for (int i = 0; i < child.Length; i++) { child[i] = "1"; control[i] = "1"; }
             int sum1 = 0, cntl, p, d, count1 = 0;
             int sum2 = 0, sum3 = 0,  count2 = 0, count3 = 0;
             p = check.Length;
             d = p;
             bool rep = false;
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
             for (int i = 0; i < control.Length; i++)
             {
                cntl = db.Operations.Where(x => x.Contragent == control[i]).Sum(u => u.Value);
                sum1 = cntl + sum1;
                cntl = db.Operations.Where(x => x.Contragent == control[i]).Where(c => c.Type == "Admission").Sum(u => u.Value);
                sum2 = cntl + sum2;
                cntl = db.Operations.Where(x => x.Contragent == control[i]).Where(c => c.Type == "Payout").Sum(u => u.Value);
                sum3 = cntl + sum3;
                cntl = db.Operations.Count(x => x.Contragent == control[i]);
                count1 = cntl + count1;
                cntl = db.Operations.Where(c => c.Type == "Admission").Count(x => x.Contragent == control[i]);
                count2 = cntl + count2;
                cntl = db.Operations.Where(c => c.Type == "Payout").Count(x => x.Contragent == control[i]);
                count3 = cntl + count3;
            }
             db.Analytics.Add(new Analytics { SumALL = sum1, CountALL = count1, SumADMISSION = sum2, CountADMISSION = count2, SumPAYOUT = sum3, CountPAYOUT = count3 });
             db.SaveChanges();
             var cnt = await db.Analytics.ToListAsync();
             return cnt;
         }
         /// <summary>
         /// Выдает аналитику для операций по заранее созданному массиву контрагентов! о том как изменить массив читайте README!
         /// </summary>
         ///<response code="200">Аналитика получена</response>
         [HttpGet("/api/analytics/2/contragent/")]
         [ProducesResponseType(StatusCodes.Status200OK)]
         public async Task<List<Analytics>> Mainc()
         {
             string[] check = { "CR_3205", "CR_4629" };

             int sum1 = 0, cntl, sum2 = 0, sum3 = 0, count1 = 0, count2 = 0, count3 = 0;
             for (int i = 0; i < check.Length; i++)
             {

                 cntl = db.Operations.Where(x => x.Contragent == check[i]).Sum(u => u.Value);
                 sum1 = cntl + sum1;
                 cntl = db.Operations.Where(x => x.Contragent == check[i]).Where(c => c.Type == "Admission").Sum(u => u.Value);
                 sum2 = cntl + sum2;
                 cntl = db.Operations.Where(x => x.Contragent == check[i]).Where(c => c.Type == "Payout").Sum(u => u.Value);
                 sum3 = cntl + sum3;
                 cntl = db.Operations.Count(x => x.Contragent == check[i]);
                 count1 = cntl + count1;
                 cntl = db.Operations.Where(c => c.Type == "Admission").Count(x => x.Contragent == check[i]);
                 count2 = cntl + count2;
                 cntl = db.Operations.Where(c => c.Type == "Payout").Count(x => x.Contragent == check[i]);
                 count3 = cntl + count3;
             }
             db.Analytics.Add(new Analytics { SumALL = sum1, CountALL = count1, SumADMISSION = sum2, CountADMISSION = count2, SumPAYOUT = sum3, CountPAYOUT = count3 });
             db.SaveChanges();
             var cnt = await db.Analytics.ToListAsync();
             return cnt;
         }
        /// <summary>
        /// Выдает аналитику для каждого контрагента по заранее созданному массиву контрагентов, для изменения массива откройте код.
        /// </summary>
        ///<response code="200">Аналитика получена</response>
        [HttpGet("/api/analytics/1/contragent/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<List<Analytics1>> Main1()
        {
            string[] check = { "CR_5001" };
            
            int sum1 = 0, sum2 = 0, sum3 = 0, count1 = 0, count2 = 0, count3 = 0;
            for (int i = 0; i < check.Length; i++)
            {

                sum1 = db.Operations.Where(x => x.Contragent == check[i]).Sum(u => u.Value);
                
                sum2 = db.Operations.Where(x => x.Contragent == check[i]).Where(c => c.Type == "Admission").Sum(u => u.Value);
                
                sum3 = db.Operations.Where(x => x.Contragent == check[i]).Where(c => c.Type == "Payout").Sum(u => u.Value);
                
                count1 = db.Operations.Count(x => x.Contragent == check[i]);
                
                count2 = db.Operations.Where(c => c.Type == "Admission").Count(x => x.Contragent == check[i]);
                
                count3 = db.Operations.Where(c => c.Type == "Payout").Count(x => x.Contragent == check[i]);
                
                db.Analytics1.Add(new Analytics1 { Contr = check[i], SumALL = sum1, CountALL = count1, SumADMISSION = sum2, CountADMISSION = count2, SumPAYOUT = sum3, CountPAYOUT = count3 });
                db.SaveChanges();
            }
            
            var cnt = await db.Analytics1.ToListAsync();
            return cnt;
        }





    }
}
