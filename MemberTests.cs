using Library.Domain.Entities;
using Xunit;

namespace Library.Tests
{
    public class MemberTests
    {
        [Fact]
        public void Member_ShouldStoreValuesCorrectly()
        {
            var member = new Member
            {
                FullName = "Sebastian Lopez",
                Email = "sebastian@example.com",
                Phone = "0891111111"
            };

            Assert.Equal("Sebastian Lopez", member.FullName);
            Assert.Equal("sebastian@example.com", member.Email);
            Assert.Equal("0891111111", member.Phone);
        }
    }
}