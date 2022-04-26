using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Baya.DataLayer.Entities
{
   public class Poll
    {
        [Key]
        public Guid Id { get; set; }

        public string Question { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public bool Active { get; set; }
        
    }

    public class PollOption
    {
        [Key]
        public Guid Id { get; set; }

        public Guid PollId { get; set; }

        public string Answer { get; set; }
        public int VoteCount { get; set; }


        [ForeignKey("PollId")]
        public virtual Poll Poll { get; set; }



    }
}
