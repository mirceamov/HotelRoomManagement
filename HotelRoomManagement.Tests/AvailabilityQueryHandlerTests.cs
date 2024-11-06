using HotelRoomManagement.Application.Queries.Availability;
using HotelRoomManagement.Domain.Entities;
using HotelRoomManagement.Domain.Interfaces;

namespace HotelRoomManagement.Tests
{
    [TestClass]
    public class AvailabilityQueryHandlerTests
    {
        private Mock<IHotelRepository> _hotelRepositoryMock = new();
        private Mock<IBookingRepository> _bookingRepositoryMock = new();
        private AvailabilityQueryHandler sut;

        [TestInitialize]
        public void Setup()
        {
            _hotelRepositoryMock = new Mock<IHotelRepository>();
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            sut = new AvailabilityQueryHandler(_hotelRepositoryMock.Object, _bookingRepositoryMock.Object);
        }

        [TestMethod]
        public async Task CheckAvailability_ShouldReturnGreaterThen0_WhenRoomsAreAvailable()
        {
            // Arrange
            var hotelId = "H1";
            var roomType = "DBL";
            var startDate = new DateTime(2024, 9, 1);
            var endDate = new DateTime(2024, 9, 3);

            _hotelRepositoryMock.Setup(repo => repo.GetHotelAsync(It.IsAny<string>()))
                .ReturnsAsync(new Hotel 
                {
                    Id = hotelId,
                    Rooms = new List<Room>
                    {
                        new Room { RoomType = roomType, RoomId = "201" },
                        new Room { RoomType = roomType, RoomId = "202" }
                    }
                });

            _bookingRepositoryMock.Setup(repo => repo.GetHotelBookingsAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<Booking>
                {
                    new Booking
                    {
                        HotelId = hotelId,
                        RoomType = roomType,
                        Arrival = new DateTime(2024, 9, 1),
                        Departure = new DateTime(2024, 9, 2)
                    }
                });

            // Act
            var result = await sut.HandleAsync(new AvailabilityQuery() { HotelId = hotelId, StartDate = startDate, EndDate = endDate, RoomType = roomType });

            // Assert
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public async Task CheckAvailability_ShouldReturnNegative_WhenRoomsAreOverbooked()
        {
            // Arrange
            var hotelId = "H1";
            var roomType = "DBL";
            var startDate = new DateTime(2024, 9, 1);
            var endDate = new DateTime(2024, 9, 3);

            _hotelRepositoryMock.Setup(repo => repo.GetHotelAsync(It.IsAny<string>()))
                .ReturnsAsync(new Hotel
                {
                    Id = hotelId,
                    Rooms = new List<Room>
                    {
                        new Room { RoomType = roomType, RoomId = "201" },
                        new Room { RoomType = roomType, RoomId = "202" }
                    }
                });

            _bookingRepositoryMock.Setup(repo => repo.GetHotelBookingsAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<Booking>
                {
                    new Booking
                    {
                        HotelId = hotelId,
                        RoomType = roomType,
                        Arrival = new DateTime(2024, 9, 1),
                        Departure = new DateTime(2024, 9, 2)
                    },
                    new Booking
                    {
                        HotelId = hotelId,
                        RoomType = roomType,
                        Arrival = new DateTime(2024, 9, 1),
                        Departure = new DateTime(2024, 9, 2)
                    },
                    new Booking
                    {
                        HotelId = hotelId,
                        RoomType = roomType,
                        Arrival = new DateTime(2024, 9, 1),
                        Departure = new DateTime(2024, 9, 2)
                    }
                });

            // Act
            var result = await sut.HandleAsync(new AvailabilityQuery() { HotelId = hotelId, StartDate = startDate, EndDate = endDate, RoomType = roomType });

            // Assert
            Assert.AreEqual(-1, result);
        }

        [TestMethod]
        public async Task CheckAvailability_ShouldReturn0_WhenBookedDepartureIsGreaterThenStartDate()
        {
            // Arrange
            var hotelId = "H1";
            var roomType = "DBL";
            var startDate = new DateTime(2024, 9, 1);
            var endDate = new DateTime(2024, 9, 3);

            _hotelRepositoryMock.Setup(repo => repo.GetHotelAsync(It.IsAny<string>()))
                .ReturnsAsync(new Hotel
                {
                    Id = hotelId,
                    Rooms = new List<Room>
                    {
                        new Room { RoomType = roomType, RoomId = "201" }
                    }
                });

            _bookingRepositoryMock.Setup(repo => repo.GetHotelBookingsAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<Booking>
                {
                    new Booking
                    {
                        HotelId = hotelId,
                        RoomType = roomType,
                        Arrival = startDate.AddDays(-1),
                        Departure = startDate.AddDays(1)
                    }
                });

            // Act
            var result = await sut.HandleAsync(new AvailabilityQuery() { HotelId = hotelId, StartDate = startDate, EndDate = endDate, RoomType = roomType });

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public async Task CheckAvailability_ShouldReturn0_WhenBookedArrivalIsLessOrEqualToEndDate()
        {
            // Arrange
            var hotelId = "H1";
            var roomType = "DBL";
            var startDate = new DateTime(2024, 9, 1);
            var endDate = new DateTime(2024, 9, 3);

            _hotelRepositoryMock.Setup(repo => repo.GetHotelAsync(It.IsAny<string>()))
                .ReturnsAsync(new Hotel
                {
                    Id = hotelId,
                    Rooms = new List<Room>
                    {
                        new Room { RoomType = roomType, RoomId = "201" }
                    }
                });

            _bookingRepositoryMock.Setup(repo => repo.GetHotelBookingsAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<Booking>
                {
                    new Booking
                    {
                        HotelId = hotelId,
                        RoomType = roomType,
                        Arrival = endDate,
                        Departure = endDate.AddDays(1)
                    }
                });

            // Act
            var result = await sut.HandleAsync(new AvailabilityQuery() { HotelId = hotelId, StartDate = startDate, EndDate = endDate, RoomType = roomType });

            // Assert
            Assert.AreEqual(0, result);
        }
    }
}