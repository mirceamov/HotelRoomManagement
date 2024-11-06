# HotelRoomManagement

Steps to run the API in terminal:
1. open windows powershell and clone the repository locally with : git clone https://github.com/mirceamov/HotelRoomManagement.git

2. cd HotelRoomManagement

3. dotnet build -c Release -o ./HRM

4. cd HRM

5. to start the API, run : .\HotelRoomManagement.exe --hotels hotels.json --bookings bookings.json

The hotels.json and bookings.json files will serve as data storage for existing hotels and bookings. 
The following commands are available :
	Availability(hotelId, date or period, roomType) 
	examples : 
		Availability(H1, 20240901, SGL) 
		Availability(H1, 20240901-20240903, DBL)
	
	RoomTypes(hotelId, date or period, numberOfGuests)
	examples : 
		RoomTypes(H1, 20240904, 3)  
		RoomTypes(H1, 20240905-20240907, 5)