using HotelRoomManagement.Application.Queries;
using HotelRoomManagement.Domain.Entities;
using HotelRoomManagement.Domain.Interfaces;

namespace HotelRoomManagement.Tests
{
    [TestClass]
    public class RoomTypesQueryHandlerTests
    {
        private Mock<IHotelRepository> _hotelRepositoryMock = new();
        private Mock<IBookingRepository> _bookingRepositoryMock = new();
        private RoomTypesQueryHandler sut;

        [TestInitialize]
        public void Setup()
        {
            _hotelRepositoryMock = new Mock<IHotelRepository>();
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            sut = new RoomTypesQueryHandler(_hotelRepositoryMock.Object, _bookingRepositoryMock.Object);
        }

        [TestMethod]
        public async Task GetRoomTypes_ShouldThrowException_WhenAllocationNotPossible()
        {
            // Arrange
            var hotelId = "H1";
            var startDate = new DateTime(2024, 9, 2);
            var endDate = new DateTime(2024, 9, 5);
            var guests = 4;

            _hotelRepositoryMock.Setup(repo => repo.GetHotelAsync(It.IsAny<string>()))
                .ReturnsAsync(new Hotel
                {
                    Id = hotelId,
                    Name = "Hotel California",
                    RoomTypes = new List<RoomType>
                    {
                        new RoomType { Code = "DBL", Size = 2 },
                        new RoomType { Code = "SGL", Size = 1 }
                    },
                    Rooms = new List<Room>
                    {
                        new Room { RoomType = "DBL", RoomId = "201" },
                        new Room { RoomType = "DBL", RoomId = "202" },
                        new Room { RoomType = "SGL", RoomId = "101" }
                    }
                });

            _bookingRepositoryMock.Setup(repo => repo.GetHotelBookingsAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<Booking>
                {
                    new Booking
                    {
                        HotelId = hotelId,
                        RoomType =  "DBL",
                        Arrival = new DateTime(2024, 9, 4),
                        Departure = new DateTime(2024, 9, 5)
                    }
                });

            // Act & Assert
            await Assert.ThrowsExceptionAsync<Exception>(() =>
                sut.HandleAsync(new RoomTypesQuery() { HotelId = hotelId, StartDate = startDate, EndDate = endDate, Guests = guests }));
        }

        [TestMethod]
        public async Task GetRoomTypes_ShouldReturnOptimalRoomAllocation_WhenRoomsAreAvailable()
        {
            // Arrange
            var hotelId = "H1";
            var startDate = new DateTime(2024, 9, 4);
            var endDate = new DateTime(2024, 9, 5);
            var guests = 3;

            _hotelRepositoryMock.Setup(repo => repo.GetHotelAsync(It.IsAny<string>()))
                .ReturnsAsync(new Hotel
                {
                    Id = hotelId,
                    Name = "Hotel California",
                    RoomTypes = new List<RoomType>
                    {
                        new RoomType { Code = "DBL", Size = 2 },
                        new RoomType { Code = "SGL", Size = 1 }
                    },
                    Rooms = new List<Room>
                    {
                        new Room { RoomType = "DBL", RoomId = "201" },
                        new Room { RoomType = "DBL", RoomId = "202" },
                        new Room { RoomType = "DBL", RoomId = "203" },
                        new Room { RoomType = "SGL", RoomId = "101" }
                    }
                });

            _bookingRepositoryMock.Setup(repo => repo.GetHotelBookingsAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<Booking>
                {
                    new Booking
                    {
                        HotelId = hotelId,
                        RoomType = "DBL",
                        Arrival = new DateTime(2024, 9, 4),
                        Departure = new DateTime(2024, 9, 5)
                    }
                });
            // Act
            var result = await sut.HandleAsync(new RoomTypesQuery() { HotelId = hotelId, StartDate = startDate, EndDate = endDate, Guests = guests });

            // Assert
            Assert.AreEqual("H1: DBL, SGL", result);
        }

        [TestMethod]
        public async Task GetRoomTypes_ShouldReturnCorrectRoomAllocation_WithPartialRoomMark()
        {
            // Arrange
            var hotelId = "H1";
            var startDate = new DateTime(2024, 9, 4);
            var endDate = new DateTime(2024, 9, 5);
            var guests = 3;

            _hotelRepositoryMock.Setup(repo => repo.GetHotelAsync(It.IsAny<string>()))
                .ReturnsAsync(new Hotel
                {
                    Id = hotelId,
                    Name = "Hotel California",
                    RoomTypes = new List<RoomType>
                    {
                        new RoomType { Code = "DBL", Size = 2 },
                        new RoomType { Code = "SGL", Size = 1 }
                    },
                    Rooms = new List<Room>
                    {
                        new Room { RoomType = "DBL", RoomId = "201" },
                        new Room { RoomType = "DBL", RoomId = "202" },
                        new Room { RoomType = "DBL", RoomId = "203" },
                        new Room { RoomType = "SGL", RoomId = "101" }
                    }
                });

            _bookingRepositoryMock.Setup(repo => repo.GetHotelBookingsAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<Booking>
                {
                    new Booking
                    {
                        HotelId = hotelId,
                        RoomType = "SGL",
                        Arrival = new DateTime(2024, 9, 4),
                        Departure = new DateTime(2024, 9, 5)
                    }
                });

            // Act
            var result = await sut.HandleAsync(new RoomTypesQuery() { HotelId = hotelId, StartDate = startDate, EndDate = endDate, Guests = guests });

            // Assert
            Assert.AreEqual("H1: DBL, DBL!", result);
        }

        [TestMethod]
        public async Task GetRoomTypes_ShouldReturnCorrectRoomAllocation()
        {
            // Arrange
            var hotelId = "H1";
            var startDate = new DateTime(2024, 9, 4);
            var endDate = new DateTime(2024, 9, 5);
            var guests = 7;

            _hotelRepositoryMock.Setup(repo => repo.GetHotelAsync(It.IsAny<string>()))
                .ReturnsAsync(new Hotel
                {
                    Id = hotelId,
                    Name = "Hotel California",
                    RoomTypes = new List<RoomType>
                    {
                        new RoomType { Code = "TRPL", Size = 3 },
                        new RoomType { Code = "DBL", Size = 2 },
                        new RoomType { Code = "SGL", Size = 1 }
                    },
                    Rooms = new List<Room>
                    {
                        new Room { RoomType = "TRPL", RoomId = "301" },
                        new Room { RoomType = "TRPL", RoomId = "302" },
                        new Room { RoomType = "DBL", RoomId = "201" },
                        new Room { RoomType = "DBL", RoomId = "202" },
                        new Room { RoomType = "DBL", RoomId = "203" },
                        new Room { RoomType = "SGL", RoomId = "101" }
                    }
                });

            _bookingRepositoryMock.Setup(repo => repo.GetHotelBookingsAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<Booking>
                {
                    new Booking
                    {
                        HotelId = hotelId,
                        RoomType = "SGL",
                        Arrival = startDate,
                        Departure = endDate
                    },
                    new Booking
                    {
                        HotelId = hotelId,
                        RoomType = "DBL",
                        Arrival = startDate,
                        Departure = endDate
                    },
                    new Booking
                    {
                        HotelId = hotelId,
                        RoomType = "DBL",
                        Arrival = startDate,
                        Departure = endDate
                    }
                });

            // Act
            var result = await sut.HandleAsync(new RoomTypesQuery() { HotelId = hotelId, StartDate = startDate, EndDate = endDate, Guests = guests });

            // Assert
            Assert.AreEqual("H1: TRPL, TRPL, DBL!", result);
        }
    }
}