$hotemRoomServiceUnitTestsDir = $PSScriptRoot.Substring(0, $PSScriptRoot.LastIndexOf("\"))

dotnet test $hotemRoomServiceUnitTestsDir\HotelRoomService\HotemRoomService.UnitTests\HotemRoomService.UnitTests.csproj