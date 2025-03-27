using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheatricalPlayersRefactoringKata.Domain.Entities;
using TheatricalPlayersRefactoringKata.Domain.Enums;

namespace TheatricalPlayersRefactoringKata.Application.DTOs.Response
{
    public class PlayResponse
    {
        public Guid Id { get; set; }
        public Guid PerformanceId { get; set; }
        public string Name { get; set; }
        public int Lines { get; set; }
        public TypesPlays Type { get; set; }
        public virtual Performance Performance { get; set; }
    }
}
