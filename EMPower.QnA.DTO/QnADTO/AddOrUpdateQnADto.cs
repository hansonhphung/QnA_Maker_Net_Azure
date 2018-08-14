using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPower.QnA.DTO.QnADTO
{
    public class AddOrUpdateQnADto
    {
        public AddQnADto add { get; set; }
        public UpdateQnADto update { get; set; }
        public DeleteQnADto delete { get; set; }
    }
}
