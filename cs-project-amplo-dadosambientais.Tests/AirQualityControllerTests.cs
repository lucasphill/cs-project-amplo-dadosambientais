using cs_project_amplo_dadosambientais.Controllers;
using FakeItEasy;
using cs_project_amplo_dadosambientais.Services.AirQuality;
using Microsoft.AspNetCore.Mvc;
using cs_project_amplo_dadosambientais.Data.DTO.AirQualityData;
using cs_project_amplo_dadosambientais.Models;

namespace cs_project_amplo_dadosambientais.Tests;

public class AirQualityControllerTests
{
    [Fact]
    public void Create_AirQuality_Log()
    {
        var fakeStation = A.Dummy<Models.StationModel>();
        var fakeLog = new CreateAirQualityDTO
        {
            Data = A.Dummy<CreateAirQualityDataDTO>(),
            Obs = "String",
            StationId = fakeStation.Id
        };
        var fakeResponse = new ServiceResponseModel<Models.AirQualityModel>
        {
            Data = null,
            Message = "Inserted",
            Status = true
        };

        var dataAirQuality = A.Fake<IAirQualityInterface>(); // crio um controller falso
        A.CallTo(() => dataAirQuality.CreateAirQuality(fakeLog)).Returns(fakeResponse);
        var controller = new AirQualityController(dataAirQuality); // instancia do controller correto com dados falsos

        // Act
        var actionResult = controller.CreateAirQuality(fakeLog); // chama a função da controller passando os dados falsos

        // Result
        var result = actionResult.Result as OkObjectResult;
        var returnData = result.Value as ServiceResponseModel<Models.AirQualityModel>;

        Assert.NotNull(result);
        Assert.NotNull(returnData);
        Assert.Equal(200, result.StatusCode); // Verifica se retorna um HTTP 200 Ok
    }

    [Fact]
    public void Get_All_AirQuality_Logs()
    {
        // Arrange
        int count = 5;
        var fakeAirQualityWStationLogs = A.CollectionOfDummy<ReadAirQualityWStationDTO>(count).ToList(); // 5 objetos ReadAirQualityWStationDTO
        var fakeResponse = new ServiceResponseModel<List<ReadAirQualityWStationDTO>>
        {
            Data = fakeAirQualityWStationLogs,
            Message = "Message",
            Status = true
        };

        var dataAirQuality = A.Fake<IAirQualityInterface>(); // dados fakes do controller
        A.CallTo(() => dataAirQuality.ReadAirQuality()).Returns(fakeResponse);
        var controller = new AirQualityController(dataAirQuality); // injeto meus dados fakes

        // Act
        var actionResult = controller.ReadAirQuality();

        // Assert
        var result = actionResult.Result as OkObjectResult;
        var returnData = result.Value as ServiceResponseModel<List<ReadAirQualityWStationDTO>>;
        Assert.NotNull(result);
        Assert.NotNull(returnData);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(count, returnData.Data.Count);
    }

    [Fact]
    public void Get_One_AirQuality_Log_By_Id()
    {
        // Arrange
        var fakeAirQualityWStationLog = A.Dummy<ReadAirQualityWStationDTO>();
        
        var fakeResponse = new ServiceResponseModel<ReadAirQualityWStationDTO>
        {
            Data = fakeAirQualityWStationLog,
            Message = "Message",
            Status = true
        };

        var dataAirQuality = A.Fake<IAirQualityInterface>(); // dados fakes do controller
        A.CallTo(() => dataAirQuality.ReadAirQualityById(fakeAirQualityWStationLog.Id)).Returns(fakeResponse);
        
        var controller = new AirQualityController(dataAirQuality); // injeto meus dados fakes

        // Act
        var actionResult = controller.ReadAirQualityById(fakeAirQualityWStationLog.Id);

        // Assert
        var result = actionResult.Result as OkObjectResult;
        var returnData = result.Value as ServiceResponseModel<ReadAirQualityWStationDTO>;
        Assert.NotNull(result);
        Assert.NotNull(returnData);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(fakeAirQualityWStationLog, returnData.Data);
    }

