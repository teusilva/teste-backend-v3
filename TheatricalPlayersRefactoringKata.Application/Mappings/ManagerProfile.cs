using AutoMapper;
using TheatricalPlayersRefactoringKata.Application.DTOs.Request;
using TheatricalPlayersRefactoringKata.Application.DTOs.Response;

namespace TheatricalPlayersRefactoringKata.Application.Mappings
{
    public class ManagerProfile : Profile
	{
		public ManagerProfile()
		{
			//Request
			CreateMap<InvoiceRequest, Domain.Entities.Invoice>().ReverseMap();
			CreateMap<PerformanceRequest, Domain.Entities.Performance>().ReverseMap();
			CreateMap<PlayRequest, Domain.Entities.Play>().ReverseMap();

			//Response
			CreateMap<InvoiceResponse, Domain.Entities.Invoice>().ReverseMap();
			CreateMap<PerformanceResponse, Domain.Entities.Performance>().ReverseMap();
			CreateMap<PlayResponse, Domain.Entities.Play>().ReverseMap();
		}
	}
}