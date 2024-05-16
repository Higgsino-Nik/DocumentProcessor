using AutoMapper;
using DocumentProcessor.Enums;
using DocumentProcessor.Extensions;
using DocumentProcessor.Models;
using DocumentProcessor.Models.Api;
using DocumentProcessor.Models.Db;

namespace DocumentProcessor.Configuration
{
    public class MapperConfigurator
    {
        private readonly IMapperConfigurationExpression _expression;

        public MapperConfigurator(IMapperConfigurationExpression expression)
        {
            MappingDal(expression);
            MappingApi(expression);
        }

        public IMapperConfigurationExpression GetConfiguration() => _expression;

        private static void MappingDal(IMapperConfigurationExpression expression)
        {
            expression.CreateMap<DalTask, DocumentTask>()
                .ForMember(
                    dst => dst.PreviousTaskId,
                    cfg => cfg.MapFrom(src => src.PreviousTaskId))
                .ForMember(
                    dst => dst.Status,
                    cfg => cfg.MapFrom(src => (Status)src.StatusId));

            expression.CreateMap<DalDocument, Document>()
                .IncludeMembers()
                .ForMember(
                    dst => dst.Status,
                    cfg => cfg.MapFrom(src => (Status)src.StatusId));
        }

        private static void MappingApi(IMapperConfigurationExpression expression)
        {
            expression.CreateMap<DocumentTask, TaskResponse>()
                .ForMember(
                    dst => dst.Status,
                    cfg => cfg.MapFrom(src => src.Status.GetDisplayName()));

            expression.CreateMap<Document, DocumentResponse>()
                .IncludeMembers()
                .ForMember(
                    dst => dst.ActiveTask,
                    cfg => cfg.MapFrom(src => src.Tasks.SingleOrDefault(x => x.Status == Status.InProgress)))
                .ForMember(
                    dst => dst.Tasks,
                    cfg => cfg.MapFrom(src => src.Tasks.OrderBy(x => x.Id).ToList()))
                .ForMember(
                    dst => dst.Status,
                    cfg => cfg.MapFrom(src => src.Status.GetDisplayName()));
        }
    }
}