    [Fact]
    public void Get_One_AirQuality_Log_By_Station_Id()
    {
        // Arrange
        var count = 5;
        var fakeStation = A.Dummy<Models.StationModel>(); 
        var fakeAirQualityLogs = A.CollectionOfDummy<ReadAirQualityDTO>(count).ToList();
        var fakeResponse = new ServiceResponseModel<List<ReadAirQualityDTO>>
        {
            Data = fakeAirQualityLogs,
            Message = "Message",
            Status = true
        };

        var dataAirQuality = A.Fake<IAirQualityInterface>(); // dados fakes do controller
        A.CallTo(() => dataAirQuality.ReadAirQualityByStationId(fakeStation.Id)).Returns(fakeResponse);

        var controller = new AirQualityController(dataAirQuality); // injeto meus dados fakes

        // Act
        var actionResult = controller.ReadAirQualityByStationId(fakeStation.Id);

        // Assert
        var result = actionResult.Result as OkObjectResult;
        var returnData = result.Value as ServiceResponseModel<List<ReadAirQualityDTO>>;
        Assert.NotNull(result);
        Assert.NotNull(returnData);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(fakeAirQualityLogs, returnData.Data);
    }

    [Fact]
    public void Update_AirQuality_Log()
    {
        // Arrange
        var fakeLog = A.Dummy<UpdateAirQualityDTO>();
        var fakeId = new Guid();
        var fakeResponse = A.Dummy<ServiceResponseModel<Models.AirQualityModel>>();

        var dataAirQuality = A.Fake<IAirQualityInterface>(); // dados fakes do controller
        A.CallTo(() => dataAirQuality.UpdateAirQuality(fakeId, fakeLog)).Returns(fakeResponse);

        var controller = new AirQualityController(dataAirQuality); // injeto meus dados fakes

        // Act
        var actionResult = controller.UpdateAirQuality(fakeId, fakeLog);

        // Assert
        var result = actionResult.Result as NoContentResult;
        Assert.NotNull(result);
        Assert.Equal(204, result.StatusCode);
    }

    [Fact]
    public void Delete_AirQuality_Log()
    {
        // Arrange
        var fakeLog = A.Dummy<Models.AirQualityModel>();
        var fakeResponse = A.Dummy<ServiceResponseModel<Models.AirQualityModel>>();

        var dataAirQuality = A.Fake<IAirQualityInterface>(); // dados fakes do controller
        A.CallTo(() => dataAirQuality.DeleteAirQuality(fakeLog.Id)).Returns(fakeResponse);

        var controller = new AirQualityController(dataAirQuality); // injeto meus dados fakes

        // Act
        var actionResult = controller.DeleteAirQuality(fakeLog.Id);

        // Assert
        var result204 = actionResult.Result as NoContentResult;
        Assert.NotNull(result204);
        Assert.Equal(204, result204.StatusCode);
    }

    [Fact]
    public void Delete_AirQuality_Log_Wrong_Id()
    {
        // Arrange
        var fakeLog = A.Dummy<Models.AirQualityModel>();
        var fakeResponse = new ServiceResponseModel<Models.AirQualityModel>
        {
            Data = null,
            Message = "Air quality data not found",
            Status = false
        };

        var dataAirQuality = A.Fake<IAirQualityInterface>(); // dados fakes do controller
        A.CallTo(() => dataAirQuality.DeleteAirQuality(fakeLog.Id)).Returns(fakeResponse);

        var controller = new AirQualityController(dataAirQuality); // injeto meus dados fakes

        // Act
        var actionResult = controller.DeleteAirQuality(fakeLog.Id);

        // Assert
        var result = actionResult.Result as BadRequestObjectResult;
        var returnData = result.Value as ServiceResponseModel<AirQualityModel>;
        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);
        Assert.Equal(fakeResponse.Message, returnData.Message);
    }
}
