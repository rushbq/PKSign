using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



namespace SignData.Models
{
    public class BaseData
    {
        public Guid Data_ID { get; set; }
        public string TraceID { get; set; }
        public string Subject { get; set; }
        public string Display { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int Place { get; set; }
        public string PlaceName { get; set; }
        public string OtherPlace { get; set; }

        public Int32 Sort { get; set; }
        public Int32 JoinCnt { get; set; }
        public Int32 SignCnt { get; set; }
        public string Create_Who { get; set; }
        public string Create_Name { get; set; }
        public string Create_Time { get; set; }
        public string Update_Who { get; set; }
        public string Update_Name { get; set; }
        public string Update_Time { get; set; }
    }


    public class NameList
    {
        public Int32 Data_ID { get; set; }
        public string Who { get; set; }
    }

    public class CheckIn
    {
        public Int32 Data_ID { get; set; }
        public string SignWho { get; set; }
        public string SignTime { get; set; }
        public string FromIP { get; set; }
        public string IsAgent { get; set; }
    }

    /// <summary>
    /// 類別
    /// </summary>
    public class ClassItem
    {
        public int ID { get; set; }
        public string Label { get; set; }

    }

}
