using AutoMapper;
using BuildingBlocks.Tests.Extensions;
using Moq;
using static BuildingBlocks.Tests.Extensions.ServiceMockExtensions;

namespace BuildingBlocks.Tests.Mocks
{
    public class MapperMock : Mock<IMapper>
    {
        public MapperMock() : base(MockBehavior.Strict)
        {
        }

        public void MockMap<TSource, TTarget>(TSource source, TTarget target)
        {
            Setup(_ => _.Map<TTarget>(IsEquivalentTo(source))).Returns(target.ToClone());
        }

        public void VerifyMap<TSource, TTarget>(Times times)
        {
            Verify(_ => _.Map<TTarget>(It.IsAny<TSource>()), times);
        }
    }
}
