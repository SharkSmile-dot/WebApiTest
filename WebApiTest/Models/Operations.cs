using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiTest.Models
{
    [Table("Operations")]
    public class Operation
    {
        [Key]
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Type { get; set; }
        public int Value { get; set; }
        public string Contragent { get; set; }
        public string Article { get; set; }
       
    }
    [Table("Analytics")]
    public class Analytics
    {
        [Key]
        public int Id { get; set; }
        public int SumALL { get; set; }
        public int CountALL { get; set; }
        public int SumADMISSION { get; set; }
        public int CountADMISSION { get; set; }
        public int SumPAYOUT { get; set; }
        public int CountPAYOUT { get; set; }
      
    }
    [Table("Contragents")]
    public class Contragent
    {[Key]
        public int Id { get; set; }
        public string Name { get; set; }

    }
    [Table("Articles")]
    public class Article
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Parent { get; set; }
        
        
    }

    [Table("Analytics1")]
    public class Analytics1
    {
        [Key]
        public int Id { get; set; }
        public string Contr { get; set; }
        public int SumALL { get; set; }
        public int CountALL { get; set; }
        public int SumADMISSION { get; set; }
        public int CountADMISSION { get; set; }
        public int SumPAYOUT { get; set; }
        public int CountPAYOUT { get; set; }

    }

}
