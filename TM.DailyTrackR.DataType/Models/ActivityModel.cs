using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TM.DailyTrackR.DataType.Enums;

namespace TM.DailyTrackR.DataType.Models
{
    public class ActivityModel
    {
        public int Id { get; set; }
        public string ProjectTypeDescription { get; set; }
        public TaskTypeEnum ActivityTypeId { get; set; }
        public string Description { get; set; }
        public StatusEnum StatusId { get; set; }
        public string UserName { get; set; }

    }
}
