using AutoMapper;

namespace EMPower.QnA.BackgroundServices.Mappers
{
    public class AutoMapperConfig
    {
        public static void Register()
        {
            Mapper.Initialize(x =>
            {
                //x.AddProfile<RolloutProfile>();
            });
        }
    }
}
