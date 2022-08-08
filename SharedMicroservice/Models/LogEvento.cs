using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Model
{
    public class LogEvento
    {
        public int Id { get; set; }
        public int? EventId { get; set; }
        public string LogLevel { get; set; }
        public string Message { get; set; }
        public DateTime? CreatedTime { get; set; }
    }

    /*
     --SCRIPT (não contém migration)
        CREATE TABLE EVENTLOG (
        ID INT IDENTITY(1,1) NOT NULL,
        EVENTID INT NULL,
        LOGLEVEL VARCHAR(50) NOT NULL,
        MESSAGE VARCHAR(1000) NOT NULL,
        CREATEDTIME DATETIME NOT NULL
        )
     */
}
